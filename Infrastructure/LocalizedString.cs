using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace Infrastructure
{
    /// <summary>
    /// 本地化字符串
    /// </summary>
    public static class LocalizedString
    {
        private static Dictionary<int, Dictionary<string, string>> s_StringDictionary;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dic"></param>
        internal static void InitWithDictionary(Dictionary<int, Dictionary<string, string>> dic)
        {
            s_StringDictionary = dic;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>如果有当前语言的字符串，则自动转译并返回</returns>
        public static string Get(string text)
        {
            if( HttpContext.Current != null &&
                HttpContext.Current.Session != null )
            {
                return Get(text, SessionMgrBase.Language);
            }

            return Get(text, Language.Unknown);
        }

        /// <summary>
        /// 获取指定语言版本的语言
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string Get(string text, Language lang)
        {
            if (s_StringDictionary == null ||
                string.IsNullOrEmpty(text) )
            {
                return text;
            }

            Dictionary<string, string> dic;
            if( !s_StringDictionary.TryGetValue( text.GetHashCode(), out dic) )
                return text;

            var info = new CultureInfo((int)lang);
            string textValue;
            dic.TryGetValue( info.TwoLetterISOLanguageName.ToLower(), out textValue);
            if (!string.IsNullOrEmpty(textValue))
                text = textValue;
            else
            {
                dic.TryGetValue("default", out textValue);
                if (!string.IsNullOrEmpty(textValue))
                    text = textValue;
            } 
            return text;
        }
    }
}
