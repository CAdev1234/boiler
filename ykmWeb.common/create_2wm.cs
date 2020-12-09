using System;
using System.IO;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;

namespace ykmWeb.common
{
    public class create_2wm
    {
        public static string re_2wm(string imgurl, string link)
        {

            if (File.Exists(imgurl) == false)
            {
                System.Drawing.Bitmap image = re_2wm_bitmap(link);
                image.Save(imgurl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return imgurl;
        }


        public static System.Drawing.Bitmap re_2wm_bitmap(string code)
        {

            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 10;
            qrCodeEncoder.QRCodeVersion = 7;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            return qrCodeEncoder.Encode(code);

        }

        public static string GetBase64FromImage(Bitmap imagefile)
        {
            string strbaser64 = "";
            try
            {
                MemoryStream ms = new MemoryStream();
                imagefile.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }
    }
}
