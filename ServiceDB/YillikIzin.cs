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
    public class YillikIzin
    {
        Connection connection = new Connection();
        Command command = new Command();
        public List<YillikIzinDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<YillikIzinDTO> list = new List<YillikIzinDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new YillikIzinDTO
                {
                    DoktorID = (int)sqlDataReader["DoktorID"],
       
                    BaslangicTarihi = sqlDataReader.GetDateTime(1),

                    BitisTarihi = sqlDataReader.GetDateTime(2),

                    AdiSoyadi = sqlDataReader["AdiSoyadi"].ToString(),
                    Uzmanlık = sqlDataReader["Uzmanlık"].ToString(),


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
