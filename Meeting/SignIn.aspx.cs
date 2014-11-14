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
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string loginName = txtLoginName.Text.Trim();
            string password = Security.MD5Encrypt(txtPassword.Text.Trim());

            if (loginName == "01000" && password == "528048836D7FF733F71DFA6BC9CEDBC5")
            {
                // SessionMgr.Empl = EmplManager.EmplGetByNo(loginName);
                SessionMgr.UserId = loginName;//SessionMgr.Empl.EmplId;
                SessionMgr.LoginName = loginName;
                // SessionMgr.RoleFunctions = ComptManager.PermissionsGetByloginName(SessionMgr.LoginName, subId);
                SessionMgr.Nodes = CustomerManager.GetNames();
                Response.Redirect("ActivityList.aspx", true);
            }
            else if (loginName == "01001" && password == "08A168032CCC785DD72B00816E44DEE6")
            {
                SessionMgr.UserId = loginName;
                SessionMgr.LoginName = loginName;
                Response.Redirect("MeetingList.aspx", true);
            }
            else
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(SignIn), "script", "alert(\"用户名密码错误\")", true);
            }
            //else
            //{
            //    Result result = ComptManager.EmployeeAuthenticationByNo(loginName, password);

            //    if (result.resultCode != 0)
            //    {
            //        if (Config.IsLoginByUserName)
            //            result = ComptManager.EmplAuthentication(loginName, password);

            //        if (result.resultCode != 0)
            //        {
            //            this.ClientScript.RegisterClientScriptBlock(typeof(SignIn), "script", "alert(\"" + result.resultMessage.HtmlAttributeEncode() + "\")", true);
            //        }
            //        else
            //        {
            //            string subId = "10000000";
            //            SessionMgr.UserId = result.returnValue;
            //            SessionMgr.LoginName = loginName;
            //            SessionMgr.Empl = EmplManager.EmplGetByLoginName(loginName);
            //            SessionMgr.RoleFunctions = ComptManager.PermissionsGetByloginName(loginName, subId);
            //            if (SessionMgr.Empl.LoginPassword == "7066A40F427769CC43347AA96B72931A")
            //                Response.Redirect("/Empl/EmplUpdatePwd.aspx", true);
            //            else
            //                Response.Redirect("Desktop.aspx", true);
            //        }
            //        //lblLoginInfo.Text = "登陆失败";
            //    }
            //    else
            //    {
            //        string subId = "10000000";
            //        SessionMgr.UserId = result.returnValue;
            //        SessionMgr.Empl = EmplManager.EmplGetByNo(loginName);
            //        SessionMgr.LoginName = SessionMgr.Empl.LoginName;
            //        SessionMgr.RoleFunctions = ComptManager.PermissionsGetByloginName(SessionMgr.LoginName, subId);
            //        if (SessionMgr.Empl.LoginPassword == "7066A40F427769CC43347AA96B72931A")
            //            Response.Redirect("/Empl/EmplUpdatePwd.aspx", true);
            //        else
            //            Response.Redirect("Desktop.aspx", true);
            //    }
            //}
        }
    }
}