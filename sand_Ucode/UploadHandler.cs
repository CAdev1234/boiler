using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Web.Mvc;
using System.Configuration;
using System.Text;

namespace sand_Ucode
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : Handler
    {

        public UploadConfig UploadConfig { get; private set; }
        public UploadResult Result { get; private set; }

        public UploadHandler(Controller context, UploadConfig config)
            : base(context)
        {
            this.UploadConfig = config;
            this.Result = new UploadResult() { State = UploadState.Unknown };
        }

        public override void Process()
        {
            byte[] uploadFileBytes = null;
            string uploadFileName = null;
           FileValidation.FileExtension[] fe = { FileValidation.FileExtension.GIF, FileValidation.FileExtension.JPG, FileValidation.FileExtension.PNG, FileValidation.FileExtension.RAR, FileValidation.FileExtension.ZIP, FileValidation.FileExtension._7Z, FileValidation.FileExtension.DOC, FileValidation.FileExtension.DOCX, FileValidation.FileExtension.XLS, FileValidation.FileExtension.XLSX };

            if (UploadConfig.Base64)
            {
                uploadFileName = UploadConfig.Base64Filename;
                uploadFileBytes = Convert.FromBase64String(Request[UploadConfig.UploadFieldName]);
            }
            else
            {
                var file = Request.Files[UploadConfig.UploadFieldName];

                uploadFileName = file.FileName;
     
                if (!CheckFileType(uploadFileName))
                {
                    Result.State = UploadState.TypeNotAllow;
                    WriteResult();
                    return;
                }

                if (!CheckFileSize(file.ContentLength))
                {
                    Result.State = UploadState.SizeLimitExceed;
                    WriteResult();
                    return;
                }

                uploadFileBytes = new byte[file.ContentLength];
                try
                {
                    file.InputStream.Read(uploadFileBytes, 0, file.ContentLength);
                }
                catch (Exception)
                {
                    Result.State = UploadState.NetworkError;
                    WriteResult();
                }
            }
            Result.OriginFileName = uploadFileName;
            string kc = System.IO.Path.GetExtension(uploadFileName);//取得扩展名
            var savePath = PathFormatter.Format(uploadFileName, UploadConfig.PathFormat);
            var localPath = Server.MapPath(savePath);
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                }
                if (!FileValidation.IsAllowedExtension(uploadFileBytes, fe))
                {
                    Result.State = UploadState.TypeNotAllow;
                    WriteResult();
                    return;
                }
                else
                {
                    if (kc.ToLower() == ".jpg" || kc.ToLower() == ".gif" || kc.ToLower() == ".png" || kc.ToLower() == ".bmp")
                    {
                        
                        int d =int.Parse(ConfigurationManager.AppSettings["uploadImageWidth"]);
                        int h = int.Parse(ConfigurationManager.AppSettings["uploadImageHeight"]);

                        MemoryStream ms = new MemoryStream(uploadFileBytes);
                        Image oImage = System.Drawing.Image.FromStream(ms);
                        int oWidth = oImage.Width;
                        //原图宽度 
                        int oHeight = oImage.Height;
                        //原图高度
                        if (oWidth <= d && oHeight <= h)
                        {
                            File.WriteAllBytes(localPath, uploadFileBytes);
                            Result.Url = savePath;
                            Result.State = UploadState.Success;
                        }
                        else
                        {
                            int tWidth = d;
                            //设置缩略图初始宽度 
                            int tHeight = h;
                            //设置缩略图初始高度 
                            //按比例计算出缩略图的宽度和高度 
                            if (oWidth >= oHeight)
                            {
                                tHeight = (int)Math.Floor(Convert.ToDouble(oHeight) * (Convert.ToDouble(tWidth) / Convert.ToDouble(oWidth)));
                            }
                            else
                            {
                                tWidth = (int)Math.Floor(Convert.ToDouble(oWidth) * (Convert.ToDouble(tHeight) / Convert.ToDouble(oHeight)));
                            }
                            //生成缩略原图 
                            Bitmap tImage = new Bitmap(tWidth, tHeight);
                            Graphics g = Graphics.FromImage(tImage);
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                            //设置高质量插值法 
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            //设置高质量,低速度呈现平滑程度 
                            g.Clear(Color.Transparent);
                            //清空画布并以透明背景色填充 
                            g.DrawImage(oImage, new Rectangle(0, 0, tWidth, tHeight), new Rectangle(0, 0, oWidth, oHeight), GraphicsUnit.Pixel);
                            //保存缩略图的物理路径 
                            try
                            {
                                //以JPG格式保存图片 
                                switch (kc.ToLower())
                                {
                                    case ".png":
                                        tImage.Save(localPath, System.Drawing.Imaging.ImageFormat.Png);
                                        Result.Url = savePath;
                                        Result.State = UploadState.Success;
                                        break;
                                    case ".gif":
                                        tImage.Save(localPath, System.Drawing.Imaging.ImageFormat.Gif);
                                        Result.Url = savePath;
                                        Result.State = UploadState.Success;
                                        break;
                                    default:
                                        tImage.Save(localPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        Result.Url = savePath;
                                        Result.State = UploadState.Success;
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                //释放资源 
                                oImage.Dispose();
                                g.Dispose();
                                tImage.Dispose();
                            }
                        }
                    }
                    else
                    {
                        File.WriteAllBytes(localPath, uploadFileBytes);
                        Result.Url = savePath;
                        Result.State = UploadState.Success;
                    }

                    command_checkfiles.read_text(localPath);
                }
            }
            catch (Exception e)
            {
                Result.State = UploadState.FileAccessError;
                Result.ErrorMessage = e.Message;
            }
            finally
            {
                WriteResult();
            }
        }

        private void WriteResult()
        {
            string domain = ConfigurationManager.AppSettings["domain"];//ykm-给路径添加个域名
            this.WriteJson(new
            {
                state = GetStateMessage(Result.State),
                url = domain + Result.Url,
                title = Result.OriginFileName,
                original = Result.OriginFileName,
                error = Result.ErrorMessage
            });
        }

        private string GetStateMessage(UploadState state)
        {
            switch (state)
            {
                case UploadState.Success:
                    return "SUCCESS";
                case UploadState.FileAccessError:
                    return "文件访问出错，请检查写入权限";
                case UploadState.SizeLimitExceed:
                    return "文件大小超出服务器限制";
                case UploadState.TypeNotAllow:
                    return "不允许的文件格式";
                case UploadState.NetworkError:
                    return "网络错误";
            }
            return "未知错误";
        }

        private bool CheckFileType(string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            return UploadConfig.AllowExtensions.Select(x => x.ToLower()).Contains(fileExtension);
        }

        private bool CheckFileSize(int size)
        {
            return size < UploadConfig.SizeLimit;
        }
    }

    public class UploadConfig
    {
        /// <summary>
        /// 文件命名规则
        /// </summary>
        public string PathFormat { get; set; }

        /// <summary>
        /// 上传表单域名称
        /// </summary>
        public string UploadFieldName { get; set; }

        /// <summary>
        /// 上传大小限制
        /// </summary>
        public int SizeLimit { get; set; }

        /// <summary>
        /// 上传允许的文件格式
        /// </summary>
        public string[] AllowExtensions { get; set; }

        /// <summary>
        /// 文件是否以 Base64 的形式上传
        /// </summary>
        public bool Base64 { get; set; }

        /// <summary>
        /// Base64 字符串所表示的文件名
        /// </summary>
        public string Base64Filename { get; set; }
    }

    public class UploadResult
    {
        public UploadState State { get; set; }
        public string Url { get; set; }
        public string OriginFileName { get; set; }

        public string ErrorMessage { get; set; }
    }

    public enum UploadState
    {
        Success = 0,
        SizeLimitExceed = -1,
        TypeNotAllow = -2,
        FileAccessError = -3,
        NetworkError = -4,
        Unknown = 1,
    }

    public class FileValidation
    {
        public enum FileExtension
        {
            JPG = 255216,
            GIF = 7173,
            PNG = 13780,
            SWF = 6787,
            RAR = 8297,
            ZIP = 8075,
            _7Z = 55122,
            XLS = 208,
            DOC = 208207,
            DOCX = 8075,
            XLSX = 8075,
            FLV = 7076,
            PDF = 3780



            // 6787 swf   

            // 7790 exe dll,   

            // 8297 rar   

            // 8075 zip   

            // 55122 7z   

            // 6063 xml   

            // 6033 html   

            // 239187 aspx   

            // 117115 cs   

            // 119105 js   

            // 102100 txt   

            // 255254 sql    

        }

        public static bool IsAllowedExtension(byte[] imgArray, FileExtension[] fileEx)
        {
        
            MemoryStream ms = new MemoryStream(imgArray);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileclass = buffer.ToString();
                buffer = br.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {

            }
            br.Close();
            ms.Close();
            ms.Dispose();
            foreach (FileExtension fe in fileEx)
            {
                if (Int32.Parse(fileclass) == (int)fe)
                    return true;
            }
            return false;
        }
    }


    public class command_checkfiles
    {
        /// <summary>
        /// 判断字符窜是否有非法信息
        /// </summary>
        /// <param name="requestStr">字符串</param>
        /// <returns></returns>
        private static bool check_uploadfiles(string requestStr)
        {
            bool chkths = true;
            if (requestStr == "" || requestStr == null)
            {

            }
            else
            {
                string Sql_1 = "using|gif89au|webhandler|language|c#|handler|system|public|class|void|httpcontext|string";
                string[] sql_c = Sql_1.Split('|');
                foreach (string sl in sql_c)
                {
                    if (requestStr.ToLower().IndexOf(sl.Trim()) >= 0)
                    {
                        chkths = false;
                        break;
                    }
                }
            }
            return chkths;
        }

        public static void read_text(string pathFiles)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            bool isresult = true;
            if (File.Exists(pathFiles) == true)
            {
                StreamReader r = new StreamReader(pathFiles, true);

                r.BaseStream.Seek(0, SeekOrigin.Begin);
                while (r.Peek() > -1)
                {
                    if (check_uploadfiles(r.ReadLine()) == false)
                    {
                        isresult = false;
                        break;
                    }
                    i++;
                    if (i >= 10)
                    {
                        break;
                    }
                }
                r.Close();
                r.Dispose();
            }

            if (i == 0)
            {
                isresult = false;
            }
            if (isresult == false)
            {
                File.Delete(pathFiles);
            }
        }
    }
}

