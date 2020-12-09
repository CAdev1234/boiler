using System;
using sand_data_ctrl;
using ykmWeb.Models;
using System.Data;
using System.Text.RegularExpressions;
using ykmWeb.common;

namespace ykmWeb.Dal
{
    public abstract class nclassdo
    {
        public string _dataname { get; private set; }
        public DbHelper db { get; private set; }

        public nclassdo(string dataname)
        {
            //this._dataname = "menuClass";
            this._dataname = dataname;
            this.db = new DbHelper();

        }

        public abstract int do_action();
    }
    /// <summary>
    /// 保存
    /// </summary>
    public class nclass_do_save : nclassdo
    {
        private nclass nc;
        public nclass_do_save(string dataname, nclass _nc)
            : base(dataname)
        {
            nc = _nc;
        }

        private string return_enname(string name)
        {
            string sql = "select count(catalogid) from  " + _dataname + " where caenname='" + name + "'";
            int c = (int)db.ExecuteScalar(db.GetSqlStringCommond(sql));
            if (c != 0)
            {
                return "-1";
            }
            else
            {
                return name;
            }
        }
        private void create_floder(string path)
        {
          //  sand_common.filecz.FolderCreate(path);
        }

        private string get_parentstr_floder(string parentstr)
        {
            string restr = "";
            DataTable ds = db.ExecuteDataTable(db.GetSqlStringCommond("select catalogid,Caenname from infoclass where catalogid in(" + parentstr + ") order by rootid,orders"));
            if (ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    restr+=(ds.Rows[i]["Caenname"].ToString() + "/");
                }
            }
            return restr;
        }
        /// <summary>
        /// 保存分类，必须制定Catalogname和Parentid
        /// </summary>
        /// <returns></returns>
        public override int do_action()
        {

            int strParent = 0;//初始化父类ID
            string strParstring = "0";//初始化父类ID集合
            int strDepth = 1;//初始化级数
            int strRootID = 1;//初始化1级分类排序
            int strOrders = 1;//初始化本类分类排序
            int strChild = 0;//初始化子类个数
            if (nc.ParentID.ToString() == "0")
            {

                DataTable dt = db.ExecuteDataTable(db.GetSqlStringCommond("select top 1 catalogid from " + _dataname + " order by catalogid desc"));
                if (dt.Rows.Count > 0)
                {
                    strRootID = ((int)db.ExecuteScalar(db.GetSqlStringCommond("select max(RootID) from " + _dataname))) + 1;
                }
                else
                {
                    strRootID = 1;
                }
                dt.Dispose();
            }
            else
            {
                DataTable mydsobj = db.ExecuteDataTable(db.GetSqlStringCommond("select ParentStr,Depth,RootID,Child,orders from " + _dataname + " where catalogid=" + nc.ParentID));
                strParent = Convert.ToInt32(nc.ParentID);
                strParstring = mydsobj.Rows[0]["ParentStr"].ToString() + "," + nc.ParentID;
                strDepth = Convert.ToInt32(mydsobj.Rows[0]["Depth"].ToString()) + 1;
                strRootID = Convert.ToInt32(mydsobj.Rows[0]["RootID"].ToString());
                int parChild = Convert.ToInt32(mydsobj.Rows[0]["Child"].ToString());
                if (parChild > 0)
                {
                    string sql1 = "select max(orders) from " + _dataname + " where parentid=" + nc.ParentID + " and rootid=" + strRootID;
                    object _o = db.ExecuteScalar(db.GetSqlStringCommond(sql1));

                    int Orders1 = 0;
                    if (Convert.IsDBNull(_o) == false && _o != null)
                    {
                        Orders1 = (int)_o;
                    }
                    object _o2 = db.ExecuteScalar(db.GetSqlStringCommond("select Max(Orders) From " + _dataname + " where ParentStr like '%" + strParstring + ",%' and rootid=" + strRootID));
                    int orders2 = 0;
                    if (Convert.IsDBNull(_o2) == false && _o2 != null)
                    {
                        orders2 = (int)_o2;
                    }
                    if (Orders1 > orders2)
                    {
                        strOrders = Orders1 + 1;
                    }
                    else
                    {
                        strOrders = orders2 + 1;
                    }
                }
                else
                {
                    strOrders = Convert.ToInt32(mydsobj.Rows[0]["orders"].ToString()) + 1;
                }
                db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + " set orders=orders+1 where orders>=" + strOrders + " and rootid=" + strRootID));
                db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + " set child=child+1 where catalogid=" + nc.ParentID));
            }
            
            string _caenname = common.hz_to_py.GetFirstPinyin(nc.Catalogname).Replace("/","").Replace(" ","").Replace("\\","").ToLower();
            int i = 1;
            while (return_enname(_caenname) == "-1")
            {
                _caenname = _caenname + "_" + i;
                i++;
            }


            t_list_Data tld = new t_list_Data();
            nc.ParentID = strParent;
            nc.ParentStr = strParstring;
            nc.Depth = strDepth;
            nc.RootID = strRootID;
            nc.Orders = strOrders;
            nc.Child = strChild;
            nc.Caenname = _caenname;
            
            int renum = tld.insert_data(nc, _dataname, true);
            if (renum != 0)
            {
                if (nc.ParentID == 0)
                {
                  //  create_floder(System.Web.HttpContext.Current.Server.MapPath("/cn/" + _caenname));
                }
                else
                {
                  //  create_floder(System.Web.HttpContext.Current.Server.MapPath("/cn/" + get_parentstr_floder(strParstring)+_caenname));
                }
                return renum;
            }
            else
            {
                return 0;
            }

        }
    }
    /// <summary>
    /// 修改名称
    /// </summary>
    public class nclass_dochangename : nclassdo
    {
        private nclass nc;
        public nclass_dochangename(string dataname, nclass _nc)
            : base(dataname)
        {
            nc = _nc;
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <returns></returns>
        public override int do_action()
        {
            int? c_id = nc.Catalogid;
            nc.Catalogid = null;
            t_list_Data tld = new t_list_Data();
            return tld.up_data(nc, _dataname, "catalogid=" + c_id);
        }
    }
    /// <summary>
    /// 删除分类
    /// </summary>
    /// <returns></returns>
    public class nclass_do_del : nclassdo
    {
        private string catalogid;
        public nclass_do_del(string dataname, string _classid)
            : base(dataname)
        {
            this.catalogid = _classid;
        }
        public override int do_action()
        {
            DataTable dt = db.ExecuteDataTable(db.GetSqlStringCommond("select parentid,child from menuClass  where catalogid=" + catalogid));
            if (dt.Rows.Count > 0)
            {
                string pid = dt.Rows[0]["parentid"].ToString();
                string c_num = dt.Rows[0]["child"].ToString();
                if (c_num != "0")
                {
                    return 2;
                }
                else
                {
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update menuClass set child=child-1 where catalogid=" + pid));
                    db.ExecuteNonQuery(db.GetSqlStringCommond("delete from menuClass where catalogid=" + catalogid));
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }
    }
    /// <summary>
    /// 删除主树
    /// </summary>
    public class nclass_do_del_root : nclassdo
    {
        private string catalogid;
        public nclass_do_del_root(string dataname, string _catalogid)
            : base(dataname)
        {
            this.catalogid = _catalogid;
        }

        public override int do_action()
        {
            db.ExecuteNonQuery(db.GetSqlStringCommond("delete from menuClass where rootid=(select rootid from " + _dataname + " where catalogid=" + catalogid + ")"));
            return 1;
        }

    }
    /// <summary>
    /// 一级分类上移
    /// </summary>
    public class nclass_oneup : nclassdo
    {
        private string catalogid;
        private string num;
        public nclass_oneup(string dataname, string _catalogid, string _num)
            : base(dataname)
        {
            this.catalogid = _catalogid;
            this.num = _num;
        }
        /// <summary>
        /// 一级分类向上调整
        /// </summary>
        /// <param name="num">移动几位</param>
        /// <returns></returns>
        public override int do_action()
        {
            string seRootID = db.ExecuteScalar(db.GetSqlStringCommond("select rootid from menuclass where catalogid=" + catalogid)).ToString();
            string mbrootid = "";
            int maxRoot = (int)db.ExecuteScalar(db.GetSqlStringCommond("select max(rootid) from menuclass"));//去的当前最大ROOTid
            db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass set rootid=" + maxRoot + " +1 where rootid=" + seRootID));//先把要改变的分类的ROOTID加一。包括子类
            DataTable myobj = db.ExecuteDataTable(db.GetSqlStringCommond("select rootid from menuclass where parentid=0 and rootid <" + seRootID + " order by rootid desc"));
            for (int i = 0; i < myobj.Rows.Count; i++)//更新比当前分类ROOTID大的，一次-1，并且更新数量小于移动数
            {
                if (i < Convert.ToInt32(num))
                {
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass  set rootid=rootid+1 where rootid=" + myobj.Rows[i]["rootid"].ToString()));
                    mbrootid = myobj.Rows[i]["rootid"].ToString();
                }
            }
            string sql = "update menuclass set rootid=" + mbrootid + " where rootid=" + (maxRoot + 1);

            db.ExecuteNonQuery(db.GetSqlStringCommond(sql));
            return 1;
        }
    }
    /// <summary>
    /// 一级分类下移
    /// </summary>
    public class nclass_onedown : nclassdo
    {
        private string num, catalogid;
        public nclass_onedown(string dataname, string _num, string _catalogid)
            : base(dataname)
        {
            this.num = _num;
            this.catalogid = _catalogid;
        }
        public override int do_action()
        {

            string seRootID = db.ExecuteScalar(db.GetSqlStringCommond("select rootid from  menuclass where catalogid=" + catalogid)).ToString();
            string mbrootid = "";
            int maxRoot = (int)db.ExecuteScalar(db.GetSqlStringCommond("select max(rootid) from  menuclass"));//去的当前最大ROOTid
            db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass set rootid=" + maxRoot + " +1 where rootid=" + seRootID));//先把要改变的分类的ROOTID加一。包括子类
            DataTable myobj = db.ExecuteDataTable(db.GetSqlStringCommond("select rootid from  menuclass where parentid=0 and rootid >" + seRootID + " order by rootid "));
            for (int i = 0; i < myobj.Rows.Count; i++)//更新比当前分类ROOTID大的，一次-1，并且更新数量小于移动数
            {
                if (i < Convert.ToInt32(num))
                {
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass set rootid=rootid-1 where rootid=" + myobj.Rows[i]["rootid"].ToString()));
                    mbrootid = myobj.Rows[i]["rootid"].ToString();
                }
            }
            string sql = "update  menuclass set rootid=" + mbrootid + " where rootid=" + (maxRoot + 1);
            db.ExecuteNonQuery(db.GetSqlStringCommond(sql));
            return 1;
        }
    }
    /// <summary>
    /// N级分类向上调整
    /// </summary>
    /// <param name="num">移动几位</param>
    /// <returns></returns>
    public class nclass_nup : nclassdo
    {
        private string num, catalogid;
        public nclass_nup(string dataname, string _num, string _catalogid)
            : base(dataname)
        {
            this.num = _num; this.catalogid = _catalogid;
        }
        public override int do_action()
        {
            DataTable myobj = db.ExecuteDataTable(db.GetSqlStringCommond("select parentid,ParentStr,Orders,child from menuclass where catalogid=" + catalogid));//得到该行分类所有信息
            string tPid = myobj.Rows[0]["parentid"].ToString();//取得当前要移动分类的ID
            string tParentStr = myobj.Rows[0]["ParentStr"].ToString();//取得当前要移动分类的父类字符串
            string tOrders = myobj.Rows[0]["Orders"].ToString();//取得当前分类的ORDERs
            string tChild = myobj.Rows[0]["child"].ToString();//取得当前分类的子类ORDERS
            myobj.Dispose();
            string thisOrders = "";
            int mNum = 0;
            if (tChild != "0")
            {
                mNum = ((int)db.ExecuteScalar(db.GetSqlStringCommond("select count(*) from menuclass where parentstr like '%" + tParentStr + "," + catalogid + "%'"))) + 1;
            }
            else
            {
                mNum = 1;
            }
            //以上取得要移动的分类数
            string sql = "select catalogid,orders,child,parentstr from menuclass where parentid=" + tPid + " and orders<" + tOrders + " order by orders desc";
            DataTable myds1 = db.ExecuteDataTable(db.GetSqlStringCommond(sql));
            for (int i = 0; i < myds1.Rows.Count; i++)
            {
                if (i < Convert.ToInt32(num))
                {
                    thisOrders = myds1.Rows[i]["orders"].ToString();
                    if (myds1.Rows[i]["child"].ToString() != "0")
                    {

                        DataTable myds2 = db.ExecuteDataTable(db.GetSqlStringCommond("select catalogid,orders from " + _dataname + "  where parentstr like '%" + myds1.Rows[i]["parentstr"].ToString() + "," + myds1.Rows[i]["catalogid"].ToString() + "%' order by orders"));
                        if (myds2.Rows.Count > 0)
                        {
                            for (int j = 0; j < myds2.Rows.Count; j++)
                            {
                                db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass  set orders=orders+" + mNum + " where catalogid=" + myds2.Rows[j]["catalogid"].ToString()));
                            }
                        }
                    }
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass  set orders=" + thisOrders + "+" + mNum + " where catalogid=" + myds1.Rows[i]["catalogid"].ToString()));
                }

            }
            db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass  set orders=" + thisOrders + " where catalogid=" + catalogid));
            if (tChild != "0")
            {
                DataTable myds3 = db.ExecuteDataTable(db.GetSqlStringCommond("select Catalogid,orders from menuclass  where parentstr like '%" + tParentStr + "," + catalogid + "%' order by orders"));
                for (int ii = 0; ii < myds3.Rows.Count; ii++)
                {
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass  set orders=" + thisOrders + "+" + (ii + 1) + " where catalogid=" + myds3.Rows[ii]["catalogid"].ToString()));
                }
                myds3.Dispose();
            }

            return 1;
        }
    }
    /// <summary>
    /// N级分类下移
    /// </summary>
    public class nclass_ndown : nclassdo
    {
        private string num, catalogid;
        public nclass_ndown(string dataname, string _num, string _catalogid)
            : base(dataname)
        {
            this.num = _num;
            this.catalogid = _catalogid;
        }

        public override int do_action()
        {


            DataTable myobj = db.ExecuteDataTable(db.GetSqlStringCommond("select parentid,ParentStr,Orders,child from menuclass  where catalogid=" + catalogid));//得到该行分类所有信息
            string tPid = myobj.Rows[0]["parentid"].ToString();//取得当前要移动分类的ID
            string tParentStr = myobj.Rows[0]["ParentStr"].ToString();//取得当前要移动分类的父类字符串
            string tOrders = myobj.Rows[0]["Orders"].ToString();//取得当前分类的ORDERs
            string tChild = myobj.Rows[0]["child"].ToString();//取得当前分类的子类ORDERS
            myobj.Dispose();
            string thisOrders = "";
            int mNum = 0;

            if (tChild != "0")
            {
                mNum = ((int)db.ExecuteScalar(db.GetSqlStringCommond("select count(*) from menuclass  where parentstr like '%" + tParentStr + "," + catalogid + "%'"))) + 1;
            }
            else
            {
                mNum = 1;
            }
            //以上取得要移动的分类数
            string sql = "select catalogid,orders,child,parentstr from menuclass  where parentid=" + tPid + " and orders > " + tOrders + " order by orders asc";
            DataTable myds1 = db.ExecuteDataTable(db.GetSqlStringCommond(sql));
            for (int i = 0; i < myds1.Rows.Count; i++)
            {
                if (i < Convert.ToInt32(num))
                {
                    thisOrders = myds1.Rows[i]["orders"].ToString();
                    if (myds1.Rows[i]["child"].ToString() != "0")
                    {
                        DataTable myds2 = db.ExecuteDataTable(db.GetSqlStringCommond("select catalogid,orders from menuclass  where parentstr like '%" + myds1.Rows[i]["parentstr"].ToString() + "," + myds1.Rows[i]["catalogid"].ToString() + "%' order by orders"));
                        if (myds2.Rows.Count > 0)
                        {
                            for (int j = 0; j < myds2.Rows.Count; j++)
                            {
                                thisOrders = myds2.Rows[j]["orders"].ToString();
                                db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass set orders=orders - " + mNum + " where catalogid=" + myds2.Rows[j]["catalogid"].ToString()));

                            }
                        }
                    }
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass set orders=orders - " + mNum + " where catalogid=" + myds1.Rows[i]["catalogid"].ToString()));


                }

            }

            db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass set orders=" + thisOrders + " - " + mNum + " +1  where catalogid=" + catalogid));
            if (tChild != "0")
            {
                DataTable myds3 = db.ExecuteDataTable(db.GetSqlStringCommond("select Catalogid,orders from menuclass where parentstr like '%" + tParentStr + "," + catalogid + "%' order by orders"));
                for (int ii = 0; ii < myds3.Rows.Count; ii++)
                {
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update menuclass  set orders=" + thisOrders + " - " + mNum + " + 1 + " + (ii + 1) + " where catalogid=" + myds3.Rows[ii]["catalogid"].ToString()));

                }
            }

            return 1;

        }
    }
    /// <summary>
    /// 调整分类顺序
    /// </summary>
    public class nclass_classCtrl : nclassdo
    {
        private string SelectCatalogID, catalogid;
        public nclass_classCtrl(string dataname, string _seid, string _catalogid)
            : base(dataname)
        {
            this.SelectCatalogID = _seid;
            this.catalogid = _catalogid;
        }

        /// <summary>
        /// 调整分类关系
        /// </summary>
        /// <param name="SelectCatalogID">调整到的分类ID</param>
        /// <returns>返回处理状态1、完成；2、不能选择自己；3、不能指定其下属分类</returns>
        public override int do_action()
        {
            //查询当前类的信息
            DataTable thisDs = db.ExecuteDataTable(db.GetSqlStringCommond("select catalogid, Parentid,ParentStr,depth,Child,Orders,rootid from " + _dataname + "  where catalogid=" + catalogid));
            //取出要移动的ID的数据
            string yParentid = thisDs.Rows[0]["Parentid"].ToString();
            string yParentStr = thisDs.Rows[0]["ParentStr"].ToString();
            string yDepth = thisDs.Rows[0]["depth"].ToString();
            string yChild = thisDs.Rows[0]["Child"].ToString();
            string yOrders = thisDs.Rows[0]["Orders"].ToString();
            string yRootID = thisDs.Rows[0]["rootid"].ToString();

            if (SelectCatalogID != thisDs.Rows[0]["ParentID"].ToString())//如果取得的分类与当前的分类的父分类不相同，则要修改
            {
                if (SelectCatalogID == catalogid) { return 2; }
                string sql = "select catalogid from " + _dataname + "  where  ParentStr like '%" + thisDs.Rows[0]["ParentStr"].ToString() + "," + thisDs.Rows[0]["Catalogid"].ToString() + "%' and catalogid=" + SelectCatalogID;
                DataTable testObj = db.ExecuteDataTable(db.GetSqlStringCommond(sql));
                if (testObj.Rows.Count > 0)
                {
                    //Response.Write(common.ErrorInfo("不能指定其下属分类为其父类"));
                    return 3;
                }
                int MaxRootID = (int)db.ExecuteScalar(db.GetSqlStringCommond("select max(Rootid) from " + _dataname + "  "));//取得目前最大ROOTID。

                //如果不是1级分类要改成1级分类
                if (SelectCatalogID == "0" && thisDs.Rows[0]["parentid"].ToString() != "0")
                {
                    db.ExecuteScalar(db.GetSqlStringCommond("update " + _dataname + "  set parentid=0,parentstr='0',depth=1,orders=1,rootid=" + (MaxRootID + 1) + " where catalogid=" + catalogid));
                    //如果有下属分类，则更新其下属分类数据。下属分类的排序不需考虑，只需更新下属分类深度和一级排序ID(rootid)数据
                    if (yChild != "0")
                    {
                        yParentStr += ",";
                        DataTable pds = db.ExecuteDataTable(db.GetSqlStringCommond("select * from " + _dataname + "  where parentstr like '%" + yParentStr + catalogid + "%'"));
                        if (pds.Rows.Count > 0)
                        {
                            // Response.Write(pds.Rows.Count.ToString() + "<br/>");
                            for (int i = 0; i < pds.Rows.Count; i++)
                            {
                                string mParentStr = "0," + pds.Rows[i]["parentstr"].ToString().Replace(yParentStr, "");
                                db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set depth=depth-" + (Convert.ToInt32(yDepth) - 1) + ",rootid=" + (MaxRootID + 1) + ",Parentstr='" + mParentStr + "' where catalogid=" + pds.Rows[i]["catalogid"].ToString()));
                            }
                        }
                    }
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set child=child-1 where catalogid= " + yParentid));//更新其原来所属分类的分类数，排序相当于剪枝而不需考虑
                }
                //如果是将一个分分类移动到其他分分类下--------------------------------------------------------------------------------------------
                else if (SelectCatalogID != "0" && thisDs.Rows[0]["parentid"].ToString() != "0")//排序复杂
                {
                    yParentStr += ",";
                    int maxOrder = 0;
                    //得到当前分类的下属子分类数
                    int thisChildCount = (int)db.ExecuteScalar(db.GetSqlStringCommond("select count(*) from " + _dataname + "  where parentstr like '%" + yParentStr + catalogid + "%'"));
                    //获得目标分类的相关信息
                    DataTable SelectDS = db.ExecuteDataTable(db.GetSqlStringCommond("select * from " + _dataname + "  where catalogid=" + SelectCatalogID));
                    if (Convert.ToInt32(SelectDS.Rows[0]["child"].ToString()) > 0)//如果目标类有子类的话
                    {
                        int maxBlOrders = (int)db.ExecuteScalar(db.GetSqlStringCommond("select max(orders) from " + _dataname + "  where parentid=" + SelectDS.Rows[0]["catalogid"].ToString()));//取得与本类同级的最大的ORDERS
                        int maxBlAllOrders = (int)db.ExecuteScalar(db.GetSqlStringCommond("select max(orders) from " + _dataname + "  where parentstr like '%" + SelectDS.Rows[0]["parentstr"].ToString() + "," + SelectDS.Rows[0]["catalogid"].ToString() + "%'"));
                        if (maxBlAllOrders > maxBlOrders)
                        {
                            maxOrder = maxBlAllOrders;
                        }
                        else
                        {
                            maxOrder = maxBlOrders;
                        }
                    }
                    else //该分类下没有子类去的目标分类ID
                    {
                        maxOrder = Convert.ToInt32(SelectDS.Rows[0]["orders"].ToString());
                    }
                    //在获得移动过来的分类数后更新排序在指定分类之后的分类排序数据
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set orders=orders+" + thisChildCount + " +1 where rootid=" + SelectDS.Rows[0]["rootid"].ToString() + " and orders > " + maxOrder));
                    //更新当前分类信息
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set parentid=" + SelectCatalogID + ",ParentStr='" + SelectDS.Rows[0]["parentstr"].ToString() + "," + SelectDS.Rows[0]["catalogid"].ToString() + "',Depth=" + (Convert.ToInt32(SelectDS.Rows[0]["Depth"].ToString()) + 1) + ",rootid=" + SelectDS.Rows[0]["rootid"].ToString() + ",orders=" + (maxOrder + 1) + " where catalogid=" + catalogid));
                    //如果当前分类有子类的话更新其子类信息
                    DataTable dqchild = db.ExecuteDataTable(db.GetSqlStringCommond("select * from " + _dataname + "  where parentstr like '%" + yParentStr + catalogid + "%' order by orders"));
                    if (dqchild.Rows.Count > 0)
                    {
                        for (int i = 0; i < dqchild.Rows.Count; i++)
                        {
                            string iParentStr = SelectDS.Rows[0]["parentstr"].ToString() + "," + SelectDS.Rows[0]["catalogid"].ToString() + "," + dqchild.Rows[i]["parentstr"].ToString().Replace(yParentStr, "");
                            string sqlthis = "update " + _dataname + "  set parentstr='" + iParentStr + "',depth=depth-" + Convert.ToInt32(yDepth) + "+" + Convert.ToInt32(SelectDS.Rows[0]["Depth"].ToString()) + "+ 1,orders=" + maxOrder + "+1+" + (i + 1) + ", rootid=" + SelectDS.Rows[0]["rootid"].ToString() + " where catalogid=" + dqchild.Rows[i]["catalogid"].ToString();
                            //Response.Write(sqlthis + "<br/>");
                            db.ExecuteNonQuery(db.GetSqlStringCommond(sqlthis));
                        }
                    }
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set child=child+1 where catalogid=" + SelectCatalogID));//更新其上级子分类数
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set child=child-1 where catalogid=" + yParentid));//更新源父类数
                }//------------------------------------------------------------------------------------------------------------------------------------
                else//如果是一级分类分配到其他子类下
                {
                    //的到分类下所有子类总数
                    int maxOrder = 0;
                    int countClass = (int)db.ExecuteScalar(db.GetSqlStringCommond("select count(*) from " + _dataname + "  where rootid=" + yRootID));
                    //获得目标分类的相关信息
                    DataTable SelectDS = db.ExecuteDataTable(db.GetSqlStringCommond("select * from " + _dataname + "  where catalogid=" + SelectCatalogID));
                    if (Convert.ToInt32(SelectDS.Rows[0]["child"].ToString()) > 0)//如果目标类有子类的话
                    {
                        int maxBlOrders = (int)db.ExecuteScalar(db.GetSqlStringCommond("select max(orders) from " + _dataname + "  where parentid=" + SelectDS.Rows[0]["catalogid"].ToString()));//取得与本类同级的最大的ORDERS
                        int maxBlAllOrders = (int)db.ExecuteScalar(db.GetSqlStringCommond("select max(orders) from " + _dataname + "  where parentstr like '%" + SelectDS.Rows[0]["parentstr"].ToString() + "," + SelectDS.Rows[0]["catalogid"].ToString() + "%'"));
                        if (maxBlAllOrders > maxBlOrders)
                        {
                            maxOrder = maxBlAllOrders;
                        }
                        else
                        {
                            maxOrder = maxBlOrders;
                        }
                    }
                    else //该分类下没有子类去的目标分类ID
                    {
                        maxOrder = Convert.ToInt32(SelectDS.Rows[0]["orders"].ToString());
                    }
                    //在获得移动过来的分类数后更新排序在指定分类之后的分类排序数据
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set orders=orders+" + countClass + " +1 where rootid=" + SelectDS.Rows[0]["rootid"].ToString() + " and orders > " + maxOrder));
                    //更新所有分类信息
                    DataTable myParDs = db.ExecuteDataTable(db.GetSqlStringCommond("select * from " + _dataname + "  where rootid=" + yRootID));
                    for (int i = 0; i < myParDs.Rows.Count; i++)
                    {
                        if (myParDs.Rows[i]["parentid"].ToString() == "0")
                        {
                            string mParentStr = SelectDS.Rows[0]["parentstr"].ToString() + "," + SelectDS.Rows[0]["catalogid"].ToString();
                            db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set parentid=" + SelectCatalogID + ",parentstr='" + mParentStr + "',depth=depth+" + SelectDS.Rows[0]["depth"].ToString() + ",rootid=" + SelectDS.Rows[0]["rootid"].ToString() + ",orders=" + maxOrder + "+" + (i + 1) + " where catalogid=" + myParDs.Rows[i]["catalogid"].ToString()));
                        }
                        else
                        {
                            string mParentStr = SelectDS.Rows[0]["parentstr"].ToString() + "," + SelectDS.Rows[0]["catalogid"].ToString() + "," + myParDs.Rows[i]["Parentstr"].ToString().Replace("0,", "");
                            db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set parentstr='" + mParentStr + "',depth=depth+" + SelectDS.Rows[0]["depth"].ToString() + ",rootid=" + SelectDS.Rows[0]["rootid"].ToString() + ",orders=" + maxOrder + "+" + (i + 1) + " where catalogid=" + myParDs.Rows[i]["catalogid"].ToString()));
                        }
                    }
                    db.ExecuteNonQuery(db.GetSqlStringCommond("update " + _dataname + "  set child=child+1 where catalogid=" + SelectCatalogID));//更新其上级子分类数
                }
            }

            return 1;
        }
    }
  
}
