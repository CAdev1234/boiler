using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ykmWeb.common;

namespace ykmWeb.Areas.management.Controllers
{
    public class FileUploadController : Controller
    {
        
        /// <summary>
        /// 文件存放路径
        /// </summary>
        const string UploadFiles = "/uploads/video/";


        [webAuthorzize]
        public ActionResult index()
        {
            return View();
        }


        // GET: management/FileUpload
        #region 获取指定文件的已上传的文件块
        /// <summary>
        /// 获取指定文件的已上传的文件块
        /// </summary>
        /// <returns></returns>
        public string GetMaxChunk()
        {
            //  sandBigFilesUpload sbf = new sandBigFilesUpload(HttpContext, UploadFiles);
            // return sbf.GetMaxChunk();
            return "";
        }
        #endregion

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        [webAuthorzize]
        public string Upload(HttpPostedFileBase file)
        {
            //  sandBigFilesUpload sbf = new sandBigFilesUpload(HttpContext, UploadFiles);
            //  return sbf.Upload(file);
            return "";
        }
        #endregion

        #region 合并文件
        /// <summary>
        /// 合并文件
        /// </summary>
        /// <returns></returns>
        public string MergeFiles()
        {
            //   sandBigFilesUpload sbf = new sandBigFilesUpload(HttpContext, UploadFiles);
            //return sbf.MergeFiles();
            return "";
        }
        #endregion

        #region 更新用户上传记录
        /// <summary>
        /// 更新用户上传记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string AddUploadRecord()
        {
            //  return CommonResult.ToJsonStr();
            return "";
        }
        #endregion
    }
}