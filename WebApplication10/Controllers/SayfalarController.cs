
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using DHBS_Sistemi.Models;
using static DHBS_Sistemi.Controllers.KimlikDoğrulamaController;
using Service.DTO;
using Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Routing;
using NuGet.Common;
using Newtonsoft.Json.Linq;

namespace DHBS_Sistemi.Controllers
{

    public class SayfalarController : Controller
    {
        Executer executer = new Executer();
        
        private static string Mtoken = "";

        [CustomValidationHK]
        public IActionResult YöneticiAnaSayfa()
        {
            List<VW_FaturalarDTO> hastas = new List<VW_FaturalarDTO>();
            VW_Faturalar hasta = new VW_Faturalar();
            hastas = hasta.Lists("select FaturaID, vf.AdiSoyadi, Tarih, Ucret, vf.HastaID from vw_Faturalar vf join Hasta h on h.HastaID = vf.HastaID where vf.FaturaID not in (select FaturaID from Odeme)");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View(hastas);

        }


        [CustomValidationHK]
        [HttpPost]
        public IActionResult YöneticiAnaSayfa([FromForm] GiderDTO gider, string id, int tip, string faturaid, string Adi, string dates)
        {
            ViewBag.id = HttpContext.Session.GetString("userid");
            ViewBag.isim = HttpContext.Session.GetString("username");
            List<VW_FaturalarDTO> hastas = new List<VW_FaturalarDTO>();
            VW_Faturalar hasta = new VW_Faturalar();
            hastas = hasta.Lists("select FaturaID, vf.AdiSoyadi, Tarih, Ucret, vf.HastaID from vw_Faturalar vf join Hasta h on h.HastaID = vf.HastaID where vf.FaturaID not in (select FaturaID from Odeme)");
            try
            {


                if (tip == 5)
                {

                    if (dates != null)
                    {
                        VW_Faturalar hasta1 = new VW_Faturalar();
                        string[] tarih = parse(dates);
                        if (tarih[0] != tarih[1])
                        {
                            hastas = hasta1.Lists("select FaturaID,vf.AdiSoyadi,Tarih,Ucret,vf.HastaID from vw_Faturalar vf join Hasta h on h.HastaID=vf.HastaID where vf.FaturaID not in (select FaturaID from Odeme) and vf.FaturaID like'%" + faturaid + "%' and vf.AdiSoyadi like'%" + Adi + "%' and vf.Tarih between '" + tarih[0] + "' and '" + tarih[1] + "'");
                        }
                        else
                        {
                            hastas = hasta1.Lists("select FaturaID,vf.AdiSoyadi,Tarih,Ucret,vf.HastaID from vw_Faturalar vf join Hasta h on h.HastaID=vf.HastaID where vf.FaturaID not in (select FaturaID from Odeme) and vf.FaturaID like'%" + faturaid + "%' and vf.AdiSoyadi like'%" + Adi + "%'");
                        }
                    }
                    else
                    {
                        VW_Faturalar hasta2 = new VW_Faturalar();
                        hastas = hasta2.Lists("select FaturaID,vf.AdiSoyadi,Tarih,Ucret,vf.HastaID from vw_Faturalar vf join Hasta h on h.HastaID=vf.HastaID where vf.FaturaID not in (select FaturaID from Odeme) and vf.FaturaID like'%" + faturaid + "%' and vf.AdiSoyadi like'%" + Adi + "%'");

                    }


                    return View(hastas);
                }
                else
                {
                    if (id != null)
                    {
                        string strtime = DateTime.UtcNow.ToString("yyyy/dd/MM") + " " + DateTime.UtcNow.ToString("HH") + ":" + DateTime.UtcNow.ToString("mm");
                        executer.execute("insert into Odeme values('" + id + "','"+strtime+"',1,"+14+")");
                        TempData["AlertMessage"] = "Gider ekleme Başarılı";
                        VW_Faturalar hasta13 = new VW_Faturalar();
                        hastas = hasta13.Lists("select FaturaID, vf.AdiSoyadi, Tarih, Ucret, vf.HastaID from vw_Faturalar vf join Hasta h on h.HastaID = vf.HastaID where vf.FaturaID not in (select FaturaID from Odeme)");

                        return View(hastas);
                    }
                    else
                    {
                        return View(hastas);
                    }
                }


            }
            catch
            {
                List<VW_FaturalarDTO> hastas1 = new List<VW_FaturalarDTO>();
                VW_Faturalar hasta1 = new VW_Faturalar();
                hastas1 = hasta1.Lists("select FaturaID, vf.AdiSoyadi, Tarih, Ucret, vf.HastaID from vw_Faturalar vf join Hasta h on h.HastaID = vf.HastaID where vf.FaturaID not in (select FaturaID from Odeme)");
                TempData["AlertMessage"] = "Gider ekleme Başarısız";
                return View(hastas1);

            }
            return View(hastas);
        }
            [CustomValidationHK]
        public IActionResult HastaKayit()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }
        [HttpPost]
        [CustomValidationHK]
        public IActionResult HastaKayit([FromForm] HastaDTO hasta)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            try
            {
                if (hasta.hastaid != 0 && hasta.AdiSoyadi != null)
                {
                    Hasta hastacommand = new Hasta();
                    hastacommand.insert("update Hasta set TC='" + hasta.TC + "',AdiSoyadi='" + hasta.AdiSoyadi + "',Iletisim='" + hasta.Iletisim
                        + "',DogumTarihi='" + hasta.DogumTarihi + "',Status='" + "Active" + "',Cinsiyet='" + hasta.Cinsiyet + "' where TC=" + hasta.TC);
                    TempData["AlertMessage"] = "Hasta Başarı ile güncellendi";
                    return View();
                }
                else if (hasta.hastaid != 0 && hasta.AdiSoyadi == null)
                {
                    Hasta hastacommand = new Hasta();
                    hastacommand.delete("update Hasta set Status='Passive' where HastaID=" + hasta.hastaid);
                    TempData["AlertMessage"] = "Hasta Başarı ile Silindi";
                    return View();
                }
                else
                {

                    if (HastaKayitYap(hasta) == true)
                    {
                        TempData["AlertMessage"] = "Hasta Başarı ile eklendi";
                        return View();
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Hasta ekleme Başarısız";
                        return View();
                    }
                }
            }
            catch
            {
                TempData["AlertMessage"] = "Hasta ekleme Başarısız";
                return View();

            }



        }

        private bool HastaKayitYap(HastaDTO hasta)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            try
            {

                string strtime = DateTime.UtcNow.ToString("yyyy/dd/MM") + " " + DateTime.UtcNow.ToString("HH") + ":" + DateTime.UtcNow.ToString("mm");
                Hasta hastacommand = new Hasta();
                hastacommand.insert("insert into Hasta values('" + hasta.TC + "','" + hasta.AdiSoyadi + "','" + hasta.Iletisim
                    + "','" + hasta.DogumTarihi + "','" + "Active" + "','" + strtime + "','" + hasta.Cinsiyet + "'," + HttpContext.Session.GetString("userid") + ",'"+hasta.Email+"','"+hasta.Adres+"')");
                return true;
            }
            catch {
                return false; }


        }
        [CustomValidationHK]
        public IActionResult AnaSayfa()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();

        }
        [CustomValidationHK]
        public IActionResult RandevularHK()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();

        }
        [CustomValidationHK]
        [HttpPost]
        public IActionResult RandevularHK([FromForm] RandevuDTO randevuDTO, int doktorid, byte type, string TC)
        {//type 0 ilk giriş type 1 insert
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            if (TempData["TChid"] == null && TC!=null)
            {
                HastaDTO hasta = new HastaDTO();
                Hasta cmd = new Hasta();
                foreach (var item in cmd.Lists("select * from Hasta where TC='"+ TC+"'"))
                {
                    TempData["TChid"] = item.hastaid;
                    TempData["TCname"] = item.AdiSoyadi;

                }

                return View();

            }
            else {

                if (type == 0)
                {
                    TempData["Doktorid"] = doktorid;
                    VW_Doktorlar dr2 = new VW_Doktorlar();
                    if (doktorid != null || doktorid == 0)
                    {

                        foreach (var item in dr2.Lists("select * from vw_Doktorlar where DoktorID=" + doktorid))
                        {
                            TempData["Uzmanlık"] = item.Uzmanlık;
                            TempData["drAdi"] = item.AdiSoyadi;

                        }
                    }
                    return View();
                }
                else if (type == 1)
                {
                    string strtime = randevuDTO.Tarih.ToString("yyyy/dd/MM") + " " + randevuDTO.Tarih.ToString("HH") + ":" + randevuDTO.Tarih.ToString("mm");
                    Randevu randevu1 = new Randevu();
                    randevu1.insert("insert into Randevu values(" + randevuDTO.HastaID + "," + randevuDTO.DoktorID + ",1,'" + strtime + "','Online')");
                    TempData["AlertMessage"] = "Randevu Alma İşlemi Başarılı";
                    return View();

                }
                else
                {
                    TempData["AlertMessage"] = "Randevu Alma İşlemi Başarısız";
                    return View();

                } }
            return View();

        }
      
        [CustomValidationHK]
        public IActionResult IslemlerHK()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");

            List<HastaDTO> hastas = new List<HastaDTO>();
            Hasta hasta = new Hasta();
            hastas = hasta.Lists("select * from Hasta where Status='Active'");
            return View(hastas);

        }
        [CustomValidationHK]
        [HttpPost]
        public IActionResult IslemlerHK(string TC,string Adi)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<HastaDTO> hastas = new List<HastaDTO>();
            Hasta hasta3 = new Hasta();
            hastas = hasta3.Lists("select * from Hasta where TC like '%" + TC + "%' and AdiSoyadi like '%" + Adi + "%'");
            return View(hastas);

        }
        [CustomValidationHK]
        public IActionResult LaboratuvarHK()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<LabDTO> hastas = new List<LabDTO>();
            Lab lab = new Lab();
            hastas = lab.Lists("select * from Lab");
            return View(hastas);
        }
        [CustomValidationHK]
        [HttpPost]
        public IActionResult LaboratuvarHK([FromForm] LabDTO labDTO,int tip,string Durum)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            try
            {
                List<LabDTO> hastas1 = new List<LabDTO>();
                Lab lab1 = new Lab();
                hastas1 = lab1.Lists("select * from Lab");
                if (tip == 5)
                {
                    List<LabDTO> hastas = new List<LabDTO>();
                    Lab lab = new Lab();
                    hastas = lab.Lists("select * from Lab where LabDurum like '%" + Durum + "%'");
                    return View(hastas);

                }
                else
                {

                    Lab lab = new Lab();
                    lab.update("Update Lab set LabDurum='" + labDTO.LabDurum + "'where LabID='" + labDTO.LabID + "'");
                    TempData["AlertMessage"] = "Lab Başarı ile güncellendi";
                    return View(hastas1);
                }
            }
            catch
            {
                TempData["AlertMessage"] = "Lab Güncelleme Başarısız";
                List<LabDTO> hastas = new List<LabDTO>();
                Lab lab = new Lab();
                hastas = lab.Lists("select * from Lab");
                return View(hastas);
            }
        }
        [CustomValidationDoktor]
        public IActionResult AnaSayfaD()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }

        [CustomValidationDoktor]
        public IActionResult RandevularD()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<RandevuDTO> hastas = new List<RandevuDTO>();
            Randevu hasta = new Randevu();
            hastas = hasta.Lists("select * from vw_randevu where Getdate()<Tarih and DoktorID=" + HttpContext.Session.GetString("userid"));
            return View(hastas);
        }
        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult RandevularD(int tip, string Tarih, string Adi,string dates)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<RandevuDTO> hastas = new List<RandevuDTO>();

            if (dates != null)
            {


                string[] tarih = parse(dates);
                if (tarih[0] != tarih[1])
                {
                    Randevu hasta1 = new Randevu();
                    hastas = hasta1.Lists("select * from vw_randevu where DoktorID=" + HttpContext.Session.GetString("userid")+"and Tarih between '" + tarih[0] +"' and '" + tarih[1] +"'");
                }
                else
                {
                    Randevu hasta1 = new Randevu();
                    hastas = hasta1.Lists("select * from vw_randevu where DoktorID=" + HttpContext.Session.GetString("userid") + " and AdiSoyadi like '%" + Adi + "%'");

                }
                return View(hastas);
            }
            else
            {
                
                Randevu hasta = new Randevu();
                hastas = hasta.Lists("select * from vw_randevu where DoktorID=" + HttpContext.Session.GetString("userid") + " and AdiSoyadi like '%" + Adi + "%'");
                return View(hastas);
            }
        }


        [AllowAnonymous]
        public IActionResult RandevuVer()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();

        }

        [CustomValidationDoktor]
        public IActionResult HastalarD()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<HastaDTO> hastas = new List<HastaDTO>();
            Hasta hasta = new Hasta();
            hastas = hasta.Lists("select * from Hasta where Status='Active'");
            return View(hastas);
        }
        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult HastalarD(int tip, string TC, string Adi)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<HastaDTO> hastas = new List<HastaDTO>();
            Hasta hasta = new Hasta();
            hastas = hasta.Lists("select * from Hasta where TC like '%" + TC + "%' and AdiSoyadi like '%" + Adi + "%'");
            return View(hastas);
        }
        [CustomValidationDoktor]
        public IActionResult IslemlerD()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<IslemlerDTO> hastas = new List<IslemlerDTO>();
            Islemler hasta = new Islemler();
            hastas = hasta.Lists("select I.HastaID, I.IslemGrubuID,I.IslemID,[Adı Soyadı] as adhasta,I.IslemAciklamasi,Tarih,c.CalisanID,c.AdiSoyadi,I.DrAciklaması from vw_IslemlerDR I join Calisan c on c.AdiSoyadi=I.AdiSoyadi where c.CalisanID=" + HttpContext.Session.GetString("userid"));
            return View(hastas);

        }

        [CustomValidationDoktor]
        public IActionResult LaboratuvarD()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<LabDTO> hastas = new List<LabDTO>();
            Lab lab = new Lab();
            hastas = lab.Lists("select * from Lab");
            return View(hastas);
        }
        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult LaboratuvarD([FromForm] LabDTO labDTO, int tip, string Durum, string Adi)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            try
            {
                List<LabDTO> hastas1 = new List<LabDTO>();
                Lab lab1 = new Lab();
                hastas1 = lab1.Lists("select * from Lab");
                if (tip == 5) 
                {
                    List<LabDTO> hastas = new List<LabDTO>();
                    Lab lab = new Lab();
                    hastas = lab.Lists("select * from Lab where LabDurum like '%"+Durum+ "%'");
                    return View(hastas);

                }
                else
                {

                    Lab lab = new Lab();
                    lab.update("Update Lab set LabDurum='" + labDTO.LabDurum + "'where LabID='" + labDTO.LabID + "'");
                    TempData["AlertMessage"] = "Lab Başarı ile güncellendi";
                    return View(hastas1);
                }
            }
            catch
            {
                TempData["AlertMessage"] = "Lab Güncelleme Başarısız";
                List<LabDTO> hastas = new List<LabDTO>();
                Lab lab = new Lab();
                hastas = lab.Lists("select * from Lab");
                return View(hastas);
            }

        }

        [CustomValidationDoktor]
        public IActionResult IslemEkleD()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<HastaDTO> hastas = new List<HastaDTO>();
            Hasta hasta = new Hasta();
            hastas = hasta.Lists("select * from Hasta where Status='Active'");
            return View(hastas);
        }

        public IActionResult FiyatListesi()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }

        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult IslemEkleD([FromForm] IslemEkle ıslemEkle,string draciklaması,string labadı, int tip, string TC, string Adi,int hastaid)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<HastaDTO> hastas = new List<HastaDTO>();
            Hasta hasta = new Hasta();
            hastas = hasta.Lists("select * from Hasta where Status='Active'");
            try
            {

                if (tip==5)
                {
                    
                    if(TC == null) { TC = " TC like '%%' "; } else { TC = " TC = '" + TC + "' "; }
                    Hasta hasta1 = new Hasta();
                    hastas = hasta1.Lists("select * from Hasta where" + TC + " and AdiSoyadi like '%" + Adi + "%'");
                    return View(hastas);
                }
                else {
                    if (ıslemEkle.prosedurid == 0 && ıslemEkle.islab == 0) {


                    }
                    else
                    {
                        if (ıslemEkle.islab == 0)
                        {
                            
                            ıslemEkle.islab = 0;
                            executer.execute("exec sp_islemekletransaction " + HttpContext.Session.GetString("userid") + "," + ıslemEkle.hastaid + "," + ıslemEkle.prosedurid + ",0," + HttpContext.Session.GetString("userid") + ",'" + draciklaması + "','1'");
                            TempData["AlertMessage"] = "İşlem Başarı ile eklendi";
                            return View(hastas);
                        }
                        else
                        {
                            
                            ıslemEkle.islab = 1;
                            executer.execute("exec sp_islemekletransaction " + HttpContext.Session.GetString("userid") + "," + ıslemEkle.hastaid + "," + ıslemEkle.prosedurid + ",1," + HttpContext.Session.GetString("userid") + ",'" + draciklaması + "','" + labadı + "'");
                            TempData["AlertMessage"] = "İşlem ve Lab Başarı ile eklendi";

                            return View(hastas);
                        }
                    } }
            }
            catch
            {
                TempData["AlertMessage"] = "İşlem ekleme başarısız";
                return View(hastas);
            }
            return View(hastas);
        }

        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult IslemlerD([FromForm] IslemlerDTO ıslemlerDTO, int tip, string id, string Adi, int islemid, string dates)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<IslemlerDTO> hastas = new List<IslemlerDTO>();
            Islemler hasta = new Islemler();
            hastas = hasta.Lists("select I.HastaID, I.IslemGrubuID,I.IslemID,[Adı Soyadı] as adhasta,I.IslemAciklamasi,Tarih,c.CalisanID,c.AdiSoyadi,I.DrAciklaması from vw_IslemlerDR I join Calisan c on c.AdiSoyadi=I.AdiSoyadi where c.CalisanID=" + HttpContext.Session.GetString("userid"));

            if (tip == 5)
            {
                if (dates != null)
                {
                    Islemler hasta1 = new Islemler();
                    string[] tarih = parse(dates);
                    if (tarih[0] != tarih[1])
                    {
                        hastas = hasta1.Lists("select I.HastaID, I.IslemGrubuID,I.IslemID,[Adı Soyadı] as adhasta,I.IslemAciklamasi,Tarih,c.CalisanID,c.AdiSoyadi,I.DrAciklaması from vw_IslemlerDR I join Calisan c on c.AdiSoyadi=I.AdiSoyadi where [Adı Soyadı] like '%" + Adi + "%' and str(I.HastaID) like '%" + id + "%' and str(I.IslemID) like '%" + islemid + "%' and c.CalisanID=" + HttpContext.Session.GetString("userid") + "and Tarih between '" + tarih[0] + "' and '" + tarih[1] + "'");
                    }
                    else {
                        hastas = hasta1.Lists("select I.HastaID, I.IslemGrubuID,I.IslemID,[Adı Soyadı] as adhasta,I.IslemAciklamasi,Tarih,c.CalisanID,c.AdiSoyadi,I.DrAciklaması from vw_IslemlerDR I join Calisan c on c.AdiSoyadi=I.AdiSoyadi where [Adı Soyadı] like '%" + Adi + "%' and str(I.HastaID) like '%" + id + "%' and str(I.IslemID) like '%" + islemid + "%' and c.CalisanID=" + HttpContext.Session.GetString("userid"));
                    }
                }
                else
                {
                    Islemler hasta2 = new Islemler();
                    hastas = hasta2.Lists("select I.HastaID, I.IslemGrubuID,I.IslemID,[Adı Soyadı] as adhasta,I.IslemAciklamasi,Tarih,c.CalisanID,c.AdiSoyadi,I.DrAciklaması from vw_IslemlerDR I join Calisan c on c.AdiSoyadi=I.AdiSoyadi where [Adı Soyadı] like '%" + Adi + "%' and str(I.HastaID) like '%" + id + "%' and str(I.IslemID) like '%" + islemid + "%' and c.CalisanID=" + HttpContext.Session.GetString("userid"));
                }
                    return View(hastas);
            }
            else if (tip == 0)
            {
                executer.execute("delete from Islem where IslemID=" + ıslemlerDTO.IslemID);
                TempData["AlertMessage"] = "İşlem Başarı ile Silindi";
                return View(hastas);
            }
            else if (tip == 1)
            {
                executer.execute("update Islem set DrAciklaması='" + ıslemlerDTO.DrAciklaması + "' where IslemID=" + ıslemlerDTO.IslemID);
                TempData["AlertMessage"] = "İşlem Başarı ile Silindi";
                return View(hastas);
            }
            else {
                return View(hastas);
            }



        }
        [CustomValidationUser]
        public IActionResult AnasayfaH()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }
        [AllowAnonymous]
        public IActionResult RandevuAlH()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }
        [HttpPost]
        public IActionResult RandevuAlH([FromForm] RandevuDTO randevuDTO, int doktorid, byte type)
        {//type 0 ilk giriş type 1 insert
            ViewBag.id = HttpContext.Session.GetString("userid");
            if (type == 0) {
                TempData["Doktorid"] = doktorid;
                VW_Doktorlar dr2 = new VW_Doktorlar();
                if(doktorid!=null || doktorid == 0) {

                    foreach(var item in dr2.Lists("select * from vw_Doktorlar where DoktorID=" + doktorid))
                    {
                        TempData["Uzmanlık"] = item.Uzmanlık;
                        TempData["drAdi"] = item.AdiSoyadi;

                    }
                }
                return View();
            }
            else if (type == 1)
            {
                string strtime = randevuDTO.Tarih.ToString("yyyy/dd/MM") + " " + randevuDTO.Tarih.ToString("HH") + ":" + randevuDTO.Tarih.ToString("mm");
                Randevu randevu1 = new Randevu();
                randevu1.insert("insert into Randevu values(" + randevuDTO.HastaID + "," + randevuDTO.DoktorID + ",1,'" + strtime + "','Online')");
                TempData["AlertMessage"] = "Randevu Alma İşlemi Başarılı";
                return View();

            }
            else
            {
                TempData["AlertMessage"] = "Randevu Alma İşlemi Başarısız";
                return View();

            }
            return View();

        }
        [CustomValidationLab]
        public IActionResult Laboratuvar()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }
        [CustomValidationLab]
        [HttpPost]
        public IActionResult Laboratuvar([FromForm] LabDTO labDTO)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            try
            {
                Lab lab = new Lab();
                lab.update("Update Lab set LabDurum='" + labDTO.LabDurum + "'where LabID='" + labDTO.LabID + "'");

                TempData["AlertMessage"] = "Lab Başarı ile güncellendi";
                return View();
            }
            catch
            {
                TempData["AlertMessage"] = "Lab Güncelleme Başarısız";
                return View();
            }

        }
        [CustomValidationYonetici]
        public IActionResult AnasayfaY()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }
        [CustomValidationYonetici]
        public IActionResult CalisanY()
        {
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<CalisanDTO> hastas = new List<CalisanDTO>();
            Calisan hasta = new Calisan();
            hastas = hasta.Lists("select * from Calisan");
            return View(hastas);
        }
        [HttpPost]
        [CustomValidationYonetici]
        public IActionResult CalisanY([FromForm] CalisanDTO calisanDTO, int tip, VW_DoktorlarDTO dto, string TC, string Adi,string unvan)
        {
            // 1 silme 2 güncelleme
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<CalisanDTO> hastas = new List<CalisanDTO>();
            Calisan hasta = new Calisan();
            hastas = hasta.Lists("select * from Calisan");


            try
            {
                if (tip == 5) 
                {

                    List<CalisanDTO> hastas1 = new List<CalisanDTO>();
                    Calisan hasta1 = new Calisan();
                    hastas1 = hasta1.Lists("select * from Calisan where Ünvan like '%"+unvan+"%' and AdiSoyadi like '%"+Adi+"%' and TC like '%"+TC+"%'");
                    return View(hastas1);
                }
                else
                {
                    if (calisanDTO.CalisanID != 0 && tip == 2)
                    {
                        Calisan calisancommand = new Calisan();
                        calisancommand.insert("update Calisan set TC='" + calisanDTO.TC + "',AdiSoyadi='" + calisanDTO.AdiSoyadi + "',Adres='" + calisanDTO.Adres
                            + "',Ünvan='" + calisanDTO.Ünvan + "'where CalisanID=" + calisanDTO.CalisanID);
                        TempData["AlertMessage"] = "Çalışan Başarı ile güncellendi";
                        return View(hastas);
                    }
                    else if (calisanDTO.CalisanID != 0 && tip == 1)
                    {
                        Calisan calisancommand = new Calisan();
                        calisancommand.delete("delete from Calisan where CalisanID=" + calisanDTO.CalisanID);
                        TempData["AlertMessage"] = "Çalışan Başarı ile silindi";
                        return View(hastas);
                    }
                    else if (dto.Uzmanlık != null)
                    {
                        executer.execute("exec sp_doktorekle '" + calisanDTO.AdiSoyadi + "','" + calisanDTO.TC + "','" + calisanDTO.Adres + "','" + calisanDTO.Ünvan + "','" + dto.Uzmanlık + "'");
                        return View(hastas);
                    }
                    else
                    {

                        if (CalisanKayıtYap(calisanDTO) == true)
                        {
                            TempData["AlertMessage"] = "Çalışan Başarı ile eklendi";
                            return View(hastas);
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Çalışan ekleme Başarısız";
                            return View(hastas);
                        }

                    }
                }
            }
            catch
            {

                List<CalisanDTO> hastas1 = new List<CalisanDTO>();
                Calisan hasta1 = new Calisan();
                hastas1 = hasta1.Lists("select * from Calisan");
                TempData["AlertMessage"] = "İşlem Başarısız";
                return View(hastas);

            }
            return View(hastas);


        }
        private bool CalisanKayıtYap(CalisanDTO calisan)
        {
            try
            {
                Calisan hastacommand = new Calisan();
                hastacommand.insert("insert into Calisan values('" + calisan.AdiSoyadi + "','" + calisan.TC + "','" + calisan.Adres + "','" + calisan.Ünvan + "',1)");
                return true;
            }
            catch
            {
                return false;
            }


        }
        [CustomValidationYonetici]
        public IActionResult MaliDurumY()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<VW_FaturalarDTO> hastas = new List<VW_FaturalarDTO>();
            VW_Faturalar hasta = new VW_Faturalar();
            hastas = hasta.Lists("select * from vw_Faturalar");
            return View(hastas);
        }
        [HttpPost]
        [CustomValidationYonetici]
        public IActionResult MaliDurumY([FromForm] GiderDTO gider, string id, int tip, string faturaid, string Adi,string dates)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<VW_FaturalarDTO> hastas = new List<VW_FaturalarDTO>();
            VW_Faturalar hasta = new VW_Faturalar();
            hastas = hasta.Lists("select * from vw_Faturalar");
            try
            {


                if (tip == 5)
                {

                    if (dates != null)
                    {
                        VW_Faturalar hasta1 = new VW_Faturalar();
                        string[] tarih = parse(dates);
                        if (tarih[0] != tarih[1])
                        {
                            hastas = hasta1.Lists("select * from vw_Faturalar where FaturaID like'%" + faturaid + "%' and AdiSoyadi like'%" + Adi + "%' and Tarih between '" + tarih[0] + "' and '" + tarih[1] + "'");
                        }
                        else
                        {
                            hastas = hasta1.Lists("select * from vw_Faturalar where FaturaID like'%" + faturaid + "%' and AdiSoyadi like'%" + Adi + "%'");
                        }
                    }
                    else {
                        VW_Faturalar hasta2 = new VW_Faturalar();
                        hastas = hasta2.Lists("select * from vw_Faturalar where FaturaID like'%" + faturaid + "%' and AdiSoyadi like'%" + Adi + "%'");
                        
                    }

                    
                    return View(hastas);
                }
                else
                {
                    if (id != null)
                    {
                        executer.execute("delete from Fatura where FaturaID='" + id + "'");
                        TempData["AlertMessage"] = "Gider ekleme Başarılı";
                        return View(hastas);
                    }
                    else
                    {

                        Gider hastacommand = new Gider();
                        hastacommand.insert("insert into Gider values(1," + gider.GiderTutari + ",'" + DateTime.UtcNow + "','" + gider.Aciklama + "')");
                        TempData["AlertMessage"] = "Gider ekleme Başarılı";
                        return View(hastas);
                    }
                }


            }
            catch
            {
                List<VW_FaturalarDTO> hastas1 = new List<VW_FaturalarDTO>();
                VW_Faturalar hasta1 = new VW_Faturalar();
                hastas1 = hasta1.Lists("select * from vw_Faturalar");
                TempData["AlertMessage"] = "Gider ekleme Başarısız";
                return View(hastas1);

            }
            return View(hastas);
        }
        [CustomValidationYonetici]
        public IActionResult MaasY()
        {
            ViewBag.id = HttpContext.Session.GetString("userid");
            ViewBag.isim = HttpContext.Session.GetString("username");
            List<VW_MaaslarDTO> hastas = new List<VW_MaaslarDTO>();
            VW_Maaslar hasta = new VW_Maaslar();
            hastas = hasta.Lists("select * from vw_Maaslar");
            return View(hastas);
        }
        [CustomValidationYonetici]
        [HttpPost]
        public IActionResult MaasY(int id, int tip, int calisanid, string Adi,string dates)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<VW_MaaslarDTO> hastas = new List<VW_MaaslarDTO>();
            VW_Maaslar hasta = new VW_Maaslar();
            hastas = hasta.Lists("select * from vw_Maaslar");

            if (tip == 5) {

                if (dates != null)
                {
                    VW_Maaslar hasta2 = new VW_Maaslar();
                    string[] tarih = parse(dates);
                    if (tarih[0] != tarih[1])
                    {
                        hastas = hasta2.Lists("select * from vw_Maaslar where AdiSoyadi like'%" + Adi + "%' and sonödenmetarihi between '" + tarih[0] +"' and '" + tarih[1]+"'");
                    }
                    else
                    {
                        hastas = hasta2.Lists("select * from vw_Maaslar where AdiSoyadi like'%" + Adi + "%'");
                    }
                }
                else
                {


                    VW_Maaslar hasta1 = new VW_Maaslar();
                    if (calisanid == 0) { hastas = hasta1.Lists("select * from vw_Maaslar where AdiSoyadi like'%" + Adi + "%'"); }
                    else { hastas = hasta1.Lists("select * from vw_Maaslar where AdiSoyadi like'%" + Adi + "%'"); }
                }
                    return View(hastas);
            }
            else
            {
                if (id > 0)
                {
                    executer.execute("exec sp_MaasOde " + id);
                    TempData["AlertMessage"] = "Gider ekleme Başarısz";
                    return View(hastas);
                }
                else
                {
                    TempData["AlertMessage"] = "Gider ekleme Başarısı";
                    executer.execute("exec sp_TumMaasOde ");
                    return View(hastas);
                }
            }

        }
        [AllowAnonymous]
        public IActionResult randevuHasta()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<RandevuHastaDTO> hastas = new List<RandevuHastaDTO>();
            RandevuHasta hasta = new RandevuHasta();
            hastas = hasta.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where Tarih<Getdate() and R.HastaID=" + HttpContext.Session.GetString("userid"));
            return View(hastas);
        }
        [CustomValidationUser]
        [HttpPost]
        public IActionResult randevuHasta([FromForm] RandevuDTO ds,int tip,string dradi,string bolum,string dates)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<RandevuHastaDTO> hastas = new List<RandevuHastaDTO>();
            RandevuHasta hasta = new RandevuHasta();
            hastas = hasta.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where Tarih<Getdate() and R.HastaID=" + HttpContext.Session.GetString("userid"));
            try
            {
                if (tip == 5)
                {
                    if (dates != null)
                    {
                        RandevuHasta hasta2 = new RandevuHasta();
                        string[] tarih = parse(dates);
                        if (tarih[0] != tarih[1])
                        {
                            hastas = hasta2.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where R.HastaID=" +   HttpContext.Session.GetString("userid") +" and Tarih between '"+ tarih[0] + "' and '" + tarih[1] + "' and C.AdiSoyadi like '%" + dradi + "%' and Uzmanlık like '%"+bolum+"%'");
                            return View(hastas);
                        }
                        else
                        {

                            hastas = hasta2.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where R.HastaID=" + HttpContext.Session.GetString("userid") + " and C.AdiSoyadi like '%" + dradi + "%' and Uzmanlık like '%" + bolum + "%'");
                            return View(hastas);
                        }
                    }
                    else
                    {
                        RandevuHasta hasta2 = new RandevuHasta();
                        hastas = hasta2.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where R.HastaID=" + HttpContext.Session.GetString("userid") + " and C.AdiSoyadi like '%" + dradi + "%' and Uzmanlık like '%" + bolum + "%'");
                        return View(hastas);
                    }

                    return View(hastas);

                }

                else {
                    string strtarih = ds.Tarih.ToString("yyyy/MM/dd") + " " + ds.Tarih.ToString("HH") + ":" + ds.Tarih.ToString("mm");
                    executer.execute("delete from Randevu where HastaID='" + ds.HastaID + "' and Tarih='" + strtarih + "'");
                    TempData["AlertMessage"] = "Randevu silme başarılı";
                    return View(hastas);
                }
            }
            catch
            {
                TempData["AlertMessage"] = "Randevu silme Başarısız";
                return View();

            }
        }
        public IActionResult Ziyaretlerim()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }
        public IActionResult Bilgilerim()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }
        [HttpPost]
        public IActionResult Bilgilerim([FromForm] HastaDTO hastaDTO)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            try {

                Hasta hastacommand = new Hasta();
                hastacommand.insert("update Hasta set Iletisim='" + hastaDTO.Iletisim + "',Adres='"+hastaDTO.Adres +"',Email='"+hastaDTO.Email+"' where HastaID=" + hastaDTO.hastaid);
                TempData["AlertMessage"] = "Güncelleme işlemi Başarılı";
                return View();
            }
            catch
            {
                TempData["AlertMessage"] = "Güncelleme işlemi Başarısız";
                return View();
            }


        }
        public IActionResult GiderEkle()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<GiderDTO> hastas = new List<GiderDTO>();
            Gider hasta = new Gider();
            hastas = hasta.Lists("select * from Gider");
            return View(hastas);
        }
        [HttpPost]
        public IActionResult GiderEkle([FromForm]GiderDTO giderDTO, int tip, string aciklama, string Adi,string dates)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<GiderDTO> hastas = new List<GiderDTO>();
            Gider hasta = new Gider();
            hastas = hasta.Lists("select * from Gider");
            try
            {
                if (tip==5) {
                    if (dates != null)
                    {
                        Gider hasta2 = new Gider();
                        string[] tarih = parse(dates);
                        if (tarih[0] != tarih[1])
                        {
                            hastas = hasta2.Lists("select * from Gider where Açıklama like '%" + aciklama + "%' and Tarih between '" + tarih[0] + "' and '" + tarih[1] + "'");
                        }
                        else
                        {
                            hastas = hasta2.Lists("select * from Gider where Açıklama like '%" + aciklama + "%'");
                        }
                        return View(hastas);
                    }
                    else
                    {

                        Gider hasta1 = new Gider();
                        hastas = hasta1.Lists("select * from Gider where Açıklama like '%" + aciklama + "%'");
                        return View(hastas);
                    }


                } else { 
                DateTime Tarih = DateTime.UtcNow;
                string strtime = Tarih.ToString("yyyy/dd/MM") + " " + Tarih.ToString("HH") + ":" + Tarih.ToString("mm");

                Gider hastacommand = new Gider();
                hastacommand.insert("insert into Gider values(1," + giderDTO.GiderTutari + ",'" + strtime + "','" + giderDTO.Aciklama + "')");
                TempData["AlertMessage"] = "Gider ekleme Başarılı";
                    return View(hastas);

                }

            }
            catch
            {
                TempData["AlertMessage"] = "Gider ekleme Başarısız";
                return View(hastas);

            }
        }
        public IActionResult labid(string value)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");

            Hasta hasta = new Hasta();
           foreach(var item in hasta.Lists("select h.HastaID,h.TC,h.AdiSoyadi,h.Iletisim,h.DogumTarihi,h.Status,h.KayıtTarihi,h.Cinsiyet,h.IslemiYapan from hasta h join Islem I on I.HastaID=h.HastaID join Lab L on I.IslemID=L.LabID where L.LabID=" +value))
            {
                TempData["labid"] = value;
                TempData["HastaAdi"] = item.AdiSoyadi;
                TempData["HastaIletisim"] = item.Iletisim;
            }
            

            return Ok();
        }
        public IActionResult HastaIDHK(string value)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            Hasta hasta = new Hasta();
            foreach (var item in hasta.Lists("select h.HastaID,h.TC,h.AdiSoyadi,h.Iletisim,h.DogumTarihi,h.Status,h.KayıtTarihi,h.Cinsiyet,h.IslemiYapan from hasta h join Islem I on I.HastaID=h.HastaID join Lab L on I.IslemID=L.LabID where h.TC='" + value+"'"))
            {
               
                TempData["Hastaid"] = item.hastaid;
                
            }


            return Ok();
        }

        
        public IActionResult HastaGüncelle()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<HastaDTO> hastas = new List<HastaDTO>();
                Hasta hasta = new Hasta();
                hastas = hasta.Lists("select * from Hasta where Status='Active'");
                return View(hastas);
        


        }
        [HttpPost]
        public IActionResult HastaGüncelle([FromForm] HastaDTO hasta, int tip, string TC,string Adi)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");

            try
            {
                List<HastaDTO> hastas = new List<HastaDTO>();
                Hasta hasta3 = new Hasta();
                hastas = hasta3.Lists("select * from Hasta where TC like '%" + TC + "%' and AdiSoyadi like '%" + Adi + "%'");
                if (tip==1)
                {
                    

                        return View(hastas);

                    
                }
                if (hasta.hastaid != 0 && hasta.AdiSoyadi != null)
                {
                    Hasta hastacommand = new Hasta();
                    hastacommand.insert("update Hasta set TC='" + hasta.TC + "',AdiSoyadi='" + hasta.AdiSoyadi + "',Iletisim='" + hasta.Iletisim
                        + "',DogumTarihi='" + hasta.DogumTarihi + "',Status='" + "Active" + "',Cinsiyet='" + hasta.Cinsiyet + "' where TC='" + hasta.TC+"'");
                    TempData["AlertMessage"] = "Hasta Başarı ile güncellendi";

                    return View(hastas);
                }
                else if (hasta.hastaid == 0 && hasta.AdiSoyadi == null)
                {
                    Hasta hastacommand = new Hasta();
                    hastacommand.delete("update Hasta set Status='Passive' where TC='" + hasta.TC+"'");
                    TempData["AlertMessage"] = "Hasta Başarı ile Silindi";
                    return View(hastas);
                }
                return View(hastas);
                
            }
            catch
            {
                TempData["AlertMessage"] = "Hasta ekleme Başarısız";
                List<HastaDTO> hastas = new List<HastaDTO>();
                Hasta hasta3 = new Hasta();
                hastas = hasta3.Lists("select * from Hasta where TC like '%" + TC + "%' and AdiSoyadi like '%" + Adi + "%'");
                return View(hastas);
            }
           

        }
        public IActionResult FiyatListesiY()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            return View();
        }
        [HttpPost]
        public IActionResult FiyatListesiY([FromForm] FiyatListesiDTO fiyat,string islemadi,string ucret,int tip)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            try
            {
                if (tip == 3)
                {
                    executer.execute("Update FiyatListesi set IsActive=0 where Adi=" + fiyat.Adi);
                    TempData["AlertMessage"] = "Fiyat Güncelleme Başarılı";
                }
                else
                {

                    if (islemadi != null)
                    {
                        executer.execute("insert into FiyatListesi values('" + islemadi + "'," + ucret + ",0)");
                        TempData["AlertMessage"] = "Fiyat Güncelleme Başarılı";
                    }
                    else
                    {
                        executer.execute("Update FiyatListesi set Ucreti=" + fiyat.Ucreti + " where ProsedurID=" + fiyat.ProsedurID);
                        TempData["AlertMessage"] = "Fiyat Güncelleme Başarılı";
                    }
                }
                return View();



            }
            catch
            {
                TempData["AlertMessage"] = "Fiyat Güncelleme Başarısız";
                return View();

            }
        }
        public IActionResult IzinYonetimi()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<YillikIzinDTO> hastas = new List<YillikIzinDTO>();
            YillikIzin hasta = new YillikIzin();
            hastas = hasta.Lists("select distinct y.DoktorID,y.BaslangicTarihi,y.BitisTarihi,d.AdiSoyadi,d.Uzmanlık from YillikIzin  y join vw_doktorlar d on d.DoktorID=y.DoktorID");
            return View(hastas);
        }
        [HttpPost]
        public IActionResult IzinYonetimi(int DoktorID, int tip, string uzmanlik, string Adi,string dates)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<YillikIzinDTO> hastas = new List<YillikIzinDTO>();
            YillikIzin hasta = new YillikIzin();
            hastas = hasta.Lists("select distinct y.DoktorID,y.BaslangicTarihi,y.BitisTarihi,d.AdiSoyadi,d.Uzmanlık from YillikIzin  y join vw_doktorlar d on d.DoktorID=y.DoktorID");
            try
            {
                if (tip == 5)
                {
                    YillikIzin hasta1 = new YillikIzin();
                    hastas = hasta1.Lists("select distinct y.DoktorID,y.BaslangicTarihi,y.BitisTarihi,d.AdiSoyadi,d.Uzmanlık from YillikIzin  y join vw_doktorlar d on d.DoktorID=y.DoktorID where d.Uzmanlık like'%"+uzmanlik+"%' and d.AdiSoyadi like'%"+Adi+"%'");
                    return View(hastas);
                }
                else
                {
                    string[] trh = parse(dates);
                    executer.execute("Update YillikIzin set BaslangicTarihi='" + trh[0] + "',BitisTarihi='" + trh[1] + "' where DoktorID=" + DoktorID);
                    TempData["AlertMessage"] = "İzin ekleme Başarılı";
                    YillikIzin hasta2 = new YillikIzin();
                    hastas = hasta2.Lists("select distinct y.DoktorID,y.BaslangicTarihi,y.BitisTarihi,d.AdiSoyadi,d.Uzmanlık from YillikIzin  y join vw_doktorlar d on d.DoktorID=y.DoktorID");

                    return View(hastas);
                }



            }
            catch
            {
                TempData["AlertMessage"] = "İzin ekleme Başarısız";
                return View(hastas);

            }
        }
        [CustomValidationHK]
        public IActionResult RandevuGRNThk()
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<RandevuHastaDTO> hastas = new List<RandevuHastaDTO>();
            RandevuHasta hasta = new RandevuHasta();
            hastas = hasta.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where Tarih<Getdate()");
            return View(hastas);

          
        }
        [CustomValidationHK]
        [HttpPost]
        public IActionResult RandevuGRNThk([FromForm] RandevuDTO ds, int tip, string TC, string Adi,string dradi,string dates)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<RandevuHastaDTO> hastas1 = new List<RandevuHastaDTO>();
            RandevuHasta hasta1 = new RandevuHasta();
            hastas1 = hasta1.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where Tarih<Getdate()");

            try
            {
                if (tip==5)
                {
                    if (dates != null)
                    {
                        RandevuHasta hasta2 = new RandevuHasta();
                        string[] tarih = parse(dates);
                        if (tarih[0] != tarih[1])
                        {
                            hastas1 = hasta2.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where Tarih between '" + tarih[0] +"' and '" + tarih[1] +"' and R.TC like '%"+TC+"%' and R.AdiSoyadi like'%"+Adi+"%' and C.AdiSoyadi like '%"+dradi+"%'");
                            return View(hastas1);
                        }
                        else
                        {

                            hastas1 = hasta2.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where R.TC like '%" + TC + "%' and R.AdiSoyadi like'%" + Adi + "%' and C.AdiSoyadi like '%" + dradi + "%'");
                            return View(hastas1);
                        }
                    }
                    else
                    {
                        RandevuHasta hasta2 = new RandevuHasta();
                        hastas1 = hasta2.Lists("select RandevuID,HastaID,R.DoktorID,R.TC,R.AdiSoyadi,Tarih,Uzmanlık,C.AdiSoyadi as dradi from vw_Randevu R join Doktor D on D.DoktorID=R.DoktorID join Calisan C on c.CalisanID=D.DoktorID where R.TC like '%" + TC + "%' and R.AdiSoyadi like'%" + Adi + "%' and C.AdiSoyadi like '%" + dradi + "%'");
                        return View(hastas1);
                    }

                            return View(hastas1);

                }
               else {
                    Hasta hasta = new Hasta();
                    foreach (var item in hasta.Lists("select * from Hasta where TC='" + ds.TC + "'"))
                    {

                        ds.HastaID = item.hastaid;

                    }



                    string strtarih = ds.Tarih.ToString("yyyy/MM/dd") + " " + ds.Tarih.ToString("HH") + ":" + ds.Tarih.ToString("mm");
                    executer.execute("delete from Randevu where HastaID='" + ds.HastaID + "' and Tarih='" + strtarih + "'");
                    TempData["AlertMessage"] = "Randevu silme başarılı";
                    return View();
                }
            
            }
            catch
            {
                TempData["AlertMessage"] = "Randevu silme Başarısız";
                return View();

            }
            


            return View();
        }
        [CustomValidationDoktor]
        public IActionResult CikisVer() 
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            List<vw_cikisverAnaKisimDTO> hastas = new List<vw_cikisverAnaKisimDTO>();
            vw_cikisverAnaKisim hasta = new vw_cikisverAnaKisim();
            hastas = hasta.Lists("select * from vw_cikisverAnaKisim");
            return View(hastas);
        }
        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult CikisVer([FromForm] IslemEkle ıslemEkle,int tip,string TC,string Adi)
        {
            ViewBag.isim = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetString("userid");
            if (tip == 1)
            {
                List<vw_cikisverAnaKisimDTO> hastas = new List<vw_cikisverAnaKisimDTO>();
                vw_cikisverAnaKisim hasta = new vw_cikisverAnaKisim();
                hastas = hasta.Lists("select * from vw_cikisverAnaKisim where TC like '%" + TC + "%' and AdiSoyadi like '%" + Adi + "%'");
                return View(hastas);

            }
            else
            {
                executer.execute("UPDATE IslemGrubu SET IslemDurumu = 0 WHERE HastaID = '" + ıslemEkle.hastaid + "'");
                TempData["AlertMessage"] = " Başarı ile Çıkış verildi";
                List<vw_cikisverAnaKisimDTO> hastas = new List<vw_cikisverAnaKisimDTO>();
                vw_cikisverAnaKisim hasta = new vw_cikisverAnaKisim();
                hastas = hasta.Lists("select * from vw_cikisverAnaKisim");
                return View(hastas);
            }


        }


        private string[] parse(string time){




            string[] tarihler = time.Replace(" ", "").Split('-');

            string[] parcalar = tarihler[0].Split('/');
            string aa = parcalar[0];
            string gg = parcalar[1];
            string yyyy = parcalar[2];

            tarihler[0] = $"{gg}/{aa}/{yyyy}";

            string[] parcalar1 = tarihler[1].Split('/');
            string aa1 = parcalar1[0];
            string gg1 = parcalar1[1];
            string yyyy1 = parcalar1[2];

            tarihler[1] = $"{gg1}/{aa1}/{yyyy1}";
            return tarihler;
        }

    }
}
