using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.OracleClient;

//using System.Data.SqlServerCe;

namespace Infrastructure
{
    /// <summary>
    /// Database connection type
    /// </summary>
    public enum DbConnectionType
    {
        /// <summary>
        /// My Sql Server
        /// </summary>
        MySql,
        /// <summary>
        /// Microsoft Sql Server
        /// </summary>
        MSSql,
        /// <summary>
        /// Microsoft Sql Server for CE
        /// </summary>
        MSSqlCe,
        /// <summary>
        /// Oracle Database
        /// </summary>
        Oracle
    }
    /// <summary>
    /// Database helper classs
    /// </summary>
    public partial class DB
    {
        /// <summary>
        /// Connection type
        /// </summary>
        public DbConnectionType ConnectionType { get; set; }

        private char ParameterLeader { get; set; }
        /// <summary>
        /// Connection object
        /// </summary>
        public DbConnection Connection { get; set; }
        /// <summary>
        /// Command object
        /// </summary>
        public DbCommand Command { get; set; }
        /// <summary>
        /// Data adapter object
        /// </summary>
        public DbDataAdapter DataAdapter { get; set; }
        /// <summary>
        /// Default connection string. If you have not assume the connection string when intialize the DB class. Always use this string.
        /// </summary>
        public static string ConnectionString { get; set; }

        #region database instance
        /// <summary>
        /// Initialize DB class
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="dbConnectionType">Database connection type</param>
        public DB(string connectionString, DbConnectionType dbConnectionType)
        {
            Initialize(connectionString, dbConnectionType);
        }
        /// <summary>
        /// Initialize DB class
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        public DB(string connectionString)
        {
            Initialize(connectionString, DbConnectionType.MSSql);
        }
        /// <summary>
        /// Initialize DB class
        /// </summary>
        public DB()
        {
            Initialize(DB.ConnectionString, DbConnectionType.MSSql);
        }
        /// <summary>
        /// Initialize DB class
        /// </summary>
        /// <param name="dbConnectionType">Connnection type</param>
        public DB(DbConnectionType dbConnectionType)
        {
            Initialize(DB.ConnectionString, dbConnectionType);
        }

        private void Initialize(string connectionString, DbConnectionType dbConnectionType)
        {
            DB.ConnectionString = connectionString;
            ConnectionType = dbConnectionType;
            switch (dbConnectionType)
            {
                case DbConnectionType.MySql:
                    Connection = new MySqlConnection(connectionString);
                    Command = Connection.CreateCommand();
                    DataAdapter = new MySqlDataAdapter((MySqlCommand)Command);
                    ParameterLeader = '?';
                    break;
                case DbConnectionType.MSSql:
                    Connection = new SqlConnection(connectionString);
                    Command = Connection.CreateCommand();
                    DataAdapter = new SqlDataAdapter((SqlCommand)Command);
                    ParameterLeader = '@';
                    break;
                //case DbConnectionType.MSSqlCe:
                //    Connection      = new SqlCeConnection(connectionString);
                //    Command         = Connection.CreateCommand();
                //    DataAdapter     = new SqlCeDataAdapter((SqlCeCommand)Command);
                //    ParameterLeader = '@';
                //    break;
                case DbConnectionType.Oracle:
                    Connection = new OracleConnection(connectionString);
                    Command = Connection.CreateCommand();
                    DataAdapter = new OracleDataAdapter((OracleCommand)Command);
                    ParameterLeader = ':';
                    break;
            }
            IsTransaction = false;
        }
        #endregion

        /// <summary>
        /// Transaction status
        /// </summary>
        public bool IsTransaction { get; set; }

        /// <summary>
        /// Begin transaction
        /// </summary>
        public void BeginTransaction()
        {
            if (!IsTransaction)
            {
                Command.Transaction = Connection.BeginTransaction();
                IsTransaction = true;
            }
        }

        /// <summary>
        /// Commit data.
        /// </summary>
        public void Commit()
        {
            if (IsTransaction)
            {
                if (Command.Transaction != null)
                    Command.Transaction.Commit();
                IsTransaction = false;
            }
        }
        /// <summary>
        /// Rollback data
        /// </summary>
        public void Rollback()
        {
            if (IsTransaction)
            {
                if (Command.Transaction != null)
                    Command.Transaction.Rollback();
                IsTransaction = false;
            }
        }
        /// <summary>
        /// Open connection
        /// </summary>
        public void Open()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
        }
        /// <summary>
        /// Close connection
        /// </summary>
        public void Close()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }
        /// <summary>
        /// Dispose the DB class and connection
        /// </summary>
        public void Dispose()
        {
            Rollback();
            Close();
            DataAdapter.Dispose();
            Command.Dispose();
            Connection.Dispose();
        }
        /// <summary>
        /// Connection status
        /// </summary>
        public ConnectionState State { get { return Connection.State; } }
        /// <summary>
        /// Command type
        /// </summary>
        public CommandType CommandType
        {
            get { return Command.CommandType; }
            set { Command.CommandType = value; }
        }
        /// <summary>
        /// Clear all parameters for DB classs
        /// </summary>
        public void ClearParameters()
        {
            Command.Parameters.Clear();
        }
        /// <summary>
        /// Add a new parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="value">value</param>
        public void AddParameter(string parameterName, object value)
        {
            SetCommand(Command.CommandText, new DB.Parameter(parameterName, value));
        }
        /// <summary>
        /// Add a new parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="sourceColumn">Source column (field mapping)</param>
        /// <param name="value">value</param>
        public void AddParameter(string parameterName, string sourceColumn, object value)
        {
            SetCommand(Command.CommandText, new DB.Parameter(parameterName, sourceColumn, value));
        }

        private void SetCommand(CommandType type, string sql, params Parameter[] parameters)
        {
            Command.CommandType = type;
            SetCommand(sql, parameters);
        }

        private void SetCommand(string sql, params Parameter[] parameters)
        {
            Command.CommandText = sql;
            if (parameters != null)
            {
                foreach (Parameter parameter in parameters)
                {
                    if (parameter == null)
                        continue;

                    string pn = parameter.ParameterName;
                    string sc = parameter.SourceColumn;
                    if (!pn.StartsWith(ParameterLeader.ToString()))
                        pn = ParameterLeader + pn;
                    if (String.IsNullOrEmpty(sc))
                        sc = pn.TrimStart(ParameterLeader);

                    if (!Command.Parameters.Contains(pn))
                    {
                        DbParameter item = Command.CreateParameter();
                        item.ParameterName = pn;
                        item.SourceColumn = sc;
                        item.Value = parameter.Value;
                        Command.Parameters.Add(item);
                    }
                    else
                    {
                        DbParameter item = Command.Parameters[pn];
                        item.SourceColumn = sc;
                        item.Value = parameter.Value;
                    }
                }
            }
        }
        /// <summary>
        /// Execute sql
        /// </summary>
        /// <param name="type">Command type</param>
        /// <param name="sql">Sql string</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>How much records will be impact on</returns>
        public int ExecuteNonQuery(CommandType type, string sql, params Parameter[] parameters)
        {
            SetCommand(type, sql, parameters);
            return Command.ExecuteNonQuery();
        }
        /// <summary>
        /// Execute sql
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>How much records will be impact on</returns>
        public int ExecuteNonQuery(string sql, params Parameter[] parameters)
        {
            SetCommand(sql, parameters);
            return Command.ExecuteNonQuery();
        }
        /// <summary>
        /// Execute sql
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <returns>How much records will be impact on</returns>
        public int ExecuteNonQuery(string sql)
        {
            SetCommand(sql, null);
            return Command.ExecuteNonQuery();
        }
        /// <summary>
        /// Excute sql and return a data reader
        /// </summary>
        /// <param name="type">Command type</param>
        /// <param name="sql">Sql string</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>Data reader</returns>
        public DbDataReader ExecuteReader(CommandType type, string sql, params Parameter[] parameters)
        {
            SetCommand(type, sql, parameters);
            return Command.ExecuteReader();
        }
        /// <summary>
        /// Excute sql and return a data reader
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>Data reader</returns>
        public DbDataReader ExecuteReader(string sql, params Parameter[] parameters)
        {
            SetCommand(sql, parameters);
            return Command.ExecuteReader();
        }
        /// <summary>
        /// Excute sql and return a data reader
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <returns>Data reader</returns>
        public DbDataReader ExecuteReader(string sql)
        {
            SetCommand(sql, null);
            return Command.ExecuteReader();
        }
        /// <summary>
        /// Excute sql and return first column in first row only
        /// </summary>
        /// <param name="type">Command type</param>
        /// <param name="sql">Sql string</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>Return single value</returns>
        public object ExecuteScalar(CommandType type, string sql, params Parameter[] parameters)
        {
            SetCommand(type, sql, parameters);
            return Command.ExecuteScalar();
        }
        /// <summary>
        /// Excute sql and return first column in first row only
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>Return single value</returns>
        public object ExecuteScalar(string sql, params Parameter[] parameters)
        {
            SetCommand(sql, parameters);
            return Command.ExecuteScalar();
        }
        /// <summary>
        /// Excute sql and return first column in first row only
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <returns>Return single value</returns>
        public object ExecuteScalar(string sql)
        {
            SetCommand(sql, null);
            return Command.ExecuteScalar();
        }
        /// <summary>
        /// Excute sql and return datatable
        /// </summary>
        /// <param name="type">Command type</param>
        /// <param name="sql">Sql string</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>Return datatable</returns>
        public DataTable ExecuteDataTable(CommandType type, string sql, params Parameter[] parameters)
        {
            SetCommand(type, sql, parameters);
            DataTable result = new DataTable();
            DataAdapter.Fill(result);
            return result;
        }
        /// <summary>
        /// Excute sql and return datatable
        /// </summary>
        /// <param name="type">Command type</param>
        /// <param name="sql">Sql string</param>
        /// <param name="startRecord">Record start index</param>
        /// <param name="maxRecords">Max to return records</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>Return datatable</returns>
        public DataTable ExecuteDataTable(CommandType type, string sql, int startRecord, int maxRecords, params Parameter[] parameters)
        {
            SetCommand(type, sql, parameters);
            DataTable result = new DataTable();
            DataAdapter.Fill(startRecord, maxRecords, result);
            return result;
        }
        /// <summary>
        /// Excute sql and return datatable
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>Return datatable</returns>
        public DataTable ExecuteDataTable(string sql, params Parameter[] parameters)
        {
            SetCommand(sql, parameters);
            DataTable result = new DataTable();
            DataAdapter.Fill(result);
            return result;
        }
        /// <summary>
        /// Excute sql and return datatable
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <returns>Return datatable</returns>
        public DataTable ExecuteDataTable(string sql)
        {
            SetCommand(sql, null);
            DataTable result = new DataTable();
            DataAdapter.Fill(result);
            return result;
        }
        /// <summary>
        /// Excute sql and return datatable
        /// </summary>
        /// <param name="sql">Sql string</param>
        /// <param name="startRecord">Record start index</param>
        /// <param name="maxRecords">Max to return records</param>
        /// <param name="parameters">Parameters array</param>
        /// <returns>Return datatable</returns>
        public DataTable ExecuteDataTable(string sql, int startRecord, int maxRecords, params Parameter[] parameters)
        {
            SetCommand(sql, parameters);
            DataTable result = new DataTable();
            DataAdapter.Fill(startRecord, maxRecords, result);
            return result;
        }

        #region insert, update, delete
        /// <summary>
        /// add a new row into table
        /// </summary>
        /// <param name="table">table name</param>
        /// <param name="parameters">field of row</param>
        /// <returns>0 = failed, 1 = success</returns>
        public int Insert(string table, params Parameter[] parameters)
        {
            StringBuilder sn = new StringBuilder();
            StringBuilder sv = new StringBuilder();
            foreach (Parameter parameter in parameters)
            {
                if (parameter == null)
                    continue;

                string pn = parameter.ParameterName;
                string sc = parameter.SourceColumn;
                if (!pn.StartsWith(ParameterLeader.ToString()))
                    pn = ParameterLeader + pn;
                if (String.IsNullOrEmpty(sc))
                    sc = pn.TrimStart(ParameterLeader);

                sn.Append(sc + ',');
                sv.Append(pn + ',');
            }
            string sql = String.Format(@"INSERT INTO {0} ({1}) VALUES ({2})",
                                        table,
                                        sn.ToString().TrimEnd(','),
                                        sv.ToString().TrimEnd(','));
            return (int)ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// update table
        /// </summary>
        /// <param name="table">table name</param>
        /// <param name="where">query condition</param>
        /// <param name="parameters">all fields that will be updating</param>
        /// <returns>0 = failed, > 0 is success</returns>
        public int Update(string table, Parameter where, params Parameter[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Parameter parameter in parameters)
            {
                if (parameter == null)
                    continue;

                string pn = parameter.ParameterName;
                string sc = parameter.SourceColumn;
                if (!pn.StartsWith(ParameterLeader.ToString()))
                    pn = ParameterLeader + pn;
                if (String.IsNullOrEmpty(sc))
                    sc = pn.TrimStart(ParameterLeader);

                sb.Append(sc + "=" + pn + ",");
            }
            if (where != null)
            {
                string pn = where.ParameterName;
                string sc = where.SourceColumn;
                if (!pn.StartsWith(ParameterLeader.ToString()))
                    pn = ParameterLeader + pn;
                if (String.IsNullOrEmpty(sc))
                    sc = pn.TrimStart(ParameterLeader);

                sb = new StringBuilder(sb.ToString().TrimEnd(',') + " WHERE " + sc + "=" + pn);

                if (!Command.Parameters.Contains(pn))
                {
                    DbParameter parameter = Command.CreateParameter();
                    parameter.ParameterName = pn;
                    parameter.SourceColumn = sc;
                    parameter.Value = where.Value;
                    Command.Parameters.Add(parameter);
                }
                else
                {
                    DbParameter parameter = Command.Parameters[where.ParameterName];
                    parameter.SourceColumn = sc;
                    parameter.Value = where.Value;
                }
            }
            string sql = String.Format(@"UPDATE {0} SET {1}", table, sb.ToString());
            return (int)ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// delete row from table
        /// </summary>
        /// <param name="table">table name</param>
        /// <param name="wheres">query condition</param>
        /// <returns>0 = failed, > 0 is success</returns>
        public int Delete(string table, params Parameter[] wheres)
        {
            StringBuilder sb = new StringBuilder();
            bool hasWhere = false;
            foreach (Parameter where in wheres)
            {
                string pn = where.ParameterName;
                string sc = where.SourceColumn;
                if (!pn.StartsWith(ParameterLeader.ToString()))
                    pn = ParameterLeader + pn;
                if (String.IsNullOrEmpty(sc))
                    sc = pn.TrimStart(ParameterLeader);

                if (hasWhere)
                {
                    sb.AppendFormat(null, @" AND {0}={1}", pn, sc);
                }
                else
                {
                    sb.AppendFormat(null, @" WHERE {0}={1}", pn, sc);
                    hasWhere = true;
                }
            }
            string sql = String.Format(@"DELETE FROM {0}{1}", table, sb.ToString());
            return (int)ExecuteNonQuery(CommandType.Text, sql, wheres);
        }
        #endregion

        /// <summary>
        /// Parameter help class
        /// </summary>
        public class Parameter
        {
            /// <summary>
            /// Parameter name
            /// </summary>
            public string ParameterName { get; set; }
            /// <summary>
            /// Source column
            /// </summary>
            public string SourceColumn { get; set; }
            /// <summary>
            /// Value
            /// </summary>
            public object Value { get; set; }
            /// <summary>
            /// Intialize class
            /// </summary>
            /// <param name="parameterName">Parameter name</param>
            /// <param name="sourceColumn">Source column</param>
            /// <param name="value">Value</param>
            public Parameter(string parameterName, string sourceColumn, object value)
            {
                ParameterName = parameterName;
                SourceColumn = sourceColumn;
                Value = value;
            }
            /// <summary>
            /// Intialize class
            /// </summary>
            /// <param name="parameterName">Parameter name</param>
            /// <param name="value">Value</param>
            public Parameter(string parameterName, object value)
            {
                ParameterName = parameterName;
                SourceColumn = parameterName;
                Value = value;
            }
        }

        /// <summary>
        /// Convert the datatable to parameters array
        /// </summary>
        /// <param name="source">Datatable</param>
        /// <returns>Parameters array</returns>
        public List<Parameter[]> CreateParameter(DataTable source)
        {
            List<Parameter[]> result = new List<Parameter[]>();
            foreach (DataRow row in source.Rows)
                result.Add(CreateParameter(row));
            return result;
        }
        /// <summary>
        /// Convert the datarow to parameters array
        /// </summary>
        /// <param name="source">Data row</param>
        /// <returns>Parameters array</returns>
        public Parameter[] CreateParameter(DataRow source)
        {
            DataColumnCollection cols = source.Table.Columns;
            Parameter[] parameters = new Parameter[cols.Count];
            for (int i = 0; i < cols.Count; i++)
                parameters[i] = new Parameter(cols[i].ColumnName, source[i]);
            return parameters;
        }

        #region get the unique id
        private static string __instance_id__ = (new Random(Environment.TickCount).Next(10, 90) * 1000).ToString();
        private static object __previous__ = -1L;

        /// <summary>
        /// <![CDATA[
        /// Generate a new id
        /// The memories of this function as following
        /// 1. It is safe on thread level.
        /// 2. The result is repeating in the very very small probability.
        /// 3. The id always is increasing. 
        /// 4. Easy to read, for example: yyMMddHHmmssffrrxxxxx (year + month + day + hour + minute + seconds + 5 randomly digit
        /// 5. Fast, It able to generate about 100,000 results in 1 second.
        /// 6. Expiry date: 1/1/2000 to 12/31/2092, And the result limit by 2^63,(e.g. 2^63=922337 203685 47 75808)
        /// ]]>
        /// </summary>
        public static long GenerateUniqueID()
        {
            lock (__previous__)
            {
                long result = long.Parse(DateTime.UtcNow.ToString("yyMMddHHmmssff") + __instance_id__);
                if (result <= (long)__previous__)
                    result = (long)__previous__ + 1;
                __previous__ = result;
                return result;
            }
        }
        #endregion
    }

   
}
