using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class vw_cikisverAraTabloDTO
    {

        public int IslemGrubuID { get; set; }
        public string HastaAdi { get; set; }
        public string DoktorAdi { get; set; }
        public string IslemAciklamasi { get; set; }
        public string DrAciklamasi { get; set; }
        public DateTime Tarih { get; set; }
    }
}
