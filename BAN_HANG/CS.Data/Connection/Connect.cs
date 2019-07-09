using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Connection
{
    public static class Connect
    {
        public static SqlConnection GetConnect()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SIConnectionStringCS"].ConnectionString);
        }
    }
}
