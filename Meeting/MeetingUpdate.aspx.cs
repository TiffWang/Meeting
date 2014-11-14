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
    public partial class MeetingUpdate : PageBase
    {
        InviteRow row;
        protected void Page_Load(object sender, EventArgs e)
        {
            long inviteID = Request["InviteID"].ParseTo<long>(-1);
            row = InviteManager.GetDetailById(inviteID);
            if (IsPostBack)
                return;

            txtCustomerId.Text = row.CustomerID;
            txtCustomerName.Text = row.CustomerName;
            txtIndustryDes.Text = row.IndustryDes;
            txtAreaDes.Text = row.AreaDes;
            txtContactName.Text = row.ContactName;
            txtContact.Text = row.Contact;
            txtAttendTime.Text = row.AttendTime.ToString();
            txtStatus.Text = row.Status;
            txtProductName.Text = row.ProductName;
            txtProductAmount.Text = row.ProductAmount.ToString();
            txtSignedTime.Text = row.SignedTime.ToString();

            if ((bool)row.Attend)
                radAttend.SelectedValue = "1";
            else
                radAttend.SelectedValue = "0";

            if ((bool)row.IsExternal)
                radIsExternal.SelectedValue = "1";
            else
                radIsExternal.SelectedValue = "0";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            row.CustomerID = txtCustomerId.Text;
            row.CustomerName = txtCustomerName.Text;
            row.IndustryDes = txtIndustryDes.Text;
            row.AreaDes = txtAreaDes.Text;
            row.ContactName = txtContactName.Text;
            row.Contact = txtContact.Text;
            row.AttendTime = txtAttendTime.Text.ParseTo<DateTime>(DateTime.Now);
            row.Status = txtStatus.Text;
            row.ProductName = txtProductName.Text;
            row.ProductAmount = txtProductAmount.Text.ParseTo<double>(0);
            row.SignedTime = txtSignedTime.Text.ParseTo<DateTime>(DateTime.Now);
            row.Attend = false;
            row.IsExternal = false;

            if (radAttend.SelectedValue == "1")
                row.Attend = true;

            if (radIsExternal.SelectedValue == "1")
                row.IsExternal = true;

            bool result = InviteManager.Update(row);
            if (result)
                Alert("修改成功", "MeetingListInput.aspx?ActivityId=" + SessionMgr.ActivityID);
        }
    }
}