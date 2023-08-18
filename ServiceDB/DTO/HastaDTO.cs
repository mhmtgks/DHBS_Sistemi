

using System;
using System.Data.SqlTypes;

namespace Service.DTO
{
    public class HastaDTO
    {
        public int hastaid { get; set; }
        public int IslemiYapan { get; set; }
        public string TC { get; set;}
        public string AdiSoyadi { get; set;}
        public string Iletisim { get; set;}
        public DateTime DogumTarihi { get; set;}
        public DateTime KayıtTarihi { get; set; }
        public string Cinsiyet { get; set;}
        public string Email { get; set;}
        public string Adres { get; set;}
        public string Status { get; set;}

        public static IEnumerable<HastaDTO> Listele() { List<HastaDTO> hasta = new List<HastaDTO>(); return hasta; }
    }

    
}
