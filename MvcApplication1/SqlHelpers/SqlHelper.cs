using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
namespace MvcApplication1.SqlHelpers
{
    public class SqlHelper
    {

        public virtual string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnection"].ToString();

            }
        }

        public static SqlDataReader ExecuteReader(CommandType type, string ProcedureName, params SqlParameter[] param)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = type;
            cmd.CommandText = ProcedureName;
            AddParameter(cmd, param);
            return cmd.ExecuteReader();

        }

        private static void AddParameter(SqlCommand cmd, SqlParameter[] param)
        {
            foreach (SqlParameter item in param)
            {
                cmd.Parameters.Add(item);
            }
        }

    }
}