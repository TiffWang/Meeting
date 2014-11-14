/*************************
 * 作者：yangxiao 2013-09-29
*************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using BusinessTier.OptimizeReflection;
using System.Web;
using System.Collections;
using System.Web.SessionState;

namespace BusinessTier.MyAjax
{
    public sealed class ControllerActionPair
    {
        public string Controller;
        public string Action;
    }

    /*
	可以解析以下格式的URL：（前三个表示包含命名空间的）
	/aa.AA.AjaxTest/Add.cspx
	/bb.BB.AjaxTest.Add.cspx
	/bb/BB/AjaxTest/Add.cspx
	/AjaxDemo/GetMd5.cspx
	/AjaxDemo.GetMd5.cspx
*/
    internal static class UrlParser
    {
        // 用于匹配Ajax请求的正则表达式，
        // 可以匹配的URL：/AjaxClass/method.cspx?id=2
        // 注意：类名必须Ajax做为前缀
        internal static readonly string AjaxUrlPattern
            = @"/(?<name>(\w[\./\w]*)?(?=Ajax)\w+)[/\.](?<method>\w+)\.[a-zA-Z]+";

        public static ControllerActionPair ParseAjaxUrl(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            Match match = Regex.Match(path, AjaxUrlPattern);
            if (match.Success == false)
                return null;

            return new ControllerActionPair
            {
                Controller = match.Groups["name"].Value.Replace("/", "."),
                Action = match.Groups["method"].Value
            };
        }
    }

    internal sealed class ControllerDescription
    {
        public Type ControllerType { get; private set; }
        public ControllerDescription(Type t)
        {
            this.ControllerType = t;
        }
    }

    internal sealed class ActionDescription
    {
        public SessionModeAttribute SessionMode { get; protected set; }

        public ControllerDescription PageController; //为PageAction保留
        public MethodInfo MethodInfo { get; private set; }
        public ActionAttribute Attr { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
        public bool HasReturn { get; private set; }

        public ActionDescription(MethodInfo m, ActionAttribute atrr)
        {
            this.MethodInfo = m;
            this.Attr = atrr;
            this.Parameters = m.GetParameters();
            this.HasReturn = m.ReturnType != ReflectionHelper.VoidType;

            this.SessionMode = m.GetMyAttribute<SessionModeAttribute>();
        }
    }

    internal sealed class InvokeInfo
    {
        public ControllerDescription Controller;
        public ActionDescription Action;
        public object Instance;

        public SessionMode GetSessionMode()
        {
            if (this.Action != null && this.Action.SessionMode != null)
                return this.Action.SessionMode.SessionMode;

            //默认为只读
            return SessionMode.ReadOnly;
        }
    }

    public interface IActionResult
    {
        void Ouput(HttpContext context);
    }

    #region string

    public static class StringExtensions
    {
        internal static readonly char[] CommaSeparatorArray = new char[] { ',' };
    }


    #endregion

    #region 错误处理

    internal static class ExceptionHelper
    {
        public static void Throw403Exception(HttpContext context)
        {
            if (context == null)
                throw new HttpException(403, "很抱歉，您没有合适的权限访问该资源。");

            throw new HttpException(403,
                "很抱歉，您没有合适的权限访问该资源：" + context.Request.RawUrl);
        }

        public static void Throw404Exception(HttpContext context)
        {
            if (context == null)
                throw new HttpException(404, "要请求的资源不存在。");

            throw new HttpException(404,
                "没有找到能处理请求的服务类，当前请求地址：" + context.Request.RawUrl);
        }
    }
    #endregion

    #region Attribute

    /// <summary>
    /// 将一个方法标记为一个Action
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ActionAttribute : Attribute
    {
        /// <summary>
        /// 允许哪些访问动词，与web.config中的httpHanlder的配置意义一致。
        /// </summary>
        public string Verb { get; set; }

        internal bool AllowExecute(string httpMethod)
        {
            if (string.IsNullOrEmpty(Verb) || Verb == "*")
            {
                return true;
            }
            else
            {
                string[] verbArray = Verb.SplitTrim(StringExtensions.CommaSeparatorArray);

                return verbArray.Contains(httpMethod, StringComparer.OrdinalIgnoreCase);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SessionModeAttribute : Attribute
    {
        /// <summary>
        /// 要支持的Session模式
        /// </summary>
        public SessionMode SessionMode { get; private set; }

        public SessionModeAttribute(SessionMode mode)
        {
            this.SessionMode = mode;
        }
    }

    #endregion

    #region ActionHandler

    /// <summary>
    /// Action所支持的Session模式
    /// </summary>
    public enum SessionMode
    {
        /// <summary>
        /// 不支持
        /// </summary>
        NotSupport,
        /// <summary>
        /// 全支持
        /// </summary>
        Support,
        /// <summary>
        /// 仅支持读取
        /// </summary>
        ReadOnly
    }


    internal class RequiresSessionActionHandler : ActionHandler, IRequiresSessionState
    {
    }

    internal class ReadOnlySessionActionHandler : ActionHandler, IRequiresSessionState, IReadOnlySessionState
    {
    }

    internal class ActionHandler : IHttpHandler
    {
        internal InvokeInfo InvokeInfo;

        public void ProcessRequest(HttpContext context)
        {
            // 调用核心的工具类，执行Action
            ActionExecutor.ExecuteAction(context, this.InvokeInfo);
        }

        public bool IsReusable
        {
            get { return false; }
        }


        public static ActionHandler CreateHandler(InvokeInfo vkInfo)
        {
            SessionMode mode = vkInfo.GetSessionMode();

            if (mode == SessionMode.NotSupport)
                return new ActionHandler { InvokeInfo = vkInfo };
            else if (mode == SessionMode.ReadOnly)
                return new ReadOnlySessionActionHandler { InvokeInfo = vkInfo };
            else
                return new RequiresSessionActionHandler { InvokeInfo = vkInfo };
        }
    }

    #endregion

    #region  返回参数类型实现

    public sealed class XmlResult : IActionResult
    {
        public object xml { get; private set; }

        public XmlResult(string xml)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");

            this.xml = xml;
        }

        void IActionResult.Ouput(HttpContext context)
        {
            context.Response.ContentType = "application/xml";
            //string xml = XmlHelper.XmlSerialize(Model, Encoding.UTF8);
            context.Response.Write(xml);
        }
    }

    //public sealed class JsonResult : IActionResult
    //{
    //    public object Model { get; private set; }

    //    public JsonResult(object model)
    //    {
    //        if (model == null)
    //            throw new ArgumentNullException("model");

    //        this.Model = model;
    //    }

    //    void IActionResult.Ouput(HttpContext context)
    //    {
    //        context.Response.ContentType = "application/json";
    //        string json = Model.ToJson();
    //        context.Response.Write(json);
    //    }
    //}

    #endregion

    internal static class ReflectionHelper
    {

        static ReflectionHelper()
        {
            InitControllers();
        }

        private static Hashtable s_modelTable = Hashtable.Synchronized(
                                        new Hashtable(4096, StringComparer.OrdinalIgnoreCase));

        /// <summary>
        /// 返回一个实体类型的描述信息（全部属性及字段）。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //public static ModelDescripton GetModelDescripton(Type type)
        //{
        //    if (type == null)
        //        throw new ArgumentNullException("type");

        //    string key = type.FullName;
        //    ModelDescripton mm = (ModelDescripton)s_modelTable[key];

        //    if (mm == null)
        //    {
        //        List<DataMember> list = new List<DataMember>();

        //        (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
        //         select new PropertyMember(p)).ToList().ForEach(x => list.Add(x));

        //        (from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public)
        //         select new FieldMember(f)).ToList().ForEach(x => list.Add(x));

        //        mm = new ModelDescripton { Fields = list.ToArray() };
        //        s_modelTable[key] = mm;
        //    }
        //    return mm;
        //}

        /// <summary>
        /// 判断是否是一个可支持的参数类型。仅包括：基元类型，string ，decimal，DateTime，Guid, string[], 枚举
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSupportableType(this Type type)
        {
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(DateTime)
                || type == typeof(decimal)
                || type == typeof(Guid)
                || type.IsEnum
                || type == typeof(string[]);
        }

        /// <summary>
        /// 得到一个实际的类型（排除Nullable类型的影响）。比如：int? 最后将得到int
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetRealType(this Type type)
        {
            if (type.IsGenericType)
                return Nullable.GetUnderlyingType(type) ?? type;
            else
                return type;
        }

        #region MyRegion


        public static readonly Type VoidType = typeof(void);

        // 保存AjaxController的列表
        private static List<ControllerDescription> s_AjaxControllerList;
        // 保存AjaxAction的字典
        private static Hashtable s_AjaxActionTable = Hashtable.Synchronized(
                                                new Hashtable(4096, StringComparer.OrdinalIgnoreCase));

        // 用于从类型查找Action的反射标记
        private static readonly BindingFlags ActionBindingFlags =
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;



        internal static T GetMyAttribute<T>(this MemberInfo m, bool inherit) where T : Attribute
        {
            T[] array = m.GetCustomAttributes(typeof(T), inherit) as T[];

            if (array.Length == 1)
                return array[0];

            if (array.Length > 1)
                throw new InvalidProgramException(string.Format("方法 {0} 不能同时指定多次 [{1}]。", m.Name, typeof(T)));

            return default(T);
        }

        internal static T GetMyAttribute<T>(this MemberInfo m) where T : Attribute
        {
            return GetMyAttribute<T>(m, false);
        }

        private static MethodInfo FindAction(string action, Type controller, HttpRequest request)
        {
            foreach (MethodInfo method in controller.GetMethods())
            {
                if (method.Name.IsSame(action))
                {
                    //if (MethodActionIsMatch(method, request))
                    return method;
                }
            }
            return null;
        }

        private static MethodInfo FindSubmitAction(Type controller, HttpRequest request)
        {
            string[] keys = request.Form.AllKeys;

            foreach (MethodInfo method in controller.GetMethods())
            {
                string key = keys.FirstOrDefault(x => method.Name.IsSame(x));
                if (key != null && MethodActionIsMatch(method, request))
                    return method;
            }

            return null;
        }

        /// <summary>
        /// 加载所有的Controller
        /// </summary>
        private static void InitControllers()
        {
            s_AjaxControllerList = new List<ControllerDescription>(1024);
            //var pageControllerList = new List<ControllerDescription>(1024);

            ICollection assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                //只加载Site下的 
                if (!assembly.FullName.StartsWith("Site", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (assembly.FullName.StartsWith("Site", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        foreach (Type t in assembly.GetExportedTypes())
                        {
                            if (t.Name.StartsWith("Ajax"))
                                s_AjaxControllerList.Add(new ControllerDescription(t));

                            //else if (t.Name.EndsWith("Controller"))
                            //    pageControllerList.Add(new ControllerDescription(t));
                        }
                    }
                    catch { }
                }

            }
        }

        /// <summary>
        /// 根据要调用的方法名返回对应的 Action （适用于Ajax调用）
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private static ActionDescription GetAjaxAction(Type controller, string action, HttpRequest request)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");
            if (string.IsNullOrEmpty(action))
                throw new ArgumentNullException("action");

            // 首先尝试从缓存中读取
            string key = request.HttpMethod + "#" + controller.FullName + "@" + action;
            ActionDescription mi = (ActionDescription)s_AjaxActionTable[key];

            if (mi == null)
            {
                bool saveToCache = true;

                MethodInfo method = FindAction(action, controller, request);

                if (method == null)
                {
                    // 如果Action的名字是submit并且是POST提交，则需要自动寻找Action
                    // 例如：多个提交都采用一样的方式：POST /AjaxProduct/submit
                    if (action.IsSame("submit") && request.HttpMethod.IsSame("POST"))
                    {
                        // 自动寻找Action
                        method = FindSubmitAction(controller, request);
                        saveToCache = false;
                    }
                }

                if (method == null)
                    return null;

                var attr = method.GetMyAttribute<ActionAttribute>();
                mi = new ActionDescription(method, attr);

                if (saveToCache)
                    s_AjaxActionTable[key] = mi;
            }

            return mi;
        }

        private static bool MethodActionIsMatch(MethodInfo method, HttpRequest request)
        {
            var attr = method.GetMyAttribute<ActionAttribute>();
            if (attr != null)
            {
                if (attr.AllowExecute(request.HttpMethod))
                    return true;
            }
            return false;
        }




        /// <summary>
        /// 根据要调用的controller名返回对应的Controller （适用于Ajax调用）
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private static ControllerDescription GetAjaxController(string controller)
        {
            if (string.IsNullOrEmpty(controller))
                throw new ArgumentNullException("controller");


            // 查找类型的方式：如果有点号，则按全名来查找(包含命名空间)，否则只看名字。
            // 本框架对于多个匹配条件的类型，将返回第一个匹配项。
            if (controller.IndexOf('.') > 0)
                return s_AjaxControllerList.FirstOrDefault(
                    t => string.Compare(t.ControllerType.FullName, controller, true) == 0);
            else
                return s_AjaxControllerList.FirstOrDefault(
                    t => string.Compare(t.ControllerType.Name, controller, true) == 0);
        }


        /// <summary>
        /// 根据一个AJAX的调用信息（类名与方法名），返回内部表示的调用信息。
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static InvokeInfo GetAjaxInvokeInfo(ControllerActionPair pair, HttpRequest request)
        {
            if (pair == null)
                throw new ArgumentNullException("pair");

            InvokeInfo vkInfo = new InvokeInfo();

            vkInfo.Controller = GetAjaxController(pair.Controller);
            if (vkInfo.Controller == null)
                return null;

            vkInfo.Action = GetAjaxAction(vkInfo.Controller.ControllerType, pair.Action, request);
            if (vkInfo.Action == null)
                return null;

            if (vkInfo.Action.MethodInfo.IsStatic == false)
                //vkInfo.Instance = Activator.CreateInstance(vkInfo.Controller.ControllerType);
                vkInfo.Instance = vkInfo.Controller.ControllerType.FastNew();

            return vkInfo;
        }

        #endregion
    }

    internal sealed class AjaxHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context,
                            string requestType, string virtualPath, string physicalPath)
        {
            // 根据请求路径，定位到要执行的Action
            ControllerActionPair pair = UrlParser.ParseAjaxUrl(virtualPath);
            if (pair == null)
                ExceptionHelper.Throw404Exception(context);

            // 获取内部表示的调用信息
            InvokeInfo vkInfo = ReflectionHelper.GetAjaxInvokeInfo(pair, context.Request);
            if (vkInfo == null)
                ExceptionHelper.Throw404Exception(context);

            // 创建能够调用Action的HttpHandler
            return ActionHandler.CreateHandler(vkInfo);
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }

    public static class ActionExecutor
    {
        /// <summary>
        /// MyMVC的版本。（dll文件版本）
        /// </summary>
        private static readonly string MvcVersion
            = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(ActionExecutor).Assembly.Location).FileVersion;
        private static void SetMvcVersionHeader(HttpContext context)
        {
            context.Response.AppendHeader("X-MyMVC-Version", MvcVersion);
        }

        internal static void ExecuteAction(HttpContext context, InvokeInfo vkInfo)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (vkInfo == null)
                throw new ArgumentNullException("vkInfo");

            SetMvcVersionHeader(context);

            // 验证请求是否允许访问（身份验证）
            //AuthorizeAttribute authorize = vkInfo.GetAuthorize();
            //if (authorize != null)
            //{
            //    if (authorize.AuthenticateRequest(context) == false)
            //        ExceptionHelper.Throw403Exception(context);
            //}

            // 调用方法
            object result = ExecuteActionInternal(context, vkInfo);

            // 设置OutputCache
            //OutputCacheAttribute outputCache = vkInfo.GetOutputCacheSetting();
            //if (outputCache != null)
            //    outputCache.SetResponseCache(context);


            // 处理方法的返回结果
            IActionResult executeResult = result as IActionResult;
            if (executeResult != null)
            {
                executeResult.Ouput(context);
            }
            else
            {
                if (result != null)
                {
                    // 普通类型结果
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(result.ToString());
                }
            }
        }

        internal static object ExecuteActionInternal(HttpContext context, InvokeInfo info)
        {
            // 准备要传给调用方法的参数
            //object[] parameters = GetActionCallParameters(context, info.Action);

            object parameters = null;
            // 调用方法
            if (info.Action.HasReturn)
                //return info.Action.MethodInfo.Invoke(info.Instance, parameters);
                return info.Action.MethodInfo.FastInvoke(info.Instance, parameters);

            else
            {
                //info.Action.MethodInfo.Invoke(info.Instance, parameters);
                info.Action.MethodInfo.FastInvoke(info.Instance, parameters);
                return null;
            }
        }

    }
}
