using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessTier;
using Entities;

namespace Meeting.Ajax
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : AjaxBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            base.Init(context, this);
            base.WriteXml();
        }

        public void GetCluesList()
        {
            int pageIndex = base.GetParam<int>("pageIndex", 1);
            int pageSize = base.GetParam<int>("pageSize", 10);

            string customerName = base.GetForm<string>("customerName", "");
            string contact = base.GetForm<string>("contact", "");
            string phoneNo = base.GetForm<string>("phoneNo", "");
            string  activityId = base.GetParam<string>("activityId", "");


            string customerId = base.GetForm<string>("customerId", "");
            string status = base.GetForm<string>("ctl00$MainContent$drpClueStatus", "");
            string orderType = base.GetForm<string>("orderType", "asc").ToLower().Trim();
            string orderField = base.GetForm<string>("orderField", "customername").ToLower().Trim();
            string ProductRequire = base.GetForm<string>("ctl00$MainContent$drpProductRequire", "");
            string assignedTo = string.Empty;
            string entryby = string.Empty;
            //assignedTo分配给谁
            //entryby 创建者.
            pageSize = 20;


            string divListHtml = "";
            int outcount;
            List<InviteRow> list = InviteManager.GetList(activityId, pageIndex, pageSize, out outcount);

            divListHtml = @"<table  cellpadding='0' cellspacing='0' style='width: 150%;'>
       <thead>
        <tr>
        <td>
            <div class='table-headerCell'>
                序号</div>
        </td>
        <td>
            <div class='table-headerCell'>
                活动主题</div>
        </td>
        <td>
            <div class='table-headerCell'>
                客户名称</div>
        </td>
        <td>
            <div class='table-headerCell'>
                行业</div>
        </td>
        <td>
            <div class='table-headerCell'>
                地域</div>
        </td>
        <td>
            <div class='table-headerCell'>
                联系人</div>
        </td>
        <td>
            <div class='table-headerCell'>
                联系方式</div>
        </td>
        <td>
            <div class='table-headerCell'>
                参会情况</div>
        </td>
        <td>
            <div class='table-headerCell'>
                参会时间</div>
        <td>
            <div class='table-headerCell'>
                是否库外线索</div>
        <td>
            <div class='table-headerCell'>
                状态</div>
        </td>
        <td>
            <div class='table-headerCell'>
                签单产品</div>
        </td>
        <td>
            <div class='table-headerCell'>
                签单金额</div>
        </td>
        <td>
            <div class='table-headerCell'>
                签单时间</div>
        </td>
        <td>
            <div class='table-headerCell'>
                创建时间</div>
        </td>
        <td>
            <div class='table-headerCell'>
                操作</div>
        </td>
    </tr>
</thead>
<tbody>";

            foreach (var item in list)
            {
                string attend = (bool)item.Attend ? "是" : "否";
                //string isExternal = (bool)item.IsExternal ? "是" : "否";
                string isExternal = !CustomerManager.isExist(item.CustomerName) ? "是" : "否";
                string attendTime = "";
                if (item.AttendTime != null)
                    attendTime = ((DateTime)item.AttendTime).ToString(23);
                string signedTime = "";
                if (item.SignedTime != null)
                    signedTime = ((DateTime)item.SignedTime).ToString(23);

                divListHtml += @"<tr onmouseover=""$(this).addClass('hover')"" onmouseout=""$(this).removeClass('hover')"">";
                divListHtml += @"<td>" + item.InviteID + "</td>";
                divListHtml += @"<td>" + ActivityManager.GetDetailById(item.ActivityID.ParseTo<long>(-1)).Name + "</td>";
                //divListHtml += @"<td>" + item.CustomerID + "</td>";
                divListHtml += @"<td>" + item.CustomerName + "</td>";
                divListHtml += @"<td>" + item.IndustryDes + "</td>";
                divListHtml += @"<td>" + item.AreaDes + "</td>";
                divListHtml += @"<td>" + item.ContactName + "</td>";
                divListHtml += @"<td>" + item.Contact + "</td>";
                divListHtml += @"<td>" + attend + "</td>";
                divListHtml += @"<td>" + attendTime + "</td>";
                divListHtml += @"<td>" + isExternal + "</td>";
                divListHtml += @"<td>" + item.Status + "</td>";
                divListHtml += @"<td>" + item.ProductName + "</td>";
                divListHtml += @"<td>" + item.ProductAmount + "</td>";
                divListHtml += @"<td>" + signedTime + "</td>";
                divListHtml += @"<td>" + ((DateTime)item.CreateTime).ToString(23) + "</td>";
                divListHtml += @"<td><a href='MeetingUpdate.aspx?InviteID=" + item.InviteID.ToString() + "&ActivityId=" + SessionMgr.ActivityID + "'>修改</a>&nbsp;&nbsp;<a href='javascript:void(0)' onclick='Delete(" + item.InviteID.ToString() + ")'>删除</a></td>";
                divListHtml += "</tr>";
            }


            divListHtml += @"</tbody></table>";


            XmlDoc.AddOrSetNode("divListHtml", divListHtml.Trim());
            XmlDoc.AddOrSetNode("totalCount", outcount.ToString());
        }

        public void GetBaiduList()
        {
            int pageIndex = base.GetParam<int>("pageIndex", 1);
            int pageSize = base.GetParam<int>("pageSize", 10);

            string customerName = base.GetForm<string>("customerName", "");
            string contact = base.GetForm<string>("contact", "");
            string phoneNo = base.GetForm<string>("phoneNo", "");

            string customerId = base.GetForm<string>("customerId", "");
            string status = base.GetForm<string>("ctl00$MainContent$drpClueStatus", "");
            string orderType = base.GetForm<string>("orderType", "asc").ToLower().Trim();
            string orderField = base.GetForm<string>("orderField", "customername").ToLower().Trim();
            string ProductRequire = base.GetForm<string>("ctl00$MainContent$drpProductRequire", "");
            string assignedTo = string.Empty;
            string entryby = string.Empty;
            //assignedTo分配给谁
            //entryby 创建者.
            pageSize = 20;


            string divListHtml = "";
            int outcount;
            List<InviteRow> list = InviteManager.GetList(pageIndex, pageSize, out outcount);

            divListHtml = @"<table  cellpadding='0' cellspacing='0' style='width: 150%;'>
        <thead>
        <tr>
        <td>
            <div class='table-headerCell'>
                序号</div>
        </td>
        <td>
            <div class='table-headerCell'>
                活动主题</div>
        </td>
        <td>
            <div class='table-headerCell'>
                客户名称</div>
        </td>
        <td>
            <div class='table-headerCell'>
                行业</div>
        </td>
        <td>
            <div class='table-headerCell'>
                地域</div>
        </td>
        <td>
            <div class='table-headerCell'>
                联系人</div>
        </td>
        <td>
            <div class='table-headerCell'>
                联系方式</div>
        </td>
        <td>
            <div class='table-headerCell'>
                参会情况</div>
        </td>
        <td>
            <div class='table-headerCell'>
                参会时间</div>
        <td>
            <div class='table-headerCell'>
                是否库外线索</div>
        <td>
            <div class='table-headerCell'>
                状态</div>
        </td>
        <td>
            <div class='table-headerCell'>
                签单产品</div>
        </td>
        <td>
            <div class='table-headerCell'>
                签单金额</div>
        </td>
        <td>
            <div class='table-headerCell'>
                签单时间</div>
        </td>
    </tr>
</thead>
<tbody>";

            foreach (var item in list)
            {
                string attend = (bool)item.Attend ? "是" : "否";
                string isExternal = (bool)item.IsExternal ? "是" : "否";
                string attendTime = "";
                if (item.AttendTime != null)
                    attendTime = ((DateTime)item.AttendTime).ToString(23);
                string signedTime = "";
                if (item.SignedTime != null)
                    signedTime = ((DateTime)item.SignedTime).ToString(23);

                divListHtml += @"<tr onmouseover=""$(this).addClass('hover')"" onmouseout=""$(this).removeClass('hover')"">";
                divListHtml += @"<td>" + item.InviteID + "</td>";
                divListHtml += @"<td>" + ActivityManager.GetDetailById(item.ActivityID.ParseTo<long>(-1)).Name + "</td>";
                //divListHtml += @"<td>" + item.CustomerID + "</td>";
                divListHtml += @"<td>" + item.CustomerName + "</td>";
                divListHtml += @"<td>" + item.IndustryDes + "</td>";
                divListHtml += @"<td>" + item.AreaDes + "</td>";
                divListHtml += @"<td>" + item.ContactName + "</td>";
                divListHtml += @"<td>" + item.Contact + "</td>";
                divListHtml += @"<td>" + attend + "</td>";
                divListHtml += @"<td>" + attendTime + "</td>";
                divListHtml += @"<td>" + isExternal + "</td>";
                divListHtml += @"<td>" + item.Status + "</td>";
                divListHtml += @"<td>" + item.ProductName + "</td>";
                divListHtml += @"<td>" + item.ProductAmount + "</td>";
                divListHtml += @"<td>" + signedTime + "</td>";
                divListHtml += "</tr>";
            }


            divListHtml += @"</tbody></table>";


            XmlDoc.AddOrSetNode("divListHtml", divListHtml.Trim());
            XmlDoc.AddOrSetNode("totalCount", outcount.ToString());
        }

        public void GetActivityList()
        {
            int pageIndex = base.GetParam<int>("pageIndex", 1);
            int pageSize = base.GetParam<int>("pageSize", 10);

            string customerName = base.GetForm<string>("customerName", "");
            string contact = base.GetForm<string>("contact", "");
            string phoneNo = base.GetForm<string>("phoneNo", "");

            string customerId = base.GetForm<string>("customerId", "");
            string status = base.GetForm<string>("ctl00$MainContent$drpClueStatus", "");
            string orderType = base.GetForm<string>("orderType", "asc").ToLower().Trim();
            string orderField = base.GetForm<string>("orderField", "customername").ToLower().Trim();
            string ProductRequire = base.GetForm<string>("ctl00$MainContent$drpProductRequire", "");
            string assignedTo = string.Empty;
            string entryby = string.Empty;
            //assignedTo分配给谁
            //entryby 创建者.
            pageSize = 20;


            string divListHtml = "";
            int outcount;
            List<ActivityRow> list = ActivityManager.GetList();

            divListHtml = @"<table  cellpadding='0' cellspacing='0' style='width: 170%;'>
        <thead>
        <tr>
        <td>
            <div class='table-headerCell'>
                活动主题</div>
        </td>
        <td>
            <div class='table-headerCell'>
                活动分类</div>
        </td>
        <td>
            <div class='table-headerCell'>
                目标客户行业</div>
        </td>
        <td>
            <div class='table-headerCell'>
                预计参会</div>
        </td>
        <td>
            <div class='table-headerCell'>
                预计签单</div>
        </td>
        <td>
            <div class='table-headerCell'>
                预计签单金额</div>
        </td>
        <td>
            <div class='table-headerCell'>
                市场获取线索总数</div>
        </td>
        <td>
            <div class='table-headerCell'>
                活动覆盖的市场线索数量</div>
        </td>
        <td>
            <div class='table-headerCell'>
                市场活动有效影响线索数</div>
        </td>
        <td>
            <div class='table-headerCell'>
                市场活动销售客保线索数</div>
        </td>
        <td>
            <div class='table-headerCell'>
                市场获取线索总成单数</div>
        </td>
        <td>
            <div class='table-headerCell'>
                活动覆盖的库外线索成单数</div>
        </td>
        <td>
            <div class='table-headerCell'>
                市场活动有效影响线索成单数</div>
        </td>
        <td>
            <div class='table-headerCell'>
                市场活动销售客保线索成单数</div>
        </td>
        <td>
            <div class='table-headerCell'>
                创建时间</div>
        </td>
        <td>
            <div class='table-headerCell'>
                操作</div>
        </td>
    </tr>
</thead>
<tbody>";

            foreach (var item in list)
            {
                //string attend = (bool)item.Attend ? "是" : "否";
                //string isExternal = (bool)item.IsExternal ? "是" : "否";
                //string attendTime = "";
                //if (item.AttendTime != null)
                //    attendTime = ((DateTime)item.AttendTime).ToString(23);
                //string signedTime = "";
                //if (item.SignedTime != null)
                //    signedTime = ((DateTime)item.SignedTime).ToString(23);

                divListHtml += @"<tr onmouseover=""$(this).addClass('hover')"" onmouseout=""$(this).removeClass('hover')"">";
                divListHtml += @"<td>" + item.Name + "</td>";
                divListHtml += @"<td>" + Enum.GetName(typeof(ActivityType), item.Type.ParseTo<int>(0)) + "</td>";
                divListHtml += @"<td>" + item.IndustryDes + "</td>";
                divListHtml += @"<td>" + item.AttendTotal + "</td>";
                divListHtml += @"<td>" + item.SignedTotal + "</td>";
                divListHtml += @"<td>" + item.SignedAmount + "</td>";
                divListHtml += @"<td>" + InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[0] + "</td>";
                divListHtml += @"<td>" + InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[1] + "</td>";
                divListHtml += @"<td>" + InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[2] + "</td>";
                divListHtml += @"<td>0</td>";
                divListHtml += @"<td>" + InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[3] + "</td>";
                divListHtml += @"<td>" + InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[4] + "</td>";
                divListHtml += @"<td>" + InviteManager.GetStatistical(item.ActivityID.ToString()).Split(',')[5] + "</td>";
                divListHtml += @"<td>0</td>";
                divListHtml += @"<td>" + item.CreateTime + "</td>";
                divListHtml += @"<td><a href='MeetingListInput.aspx?ActivityId=" + item.ActivityID + "'>查看</a>&nbsp;&nbsp;<a href='javascript:void(0)' onclick='Delete(" + item.ActivityID.ToString() + ")'>删除</a></td>";
                divListHtml += "</tr>";
            }


            divListHtml += @"</tbody></table>";


            XmlDoc.AddOrSetNode("divListHtml", divListHtml.Trim());
            // XmlDoc.AddOrSetNode("totalCount", outcount.ToString());
        }
    }
}