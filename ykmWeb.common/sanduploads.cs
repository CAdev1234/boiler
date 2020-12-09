using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Text;
using System.Security.Cryptography;

namespace ykmWeb.common
{
    public class sanduploads
    {
        public int UpMaxSize { get; set; }
        public int imgW { get; set; }
        public int imgH { get; set; }
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
        public string uploadfiles(HttpPostedFile uploadFile, string Savepath)
        {
            string returnFilename = "";
            if (uploadFile.ContentLength > 0)
            {
                string ftype = uploadFile.ContentType;
                int fizesize = uploadFile.ContentLength;
                string kc = System.IO.Path.GetExtension(uploadFile.FileName);
                string filename = randfilename(4) + kc;
                FileExtension[] fe = { FileExtension.JPG, FileExtension.PNG, FileExtension.GIF, FileExtension.DOCX, FileExtension.DOC, FileExtension._7Z, FileExtension.FLV, FileExtension.PDF, FileExtension.RAR, FileExtension.SWF, FileExtension.XLS, FileExtension.XLSX, FileExtension.ZIP };
                if (IsAllowedExtension(uploadFile, fe))
                {
                    if (kc.ToLower() == ".jpg" || kc.ToLower() == ".gif" || kc.ToLower() == ".png" || kc.ToLower() == ".bmp")
                    {
                        int d = imgW;
                        int h = imgH;
                        Byte[] oFileByte = new byte[uploadFile.ContentLength];
                        Stream oStream = uploadFile.InputStream;
                        Image oImage = Image.FromStream(oStream);
                        int oWidth = oImage.Width;
                        //原图宽度 
                        int oHeight = oImage.Height;
                        //原图高度
                        if (oWidth <= d && oHeight <= h)
                        {
                            uploadFile.SaveAs(HttpContext.Current.Server.MapPath(Savepath + filename));
                            returnFilename = ("ok|" + Savepath + filename);
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
                                        tImage.Save(HttpContext.Current.Server.MapPath(Savepath + filename), System.Drawing.Imaging.ImageFormat.Png);
                                        break;
                                    case ".gif":
                                        tImage.Save(HttpContext.Current.Server.MapPath(Savepath + filename), System.Drawing.Imaging.ImageFormat.Gif);
                                        break;
                                    default:
                                        tImage.Save(HttpContext.Current.Server.MapPath(Savepath + filename), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        break;
                                }
                                returnFilename = ("ok|" + Savepath + filename);
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
                        uploadFile.SaveAs(HttpContext.Current.Server.MapPath(Savepath + filename));
                    }

                    common.read_text(HttpContext.Current.Server.MapPath(Savepath + filename));
                }
                else
                {
                    returnFilename = ("error|类型错误");
                }
            }
            else
            {
                returnFilename = ("error|请选择上传文件");
            }

            return returnFilename;
        }
        public string uploadfiles_mvc(HttpPostedFileBase uploadFile, string Savepath)
        {
            string returnFilename = "";
            if (uploadFile.ContentLength > 0)
            {
                string ftype = uploadFile.ContentType;
                int fizesize = uploadFile.ContentLength;
                string kc = System.IO.Path.GetExtension(uploadFile.FileName);
                string filename = randfilename(4) + kc;
                //string filename = "";
                //if (File.Exists(HttpContext.Current.Server.MapPath(Savepath +uploadFile.FileName))){
                //    filename = uploadFile.FileName.Replace(kc, "") + "_附件" + kc;
                //}
                //else {
                //    filename = uploadFile.FileName;
                //}
                   FileExtension[] fe = { FileExtension.JPG, FileExtension.PNG, FileExtension.GIF, FileExtension.DOCX, FileExtension.DOC, FileExtension._7Z, FileExtension.FLV, FileExtension.PDF, FileExtension.RAR, FileExtension.SWF, FileExtension.XLS, FileExtension.XLSX, FileExtension.ZIP };
                if (IsAllowedExtension_mvc(uploadFile, fe))
                {
                    if (kc.ToLower() == ".jpg" || kc.ToLower() == ".gif" || kc.ToLower() == ".png" || kc.ToLower() == ".bmp")
                    {
                        int d = imgW;
                        int h = imgH;
                        Byte[] oFileByte = new byte[uploadFile.ContentLength];
                        Stream oStream = uploadFile.InputStream;
                        Image oImage = Image.FromStream(oStream);
                        int oWidth = oImage.Width;
                        //原图宽度 
                        int oHeight = oImage.Height;
                        //原图高度
                        if (oWidth <= d && oHeight <= h)
                        {
                            uploadFile.SaveAs(HttpContext.Current.Server.MapPath(Savepath + filename));
                            returnFilename = ("ok|" + Savepath + filename);
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
                                        tImage.Save(HttpContext.Current.Server.MapPath(Savepath + filename), System.Drawing.Imaging.ImageFormat.Png);
                                        break;
                                    case ".gif":
                                        tImage.Save(HttpContext.Current.Server.MapPath(Savepath + filename), System.Drawing.Imaging.ImageFormat.Gif);
                                        break;
                                    default:
                                        tImage.Save(HttpContext.Current.Server.MapPath(Savepath + filename), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        break;
                                }
                                returnFilename = ("ok|" + Savepath + filename);
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
                        uploadFile.SaveAs(HttpContext.Current.Server.MapPath(Savepath + filename));
                        returnFilename = ("ok|" + Savepath + filename);
                    }
                    common.read_text(HttpContext.Current.Server.MapPath(Savepath + filename));
                }
                else
                {
                    returnFilename = ("error|类型错误");
                }
            }
            else
            {
                returnFilename = ("error|请选择上传文件");
            }

            return returnFilename;
        }
        private bool IsAllowedExtension(HttpPostedFile fu, FileExtension[] fileEx)
        {
            int fileLen = fu.ContentLength;
            byte[] imgArray = new byte[fileLen];
            fu.InputStream.Read(imgArray, 0, fileLen);
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

            ms.Dispose();
            foreach (FileExtension fe in fileEx)
            {
                if (Int32.Parse(fileclass) == (int)fe)
                    return true;
            }
            return false;
        }

        private bool IsAllowedExtension_mvc(HttpPostedFileBase fu, FileExtension[] fileEx)
        {
            int fileLen = fu.ContentLength;
            byte[] imgArray = new byte[fileLen];
            fu.InputStream.Read(imgArray, 0, fileLen);
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

            ms.Dispose();
            foreach (FileExtension fe in fileEx)
            {
                if (Int32.Parse(fileclass) == (int)fe)
                    return true;
            }
            return false;
        }


        private string randfilename(int length)
        {
            byte[] random = new Byte[length / 2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(random);
            string timestr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            StringBuilder sb = new StringBuilder(length);
            int i;
            for (i = 0; i < random.Length; i++)
            {
                sb.Append(String.Format("{0:X2}", random[i]));
            }
            sb.Append(timestr);
            return sb.ToString();
        }


    }
}
