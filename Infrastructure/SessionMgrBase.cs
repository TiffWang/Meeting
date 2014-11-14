using System;
using System.Web;
using System.Diagnostics;
using System.Globalization;

namespace Infrastructure
{
    /// <summary>
    /// The language type in current.
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// English - Canada 
        /// </summary>
        English = 2052,
        /// <summary>
        /// French - Canada 
        /// </summary>
        French = 3084,
        /// <summary>
        /// Chinese - China 
        /// </summary>
        Chinese = 4105,
    }

    /// <summary>
    /// This class is base of SessionMgr. And only provide the base method for the user.
    /// </summary>
    public abstract class SessionMgrBase
    {
        private static readonly string COOKIE_NAME = "COOKIE";

        /// <summary>
        /// Set the cookie
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        public static void SetCookie<T>(object name, T value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(COOKIE_NAME);
            if (cookie == null)
                cookie = new HttpCookie(COOKIE_NAME);
            cookie.Expires = DateTime.Now.AddYears(1);
            cookie.Values[name.ToString()] = value.ToString();
            cookie.HttpOnly = false;

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// Get the cookie
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="name">name</param>
        /// <param name="def">default value (If the value is null)</param>
        /// <returns>Result</returns>
        public static T GetCookie<T>(object name, T def)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(COOKIE_NAME);
            if (cookie == null)
                return def;
            try
            {
                string temp = cookie.Values.Get(name.ToString());
                if (temp == null ||
                    temp.Length == 0)
                {
                    return def;
                }
                return (T)Convert.ChangeType(temp, typeof(T));
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        /// Set the session
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        public static void SetSession<T>(object name, T value)
        {
            Debug.Assert(HttpContext.Current != null);
            HttpContext.Current.Session[name.ToString()] = value;
        }

        /// <summary>
        /// Get the session
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="name">name</param>
        /// <param name="def">default value (If the value is null)</param>
        /// <returns>Result</returns>
        public static T GetSession<T>(object name, T def)
        {
            Debug.Assert(HttpContext.Current != null);

            try
            {
                var value = HttpContext.Current.Session[name.ToString()];
                if (value == null)
                    return def;
                T t = (T)Convert.ChangeType(value, typeof(T));
                if (Equals(t, default(T)))
                    return def;

                return t;
            }
            catch
            {
                return def;
            }
        }

        public static T GetSession<T>(HttpContext context, object name, T def)
        {
            Debug.Assert(context != null);

            try
            {
                var value = context.Session[name.ToString()];
                if (value == null)
                    return def;
                T t = (T)Convert.ChangeType(value, typeof(T));
                if (Equals(t, default(T)))
                    return def;

                return t;
            }
            catch
            {
                return def;
            }
        }


        /// <summary>
        /// Get/Set the current lanaguge.
        /// </summary>
        public static Language Language
        {
            get
            {
                var lang = (Language)GetSession("Language", GetCookie("Language", (int)Language.Unknown));

                if (lang == Language.Unknown)
                {
                    lang = Language.English;
                    if (HttpContext.Current.Request.UserLanguages != null &&
                        HttpContext.Current.Request.UserLanguages.Length > 0)
                    {
                        CultureInfo info = new CultureInfo(HttpContext.Current.Request.UserLanguages[0]);
                        switch (info.TwoLetterISOLanguageName)
                        {
                            case "fr":
                                lang = Language.French;
                                break;
                            case "en":
                                lang = Language.English;
                                break;
                            case "zh":
                                lang = Language.Chinese;
                                break;
                        }
                    }
                    SetSession("Language", (int)lang);
                    SetCookie("Language", (int)lang);
                }

                return lang;
            }
            set
            {
                SetSession("Language", (int)value);
                SetCookie("Language", (int)value);
            }
        }
    }
}
