using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Evisou.Ims.Contract.Model
{
    public class EncodingHelper
    {
        public static readonly Encoding GBK_ENCODING = System.Text.Encoding.GetEncoding("GB2312");
        public static readonly Encoding UTF8_ENCODING = System.Text.Encoding.UTF8;

        /// <summary>
        /// 进行UTF-8编码
        /// </summary>
        /// <param name="s">要编码的字符串</param>
        /// <returns></returns>
        public static String UrlEncodeU8(String s)
        {
            return HttpUtility.UrlEncode(s, UTF8_ENCODING);
        }

        /// <summary>
        /// 进行UTF-8解码
        /// </summary>
        /// <param name="s">要解码的字符串</param>
        /// <returns></returns>
        public static String UrlDecodeU8(String s)
        {
            return HttpUtility.UrlDecode(s, UTF8_ENCODING);
        }

        /// <summary>
        /// 进行GBK编码
        /// </summary>
        /// <param name="s">要编码的字符串</param>
        /// <returns></returns>
        public static String UrlEncodeGB(String s)
        {
            return HttpUtility.UrlEncode(s, GBK_ENCODING);
        }

        /// <summary>
        /// 进行GBK解码
        /// </summary>
        /// <param name="s">要解码的字符串</param>
        /// <returns></returns>
        public static String UrlDecodeGB(String s)
        {
            return HttpUtility.UrlDecode(s, GBK_ENCODING);
        }
    }
}