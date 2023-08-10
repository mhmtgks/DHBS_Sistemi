using Data;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Islemler
    {

        Connection connection = new Connection();
        Command command = new Command();
        public List<IslemlerDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<IslemlerDTO> list = new List<IslemlerDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new IslemlerDTO
                {
                    IslemGrubuID= (int)sqlDataReader["IslemGrubuID"],
                    IslemID =(int)sqlDataReader["IslemID"],
                    HastaID =(int)sqlDataReader["HastaID"],
                    Adi_SoyadiH = sqlDataReader["adhasta"].ToString(),
                    Adi_SoyadiD = sqlDataReader["AdiSoyadi"].ToString(),
                    IslemAciklamasi = sqlDataReader["IslemAciklamasi"].ToString(),
                    Tarih = sqlDataReader.GetDateTime(5),
                    CalisanID = (int)sqlDataReader["CalisanID"]
                 


                });


            }
            return list;
        }
    }
}
