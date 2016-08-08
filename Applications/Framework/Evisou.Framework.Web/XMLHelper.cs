using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Evisou.Framework.Web
{
   public class XMLHelper
    {

       public static void SaveXML(DataTable dt,string filename)
       {
           dt.WriteXml(filename);
       }

       public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName, string[] oldColumnNames, string[] newColumnNames)
       {
           HttpContext curContext = HttpContext.Current;
           XmlTextWriter writer = new XmlTextWriter(curContext.Response.OutputStream, curContext.Response.ContentEncoding);
           writer.Formatting = Formatting.Indented;
           writer.Indentation = 4;
           writer.IndentChar = ' ';
           writer.WriteStartDocument();
           dtSource.WriteXml(writer);
           curContext.Response.ContentType = "application/octet-stream";
           curContext.Response.AppendHeader("Content-Disposition",
               "attachment;filename=" + HttpUtility.UrlEncode(strFileName + ".xml", Encoding.UTF8));
           curContext.Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });  
           writer.Flush();
           curContext.Response.End();
           writer.Close();
           //ExportByWeb(dtSource, strHeaderText, strFileName, "sheet", oldColumnNames, newColumnNames);
       }

        public static HttpResponseMessage ExportByHttpResponseMessage(DataTable dtSource, string strHeaderText, string strFileName, string[] oldColumnNames, string[] newColumnNames)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            HttpContext curContext = HttpContext.Current;
            XmlTextWriter writer = new XmlTextWriter(curContext.Response.OutputStream, curContext.Response.ContentEncoding);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            writer.IndentChar = ' ';
            writer.WriteStartDocument();
            dtSource.WriteXml(writer);

            writer.Flush();
            writer.Close();

            response.Content = new ByteArrayContent(new byte[] { 0xEF, 0xBB, 0xBF });
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.Add("x-filename", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)); // 中文乱码
            return response;


           
           
            
            
            //ExportByWeb(dtSource, strHeaderText, strFileName, "sheet", oldColumnNames, newColumnNames);
        }
    }
}
