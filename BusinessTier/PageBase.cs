using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Infrastructure;

namespace BusinessTier
{
    public class PageBase : System.Web.UI.Page
    {
        protected void Alert(string message)
        {
            string js = "Boxy.alert('" + message + ".', null, {title: '提示'});";
            this.ClientScript.RegisterClientScriptBlock(typeof(PageBase), "script", js, true);
        }

        protected void Alert(string message,string url)
        {
            string js = "Boxy.alert('" + message + ".', function() { window.location.href = '" + url + "'; }, {title: '提示'});";
            this.ClientScript.RegisterClientScriptBlock(typeof(PageBase), "script", js, true);
        }

        //protected void Alert(string message, string callback)
        //{
        //    string js = "Boxy.alert('" + message + ".', " + callback + ", {title: '提示'});";
        //    this.ClientScript.RegisterClientScriptBlock(typeof(PageBase), "script", js, true);
        //}

        protected void Alert(string message, string callback, string options)
        {
            string js = "Boxy.alert('" + message + ".', " + callback + ", {title: '" + options + "'});";
            this.ClientScript.RegisterClientScriptBlock(typeof(PageBase), "script", js, true);
        }

        /// <summary>
        /// 确认选择对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback">确认返回执行脚本</param>
        /// <param name="options"></param>
        protected void Confirm(string message, string callback, string options)
        {
            string js = "Boxy.confirm('" + message + "?', " + callback + ", {title: '" + options + "'});";
            this.ClientScript.RegisterClientScriptBlock(typeof(PageBase), "script", js, true);
        }

        /// <summary>
        /// 确认选择对话框,指定返回链接
        /// </summary>
        /// <param name="message"></param>
        /// <param name="url">确认返回URL</param>
        /// <param name="options"></param>
        protected void ConfirmRedirect(string message, string url, string options)
        {
            string js = "Boxy.confirm('" + message + "?', function() { window.location.href = '" + url + "'; }, {title: '" + options + "'});";
            this.ClientScript.RegisterClientScriptBlock(typeof(PageBase), "script", js, true);
        }
    }
}
