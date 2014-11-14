using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Xml;
using System.Net;
using System.Collections;


namespace BusinessTier
{
    public static class Extension
    {
        public static string GetPageUrl(this System.Web.HttpContext httpContext)
        {
            string url = httpContext.Request.Url.ToString().ToLower();
            url = url.Replace("http://", "");
            int postion = url.IndexOf("/");
            string pageUrl = url.Substring(postion + 1);
            return pageUrl;
        }

        #region CreateWebRequest

        /// <summary>
        /// 动态创建POST请求
        /// </summary>
        /// <param name="url">地址。注意要以Http://开头</param>
        /// <param name="cookies">请求时的所带的cookies。返回是自带Response的Cookies</param>
        /// <param name="requestData">请求的值。如：a=1&c=2</param>
        /// <returns>返回Response的字符</returns>
        public static string CreateWebRequest(string url, CookieContainer cookies, string requestData)
        {
            string outData = string.Empty;
            //新建一个cookiecontainer来存放cookie集合
            HttpWebRequest myhttpwebrequest = (HttpWebRequest)WebRequest.Create(url);
            //新建一个httpwebrequest
            myhttpwebrequest.ContentType = "application/x-www-form-urlencoded";
            myhttpwebrequest.ContentLength = requestData.Length;
            myhttpwebrequest.Method = "post";
            myhttpwebrequest.CookieContainer = cookies;
            //设置httpwebrequest的cookiecontainer为刚才建立的那个mycookiecontainer
            using (StreamWriter mystreamwriter = new StreamWriter(myhttpwebrequest.GetRequestStream()))
            {
                //把数据写入httpwebrequest的request流
                mystreamwriter.Write(requestData);
            }
            //新建一个httpwebresponse
            HttpWebResponse myhttpwebresponse = (HttpWebResponse)myhttpwebrequest.GetResponse();

            //获取一个包含url的cookie集合的cookiecollection
            myhttpwebresponse.Cookies = cookies.GetCookies(myhttpwebrequest.RequestUri);

            using (StreamReader mystreamreader = new StreamReader(myhttpwebresponse.GetResponseStream()))
            {
                //获取请求后服务器输出数据
                outData = mystreamreader.ReadToEnd();
            }

            return outData;
        }

        /// <summary>
        /// 获取CookieContainer里Cookie
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static List<Cookie> GetAllCookies(this CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;
        }

        #endregion

        public static T ParseTo<T>(this object source, T defvalue)
        {
            if (source == null || source.ToString() == "") return defvalue;
            try { return (T)Convert.ChangeType(source, typeof(T)); }
            catch (Exception e)
            {
                return defvalue;
            }
        }

        public static string FormatContent(this string source, string defvalue)
        {
            if (source == null || source.ToString() == "") return defvalue;
            try
            {
                source = HttpUtility.HtmlDecode(source);
                return source.Substring(source.IndexOf('<') + 3, source.LastIndexOf('<') - 3);
            }
            catch (Exception e)
            {
                return defvalue;
            }
        }

        public static string JsonFormat(this string source)
        {
            if (source == null || source.ToString() == "") return "";
            try
            {
                source = source.Replace("[", "").Replace("]", "");
                source = "[" + source + "]";
                return source;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        /// <summary>
        /// 编码js字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SafeJavascriptStringEncode(this string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;

            string cacheKey = string.Format("StringExtension.SafeJavascriptStringEncode.{0}", source);
            string cached = HttpRuntime.Cache[cacheKey] as string;
            if (cached != null)
                return cached;

            StringBuilder sb = new StringBuilder();
            foreach (char c in source)
            {

                int code = (int)c;
                if (code > 0 && c <= 0x7F)
                {
                    switch (code)
                    {
                        case 0x26: // &
                        case 0x22: // "
                        case 0x27: // '
                        case 0x5c: // \
                        case 0x3c: // <
                        case 0x3E: // >
                        case 0x0A: // \r
                        case 0x0D: // \n
                            {
                                sb.AppendFormat("\\u{0:X4}", code);
                                break;
                            }

                        default:
                            sb.Append(c);
                            break;
                    }
                }
                else
                    sb.AppendFormat("\\u{0:X4}", code);
            }

            cached = sb.ToString();
            HttpRuntime.Cache.Insert(cacheKey, cached, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

            return cached;
        }

        /// <summary>
        /// html代码提取纯文字
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripHTML(this string source)
        {
            if (source == null || source.ToString() == "") return "";
            try
            {
                string[] aryReg ={
              @"<script[^>]*?>.*?</script>",
              @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
              @"([\r\n])[\s]+",
              @"&(quot|#34);",
              @"&(amp|#38);",
              @"&(lt|#60);",
              @"&(gt|#62);", 
              @"&(nbsp|#160);", 
              @"&(iexcl|#161);",
              @"&(cent|#162);",
              @"&(pound|#163);",
              @"&(copy|#169);",
              @"&#(\d+);",
              @"-->",
              @"<!--.*\n"         
            };

                string[] aryRep = {
               "",
               "",
               "",
               "\"",
               "&",
               "<",
               ">",
               " ",
               "\xa1",//chr(161),
               "\xa2",//chr(162),
               "\xa3",//chr(163),
               "\xa9",//chr(169),
               "",
               "\r\n",
               ""
            };

                string newReg = aryReg[0];
                string strOutput = source;
                for (int i = 0; i < aryReg.Length; i++)
                {
                    Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                    strOutput = regex.Replace(strOutput, aryRep[i]);
                }

                strOutput = strOutput.Replace("<", "");
                strOutput = strOutput.Replace(">", "");
                strOutput = strOutput.Replace("\r\n", "");
                strOutput = strOutput.Replace("\n", "");

                return strOutput;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        /// <summary>
        /// 字符串截取指定长度
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubStr(this string source, int length)
        {
            if (source == null || source.ToString() == "") return "";
            try
            {
                if (source.Length > length)
                    return source.Substring(0, length) + "……";
                else
                    return source;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        /// <summary>
        /// 判断二个字符串是否相等，忽略大小写的比较方式。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsSame(this string a, string b)
        {
            return string.Compare(a, b, StringComparison.OrdinalIgnoreCase) == 0;
        }


        /// <summary>
        /// 等效于 string.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries)
        /// 且为每个拆分后的结果又做了Trim()操作。
        /// </summary>
        /// <param name="value">要拆分的字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string[] SplitTrim(this string str, params char[] separator)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            else
                return (from s in str.Split(',')
                        let u = s.Trim()
                        where u.Length > 0
                        select u).ToArray();
        }


        #region 将字符串最小限度地转换为 HTML 编码的字符串

        /// <summary>
        /// 将字符串最小限度地转换为 HTML 编码的字符串。
        /// </summary>
        /// <param name="str">要编码的字符串。</param>
        /// <returns>一个已编码的字符串。</returns>
        public static string HtmlAttributeEncode(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return HttpUtility.HtmlAttributeEncode(str);
        }

        #endregion

        #region 将字符串转换为 HTML 编码的字符串
        /// <summary>
        /// 将字符串转换为 HTML 编码的字符串。
        /// </summary>
        /// <param name="str">要编码的字符串。</param>
        /// <returns>一个已编码的字符串。</returns>
        public static string HtmlEncode(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return HttpUtility.HtmlEncode(str);
        }
        #endregion


        #region xml

        /// <summary>
        /// 新增节点且设值,根节点默认为xmlRoot
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="nodeName"></param>
        /// <param name="value"></param>
        public static XmlNode AddOrSetNode(this XmlDocument doc, string nodeName, string value)
        {
            if (string.IsNullOrEmpty(value))
                value = string.Empty;

            XmlNode root = doc.SelectSingleNode("/xmlRoot");
            if (root == null)
            {
                root = doc.CreateElement("xmlRoot");
                doc.AppendChild(root);
            }

            XmlNode rootChild = doc.SelectSingleNode("/xmlRoot/" + nodeName);
            if (rootChild == null)
            {
                XmlNode node = doc.CreateElement(nodeName);
                if (value == null)
                    node.InnerText = string.Empty;
                node.InnerText = value;
                root.AppendChild(node);
                rootChild = node;
            }
            else
                rootChild.InnerText = value;

            return rootChild;
        }

        /// <summary>
        /// 编码成Xml文本支持格式
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string FormatXml(this string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return string.Empty;
            xml = xml.Replace("<", "&lt;");
            xml = xml.Replace(">", "&gt;");
            xml = xml.Replace("&", "&amp;");
            xml = xml.Replace("\"", "&quot;");
            xml = xml.Replace("'", "&apos;");
            return xml;
        }

        #endregion

        /// <summary>
        /// 把Enum转换成对应的int再Tostring
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        public static string EnumToIntToString(this object myEnum)
        {
            try
            {
                return ((int)myEnum).ToString();
            }
            catch (Exception)
            {
                return ((long)myEnum).ToString();
            }

        }


        /// <summary>
        /// 把Enum转换成对应的int再Tostring
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        public static string EnumTolongToString(this object myEnum)
        {
            return ((long)myEnum).ToString();
        }

        public static DateTime DateOfMonth(this DateTime source)
        {
            return source.Date.AddDays(1 - source.Day);
        }
     
        /// <summary>
        /// 转换成带两位小数点的字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToStringF2(this Double? source)
        {
            if (source == null)
                return string.Empty;
            return ((Double)source).ToString("F2");
        }

        public static string ToString(this Double? source, string format)
        {
            if (source == null)
                return string.Empty;
            return ((Double)source).ToString(format);
        }

        /// <summary>
        ///保留两位小数点
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToStringF2(this decimal? source)
        {
            if (source == null)
                return string.Empty;
            return ((decimal)source).ToString("F2");
        }

        public static string ToString(this decimal? source, string format)
        {
            if (source == null)
                return string.Empty;
            return ((decimal)source).ToString(format);
        }
        /// <summary>
        /// 时间格式化
        /// <para>8：yyyy-MM</para>
        /// <para>23：yyyy-MM-dd</para>
        /// <para>120：yyyy-MM-dd HH:mm:ss</para>
        /// <para>123：yyyy-MM-dd HH</para>
        /// <para>121:yyyyMMddHHmmss </para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ToString(this DateTime source, int code)
        {
            switch (code)
            {
                case 8: return source.ToString("yyyy-MM");
                case 23: return source.ToString("yyyy-MM-dd");
                case 123: return source.ToString("yyyy-MM-dd HH");
                case 120: return source.ToString("yyyy-MM-dd HH:mm:ss");
                case 121: return source.ToString("yyyyMMddHHmmss");
                default:
                    return source.ToString("yyyy-MM-dd");
            }
            //return ((DateTime)source).ToString("yyyy-MM-dd HH:mm:ss");
        }



        public static IQueryable<TEntity> GetEntitysByPage<TEntity, TKey>(this IQueryable<TEntity> source,
         int pageSize, int pageIndex,
         Expression<Func<TEntity, TKey>> orderBy, bool isOrderByDescending,
         Expression<Func<TEntity, bool>> condition) where TEntity : class
        {
            IQueryable<TEntity> results = source;

            if (condition != null)
                results = results.Where(condition);

            if (orderBy != null)
            {
                if (isOrderByDescending)
                    results = results.OrderByDescending(orderBy);
                else
                    results = results.OrderBy(orderBy);
            }

            results = results.Skip(pageIndex * pageSize).Take(pageSize);

            return results;
        }

        public static int GetEntitysCount<TEntity>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> condition) where TEntity : class
        {
            IQueryable<TEntity> results = source;

            if (condition != null)
                results = results.Where(condition);

            return results.Count();
        }


        #region 用于Linq获取部分字段
        public static List<T> ExecuteQuery<T>(this DataContext dataContext, IQueryable query)
        {
            DbCommand command = dataContext.GetCommand(query);
            dataContext.OpenConnection();
            log4net.LogManager.GetLogger(typeof(Extension)).Debug(command.CommandText);
            using (DbDataReader reader = command.ExecuteReader())
            {
                return dataContext.Translate<T>(reader).ToList();
            }
        }

        private static void OpenConnection(this DataContext dataContext)
        {
            if (dataContext.Connection.State == ConnectionState.Closed)
            {
                dataContext.Connection.Open();
            }
        }
        #endregion
        /// <summary>
        /// 判断是不是图片地址
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsPictureUrl(this string source)
        {
            if (source == null) return false;
            string strExtension = Path.GetExtension(source).ToLower();
            if (strExtension == ".jpg" || strExtension == ".gif" || strExtension == ".png" || strExtension == ".jpeg")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是不是SWF地址
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsMediaUrl(this string source)
        {
            if (source == null) return false;
            string strExtension = Path.GetExtension(source).ToLower();
            return strExtension == ".flv" || strExtension == ".swf";
        }

        public static bool IsFlvUrl(this string source)
        {
            if (source == null) return false;
            string strExtension = Path.GetExtension(source).ToLower();
            return strExtension == ".flv";
        }

        private static readonly Random s_Rand = new Random();
        public static string GenerateRandomID()
        {
            string ret;
            lock (s_Rand)
            {
                ret = System.DateTime.Now.ToString("yyMM");
                for (int i = 1; i <= 6; i++)
                {
                    ret += s_Rand.Next(10).ToString();
                }
            }
            return ret;
        }

        #region  结束请求，只输出指定的数据

        /// <summary>
        /// 结束请求，只输出指定的数据
        /// </summary>
        /// <param name="text"></param>
        public static void EndRequestWriteText(string text)
        {
            if (HttpContext.Current == null)
                return;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }

        #endregion
        #region 表单
        /// <summary>
        /// 是否在提交表单中
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsExistsForm(this HttpRequest request, string name)
        {
            if (request == null)
                return false;
            return request.Form[name] != null;
        }

        /// <summary>
        /// 获取表单值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetForm<T>(this HttpRequest request, string name, T def)
        {
            string value = string.Empty;
            if (request.Form[name] != null)
                value = request.Form[name];

            if (string.IsNullOrEmpty(value))
                return def;
            else
            {
                try
                {
                    //return <T>(name);
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return def;
                }
            }

        }
        /// <summary>
        /// 获取querystring
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetParam<T>(this HttpRequest request, string name, T def)
        {
            string value = HttpUtility.ParseQueryString(request.Url.Query, Encoding.UTF8)[name];
            if (string.IsNullOrEmpty(value))
                return def;
            else
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return def;
                }
            }

        }
        #endregion
    }
}

namespace BusinessTier.HtmlExtension
{
    /// <summary>
    /// 生成HTML时一些扩展
    /// </summary>
    public static class HtmlExtension
    {
        /// <summary>
        /// 如果相等就返回 checked='true'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="compareVal">比较值</param>
        /// <returns></returns>
        public static string CheckedSelected(this string str, string compareVal)
        {
            if (str == null)
                str = string.Empty;

            if (str.IsSame(compareVal))
                return "checked='true'";

            return string.Empty;
        }
        /// <summary>
        /// 下拉框选择
        /// </summary>
        /// <param name="str"></param>
        /// <param name="compareVal"></param>
        /// <returns></returns>
        public static string OptionSelected(this string str, string compareVal)
        {
            if (str == null)
                str = string.Empty;

            if (str.IsSame(compareVal))
                return "selected='true'";

            return string.Empty;
        }
        /// <summary>
        /// 可多选
        /// </summary>
        /// <param name="str"></param>
        /// <param name="compareVal"></param>
        /// <param name="isCheckBox">是否是多选</param>
        /// <returns></returns>
        public static string CheckedSelected(this string str, string compareVal, bool isCheckBox)
        {
            if (str == null)
                str = string.Empty;

            var arr = str.SplitTrim();
            if (arr.Contains(compareVal))
                return "checked='true'";

            return string.Empty;
        }
        /// <summary>
        /// 如果存在就返回 checked='true'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="compareVal"></param>
        /// <returns></returns>
        public static string CheckedSelected(this string[] str, string compareVal)
        {
            if (str == null)
                return string.Empty;

            if (str.Contains(compareVal))
                return "checked='true'";

            return string.Empty;
        }


        /// <summary>
        /// 如果相等就返回 checked='true'
        /// </summary>
        /// <param name="val"></param>
        /// <param name="compareVal">比较值</param>
        /// <returns></returns>
        public static string CheckedSelected(this int val, int compareVal)
        {
            if (val == compareVal)
                return "checked='true'";
            return string.Empty;
        }

    }
}
