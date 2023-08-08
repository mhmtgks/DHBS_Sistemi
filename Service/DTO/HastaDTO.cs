

using System.Data.SqlTypes;

namespace Service.DTO
{
    public class HastaDTO
    {
        public int hastaid { get; set; }
        public string TC { get; set;}
        public string AdiSoyadi { get; set;}
        public string Iletisim { get; set;}
        public SqlDateTime DogumTarihi { get; set;}
        public string Cinsiyet { get; set;}
    }
}
