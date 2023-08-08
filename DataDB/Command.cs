using System;
using System.Data.SqlClient;

namespace Data
{
    public class Command
    {
        Connection connection = new Connection();

        public SqlCommand ProccesSql(string sqlString)
        {

            SqlCommand command = new SqlCommand(sqlString, connection.Connectstart());
            return command;
        }
    }
}
