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
    public class Calisan
    {
        Connection connection = new Connection();
        Command command = new Command();
        public List<CalisanDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<CalisanDTO> list = new List<CalisanDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new CalisanDTO
                {
                    CalisanID = (int)sqlDataReader["CalisanID"],
                    TC = sqlDataReader["TC"].ToString(),
                    AdiSoyadi = sqlDataReader["AdiSoyadi"].ToString(),
                    Adres = sqlDataReader["Adres"].ToString(),
                    Ünvan = sqlDataReader["Ünvan"].ToString(),
                    KurumID = (int)sqlDataReader["KurumID"],


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
