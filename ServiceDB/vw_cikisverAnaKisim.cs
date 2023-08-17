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
    public class vw_cikisverAnaKisim
    {

        Connection connection = new Connection();
        Command command = new Command();
        public List<vw_cikisverAnaKisimDTO> Lists(string sqlstring)
        {
            SqlCommand listcommand = command.ProccesSql(sqlstring);
            SqlDataReader sqlDataReader = listcommand.ExecuteReader();
            List<vw_cikisverAnaKisimDTO> list = new List<vw_cikisverAnaKisimDTO>();
            while (sqlDataReader.Read())
            {

                list.Add(new vw_cikisverAnaKisimDTO
                {
                    HastaID = (int)sqlDataReader["HastaID"],
                    TC = sqlDataReader["TC"].ToString(),
                    AdiSoyadi = sqlDataReader["AdiSoyadi"].ToString(),
                    Iletisim = sqlDataReader["Iletisim"].ToString(),
                   IslemGrubuID = (int)sqlDataReader["IslemGrubuID"],
                    
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
