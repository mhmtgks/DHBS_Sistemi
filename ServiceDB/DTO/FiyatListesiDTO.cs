using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class FiyatListesiDTO
    {

        public int ProsedurID { get; set; }
        public string Adi { get; set; }
     
        public Decimal Ucreti { get; set; }

    }
}
