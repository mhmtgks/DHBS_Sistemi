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
    public class vw_cikisverAraTablo
    {

        Connection connection = new Connection();
        Command command = new Command();
        public List<vw_cikisverAraTabloDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<vw_cikisverAraTabloDTO> list = new List<vw_cikisverAraTabloDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new vw_cikisverAraTabloDTO
                {
                    IslemGrubuID = (int)sqlDataReader["IslemGrubuID"],
                    HastaAdi = sqlDataReader["hastaadi"].ToString(),
                    DoktorAdi = sqlDataReader["AdiSoyadi"].ToString(),
                    IslemAciklamasi = sqlDataReader["IslemAciklamasi"].ToString(),
                    Tarih = sqlDataReader.GetDateTime(4),
                    DrAciklamasi = sqlDataReader["DrAciklaması"].ToString(),
                    
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
