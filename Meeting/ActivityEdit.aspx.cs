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
    public partial class ActivityEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            Binding.BindDropDownList(drpType, typeof(ActivityType));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ActivityRow row = new ActivityRow();
            row.Name = txtName.Text.Trim();
            row.Type = drpType.SelectedValue;
            row.City = txtCity.Text.Trim();
            row.Address = txtAddress.Text.Trim();
            row.IndustryDes = txtIndustDes.Text.Trim();
            row.AttendTotal = txtAttendTotal.Text.ParseTo<int>(0);
            row.SignedTotal = txtSignedTotal.Text.ParseTo<int>(0);
            row.SignedAmount = txtSignedAmount.Text.ParseTo<decimal>(0);
            row.StartTime = txtStartTime.Text.ParseTo<DateTime>(DateTime.Now);
            row.EndTime = txtEndTime.Text.ParseTo<DateTime>(DateTime.Now);
            row.CreateTime = DateTime.Now;


            string result = ActivityManager.Create(row);

            if (result.ParseTo<int>(0) > 0)
                Alert("添加成功");

        }
    }
}