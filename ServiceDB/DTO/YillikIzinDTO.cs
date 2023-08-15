using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class YillikIzinDTO
    {
        public int DoktorID { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }

        public string AdiSoyadi { get; set; }
        public string Uzmanlık { get; set; }
    }
}
