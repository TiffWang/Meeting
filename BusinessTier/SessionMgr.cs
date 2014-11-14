using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Entities;

namespace BusinessTier
{
    public class SessionMgr : SessionMgrBase
    {
        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public static string UserId
        {
            get
            {
                return GetSession<string>(SessionName.UserId, "");
            }
            set
            {
                SetSession<string>(SessionName.UserId, value);
            }
        }



        public static string LoginName
        {
            get
            {
                return GetSession<string>(SessionName.LoginName, "");
            }
            set
            {
                SetSession<string>(SessionName.LoginName, value);
            }
        }

        public static string FunId
        {
            get
            {
                return GetSession<string>(SessionName.FunId, "");
            }
            set
            {
                SetSession<string>(SessionName.FunId, value);
            }
        }

        public static string Pid
        {
            get
            {
                return GetSession<string>(SessionName.Pid, "");
            }
            set
            {
                SetSession<string>(SessionName.Pid, value);
            }
        }

        public static string FinalId
        {
            get
            {
                return GetSession<string>(SessionName.FinalId, "");
            }
            set
            {
                SetSession<string>(SessionName.FinalId, value);
            }
        }

        public static string EmplId
        {
            get
            {
                return GetSession<string>(SessionName.EmplId, "");
            }
            set
            {
                SetSession<string>(SessionName.EmplId, value);
            }
        }

        public static string BackUrl
        {
            get
            {
                return GetSession<string>(SessionName.BackUrl, "");
            }
            set
            {
                SetSession<string>(SessionName.BackUrl, value);
            }
        }

        public static string ActivityID
        {
            get
            {
                return GetSession<string>(SessionName.ActivityID, "");
            }
            set
            {
                SetSession<string>(SessionName.ActivityID, value);
            }
        }
        /// <summary>
        /// 任务处理人ID
        /// </summary>
        public static string TaskAssignee
        {
            get
            {
                return GetSession<string>(SessionName.TaskAssignee, "");
            }
            set
            {
                SetSession<string>(SessionName.TaskAssignee, value);
            }
        }


   


        public static string Nodes
        {
            get
            {
                return GetSession<string>(SessionName.Nodes, "");
            }
            set
            {
                SetSession<string>(SessionName.Nodes, value);
            }
        }
    }
}
