using Evious.Core.Config;
using Evious.Core.Upload;
using Evious.Account.Contract;
using Evious.Ims.Contract.Model;
using Evious.Framework.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using Evious.Ims.BLL;

namespace Evious.Web.AdminApplication
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        #region 设定
       

        public virtual string FileInputName
        {
            get { return "filedata"; }
        }

        public string UploadPath
        {
            get { return UploadConfigContext.UploadPath; }
        }

        public int MaxFilesize
        {
            //10M 
            get { return 10971520; }
        }

        public virtual string[] AllowExt
        {
            get { return new string[] { "txt", "rar", "zip", "jpg", "jpeg", "gif", "png", "swf" }; }
        }

        public virtual string[] ImageExt
        {
            get { return new string[] { "jpg", "jpeg", "gif", "png" }; }
        }


        public  void OnUploaded(HttpContext context, string filePath)
        {
            var ext = filePath.Substring(filePath.LastIndexOf('.') + 1).ToLower();
            if (!ImageExt.Contains(ext))
                return;

            if (string.IsNullOrEmpty(context.Request["thumbs"]))
            {
                this.MakeThumbnail(filePath, "s", context.Request["thumbwidth"].ToInt(85), context.Request["thumbheight"].ToInt(85), string.IsNullOrEmpty(context.Request["mode"]) ? "H" : context.Request["mode"]);
            }
            else
            {
                var thumbs = context.Request["thumbs"];
                foreach (var thumb in thumbs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var thumbparts = thumb.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    this.MakeThumbnail(filePath, thumbparts[0], thumbparts[1].ToInt(), thumbparts[2].ToInt(), thumbparts[3]);
                }
            }
        }

        private void MakeThumbnail(string filePath, string suffix, int width, int height, string mode)
        {
            string fileExt = filePath.Substring(filePath.LastIndexOf('.'));
            string fileHead = filePath.Substring(0, filePath.LastIndexOf('.'));

            var thumbPath = string.Format("{0}_{1}{2}", fileHead, suffix, fileExt); ;
            ThumbnailHelper.MakeThumbnail(filePath, thumbPath, width, height, mode, false);
        }

        private static string GetUrlFileName(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }  
        #endregion

        private string ServerPatch { get; set; }

        private readonly JavaScriptSerializer js;

        private string StorageRoot
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Files/")); } //Path should! always end with '/'
        }

        public UploadHandler()
        {
            js = new JavaScriptSerializer();
            js.MaxJsonLength = 41943040;
        }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.AddHeader("Cache-Control", "private, no-cache");

            HandleMethod(context);
        }

        // Handle request based on method
        private void HandleMethod(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "HEAD":
                case "GET":
                    if (GivenFilename(context)) DeliverFile(context);
                   // else ListCurrentFiles(context);
                    //修改
                    ListCurrentFilesFromDb(context);
                    break;

                case "POST":
                case "PUT":
                    UploadFile(context);
                    break;

                case "DELETE":
                    DeleteFile(context);
                    break;

                case "OPTIONS":
                    ReturnOptions(context);
                    break;

                default:
                    context.Response.ClearHeaders();
                    context.Response.StatusCode = 405;
                    break;
            }
        }

        private static void ReturnOptions(HttpContext context)
        {
            context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
            context.Response.StatusCode = 200;
        }

        // Delete file from the server
        private void DeleteFile(HttpContext context)
        {
            string fileName = context.Request["f"];          ;
            string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(fileName));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);                
            }

            string thumnailFileName = GetUrlFileName(fileName);
            string thumnailName = thumnailFileName.Substring(0, thumnailFileName.Length - thumnailFileName.Substring(thumnailFileName.LastIndexOf('.') + 1).Length - 1) + "_s." + thumnailFileName.Substring(thumnailFileName.LastIndexOf('.') + 1).ToLower();
            string thumNailPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(fileName.Replace(thumnailFileName, string.Empty)+thumnailName));

            if (File.Exists(thumNailPath))
            {
                File.Delete(thumNailPath);     
            }



        }

        // Upload file to the server
        private void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(context, statuses);
            }
            else
            {
                UploadPartialFile(headers["X-File-Name"], context, statuses);
            }

           
            WriteJsonIframeSafe(context, statuses);
        }

        // Upload partial file
        private void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> statuses)
        {
            if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var inputStream = context.Request.Files[0].InputStream;
            var fullName = StorageRoot + Path.GetFileName(fileName);

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new FilesStatus(new FileInfo(fullName)));
        }

        // Upload entire file
        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                context.Response.Charset = "UTF-8";

                byte[] file;
                string localFileName = string.Empty;
                string err = string.Empty;
                string subFolder = string.Empty;
                string fileFolder = string.Empty;
                string filePath = string.Empty; 
                string thumbnailFilePath= string.Empty; 
                #region 上传文件
                var disposition = context.Request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
                if (disposition != null)
                {
                    // HTML5上传
                    file = context.Request.BinaryRead(context.Request.TotalBytes);
                    localFileName = Regex.Match(disposition, "filename=\"(.+?)\"").Groups[1].Value;// 读取原始文件名
                }
                else
                {
                    HttpFileCollection filecollection = context.Request.Files;
                    HttpPostedFile postedfile = filecollection.Get(i);

                    // 读取原始文件名
                    localFileName = Path.GetFileName(postedfile.FileName);

                    // 初始化byte长度.
                    file = new Byte[postedfile.ContentLength];

                    // 转换为byte类型
                    System.IO.Stream stream = postedfile.InputStream;
                    stream.Read(file, 0, postedfile.ContentLength);
                    stream.Close();

                    filecollection = null;
                }

                var ext = localFileName.Substring(localFileName.LastIndexOf('.') + 1).ToLower();

                if (file.Length == 0)
                    err = "无数据提交";
                else if (file.Length > this.MaxFilesize)
                    err = "文件大小超过" + this.MaxFilesize + "字节";
                else if (!AllowExt.Contains(ext))
                    err = "上传文件扩展名必需为：" + string.Join(",", AllowExt);
                else
                {
                    var folder = context.Request["subfolder"] ?? "default";
                    var uploadFolderConfig = UploadConfigContext.UploadConfig.UploadFolders.FirstOrDefault(u => string.Equals(folder, u.Path, StringComparison.OrdinalIgnoreCase));
                    var dirType = uploadFolderConfig == null ? DirType.Day : uploadFolderConfig.DirType;

                    //根据配置里的DirType决定子文件夹的层次（月，天，扩展名）
                    switch (dirType)
                    {
                        case DirType.Month:
                            subFolder = "month_" + DateTime.Now.ToString("yyMM");
                            break;
                        case DirType.Ext:
                            subFolder = "ext_" + ext;
                            break;
                        case DirType.Day:
                            subFolder = "day_" + DateTime.Now.ToString("yyMMdd");
                            break;
                    }

                    fileFolder = Path.Combine(UploadConfigContext.UploadPath,
                        folder,
                        subFolder
                        );

                    filePath = Path.Combine(fileFolder,
                        string.Format("{0}{1}.{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random(DateTime.Now.Millisecond).Next(10000), ext)
                        );
                    
                    if (!Directory.Exists(fileFolder))
                        Directory.CreateDirectory(fileFolder);

                    var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    fs.Write(file, 0, file.Length);
                    fs.Flush();
                    fs.Close();

                    //是图片，即使生成对应尺寸
                    if (ImageExt.Contains(ext))                   
                        ThumbnailService.HandleImmediateThumbnail(filePath); 
                    this.OnUploaded(context, filePath);
                }
                string virtualPatchFile = filePath.Substring(filePath.IndexOf("\\upload", StringComparison.OrdinalIgnoreCase)).Replace("\\", "/");
                string fullName = GetUrlFileName(virtualPatchFile);
               // this.ServerPatch = virtualPatchFile.Replace(fullName, string.Empty);
                statuses.Add(new FilesStatus(fullName, file.Length, filePath));
                
                file = null;
                #endregion

                
            }
        }

        private void WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                    context.Response.ContentType = "application/json";
                else
                    context.Response.ContentType = "text/plain";
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }

            var result = new
            {
                files = statuses
            };
            var jsonObj = js.Serialize(result);
            context.Response.Write(jsonObj);
        }

        private static bool GivenFilename(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Request["f"]);
        }

        private void DeliverFile(HttpContext context)
        {
            var filename = context.Request["f"];
            var filePath = StorageRoot + filename;

            if (File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        private void ListCurrentFiles(HttpContext context)
        {
            var files =
                new DirectoryInfo(StorageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => new FilesStatus(f))
                    .ToArray();

            string jsonObj = js.Serialize(files);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="context"></param>
        private void ListCurrentFilesFromDb(HttpContext context)
        {   
            var id = context.Request["productid"].ToInt();
            var files = new object();
            if (id!=0)
            {
                ImsService ims = new ImsService();
                 List<Image> images = ims.GetProduct(id).Images;
                files = images.Select(f => new FilesStatus(GetUrlFileName(f.PictureURL), f.SortOrder, f.PictureURL, f.Label,f.ID)).ToArray();
            }

           
            
            var file = new
            {
                files = files
            };

            
            
            string jsonObj = js.Serialize(file);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
        }

    }
}