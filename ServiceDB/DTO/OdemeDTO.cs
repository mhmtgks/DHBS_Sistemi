using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class OdemeDTO
    {
        public string FaturaID { get; set; }
        public DateTime Tarih { get; set; }
        public byte Odendi { get; set; }
        public int IslemiYapan { get; set; }
    }
}
