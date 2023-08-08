using System.Data.SqlClient;

namespace Data
{
    public class Connection
    {

        SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DHBS;Integrated Security=True;Connect Timeout=30;");

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
