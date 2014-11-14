using System;
using System.Web;
using System.IO;
using System.Collections.Generic;

namespace Infrastructure
{
    /// <summary>
    /// Log type
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Application
        /// </summary>
        Application,
        /// <summary>
        /// System
        /// </summary>
        System,
        /// <summary>
        /// Security
        /// </summary>
        Security,
        /// <summary>
        /// SQL server
        /// </summary>
        SQL,
        /// <summary>
        /// Engine or Windows service
        /// </summary>
        Service,
        /// <summary>
        /// Unknown log type
        /// </summary>
        Other
    }
    /// <summary>
    /// Log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Low
        /// </summary>
        Low,
        /// <summary>
        /// Middle
        /// </summary>
        Middle,
        /// <summary>
        /// High
        /// </summary>
        High,
        /// <summary>
        /// Rugent
        /// </summary>
        Rugent,
        /// <summary>
        /// Other, Maybe it is not an exception
        /// </summary>
        Other
    }
    /// <summary>
    /// This class is used to Log Exception
    /// </summary>
    public class Log
    {
        public string Category { get; set; }
        public LogType LogType { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string Memoire { get; set; }
        public string StackTrace { get; set; }
        public DateTime DateTime { get; set; }

        private static Dictionary<string, string> __directory__ = new Dictionary<string, string>();
        /// <summary>
        /// Get the directory path, If the folder is not exists in system. It will be created.
        /// </summary>
        /// <param name="name">Directory name</param>
        /// <returns>Directory path</returns>
        public static string GetLogDirectory(string name)
        {
            string ym = DateTime.Now.ToString("yyyyMM");
            if (__directory__.ContainsKey(name + ym))
                return __directory__[name + ym];
            string directory = String.Format(@"{0}{1}\{2}\"
                , GetLogDirectory()
                , name
                , ym);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            __directory__.Add(name + ym, directory);
            return directory;
        }
        /// <summary>
        /// Get root of log directory
        /// </summary>
        /// <returns>Directory path</returns>
        public static string GetLogDirectory()
        {
            string directory = String.Empty;
            if (HttpContext.Current != null &&
                HttpContext.Current.Server != null)
                directory = HttpContext.Current.Server.MapPath("~");
            else
                directory = AppDomain.CurrentDomain.BaseDirectory;
            if (directory[directory.Length - 1] != '/' &&
                directory[directory.Length - 1] != '\\')
                directory += "\\";
            return directory + "Log\\";
        }

        /// <summary>
        /// Save the exception
        /// </summary>
        /// <param name="e">The exception should be saved</param>
        public static void SaveException(Exception e)
        {
            SaveException(e, string.Empty, LogType.Other, LogLevel.Other);
        }
        /// <summary>
        /// Save the exception
        /// </summary>
        /// <param name="e">the exception should be saved</param>
        /// <param name="memo">The memo of exception</param>
        public static void SaveException(Exception e, string memo)
        {
            SaveException(e, memo, LogType.Other, LogLevel.Other);
        }

        /// <summary>
        /// Save the exception
        /// </summary>
        /// <param name="e">The exception should be saved</param>
        /// <param name="memo">The memo of exception</param>
        /// <param name="logType">Log type</param>
        /// <param name="logLevel">Log level</param>
        public static void SaveException(Exception e, string memo, LogType logType, LogLevel logLevel)
        {
            FileStream fs = null;
            StreamWriter writer = null;
            try
            {
                string requestUrl = string.Empty;
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null)
                {
                    requestUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                }
                DateTime now = DateTime.Now;
                string filename = GetLogDirectory("Common") + now.ToString("yyyy-MM-dd") + ".txt";
                fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read | FileShare.Write | FileShare.Delete);
                writer = new StreamWriter(fs);
                writer.Write(String.Format(@"
----------Start---------------
rquest  {6}
#EXCEPTION: {0} - {1} at {2:yyyy-MM-dd HH:mm:ss}
Memoire:{3}
Message:{4}
----------
{5}
----------End---------------

"
                    , logType.ToString()
                    , logLevel.ToString()
                    , now
                    , memo
                    , e.Message
                    , e.StackTrace, requestUrl));
                fs.Flush();
            }
            catch
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// Save the note
        /// </summary>
        /// <param name="note">Note</param>
        public static void SaveNote(string note)
        {
            SaveNote(note, LogType.Other, LogLevel.Other);
        }
        /// <summary>
        /// Save the note
        /// </summary>
        /// <param name="note">Note</param>
        /// <param name="logType">Log type</param>
        /// <param name="logLevel">Log level</param>
        public static void SaveNote(string note, LogType logType, LogLevel logLevel)
        {
            FileStream fs = null;
            StreamWriter writer = null;
            try
            {
                DateTime now = DateTime.Now;
                string filename = GetLogDirectory("Common") + now.ToString("yyyy-MM-dd") + ".txt";
                fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read | FileShare.Write | FileShare.Delete);
                writer = new StreamWriter(fs);
                writer.Write(String.Format(@"#NOTE: {0} - {1} at {2:yyyy-MM-dd HH:mm:ss}
Memoire:{3}

"
                    , logType.ToString()
                    , logLevel.ToString()
                    , now
                    , note));
                fs.Flush();
            }
            catch
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// 自定义文件
        /// </summary>
        /// <param name="filename">文件夹名称</param>
        /// <param name="note"></param>
        public static void SaveNote(string filename, string note)
        {
            FileStream fs = null;
            StreamWriter writer = null;
            try
            {
                DateTime now = DateTime.Now;
                filename = GetLogDirectory(filename) + now.ToString("yyyy-MM-dd") + ".txt";
                fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.Read | FileShare.Write | FileShare.Delete);
                writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                writer.WriteLine(String.Format(@"#Note {0}  {1} ", note, now));
                fs.Flush();
            }
            catch
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
        /// <summary>
        /// Get the logs list
        /// </summary>
        /// <param name="date">Expected query date</param>
        /// <returns>Log list</returns>
        public static Log[] GetLogs(DateTime date)
        {
            return GetLogs(date, null);
        }

        /// <summary>
        /// Get the logs list
        /// </summary>
        /// <param name="date">Expected query date</param>
        /// <param name="category">e.g. #EXCEPTION, #NOTE</param>
        /// <returns>Log list</returns>
        public static Log[] GetLogs(DateTime date, string category)
        {
            List<Log> logs = new List<Log>();
            //FileStream fs   = null;
            StreamReader sr = null;
            try
            {
                string name = "Common";
                string ym = DateTime.Now.ToString("yyyyMM");
                string directory = String.Empty;
                if (__directory__.ContainsKey(name + ym))
                    directory = __directory__[name + ym];
                else
                    directory = String.Format(@"{0}\{1}\{2}\"
                    , GetLogDirectory()
                    , name
                    , ym);
                string filename = directory + date.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(filename))
                    return logs.ToArray();
                //fs = new FileStream(filename, FileMode.Open, FileAccess.Write, FileShare.Read | FileShare.Write | FileShare.Delete);
                sr = new StreamReader(filename);
                string readLine = null;
                while ((readLine = sr.ReadLine()) != null)
                {
                    if (!readLine.StartsWith("#"))
                        continue;
                    else if (!String.IsNullOrEmpty(category) && !readLine.StartsWith(category))
                        continue;
                    string[] items = readLine.Split(' ');
                    Log log = new Log();
                    log.Category = items[0].TrimEnd(':');
                    if (items[1] == LogType.Application.ToString())
                        log.LogType = LogType.Application;
                    else if (items[1] == LogType.Security.ToString())
                        log.LogType = LogType.Security;
                    else if (items[1] == LogType.Service.ToString())
                        log.LogType = LogType.Service;
                    else if (items[1] == LogType.SQL.ToString())
                        log.LogType = LogType.SQL;
                    else if (items[1] == LogType.System.ToString())
                        log.LogType = LogType.System;
                    else
                        log.LogType = LogType.Other;
                    if (items[3] == LogLevel.Low.ToString())
                        log.LogLevel = LogLevel.Low;
                    else if (items[3] == LogLevel.Middle.ToString())
                        log.LogLevel = LogLevel.Middle;
                    else if (items[3] == LogLevel.High.ToString())
                        log.LogLevel = LogLevel.High;
                    else if (items[3] == LogLevel.Rugent.ToString())
                        log.LogLevel = LogLevel.Rugent;
                    else
                        log.LogLevel = LogLevel.Other;

                    DateTime dateTime = new DateTime(1999, 12, 31);
                    DateTime.TryParseExact(
                        items[5] + " " + items[6]
                        , "yyyy-MM-dd HH:mm:ss"
                        , null
                        , System.Globalization.DateTimeStyles.None
                        , out dateTime);
                    log.DateTime = dateTime;

                    logs.Add(log);
                    //memoire
                    readLine = sr.ReadLine();
                    if (readLine == null) break;
                    log.Memoire = readLine.Substring("Memoire:".Length);
                    //message
                    readLine = sr.ReadLine();
                    if (readLine == null) break;
                    if (readLine == String.Empty) continue;
                    log.Message = readLine.Substring("Message:".Length);
                    //stack
                    readLine = sr.ReadLine();
                    if (readLine == null) break;
                    if (readLine == String.Empty) continue;
                    log.StackTrace = String.Empty;
                    while (!(readLine = sr.ReadLine()).StartsWith("-"))
                        log.StackTrace += readLine + "\r\n";
                    log.StackTrace = log.StackTrace.TrimEnd();
                }
                //fs.Flush();
            }
            catch (Exception c)
            {
                throw c;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                //if (fs != null)
                //{
                //    fs.Close();
                //    fs.Dispose();
                //}
            }
            return logs.ToArray();
        }
    }
}
