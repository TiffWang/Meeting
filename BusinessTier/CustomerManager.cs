using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entities;

namespace BusinessTier
{
    public class CustomerManager
    {
        public static bool isExist(string customerName)
        {
            using (DataBase db = new DataBase())
            {
                var rows = db.tblCustomer.Where(n => n.CustomerName.Contains(customerName)).ToList();
                if (rows.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public static string GetNames()
        {
            using (DataBase db = new DataBase())
            {
                string names="";
                var rows = db.tblCustomer.ToList();
                foreach (var item in rows)
                {
                    names += item.CustomerName + ",";
                }
                return names;
            }
        }
    }
}
