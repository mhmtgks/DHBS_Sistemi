using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class RandevuDTO
    {
        public int RandevuID { get; set; }
        public int HastaID { get; set; }
        public int DoktorID { get; set; }
        public string TC { get; set; }
        public string AdiSoyadi { get; set; }
        public string Iletisim { get; set; }
        public DateTime Tarih{ get; set; }
    }
}
