using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class IslemlerDTO
    {
        public int IslemGrubuID { get; set; }
        public int HastaID { get; set; }
        public int IslemID { get; set; }
        public string Adi_SoyadiH { get; set; }
        public string Adi_SoyadiD { get; set; }
        public string IslemAciklamasi { get; set; }
        public string DrAciklaması { get; set; }
        public DateTime Tarih { get; set; }
        public int CalisanID { get; set; }


    }
}
