using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Data;
using Service.DTO;

namespace Service
{
    public class Hasta
    {
        Connection connection = new Connection();
        Command command = new Command();
        public List<HastaDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<HastaDTO> list = new List<HastaDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new HastaDTO
                {
                    hastaid = (int)sqlDataReader["HastaID"],
                    TC = sqlDataReader["TC"].ToString(),
                    AdiSoyadi = sqlDataReader["AdiSoyadi"].ToString(),
                    Iletisim = sqlDataReader["Iletisim"].ToString(),
                    DogumTarihi = sqlDataReader.GetDateTime(4),
                    Cinsiyet = sqlDataReader["Cinsiyet"].ToString(),
                    Status = sqlDataReader["Status"].ToString(),
                    IslemiYapan = (int)sqlDataReader["IslemiYapan"],
                    KayıtTarihi = sqlDataReader.GetDateTime(6),

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
