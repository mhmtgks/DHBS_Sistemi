using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class VW_DoktorlarDTO
    {

        public string AdiSoyadi { get; set; }
        public string   Uzmanlık { get; set; }
        public string   Unvan { get; set; }
        public int   KurumID { get; set; }
        public int   DoktorID { get; set; }
    }
}
