using System.Collections.Generic;
using System.Data.SqlClient;
using Data;

using Service.DTO;

namespace Service
{
    public class Giris
    {
        Connection connection = new Connection();
        Command command = new Command();    
        public List<GirisDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<GirisDTO> list = new List<GirisDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new GirisDTO
                {
                    KullaniciAdi = sqlDataReader["KullaniciAdi"].ToString(),
                    Sifre = sqlDataReader["Sifre"].ToString(),
                    Yetki = (int)sqlDataReader["YetkiID"],
                  

                });


            }
            return list;
        }
    }
}
