
namespace ykmWeb.Models
{
    public class menuClass : nclass
    {
        public string tabletype { get; set; }
        public string pagename { get; set; }
        public string i_pagename { get; set; }
        public int? mainnavshow { get; set; }
        public int? leftnavshow { get; set; }
        public string sitename { get; set; }
        public string keyword { get; set; }
        public string keycont { get; set; }
        public string pclisttype { get; set; }
        public string mblisttype { get; set; }
        public string defaultpic { get; set; }
        public int? linktype { get; set; }//0 自己，1 子类，2 跳转
        public string linkurl { get; set; }//0为空 ，linktype 为2时为url地址，linktype 为1是为子类ID
        public int? linkCid { get; set; }//子类链接id
        public string subtitle { get; set; } //副标题
        public string language { get; set; } 
        public int? h5linktype { get; set; }//0 自己，1 子类，2 跳转
        public string h5linkurl { get; set; }//0为空 ，linktype 为2时为url地址，linktype 为1是为子类ID
        public int? h5linkCid { get; set; }//子类链接id
        public string downloadfiles { get; set; }//附件
    }

    public class viewMenuClass
    {
        public int? Catalogid { get; set; }
        public string Catalogname { get; set; }
        public string Caenname { get; set; }
        public string defaultpic { get; set; }
        public string fbt { get; set; }
        public string Link { get; set; }
    }

}
