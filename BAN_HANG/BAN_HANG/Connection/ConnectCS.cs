using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BAN_HANG.Connection
{
    public class ConnectCS
    {
        public static SqlConnection GetConnectCRM()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["DB_CSEntities"].ConnectionString);
        }
    }
}