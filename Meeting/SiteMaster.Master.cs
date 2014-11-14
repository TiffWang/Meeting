using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;

using BusinessTier;

namespace Site
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //string script = string.Format(@"top.strLang = ""{0}"";", RegMgr.GetCulture());
            //this.Page.ClientScript.RegisterClientScriptBlock(typeof(MasterPage), "", script, true);

            string pid = Request["pid"];
            string funId = Request["funId"];
            //spanUserName.InnerHtml = SessionMgr.Empl.EmplNo + "，" + SessionMgr.Empl.RealName;

            string pName = "";
            string fName = "";

            //List<EmplFunction> top = SessionMgr.RoleFunctions.Where(n => n.FunLevel == 1 && n.Type == "1").OrderBy(n => n.No).ToList();
            string menu = "<li class='side'></li>";
            int i = 1;
            //foreach (EmplFunction funcc in top)
            //{
            //    if (pid == funcc.FunId)
            //    {
            //        menu += string.Format(@"<li><a href='{0}?pid={1}' class='selected'>{3}</a></li>", funcc.Url, funcc.FunId, i, funcc.Name);
            //        pName = funcc.Name;
            //    }
            //    else
            //    {
            //        menu += string.Format(@"<li><a href='{0}?pid={1}'>{3}</a></li>", funcc.Url, funcc.FunId, i, funcc.Name);
            //    }
            //    i++;
            //}
            if(SessionMgr.UserId == "01000")
                menu = "<li><a href='ActivityList.aspx' class='selected'>活动管理</a></li>";
            else
                menu = "<li><a href='MeetingList.aspx' class='selected'>数据列表</a></li>";

            menuList.InnerHtml = menu;

            if (!string.IsNullOrWhiteSpace(pid))
            {
                string submenuHtml = "";
                // menuName = ComptManager.ComptFuncGetById(pid).NAME;

                //List<EmplFunction> subList = SessionMgr.RoleFunctions.Where(n => n.ParentId == pid).OrderBy(n => n.No).ToList();

                //foreach (EmplFunction sub in subList)
                //{
                //    if (funId == "")
                //    {
                //        funId = subList[0].FunId;
                //        SessionMgr.FunId = funId;
                //    }

                //    if (funId == sub.FunId)
                //    {
                //        submenuHtml += string.Format(@"<a href='{0}?funId={1}&pid={2}' class='selected'>{3}</a> |  ", sub.Url, sub.FunId, sub.ParentId, sub.Name);
                //        fName = sub.Name;
                //    }
                //    else
                //        submenuHtml += string.Format(@"<a href='{0}?funId={1}&pid={2}'>{3}</a> |  ", sub.Url, sub.FunId, sub.ParentId, sub.Name);
                //}

                subNav.InnerHtml = submenuHtml;
            }

            divPath.InnerHtml = "";
            if (pName != "")
                divPath.InnerHtml += "您正在：" + pName;
            if (fName != "")
                divPath.InnerHtml += "> <font color='#0c69b5'>" + fName + "</font>";
        }
    }
}