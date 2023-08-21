using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class idExtractor
    {
        public static string sadeceSayilar;
        public static string username;
        public static int userid;


        public static void Extractor(string str,int type)
        {
            /*
             1 yönetici, doktor,hastakayıt 2hasta(user)
             */

            string inputString = str;
            string nameIdentifier = "nameidentifier";

            int nameIdentifierIndex = inputString.IndexOf(nameIdentifier);

            if (nameIdentifierIndex != -1)
            {
                // nameidentifier'ın sonrasındaki 20 karakteri alalım.
                if (nameIdentifierIndex + nameIdentifier.Length + 20 <= inputString.Length)
                {
                    string sonraki20Karakter = inputString.Substring(nameIdentifierIndex + nameIdentifier.Length, 20);
                    sadeceSayilar = new string(sonraki20Karakter.Where(char.IsDigit).ToArray());
                    Console.WriteLine("Sonraki 20 karakterlik kısım: " + sadeceSayilar);
                }
                else
                {
                    Console.WriteLine("Sonraki 20 karakter için yeterli karakter yok.");
                }
            }
            else
            {
                Console.WriteLine("nameidentifier bulunamadı.");
            }

            switch (type) {
                case 1:
                    Calisan calisan = new Calisan();
                    List<CalisanDTO> dTOs = new List<CalisanDTO>();
                    dTOs = calisan.Lists("select * from Calisan where TC='" + sadeceSayilar + "'");
                    userid = dTOs.FirstOrDefault().CalisanID;
                    username = dTOs.FirstOrDefault().AdiSoyadi.ToString();
                    break;
                case 2:
                    Hasta hasta = new Hasta();
                    List<HastaDTO> dTOs1 = new List<HastaDTO>();
                    dTOs1 = hasta.Lists("select * from Hasta where TC='" + sadeceSayilar + "'");
                    userid = dTOs1.FirstOrDefault().hastaid;
                    username = dTOs1.FirstOrDefault().AdiSoyadi.ToString();
                    break;
                case 3:
                    userid = 0;
                    username = "lab";
                    break;
                default:
                    break;
            }

            return;
        }
    }
}
