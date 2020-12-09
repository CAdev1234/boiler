using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ykmWeb.common
{
    public  class create_img
    {
        /// <summary>
        /// 生成文字图片
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isBold"></param>
        /// <param name="fontSize"></param>
        public Image CreateTextImage(string text, bool isBold, int fontSize,Color fontcolor)
        {
            int wid = 400;
            int high = 200;
            Font font;
            if (isBold)
            {
                font = new Font("Microsoft YaHei", fontSize, FontStyle.Bold);
            }
            else
            {
                font = new Font("Microsoft YaHei", fontSize, FontStyle.Regular);
            }
          
            //绘笔颜色
            SolidBrush brush = new SolidBrush(fontcolor);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            Bitmap image = new Bitmap(wid, high);
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);//得到文本的宽高
            int width = (int)(sizef.Width + 1);
            int height = (int)(sizef.Height + 1);
            image.Dispose();
            image = new Bitmap(width, height);
            g = Graphics.FromImage(image);
            g.Clear(Color.White);//透明

            RectangleF rect = new RectangleF(0, 0, width, height);
            //绘制图片
            g.DrawString(text, font, brush, rect);
            //释放对象
            g.Dispose();
            return image;
        }

        /// <summary>  
        /// 合并图片  
        /// </summary>  
        /// <param name="imgBack"></param>  
        /// <param name="img"></param>  
        /// <returns></returns>  
        public  Bitmap CombinImage(Image imgBack, Image img, int xDeviation = 0, int yDeviation = 0)
        {

            Bitmap bmp = new Bitmap(imgBack.Width, imgBack.Height + img.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2 + xDeviation, imgBack.Height + yDeviation, img.Width, img.Height);
            GC.Collect();
            return bmp;
        }


        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        public Image ResizeImage(Image bmp, int newW, int newH)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }


        public string getProductImg(string title, string price, string imgUrl, Image ermUrl)
        {
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(imgUrl)))
            {
                Image mainimg = null; //主图
                mainimg = Image.FromFile(HttpContext.Current.Server.MapPath(imgUrl));
                mainimg = ResizeImage(mainimg, 575, 431); //改变图片大小
                Image imgtextTitle = CreateTextImage(createText(title), false, 18, Color.FromArgb(60, 60, 60));
                Image ewmText = CreateTextImage("使用微信扫一扫", false, 14, Color.FromArgb(180, 180, 180));
                Image dlf = CreateTextImage("￥", false, 18, Color.FromArgb(255, 54, 0));
                Image imgtextPrice = CreateTextImage(price, false, 24, Color.FromArgb(255, 54, 0));
                Bitmap tImage = new Bitmap(575, 686);
                Graphics g = Graphics.FromImage(tImage);
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawImage(mainimg, 0, 0, mainimg.Width, mainimg.Height);
                g.DrawImage(imgtextTitle, 20, mainimg.Height + 58, imgtextTitle.Width, imgtextTitle.Height);            //绘制标题
                g.DrawImage(ermUrl, 365, mainimg.Height + 10, 180, 180); //绘制二维码

                g.DrawImage(ewmText, 385, (mainimg.Height+180+ ewmText.Height), ewmText.Width, ewmText.Height); //绘制二维码文字

                g.DrawImage(dlf, 20, (mainimg.Height + 22 + 58 + imgtextTitle.Height + 8), dlf.Width, dlf.Height); //绘制人民币符号
                g.DrawImage(imgtextPrice, 20 + dlf.Width, (mainimg.Height + 22 + 58 + imgtextTitle.Height), imgtextPrice.Width, imgtextPrice.Height); //绘制钱额
                string flename = "/uploads/" +common.reRand_abc(10)+".jpg";
                tImage.Save(HttpContext.Current.Server.MapPath( flename), System.Drawing.Imaging.ImageFormat.Jpeg);

                g.Dispose();
                mainimg.Dispose();
                ermUrl.Dispose();
                imgtextTitle.Dispose();
                imgtextPrice.Dispose();
                dlf.Dispose();
                tImage.Dispose();
                return flename;
            }
            else
            {
                return "";
            }
        }

        private string createText(string text)
        {
            string str = "";
            if (text.Length > 12)
            {
                int i = 1;
                foreach (var o in text)
                {
                    if (i <= 12)
                    {
                        str = str + o;
                    }
                    else
                    {
                        str = str + "\r\n";
                        i = 1;
                    }
                    i++;
                }
            }
            else
            {
                str = text;
            }
            return str;
        }




        /// <summary>
        /// 将Base64位码保存成图片
        /// </summary>
        /// <param name="UserPhoto">Base64位码</param>
        /// <returns></returns>
        public string UploadImageByBase64String(string UserPhoto)
        {
            string result = "/web_images/defaultheadimg.jpg";
            if (!string.IsNullOrEmpty(UserPhoto))
            {
                if (UserPhoto.IndexOf("base64") >= 0)
                {
                    //图片路径
                    string filePath = HttpContext.Current.Server.MapPath("~/" + @System.Configuration.ConfigurationManager.AppSettings["ImagePath"]);
                    try
                    {
                        var head = UserPhoto.IndexOf("4") + 2;
                        var imgBase64Data = UserPhoto.Substring(head, UserPhoto.Length - head);

                        byte[] bt = Convert.FromBase64String(imgBase64Data);//获取图片base64
                        string suffix = common.MidStrEx_New(UserPhoto, "data:image/", ";base64,");
                        string fileFolder = DateTime.Now.ToString("yyyyMMdd");//年月日
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + suffix;
                        string ImageFilePath = "/uploads/Gallery/" + fileFolder;
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(ImageFilePath)) == false)//如果不存在就创建文件夹
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ImageFilePath));
                        }
                        string ImagePath = HttpContext.Current.Server.MapPath(ImageFilePath) + "/" + fileName;//定义图片名称

                        File.WriteAllBytes(ImagePath, bt); //保存图片到服务器，然后获取路径
                        System.Threading.Thread.Sleep(1000);

                        result = ImageFilePath + "/" + fileName;//获取保存后的路径
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    result = UserPhoto;
                }
            }
            return result;
        }


    }
}
