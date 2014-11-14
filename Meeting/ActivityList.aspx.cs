using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entities;
using BusinessTier;
using System.Data;

namespace Meeting
{
    public partial class ActivityList : PageBase
    {
        public string Total;
        public string Signed;
        public string Quarter;
        public string QuaTotal;
        public string QuaSigned;
        public string Week;
        public string WeekSigned;
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["Action"].ParseTo<string>("");
            long activityId = Request["ActivityID"].ParseTo<long>(-1);

            if (action == "del")
            {
                if (ActivityManager.Delete(activityId) && InviteManager.DeleteByAct(activityId))
                    Alert("删除成功");
                else
                    Alert("删除失败");
            }

            CountRow row = ActivityManager.TotalCount();
            if (row != null)
            {
                Total = row.Total.ToString();
                Signed = row.Signed.ToString();
                Quarter = row.Quarter.ToString();
                QuaTotal = row.QuaTotal.ToString();
                QuaSigned = row.QuaSigned.ToString();
                Week = row.Week.ToString();
                WeekSigned = row.WeekSigned.ToString();
            }
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            List<ActivityRow> list = ActivityManager.GetList();

            string excel = ExcelExport.CreateExcelTable(CreateDataSet(list), AppDomain.CurrentDomain.BaseDirectory + "Export", "Activity", "/Export");

            Response.Redirect(excel, true);
        }


        DataSet CreateDataSet(List<ActivityRow> list)
        {
            DataSet ds = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("活动主题", typeof(System.String));
            dt.Columns.Add("活动分类", typeof(System.String));
            dt.Columns.Add("所在城市", typeof(System.String));
            dt.Columns.Add("覆盖行业", typeof(System.String));
            dt.Columns.Add("举办日期", typeof(System.String));
            dt.Columns.Add("市场获取线索总数", typeof(System.String));
            dt.Columns.Add("活动覆盖的市场线索数量", typeof(System.String));
            dt.Columns.Add("市场活动有效影响线索数", typeof(System.String));
            dt.Columns.Add("市场活动销售客保线索数", typeof(System.String));
            dt.Columns.Add("市场获取线索总成单数", typeof(System.String));
            dt.Columns.Add("活动覆盖的库外线索成单数", typeof(System.String));
            dt.Columns.Add("市场活动有效影响线索成单数", typeof(System.String));
            dt.Columns.Add("市场活动销售客保线索成单数", typeof(System.String));
        

            //先用字典缓存所有的company 
            //var companyList = ERPFileHelper.FileOperate.GetXmlSysCodeJSONByType("company").ToDictionary(q => q.CODE, v => v.VALUE);
            //修改使用下标
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.Name;
                dr[1] = Enum.GetName(typeof(ActivityType), item.Type.ParseTo<int>(0));
                dr[2] = item.City;
                dr[3] = item.IndustryDes;
                dr[4] = item.StartTime;
                dr[5] = InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[0];
                dr[6] = InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[1];
                dr[7] = InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[2];
                dr[8] = 0;
                dr[9] = InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[3];
                dr[10] = InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[4];
                dr[11] = InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[5];
                dr[12] = 0;

                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            return ds;
        }
    }
}