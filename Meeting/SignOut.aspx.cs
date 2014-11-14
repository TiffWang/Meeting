using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;
using BusinessTier;
using Infrastructure;
namespace Site
{
    public partial class SignOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SessionMgr.RoleFunctions = null;
            SessionMgr.UserId = "";
            //SessionMgr.Empl = null;
            //SessionMgr.CurRoleIds = null;
            //Response.Headers.Add("","");
            this.ClientScript.RegisterClientScriptBlock(typeof(string), "script", "logOut()", true);

            //Response.Redirect("http://" + System.Web.HttpContext.Current.Request.Url.Host.ToString() + ":"+System.Web.HttpContext.Current.Request.Url.Port + "/signin.aspx",);
        }


    }
}