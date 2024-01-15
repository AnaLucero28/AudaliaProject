using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBServer
{

    public class Database
    {
        static SqlConnection connection;

        public static SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(@"Data Source=.\AUDALIA;Initial Catalog=master;" +
                        @"Database=Audalia;" +
                        @"Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
                        @"MultipleActiveResultSets=True");

                    connection.Open();                    
                }
                return connection;
            }
        }
    }

}
