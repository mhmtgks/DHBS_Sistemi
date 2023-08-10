using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class VW_MaaslarDTO
    {

        public int CalisanID { get; set; }
        public string AdiSoyadi  { get; set; }
        public decimal Maas  { get; set; }
        public DateTime SonÖdenmeTarihi  { get; set; }
        public int IslemiYapan  { get; set; }
    }
}
