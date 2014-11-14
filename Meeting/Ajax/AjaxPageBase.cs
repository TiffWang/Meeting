using System;
using System.Web;
using System.Xml;
using System.Text;
using BusinessTier;
using System.Web.SessionState;
using BusinessTier.OptimizeReflection;



namespace Meeting.Ajax
{

    /// <summary>
    /// Ajax ashx 的父类
    /// </summary>
    public class AjaxBase :PageBase, IRequiresSessionState
    {
        public HttpContext Context = null;
        public XmlDocument XmlDoc = null;

        /// <summary>
        /// (必须)初始化HttpContext。
        /// 同时可以根据Action参数动态调用方法。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="classObj">当前类的实例，用this就可以了。</param>
        /// <returns></returns>
        protected XmlDocument Init(HttpContext context, object classObj)
        {
            HttpContext.Current = context;
            this.Context = context;

            XmlDoc = new XmlDocument();

            //var userId = Infrastructure.SessionMgrBase.GetSession<long>(context, SessionName.UserId.ToString(), -1);
            if (string.IsNullOrEmpty(SessionMgr.UserId))
            {
                SetResult(ResultCode.未登录, "未登录！");
                WriteXml();
            }

            SetResult(ResultCode.处理成功, "处理成功");

            if (classObj != null)
            {
                //是否存在方法
                var method = GetMethodInfoByClassType(classObj.GetType(), GetParam<string>("Action", string.Empty));
                if (method == null)
                {
                    SetResult(ResultCode.非法请求, "非法请求");
                    WriteXml();
                    return null;
                }
                //执行方法 不支持有参数的方法
                try
                {
                    method.FastInvoke(classObj, null);
                }
                catch (Exception ex)
                {
                    SetResult(ResultCode.未知异常, "请求失败！" + ex.Message);
                }

            }

            return XmlDoc;
        }



        /// <summary>
        /// 设置返回状态和信息
        /// </summary>
        /// <param name="result"></param>
        /// <param name="description"></param>
        protected void SetResult(ResultCode result, string description)
        {
            XmlDoc.AddOrSetNode("Result", ((int)result).ToString());
            XmlDoc.AddOrSetNode("Description", description);
        }

        /// <summary>
        /// 快速添加xml节点 总条数和列表
        /// <para>节点名：totalCount</para>
        /// <para>节点名：resultData</para>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="html"></param>
        protected void SetTotalAndHtml(int totalCount, string html)
        {
            XmlDoc.AddOrSetNode("totalCount", totalCount.ToString());
            XmlDoc.AddOrSetNode("resultData", html.Trim());
        }
        /// <summary>
        /// 快速添加xml节点
        /// <para>节点名： resultData</para>
        /// </summary>
        /// <param name="html"></param>
        protected void SetXmlHtml(string html)
        {
            XmlDoc.AddOrSetNode("resultData", html.Trim());
        }

        protected T GetParam<T>(string name, T def)
        {
            return HttpContext.Current.Request.GetParam<T>(name, def);
        }

        protected T GetForm<T>(string name, T def)
        {
            return HttpContext.Current.Request.GetForm<T>(name, def);
        }

        protected void WriteXml()
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Write(XmlDoc.InnerXml);
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// 查找类中方法
        /// </summary>
        /// <param name="classModel"></param>
        /// <returns></returns>
        public System.Reflection.MethodInfo GetMethodInfoByClassType(Type classType, string funName)
        {

            if (string.IsNullOrEmpty(funName))
            {
                return null;
            }
            foreach (System.Reflection.MethodInfo method in classType.GetMethods())
            {
                if (method.Name.IsSame(funName))
                {
                    return method;
                }
            }
            return null;
        }
    }
}