using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;

namespace Infrastructure
{
    public class DBOracle
    {
        /// <summary>
        /// 链接字符
        /// </summary>
        public static readonly string CONNSTRING = System.Configuration.ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;


        /// <summary>
        /// 例子
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataTable()
        {
            //定义参数，注意参数名必须与存储过程定义时一致，且类型为OracleType.Cursor
            //OracleParameter cur_set = new OracleParameter("cur", OracleDbType.RefCursor);
            //设置参数为输出类型
            //cur_set.Direction = ParameterDirection.Output;
            //添加参数
            OracleParameter[] cur_set = new OracleParameter[3];
            cur_set[0] = new OracleParameter(":v_sdate", OracleDbType.Date);
            cur_set[0].Value = DateTime.Now.AddMonths(-1);
            cur_set[0].Direction = ParameterDirection.Input;

            cur_set[1] = new OracleParameter(":v_edate", OracleDbType.Date);
            cur_set[1].Value = DateTime.Now;
            cur_set[1].Direction = ParameterDirection.Input;

            cur_set[2] = new OracleParameter(":cur", OracleDbType.RefCursor);
            cur_set[2].Direction = ParameterDirection.Output;

            //注意：包名.存储过程名的形式

            return DBOracle.ExecuteDataTable("PKG_TEST.get_data", CommandType.StoredProcedure, cur_set);
        }

        /// <summary>
        /// 装载DataTable
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string cmdText, CommandType cmdType, OracleParameter[] cmdParms)
        {
            using (OracleConnection conn = new OracleConnection(CONNSTRING))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(cmdText, conn))
                {
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(cmdParms);
                    var oda = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    return dt;
                }
            }
        }



    }
}

