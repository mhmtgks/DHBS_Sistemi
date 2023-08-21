using Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class Odeme
    {
        Connection connection = new Connection();
        Command command = new Command();
        public List<OdemeDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<OdemeDTO> list = new List<OdemeDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new OdemeDTO
                {
                    Odendi = (byte)sqlDataReader["Odendi"],
                    FaturaID = sqlDataReader["FaturaID"].ToString(),
                    IslemiYapan = (int)sqlDataReader["IslemiYapan"],
                    Tarih = sqlDataReader.GetDateTime(1),

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
