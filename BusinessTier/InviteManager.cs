using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entities;
using Infrastructure;
using System.Data;

namespace BusinessTier
{
    public class InviteManager
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns>数据ID</returns>
        /// by: Tiff.Wang 2014-05-28
        public static string Create(InviteRow row)
        {
            DataBase db = new DataBase();

            db.AddTotblInvite(row);
            db.SaveChanges();
            db.Dispose();

            return row.InviteID.ToString();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// by: Tiff.Wang 2014-06-09
        public static bool Update(InviteRow row)
        {
            using (DataBase db = new DataBase())
            {
                var query = db.tblInvite.Where(n => n.InviteID == row.InviteID).FirstOrDefault();
                if (query != null)
                {
                    query.CustomerID = row.CustomerID;
                    query.CustomerName = row.CustomerName;
                    query.IndustryDes = row.IndustryDes;
                    query.AreaDes = row.AreaDes;
                    query.ContactName = row.ContactName;
                    query.Contact = row.Contact;
                    query.Attend = row.Attend;
                    query.AttendTime = row.AttendTime;
                    query.IsExternal = row.IsExternal;
                    query.Status = row.Status;
                    query.ProductName = row.ProductName;
                    query.ProductAmount = row.ProductAmount;
                    query.SignedTime = row.SignedTime;
                    query.ActivityID = row.ActivityID;

                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="inviteID"></param>
        /// <returns></returns>
        public static bool Delete(long inviteID)
        {
            using (DataBase db = new DataBase())
            {
                var row = db.tblInvite.Single(u => u.InviteID == inviteID);

                if (row != null)
                {
                    db.tblInvite.DeleteObject(row);
                    db.SaveChanges();
                    db.Dispose();

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 根据活动ID删除所有记录
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public static bool DeleteByAct(long activityId)
        {
            using (DataBase db = new DataBase())
            {
                var rowQuery = (from tb in db.tblInvite select tb).Where(a => a.ActivityID == activityId.ToString());

                if (rowQuery != null)
                {
                    foreach (var item in rowQuery)
                    {
                        db.tblInvite.DeleteObject(item);
                        db.SaveChanges();
                        db.Dispose();
                    }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取详细信息
        /// </summary>
        /// <param name="InviteID"></param>
        /// <returns></returns>
        /// by: Tiff.Wang 2014-06-09
        public static InviteRow GetDetailById(long inviteID)
        {
            if (inviteID <= 0)
                return null;

            DataBase db = new DataBase();
            InviteRow row = db.tblInvite.Where(n => n.InviteID == inviteID).FirstOrDefault();
            return row;
        }

        /// <summary>
        /// 新增集合
        /// </summary>
        /// <param name="rowList">数据集合</param>
        /// <returns>成功记录数</returns>
        /// by: Tiff.Wang 2014-05-28
        public static string CreateList(List<InviteRow> rowList)
        {
            DataBase db = new DataBase();

            foreach (var row in rowList)
            {
                db.AddTotblInvite(row);
                db.SaveChanges();
                db.Dispose();
            }
           
            return rowList.Count.ToString();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        /// by: Tiff.Wang 2014-05-28
        public static List<InviteRow> GetList()
        {
            using (DataBase db = new DataBase())
            {
                List<InviteRow> list = null;

                var result = (from a in db.tblInvite
                              orderby a.CreateTime descending
                              select a).Take(50);

                using (db.Connection)
                {
                    list = result.ToList();
                }
                return list;
            }
        }

        /// <summary>
        /// 列表数据分页
        /// </summary>
        /// <param name="pageNow"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        /// by: Tiff.Wang 2014-05-28
        public static List<InviteRow> GetList(string activityId, int pageNow, int pageSize, out int recordCount)
        {
            using (DataBase db = new DataBase())
            {
                List<InviteRow> list = null;
                var rowQuery = (from tb in db.tblInvite select tb).Where(a => a.ActivityID == activityId);
                recordCount = rowQuery.Count();

                var result = (from a in db.tblInvite
                              orderby a.CreateTime descending
                              select a).Where(a => a.ActivityID == activityId).Skip((pageNow - 1) * pageSize).Take(pageSize);

                using (db.Connection)
                {
                    list = result.ToList();
                }
                return list;
            }
        }

        public static List<InviteRow> GetList(int pageNow, int pageSize, out int recordCount)
        {
            using (DataBase db = new DataBase())
            {
                List<InviteRow> list = null;
                var rowQuery = from tb in db.tblInvite select tb;
                recordCount = rowQuery.Count();

                var result = (from a in db.tblInvite
                              orderby a.CreateTime descending
                              select a).Skip((pageNow - 1) * pageSize).Take(pageSize);

                using (db.Connection)
                {
                    list = result.ToList();
                }
                return list;
            }
        }

        /// <summary>
        /// 读取excel数据
        /// </summary>
        /// <param name="pathName">文件路径</param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        /// by: Tiff.Wang 2014-05-28
        public static List<InviteRow> ReaderData(string pathName, string sheetName, long activityId)
        {
            DataTable tb = Common.ExcelToDataTable(pathName, sheetName);

            List<InviteRow> list = new List<InviteRow>();
            foreach (DataRow dr in tb.Rows)
            {
                InviteRow row = new InviteRow();
                row.CustomerID = dr[0].ToString();
                if (string.IsNullOrEmpty(row.CustomerID))
                    continue;

                row.CustomerName = dr[1].ToString();
                row.IndustryDes = dr[2].ToString();
                row.AreaDes = dr[3].ToString();
                row.ContactName = dr[4].ToString();
                row.Contact = dr[5].ToString();
                row.Attend = dr[6].ToString() == "是" ? true : false;

                if (dr[7].ToString() == "")
                    row.AttendTime = null;
                else
                    row.AttendTime = dr[7].ToString().ParseTo<DateTime>(DateTime.Now);

                row.IsExternal = dr[8].ToString() == "是" ? true : false;
                row.Status = dr[9].ToString();
                row.ProductName = dr[10].ToString();
                row.ProductAmount = dr[11].ToString().ParseTo<double>(0);
                if (dr[12].ToString() == "")
                    row.SignedTime = null;
                else
                    row.SignedTime = dr[12].ToString().ParseTo<DateTime>(DateTime.Now);

                row.CreateTime = DateTime.Now;
                row.ActivityID = activityId.ToString();

                list.Add(row);
            }

            return list;
        }


        public static string GetStatistical(string activityId)
        {
            using (DataBase db = new DataBase())
            {
                var rowQuery = (from tb in db.tblInvite select tb).Where(a => a.ActivityID == activityId);
                
                int recordCount = rowQuery.Count();
                int attendCount = rowQuery.Where(a => a.Attend == true).Count();
                int isExternalCount = rowQuery.Where(a => a.IsExternal == false).Count();

                int signedCount = rowQuery.Where(a => a.ProductName != "" && a.ProductAmount > 0 && a.SignedTime != null).Count();
                int signed1 = rowQuery.Where(a => a.ProductName != "" && a.ProductAmount > 0 && a.SignedTime != null && a.Attend == true).Count();
                int signed2 = rowQuery.Where(a => a.ProductName != "" && a.ProductAmount > 0 && a.SignedTime != null && a.IsExternal == false).Count();

                return recordCount.ToString() + "," + attendCount + "," + isExternalCount + "," + signedCount + "," + signed1 + "," + signed2;
            }
        }
    }
}
