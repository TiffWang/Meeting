using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entities;

namespace BusinessTier
{
    public class ActivityManager
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns>数据ID</returns>
        /// by: Tiff.Wang 2014-11-09
        public static string Create(ActivityRow row)
        {
            using (DataBase db = new DataBase())
            {
                db.AddTotblActivity(row);
                db.SaveChanges();
                db.Dispose();
                return row.ActivityID.ToString();
            }


        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        /// by: Tiff.Wang 2014-06-21
        public static bool Update(ActivityRow row)
        {
            using (DataBase db = new DataBase())
            {
                var query = db.tblActivity.Where(n => n.ActivityID == row.ActivityID).FirstOrDefault();
                if (query != null)
                {
                    query.Name = row.Name;
                    query.Type = row.Type;
                    query.City = row.City;
                    query.Address = row.Address;
                    query.IndustryDes = row.IndustryDes;
                    query.AttendTotal = row.AttendTotal;
                    query.SignedTotal = row.SignedTotal;
                    query.SignedAmount = row.SignedAmount;
                    query.StartTime = row.StartTime;
                    query.EndTime = row.EndTime;
                    query.CreateTime = row.CreateTime;
                    query.ActivityTime = row.ActivityTime;

                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static ActivityRow GetDetailById(long? activityID)
        {
            if (activityID <= 0)
                return null;

            DataBase db = new DataBase();
            ActivityRow row = db.tblActivity.Where(n => n.ActivityID == activityID).FirstOrDefault();
            return row;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="activityID"></param>
        /// <returns></returns>
        public static bool Delete(long activityID)
        {
            using (DataBase db = new DataBase())
            {
                var row = db.tblActivity.Single(u => u.ActivityID == activityID);

                if (row != null)
                {
                    db.tblActivity.DeleteObject(row);
                    db.SaveChanges();
                    db.Dispose();

                    return true;
                }
                return false;
            }
        }

        public static List<ActivityRow> GetList()
        {
            using (DataBase db = new DataBase())
            {
                List<ActivityRow> list = null;

                var result = (from a in db.tblActivity
                              orderby a.CreateTime descending
                              select a);

                using (db.Connection)
                {
                    list = result.ToList();
                }
                return list;
            }
        }

        public static CountRow TotalCount()
        {
            using (DataBase db = new DataBase())
            {
                db.ExecuteFunction("ProCount");
                CountRow row = db.tblCount.FirstOrDefault();

                return row;
            }
        }

        public static string TypeCount(string type)
        {
            using (DataBase db = new DataBase())
            {
                int total = 0;
                var query = (from a in db.tblActivity select a).Where(a => a.Type == type);
                if (query.Count() < 0)
                    return total.ToString();

                string strAct = "";
                foreach (var item in query)
                {
                    strAct += item.ActivityID + ",";
                }

                var ids = strAct.Trim(',').Split(',');

            
                var count = (from b in db.tblInvite select b).Where(a => ids.Contains(a.ActivityID));

                
                if (count.Count() > 0)
                {
                    foreach (var row in count)
                    {
                        if (!SessionMgr.Nodes.Contains(row.ProductName))
                        {
                            total++;
                        }
                    }
                }

                return total.ToString();
            }
        }

        public static string TypeSignedCount(string type)
        {
            using (DataBase db = new DataBase())
            {
                int total = 0;
                var query = (from a in db.tblActivity select a).Where(a => a.Type == type);
                if (query.Count() < 0)
                    return total.ToString();

                string strAct = "";
                foreach (var item in query)
                {
                    strAct += item.ActivityID + ",";
                }

                var ids = strAct.Trim(',').Split(',');


                var count = (from b in db.tblInvite select b).Where(a => ids.Contains(a.ActivityID));
                if (count.Count() > 0)
                {
                    foreach (var row in count)
                    {
                        if (!SessionMgr.Nodes.Contains(row.ProductName))
                        {
                            total++;
                        }
                    }
                }

                return total.ToString();
            }
        }
    }
}
