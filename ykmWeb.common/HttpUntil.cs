using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace ykmWeb.common
{
    /// <summary>
    /// HttpUntil 的摘要说明
    /// </summary>
    public class HttpUntil
    {
        /// <summary>
        /// 向固定地址发送post请求
        /// </summary>
        /// <param name="data">String 行数据</param>
        /// <param name="url">要发送的URL地址</param>
        /// <returns>返回url地址服务器反馈结果</returns>
        public static string Send(string data, string url)
        {
            byte[] _data = Encoding.GetEncoding("UTF-8").GetBytes(data);
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(CheckValidationResult);
            }


            Stream responseStream;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = _data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(_data, 0, _data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            return str;
        }

        /// <summary>
        /// 带证书发送url
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <param name="url">发送地址</param>
        /// <param name="szurl">证书绝对路径</param>
        /// <param name="_password">证书密码</param>
        /// <returns></returns>
        public static string Send(string data, string url, string szurl, string _password)
        {

            byte[] _data = Encoding.GetEncoding("UTF-8").GetBytes(data);
            string cert = szurl;
            string password = _password;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(CheckValidationResult);
            }


            X509Certificate2 cer = new System.Security.Cryptography.X509Certificates.X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            Stream responseStream;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ClientCertificates.Add(cer);

            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = _data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(_data, 0, _data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            return str;
        }

        public static string send_httpget(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

    }
}
