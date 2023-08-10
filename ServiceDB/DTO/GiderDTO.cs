using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class GiderDTO
    {

        public int GiderID { get; set; }
        public int KurumID { get; set; }
        public decimal GiderTutari { get; set; }
        public  DateTime Tarih { get; set; }
        public  string Aciklama { get; set; }
    }
}
