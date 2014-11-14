using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BusinessTier
{
    public class Config
    {
        /// <summary>
        /// 管理员ID
        /// </summary>
        public static string AdminId
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminId"];
            }
        }
        /// <summary>
        /// 系统分配员
        /// </summary>
        public static string SystemAssignId
        {
            get
            {
                return ConfigurationManager.AppSettings["SystemAssignId"];
            }
        }
        /// <summary>
        /// 线索审核
        /// </summary>
        public static string ClueCheckOwner
        {
            get
            {
                return ConfigurationManager.AppSettings["ClueCheckOwner"];
            }
        }

        /// <summary>
        /// 线索清洗
        /// </summary>
        public static string ClueCleanOwner
        {
            get
            {
                return ConfigurationManager.AppSettings["ClueCleanOwner"];
            }
        }

        /// <summary>
        /// 线索分配
        /// </summary>
        public static string ClueAssignOwner
        {
            get
            {
                return ConfigurationManager.AppSettings["ClueAssignOwner"];
            }
        }

        /// <summary>
        /// 商机审核
        /// </summary>
        public static string BizCheckOwner
        {
            get
            {
                return ConfigurationManager.AppSettings["BizCheckOwner"];
            }
        }

        /// <summary>
        /// 商机分配
        /// </summary>
        public static string BizAssignOwner
        {
            get
            {
                return ConfigurationManager.AppSettings["BizAssignOwner"];
            }
        }

        /// <summary>
        /// 订单启动
        /// </summary>
        public static string OrderStrat
        {
            get
            {
                return ConfigurationManager.AppSettings["OrderStrat"];
            }
        }

        #region 折扣审核

        /// <summary>
        /// 折扣总监审核
        /// </summary>
        public static int DiscountDirector
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["DiscountDirector"]);
            }
        }
        /// <summary>
        /// 折扣副总审核
        /// </summary>
        public static int DiscountVGM
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["DiscountVGM"]);
            }
        }
        /// <summary>
        /// 折扣总经理审核
        /// </summary>
        public static int DiscountGM
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["DiscountGM"]);
            }
        }

        #endregion

        public static bool IsLoginByUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["IsLoginByUserName"].ParseTo<bool>(false);
            }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version
        {
            get
            {
                return ConfigurationManager.AppSettings["Version"];
            }
        }

        public static string CCUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CCUrl"];
            }
        }

        public static string BPMUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BPMUrl"];
            }
        }
    }
}
