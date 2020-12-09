using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text;
namespace ykmWeb.common
{
    /// <summary>
    ///fso 的摘要说明
    /// </summary>
    public class fso
    {
        public fso()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 创建磁盘文件
        /// </summary>
        /// <param name="pathFiles">路径及文件名</param>
        /// <param name="cont">文件内容</param>
        public void WriteHtmlTo(string pathFiles, string cont)
        {
            if (File.Exists(pathFiles) == true)
            {
                File.Delete(pathFiles);
            }
            StreamWriter fs = new StreamWriter(pathFiles, true, System.Text.Encoding.UTF8);
            fs.WriteLine(cont);
            fs.Flush();
            fs.Close();
            fs.Dispose();
        }
        public string ResponseHTML(string pathFiles)
        {
            StringBuilder sb = new StringBuilder();
            if (File.Exists(pathFiles) == true)
            {
                StreamReader r = new StreamReader(pathFiles, true);

                r.BaseStream.Seek(0, SeekOrigin.Begin);
                while (r.Peek() > -1)
                {
                    sb.Append(r.ReadLine());
                }
                r.Close();
                r.Dispose();
            }
            else
            {
                sb.Append("reader Error!");
            }
            return sb.ToString();
        }
        public void CreatForld(string fordname)
        {
            DirectoryInfo d = Directory.CreateDirectory(fordname);
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir">文件夹名称</param>
        public void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件 
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir); //删除已空文件夹 
            }
        }
    }
}
