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
    public class RandevuHasta
    {
        Connection connection = new Connection();
        Command command = new Command();
        public List<RandevuHastaDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<RandevuHastaDTO> list = new List<RandevuHastaDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new RandevuHastaDTO
                {
                    DoktorAdi = sqlDataReader["dradi"].ToString(),
                    TC = sqlDataReader["TC"].ToString(),
                    AdiSoyadi = sqlDataReader["AdiSoyadi"].ToString(),
                    Uzmanlık = sqlDataReader["Uzmanlık"].ToString(),
                    Tarih = sqlDataReader.GetDateTime(5),
                    DoktorID = (int)sqlDataReader["DoktorID"],
                    RandevuID = (int)sqlDataReader["RandevuID"],
                    HastaID = (int)sqlDataReader["HastaID"],

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
