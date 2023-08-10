using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class VW_FaturalarDTO
    {
        public string FaturaID { get; set; }
        public string AdiSoyadi { get; set; }
        public DateTime Tarih { get; set; }
        public decimal Ucret { get; set; }
        public int HastaID{ get; set; }
    }
}
