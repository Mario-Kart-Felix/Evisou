using Evisou.Framework.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evisou.Core.Upload
{
    public class FilesStatus
    {
        public const string HandlerPath ="/Upload/";

        //public string files { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string progress { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteUrl { get; set; }
        public string deleteType { get; set; }
        public string error { get; set; }
        public string label { get; set; }
        public int sortorder { get; set; }
        public int id { get; set; }
        public FilesStatus() { }

        public FilesStatus(FileInfo fileInfo) { SetValues(fileInfo.Name, (int)fileInfo.Length, fileInfo.FullName); }

        public FilesStatus(string fileName, int fileLength, string fullPath) { SetValues(fileName, fileLength, fullPath); }

        public FilesStatus(string fileName, int SortOrder, string ServerPath, string Label,int PictureID) { SetValues(fileName, SortOrder, ServerPath,Label,PictureID); }
       
        private void SetValues(string fileName, int fileLength, string fullPath)
        {
            name = fileName;
            type = "image/png";
            size = fileLength;
            progress = "1.0";
            url = fullPath.Substring(fullPath.IndexOf("\\upload", StringComparison.OrdinalIgnoreCase)).Replace("\\", "/");//fullPath.Substring(fullPath.IndexOf("\\upload", StringComparison.OrdinalIgnoreCase)).Replace("\'\\'","\'/\'"); //System.Web.HttpContext.Current.Request.Url + "?f=" + fileName;//HandlerPath + "UploadHandler.ashx?f=" + fileName;
            deleteUrl = System.Web.HttpContext.Current.Request.Url + "?f=" + url;//fullPath.Substring(fullPath.IndexOf("\\upload", StringComparison.OrdinalIgnoreCase)).Replace("\\", "/");//HandlerPath + "UploadHandler.ashx?f=" + fileName;
            deleteType = "DELETE";

            var ext = Path.GetExtension(fullPath);         
            string thumnailName = Path.GetFileNameWithoutExtension(fullPath) + "_s" + ext;
            string thumnailPath = fullPath.Replace(name, thumnailName);

            var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
            if (fileSize > 3 || !IsImage(ext)) thumbnailUrl = "/Content/img/generalFile.png";
            else thumbnailUrl = @"data:image/png;base64," + EncodeFile(thumnailPath);
        }

        private void SetValues(string fileName, int SortOrder, string ServerPath, string Label, int PictureID)
        {
            name = fileName;
            type = "image/png";
            url = ServerPath;
            sortorder = SortOrder;
            label = Label;
            id = PictureID;
            deleteUrl = System.Web.HttpContext.Current.Request.Url + "?f=" + url;//fullPath.Substring(fullPath.IndexOf("\\upload", StringComparison.OrdinalIgnoreCase)).Replace("\\", "/");//HandlerPath + "UploadHandler.ashx?f=" + fileName;
            deleteType = "DELETE";          
            string thumnailName =fileName.Replace(".","_s.") ;
            thumbnailUrl = url.Replace(fileName, thumnailName);
        }

        private bool IsImage(string ext)
        {
            return ext == ".gif" || ext == ".jpg" || ext == ".png";
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}
