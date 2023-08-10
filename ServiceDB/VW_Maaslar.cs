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
    public class VW_Maaslar
    {

        Connection connection = new Connection();
        Command command = new Command();
        public List<VW_MaaslarDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<VW_MaaslarDTO> list = new List<VW_MaaslarDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new VW_MaaslarDTO
                {
                    CalisanID = (int)sqlDataReader["CalisanID"],
                    Maas= (decimal)sqlDataReader["Maas"],
                    AdiSoyadi = sqlDataReader["AdiSoyadi"].ToString(),
                    IslemiYapan = (int)sqlDataReader["IslemiYapan"],
                    SonÖdenmeTarihi = sqlDataReader.GetDateTime(3),

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
