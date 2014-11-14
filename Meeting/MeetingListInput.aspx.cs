using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;
using BusinessTier;

namespace Meeting
{
    public partial class MeetingListInput : PageBase
    {
        public string activityID;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["Action"].ParseTo<string>("");
            long inviteID = Request["InviteID"].ParseTo<long>(-1);

            if (action == "del")
            {
                if (InviteManager.Delete(inviteID))
                    Alert("删除成功");
                else
                    Alert("删除失败");
            }

            if (IsPostBack)
                return;

            List<ActivityRow> list = ActivityManager.GetList();
            activityID = Request["ActivityID"];
            Binding.DropFill(drpActivity, list, "Name", "ActivityID");
            drpActivity.SelectedValue = activityID;
            SessionMgr.ActivityID = activityID;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string pathName = AppDomain.CurrentDomain.BaseDirectory + txtPath.Value;

            string result = "";
            try
            {
                result = InviteManager.CreateList(InviteManager.ReaderData(pathName, "", drpActivity.SelectedValue.ParseTo<long>(0)));
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return;
            }

            Alert("成功导入" + result + "条记录");
            txtPath.Value = "";
        }
    }
}