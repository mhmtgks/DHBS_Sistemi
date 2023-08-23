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
    public class FiyatListesi
    {
        Connection connection = new Connection();
        Command command = new Command();
        public List<FiyatListesiDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<FiyatListesiDTO> list = new List<FiyatListesiDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new FiyatListesiDTO
                {
                    ProsedurID = (int)sqlDataReader["ProsedurID"],
                    Adi = sqlDataReader["Adi"].ToString(),
                    Ucreti = (Decimal)sqlDataReader["Ucreti"],
                    IsActive = (int)sqlDataReader["IsActive"],

                });


            }
            return list;
        }
    }
}
