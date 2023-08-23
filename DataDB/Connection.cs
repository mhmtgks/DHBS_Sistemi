

using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Data
{
    public class Connection
    {


        SqlConnection connection = new SqlConnection(@"Data Source=WIN-H3MLS6FPUAQ\MSSQLSERVER2012;Initial Catalog=DHBSDatabase;User ID=mehmet;Password=Goeksu2001");
        //SqlConnection connection = new SqlConnection(@"Data Source = MEHMET_G™KSU\MSSQLSERVER2012;Initial Catalog = DHBSDatabase; Integrated Security = True; Connect Timeout = 30;");

        //Data Source = MEHMET_G™KSU\MSSQLSERVER2012;Initial Catalog = DHBSDatabase; Integrated Security = True; Connect Timeout = 30;


        public SqlConnection Connectstart()
        {
            connection.Open();

            return connection;
        }

        public SqlConnection ConnectEnd()
        {

            connection.Close();
            return connection;
        }
    }
}
