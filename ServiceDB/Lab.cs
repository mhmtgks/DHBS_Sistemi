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
    public class Lab
    {

        Connection connection = new Connection();
        Command command = new Command();
        public List<LabDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<LabDTO> list = new List<LabDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new LabDTO
                {
                    LabID = (int)sqlDataReader["LabID"],
                    LabDurum = sqlDataReader["LabDurum"].ToString(),
                    LabTestAdi = sqlDataReader["LabTestAdi"].ToString(),
                    LabAdı = sqlDataReader["LabAdı"].ToString(),
                });


            }
            return list;
        }

        public void insert(string sqlstring)
        {

            SqlCommand insertcommand = command.ProccesSql(sqlstring);
            insertcommand.ExecuteNonQuery();
            connection.ConnectEnd();
            //  throw new NotImplementedException();
        }
        public void delete(string sqlstring)
        {
            SqlCommand deletecommand = command.ProccesSql(sqlstring);
            deletecommand.ExecuteNonQuery();
            connection.ConnectEnd();
            //throw new NotImplementedException();


        }
        public void update(string sqlstring)
        {
            SqlCommand updatecommand = command.ProccesSql(sqlstring);
            updatecommand.ExecuteNonQuery();
            connection.ConnectEnd();
            //throw new NotImplementedException();
        }
    }
}
