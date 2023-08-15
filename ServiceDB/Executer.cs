using Data;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class Executer
    {
         Connection connection = new Connection();
         Command command = new Command();
        public void execute(string sqlstring)
        {

            SqlCommand insertcommand = command.ProccesSql(sqlstring);
            insertcommand.ExecuteNonQuery();
            connection.ConnectEnd();
        }
        public string executeSTR(string sqlstring)
        {

            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<LabDTO> list = new List<LabDTO>();
            return sqlDataReader[0].ToString();

        }
    }
}
