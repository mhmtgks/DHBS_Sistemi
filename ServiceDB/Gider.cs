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
    public class Gider
    {

        Connection connection = new Connection();
        Command command = new Command();
        public List<GiderDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<GiderDTO> list = new List<GiderDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new GiderDTO
                {
                    GiderID = (int)sqlDataReader["GiderID"],
                    KurumID = (int)sqlDataReader["KurumID"],
                    GiderTutari = (decimal)sqlDataReader["GiderTutarı"],
                    Aciklama = sqlDataReader["Açıklama"].ToString(),
                    Tarih = sqlDataReader.GetDateTime(3),

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
