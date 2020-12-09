using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace ykmWeb.common
{
    public class sandBigFilesUpload
    {
        private  HttpContextBase contextBase;
        private string UploadFiles = "";
        public sandBigFilesUpload(HttpContextBase _contextBase,string _uploadFiles)
        {
            contextBase = _contextBase;
            UploadFiles = _uploadFiles;
        }
      
        // GET: management/FileUpload
        #region 获取指定文件的已上传的文件块
        /// <summary>
        /// 获取指定文件的已上传的文件块
        /// </summary>
        /// <returns></returns>
        public string GetMaxChunk()
        {
            string root = contextBase.Server.MapPath(UploadFiles);
            try
            {
                var md5 = Convert.ToString(contextBase.Request["md5"]);
                var ext = Convert.ToString(contextBase.Request["ext"]);
                int chunk = 0;

                var fileName = md5 + "." + ext;

                FileInfo file = new FileInfo(root + fileName);
                if (file.Exists)
                {
                    chunk = Int32.MaxValue;
                }
                else
                {
                    if (Directory.Exists(root + "chunk\\" + md5))
                    {
                        DirectoryInfo dicInfo = new DirectoryInfo(root + "chunk\\" + md5);
                        var files = dicInfo.GetFiles();
                        chunk = files.Count();
                        if (chunk > 1)
                        {
                            chunk = chunk - 1; //当文件上传中时，页面刷新，上传中断，这时最后一个保存的块的大小可能会有异常，所以这里直接删除最后一个块文件
                        }
                    }
                }

                return CommonResult.ToJsonStr(0, string.Empty, chunk);
            }
            catch
            {
                return CommonResult.ToJsonStr(0, string.Empty, 0);
            }
        }
        #endregion

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public string Upload(HttpPostedFileBase file)
        {
            string root = contextBase.Server.MapPath(UploadFiles);
            string md5_key = string.Format("{0}md5", contextBase.Request["id"]);
            string md5_val = contextBase.Request[md5_key];
            //如果进行了分片
            if (contextBase.Request.Form.AllKeys.Any(m => m == "chunk"))
            {
                //取得chunk和chunks
                int chunk = Convert.ToInt32(contextBase.Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                int chunks = Convert.ToInt32(contextBase.Request.Form["chunks"]);//总分片数
                //根据GUID创建用该GUID命名的临时文件夹
                //string folder = Server.MapPath("~/UploadFiles/" + Request["md5"] + "/");
                string folder = root + "chunk\\" + md5_val + "\\";
                string path = folder + chunk;

                //建立临时传输文件夹
                if (!Directory.Exists(Path.GetDirectoryName(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                FileStream addFile = null;
                BinaryWriter AddWriter = null;
                Stream stream = null;
                BinaryReader TempReader = null;

                try
                {
                    //addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                    addFile = new FileStream(path, FileMode.Create, FileAccess.Write);
                    AddWriter = new BinaryWriter(addFile);
                    //获得上传的分片数据流
                    stream = file.InputStream;
                    TempReader = new BinaryReader(stream);
                    //将上传的分片追加到临时文件末尾
                    AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                }
                finally
                {
                    if (addFile != null)
                    {
                        addFile.Close();
                        addFile.Dispose();
                    }
                    if (AddWriter != null)
                    {
                        AddWriter.Close();
                        AddWriter.Dispose();
                    }
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                    if (TempReader != null)
                    {
                        TempReader.Close();
                        TempReader.Dispose();
                    }
                }

                //context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"f_ext\" : \"" + Path.GetExtension(file.FileName) + "\"}");
                return CommonResult.ToJsonStr(0, string.Empty, "{\"chunked\" : true, \"ext\" : \"" + Path.GetExtension(file.FileName) + "\"}");
            }
            else//没有分片直接保存
            {
                string filename= md5_val + Path.GetExtension(contextBase.Request.Files[0].FileName);
                string path = root + filename;
                //Request.Files[0].SaveAs(path);
                file.SaveAs(path);
                //context.Response.Write("{\"chunked\" : false, \"hasError\" : false}");
                return CommonResult.ToJsonStr(0, string.Empty, "{\"chunked\" : false,\"data\":\""+ filename+"\"}");
            }
        }
        #endregion

        #region 合并文件
        /// <summary>
        /// 合并文件
        /// </summary>
        /// <returns></returns>
        public string MergeFiles()
        {
            string root = contextBase.Server.MapPath(UploadFiles);

            string guid = contextBase.Request["md5"];
            string ext = contextBase.Request["ext"];
            string filename = guid + ext;
            string sourcePath = Path.Combine(root, "chunk\\" + guid + "\\");//源数据文件夹
            string targetPath = Path.Combine(root, guid + ext);//合并后的文件

            DirectoryInfo dicInfo = new DirectoryInfo(sourcePath);
            if (Directory.Exists(Path.GetDirectoryName(sourcePath)))
            {
                FileInfo[] files = dicInfo.GetFiles();
                foreach (FileInfo file in files.OrderBy(f => int.Parse(f.Name)))
                {
                    FileStream addFile = new FileStream(targetPath, FileMode.Append, FileAccess.Write);
                    BinaryWriter AddWriter = new BinaryWriter(addFile);

                    //获得上传的分片数据流 
                    Stream stream = file.Open(FileMode.Open);
                    BinaryReader TempReader = new BinaryReader(stream);
                    //将上传的分片追加到临时文件末尾
                    AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                    //关闭BinaryReader文件阅读器
                    TempReader.Close();
                    stream.Close();
                    AddWriter.Close();
                    addFile.Close();

                    TempReader.Dispose();
                    stream.Dispose();
                    AddWriter.Dispose();
                    addFile.Dispose();
                }
                DeleteFolder(sourcePath);
                //context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"savePath\" :\"" + System.Web.HttpUtility.UrlEncode(targetPath) + "\"}");
                return CommonResult.ToJsonStr(0, string.Empty, "{\"chunked\" : true, \"hasError\" : false, \"savePath\" :\"\",\"data\":\""+ filename + "\"}");
            }
            else
            {
                //context.Response.Write("{\"hasError\" : true}");
                return CommonResult.ToJsonStr(0, string.Empty, "{\"hasError\" : true}");
            }
        }
        #endregion

        #region 删除文件夹及其内容
        /// <summary>
        /// 删除文件夹及其内容
        /// </summary>
        /// <param name="dir"></param>
        private void DeleteFolder(string strPath)
        {
            //删除这个目录下的所有子目录
            if (Directory.GetDirectories(strPath).Length > 0)
            {
                foreach (string fl in Directory.GetDirectories(strPath))
                {
                    Directory.Delete(fl, true);
                }
            }
            //删除这个目录下的所有文件
            if (Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string f in Directory.GetFiles(strPath))
                {
                    System.IO.File.Delete(f);
                }
            }
            Directory.Delete(strPath, true);
        }
        #endregion
    }

    public class CommonResult
    {
        static JavaScriptSerializer jss = new JavaScriptSerializer();

        /// <summary>
        /// 操作成功
        /// </summary>
        public CommonResult()
        {
            this.code = 0;
            this.errmsg = string.Empty;
            this.data = string.Empty;
        }
        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        public CommonResult(string errMsg)
        {
            this.code = -1;
            this.errmsg = errMsg;
            this.data = string.Empty;
        }
        public CommonResult(int code, string errMsg, string data)
        {
            this.code = code;
            this.errmsg = errMsg;
            this.data = data;
        }
        public CommonResult(int code, string errMsg, object data)
        {
            this.code = code;
            this.errmsg = errMsg;
            this.data = data;
        }
        /// <summary>
        /// 操作成功(返回json字符串)
        /// </summary>
        /// <returns></returns>
        public static string ToJsonStr()
        {
            var instance = new CommonResult();
            return jss.Serialize(instance);
        }
        /// <summary>
        /// 操作失败(返回json字符串)
        /// </summary>
        /// <returns></returns>
        public static string ToJsonStr(string errMsg)
        {
            var instance = new CommonResult(errMsg);
            return jss.Serialize(instance);
        }
        /// <summary>
        /// 操作结果(返回json字符串)
        /// </summary>
        /// <returns></returns>
        public static string ToJsonStr(int code, string errMsg, string data)
        {
            var instance = new CommonResult(code, errMsg, data);
            return jss.Serialize(instance);
        }
        /// <summary>
        /// 操作结果(返回json字符串)
        /// </summary>
        /// <returns></returns>
        public static string ToJsonStr(int code, string errMsg, object data)
        {
            var instance = new CommonResult(code, errMsg, data);
            return jss.Serialize(instance);
        }
        /// <summary>
        /// 操作成功(返回CommonResult实例)
        /// </summary>
        /// <returns></returns>
        public static CommonResult Instance()
        {
            return Instance(0, string.Empty, string.Empty);
        }
        /// <summary>
        /// 操作失败(返回CommonResult实例)
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static CommonResult Instance(string errMsg = null)
        {
            return Instance(-1, errMsg, string.Empty);
        }
        /// <summary>
        /// 操作结果(返回CommonResult实例)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errMsg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static CommonResult Instance(int code = 0, string errMsg = null, string data = null)
        {
            return new CommonResult(code, errMsg, data);
        }
        public int code { get; set; }
        public string errmsg { get; set; }
        public object data { get; set; }
    }
}
