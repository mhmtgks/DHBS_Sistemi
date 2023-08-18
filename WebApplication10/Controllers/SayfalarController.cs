﻿
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

        [CustomValidationUser]
        public IActionResult YöneticiAnaSayfa()
        {
            return View();


        }
        [CustomValidationHK]
        public IActionResult HastaKayit()
        {
            return View();
        }
        [HttpPost]
        [CustomValidationHK]
        public IActionResult HastaKayit([FromForm] HastaDTO hasta)
        {
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
            try
            {

                string strtime = DateTime.UtcNow.ToString("yyyy/MM/dd") + " " + DateTime.UtcNow.ToString("HH") + ":" + DateTime.UtcNow.ToString("mm");
                Hasta hastacommand = new Hasta();
                hastacommand.insert("insert into Hasta values('" + hasta.TC + "','" + hasta.AdiSoyadi + "','" + hasta.Iletisim
                    + "','" + hasta.DogumTarihi + "','" + "Active" + "','" + strtime + "','" + hasta.Cinsiyet + "'," + idExtractor.userid + ",'"+hasta.Email+"','"+hasta.Adres+"')");
                return true;
            }
            catch {
                return false; }


        }
        [CustomValidationHK]
        public IActionResult AnaSayfa()
        {

            return View();

        }
        [CustomValidationHK]
        public IActionResult RandevularHK()
        {

            return View();

        }
        [CustomValidationHK]
        [HttpPost]
        public IActionResult RandevularHK([FromForm] RandevuDTO randevuDTO, int doktorid, byte type, string TC)
        {//type 0 ilk giriş type 1 insert
            if(TempData["TChid"] == null && TC!=null)
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
                    string strtime = randevuDTO.Tarih.ToString("yyyy/MM/dd") + " " + randevuDTO.Tarih.ToString("HH") + ":" + randevuDTO.Tarih.ToString("mm");
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

            List<HastaDTO> hastas = new List<HastaDTO>();
            Hasta hasta = new Hasta();
            hastas = hasta.Lists("select * from Hasta where Status='Active'");
            return View(hastas);

        }
        [CustomValidationHK]
        [HttpPost]
        public IActionResult IslemlerHK(string TC,string Adi)
        {
            List<HastaDTO> hastas = new List<HastaDTO>();
            Hasta hasta3 = new Hasta();
            hastas = hasta3.Lists("select * from Hasta where TC like '%" + TC + "%' and AdiSoyadi like '%" + Adi + "%'");
            return View(hastas);

        }
        [CustomValidationHK]
        public IActionResult LaboratuvarHK()
        {

            return View();
        }
        [CustomValidationHK]
        [HttpPost]
        public IActionResult LaboratuvarHK([FromForm] LabDTO labDTO)
        {
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
        [CustomValidationDoktor]
        public IActionResult AnaSayfaD()
        {
            return View();
        }

        [CustomValidationDoktor]
        public IActionResult RandevularD()
        {

            return View();

        }


        [AllowAnonymous]
        public IActionResult RandevuVer()
        {

            return View();

        }

        [CustomValidationDoktor]
        public IActionResult HastalarD()
        {
            return View();
        }
        [CustomValidationDoktor]
        public IActionResult IslemlerD()
        {
            List<IslemlerDTO> hastas = new List<IslemlerDTO>();
            Islemler hasta = new Islemler();
            hastas = hasta.Lists("select I.HastaID, I.IslemGrubuID,I.IslemID,[Adı Soyadı] as adhasta,I.IslemAciklamasi,Tarih,c.CalisanID,c.AdiSoyadi,I.DrAciklaması from vw_IslemlerDR I join Calisan c on c.AdiSoyadi=I.AdiSoyadi where c.CalisanID=" + idExtractor.userid);
            return View(hastas);

        }

        [CustomValidationDoktor]
        public IActionResult LaboratuvarD()
        {
            return View();
        }
        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult LaboratuvarD([FromForm] LabDTO labDTO)
        {
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

        [CustomValidationDoktor]
        public IActionResult IslemEkleD()
        {
            return View();
        }

        public IActionResult FiyatListesi()
        {
            return View();
        }

        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult IslemEkleD([FromForm] IslemEkle ıslemEkle,string draciklaması,string labadı)
        {
            try
            {
                if (ıslemEkle.prosedurid == 0 && ıslemEkle.islab == 0) {


                }
                else
                {
                    if (ıslemEkle.islab == 0)
                    {
                        ıslemEkle.doktorid = idExtractor.userid;
                        ıslemEkle.islab = 0;
                        executer.execute("exec sp_islemekletransaction " + ıslemEkle.doktorid + "," + ıslemEkle.hastaid + "," + ıslemEkle.prosedurid + ",0," + idExtractor.userid+",'"+draciklaması+"','1'");
                        TempData["AlertMessage"] = "İşlem Başarı ile eklendi";
                    }
                    else
                    {
                        ıslemEkle.doktorid = idExtractor.userid;
                        ıslemEkle.islab = 1;
                        executer.execute("exec sp_islemekletransaction " + ıslemEkle.doktorid + "," + ıslemEkle.hastaid + "," + ıslemEkle.prosedurid + ",1," + idExtractor.userid + ",'" + draciklaması + "','"+labadı+"'");
                        TempData["AlertMessage"] = "İşlem ve Lab Başarı ile eklendi";


                    }
                }
            }
            catch
            {
                TempData["AlertMessage"] = "İşlem ekleme başarısız";
                return View();
            }
            return View();
        }

        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult IslemlerD([FromForm] IslemlerDTO ıslemlerDTO, int tip, string id, string Adi, int islemid)
        {
            if (tip == 5)
            {
                List<IslemlerDTO> hastas = new List<IslemlerDTO>();
                Islemler hasta3 = new Islemler();
                hastas = hasta3.Lists("select I.HastaID, I.IslemGrubuID,I.IslemID,[Adı Soyadı] as adhasta,I.IslemAciklamasi,Tarih,c.CalisanID,c.AdiSoyadi,I.DrAciklaması from vw_IslemlerDR I join Calisan c on c.AdiSoyadi=I.AdiSoyadi where [Adı Soyadı] like '%" + Adi + "%' and str(I.HastaID) like '%" + id + "%' and str(I.IslemID) like '%" + islemid + "%' and c.CalisanID=" + idExtractor.userid);
                return View(hastas);
            }
            else if (tip == 0)
            {
                executer.execute("delete from Islem where IslemID=" + ıslemlerDTO.IslemID);
                TempData["AlertMessage"] = "İşlem Başarı ile Silindi";
                return View();
            }
            else if (tip == 1)
            {
                executer.execute("update Islem set DrAciklaması='" + ıslemlerDTO.DrAciklaması + "' where IslemID=" + ıslemlerDTO.IslemID);
                TempData["AlertMessage"] = "İşlem Başarı ile Silindi";
                return View();
            }
            else { 
                return View();
        }



        }
        [CustomValidationUser]
        public IActionResult AnasayfaH()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult RandevuAlH()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RandevuAlH([FromForm] RandevuDTO randevuDTO, int doktorid, byte type)
        {//type 0 ilk giriş type 1 insert
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
                string strtime = randevuDTO.Tarih.ToString("yyyy/MM/dd") + " " + randevuDTO.Tarih.ToString("HH") + ":" + randevuDTO.Tarih.ToString("mm");
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

            return View();
        }
        [CustomValidationLab]
        [HttpPost]
        public IActionResult Laboratuvar([FromForm] LabDTO labDTO)
        {
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

            return View();
        }
        [CustomValidationYonetici]
        public IActionResult CalisanY()
        {

            return View();
        }
        [HttpPost]
        [CustomValidationYonetici]
        public IActionResult CalisanY([FromForm] CalisanDTO calisanDTO, int tip, VW_DoktorlarDTO dto)
        {
            // 1 silme 2 güncelleme

            try
            {
                if (calisanDTO.CalisanID != 0 && tip == 2)
                {
                    Calisan calisancommand = new Calisan();
                    calisancommand.insert("update Calisan set TC='" + calisanDTO.TC + "',AdiSoyadi='" + calisanDTO.AdiSoyadi + "',Adres='" + calisanDTO.Adres
                        + "',Ünvan='" + calisanDTO.Ünvan + "'where CalisanID=" + calisanDTO.CalisanID);
                    TempData["AlertMessage"] = "Çalışan Başarı ile güncellendi";
                    return View();
                }
                else if (calisanDTO.CalisanID != 0 && tip == 1)
                {
                    Calisan calisancommand = new Calisan();
                    calisancommand.delete("delete from Calisan where CalisanID=" + calisanDTO.CalisanID);
                    TempData["AlertMessage"] = "Çalışan Başarı ile silindi";
                    return View();
                }
                else if (dto.Uzmanlık != null)
                {
                    executer.execute("exec sp_doktorekle '" + calisanDTO.AdiSoyadi + "','" + calisanDTO.TC + "','" + calisanDTO.Adres + "','" + calisanDTO.Ünvan + "','" + dto.Uzmanlık + "'");
                    return View();
                }
                else
                {

                    if (CalisanKayıtYap(calisanDTO) == true)
                    {
                        TempData["AlertMessage"] = "Çalışan Başarı ile eklendi";
                        return View();
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Çalışan ekleme Başarısız";
                        return View();
                    }

                }
            }
            catch
            {
                TempData["AlertMessage"] = "İşlem Başarısız";
                return View();

            }



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

            return View();
        }
        [HttpPost]
        [CustomValidationYonetici]
        public IActionResult MaliDurumY([FromForm] GiderDTO gider, string id)
        {

            try
            {
                if (id != null)
                {
                    executer.execute("delete from Fatura where FaturaID='" + id + "'");
                    TempData["AlertMessage"] = "Gider ekleme Başarılı";
                    return View();
                }
                else {

                    Gider hastacommand = new Gider();
                    hastacommand.insert("insert into Gider values(1," + gider.GiderTutari + ",'" + DateTime.UtcNow + "','" + gider.Aciklama + "')");
                    TempData["AlertMessage"] = "Gider ekleme Başarılı";
                    return View();
                }


            }
            catch
            {
                TempData["AlertMessage"] = "Gider ekleme Başarısız";
                return View();

            }

        }
        [CustomValidationYonetici]
        public IActionResult MaasY()
        {

            return View();
        }
        [CustomValidationYonetici]
        [HttpPost]
        public IActionResult MaasY(int id)
        {
            if (id > 0) {
                executer.execute("exec sp_MaasOde " + id);
                TempData["AlertMessage"] = "Gider ekleme Başarısz";
                return View();
            }
            else
            {
                TempData["AlertMessage"] = "Gider ekleme Başarısı";
                executer.execute("exec sp_TumMaasOde ");
                return View();
            }

        }
        [AllowAnonymous]
        public IActionResult randevuHasta()
        {


            return View();
        }
        [CustomValidationUser]
        [HttpPost]
        public IActionResult randevuHasta([FromForm] RandevuDTO ds)
        {
            try
            {

                string strtarih = ds.Tarih.ToString("yyyy/MM/dd") + " " + ds.Tarih.ToString("HH") + ":" + ds.Tarih.ToString("mm");
                executer.execute("delete from Randevu where HastaID='" + ds.HastaID + "' and Tarih='" + strtarih + "'");
                TempData["AlertMessage"] = "Randevu silme başarılı";
                return View();
            }
            catch
            {
                TempData["AlertMessage"] = "Randevu silme Başarısız";
                return View();

            }
        }
        public IActionResult Ziyaretlerim()
        {


            return View();
        }
        public IActionResult Bilgilerim()
        {


            return View();
        }
        [HttpPost]
        public IActionResult Bilgilerim([FromForm] HastaDTO hastaDTO)
        {
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


            return View();
        }
        [HttpPost]
        public IActionResult GiderEkle([FromForm]GiderDTO giderDTO)
        {
            try
            {
                DateTime Tarih = DateTime.UtcNow; 
                string strtime = Tarih.ToString("yyyy/MM/dd") + " " +Tarih.ToString("HH") + ":" +Tarih.ToString("mm");

                Gider hastacommand = new Gider();
                    hastacommand.insert("insert into Gider values(1," + giderDTO.GiderTutari + ",'" + strtime + "','" + giderDTO.Aciklama + "')");
                    TempData["AlertMessage"] = "Gider ekleme Başarılı";
                    return View();
                


            }
            catch
            {
                TempData["AlertMessage"] = "Gider ekleme Başarısız";
                return View();

            }
        }
        public IActionResult labid(string value)
        {

                                   
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


            Hasta hasta = new Hasta();
            foreach (var item in hasta.Lists("select h.HastaID,h.TC,h.AdiSoyadi,h.Iletisim,h.DogumTarihi,h.Status,h.KayıtTarihi,h.Cinsiyet,h.IslemiYapan from hasta h join Islem I on I.HastaID=h.HastaID join Lab L on I.IslemID=L.LabID where h.TC='" + value+"'"))
            {
               
                TempData["Hastaid"] = item.hastaid;
                
            }


            return Ok();
        }

        
        public IActionResult HastaGüncelle()
        {

                List<HastaDTO> hastas = new List<HastaDTO>();
                Hasta hasta = new Hasta();
                hastas = hasta.Lists("select * from Hasta where Status='Active'");
                return View(hastas);
        


        }
        [HttpPost]
        public IActionResult HastaGüncelle([FromForm] HastaDTO hasta, int tip, string TC,string Adi)
        {

            try
            {
                if(tip==1)
                {
                    
                        List<HastaDTO> hastas = new List<HastaDTO>();
                        Hasta hasta3 = new Hasta();
                        hastas = hasta3.Lists("select * from Hasta where TC like '%"+TC+ "%' and AdiSoyadi like '%"+Adi+"%'");
                        return View(hastas);

                    
                }
                if (hasta.hastaid != 0 && hasta.AdiSoyadi != null)
                {
                    Hasta hastacommand = new Hasta();
                    hastacommand.insert("update Hasta set TC='" + hasta.TC + "',AdiSoyadi='" + hasta.AdiSoyadi + "',Iletisim='" + hasta.Iletisim
                        + "',DogumTarihi='" + hasta.DogumTarihi + "',Status='" + "Active" + "',Cinsiyet='" + hasta.Cinsiyet + "' where TC='" + hasta.TC+"'");
                    TempData["AlertMessage"] = "Hasta Başarı ile güncellendi";
                    return View();
                }
                else if (hasta.hastaid == 0 && hasta.AdiSoyadi == null)
                {
                    Hasta hastacommand = new Hasta();
                    hastacommand.delete("update Hasta set Status='Passive' where TC='" + hasta.TC+"'");
                    TempData["AlertMessage"] = "Hasta Başarı ile Silindi";
                    return View();
                }
                
            }
            catch
            {
                TempData["AlertMessage"] = "Hasta ekleme Başarısız";
                return View();

            }
            return View();

        }
        public IActionResult FiyatListesiY()
        {


            return View();
        }
        [HttpPost]
        public IActionResult FiyatListesiY([FromForm] FiyatListesiDTO fiyat)
        {


            try
            {
                executer.execute("Update FiyatListesi set Ucreti=" + fiyat.Ucreti + " where ProsedurID=" + fiyat.ProsedurID);
                TempData["AlertMessage"] = "Fiyat Güncelleme Başarılı";
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


            return View();
        }
        [HttpPost]
        public IActionResult IzinYonetimi(int DoktorID,DateTime BaslangicTarihi,DateTime BitisTarihi)
        {

            try
            {
                string strbaslangic = BaslangicTarihi.ToString("yyyy/MM/dd") + " " + BaslangicTarihi.ToString("HH") + ":" + BaslangicTarihi.ToString("mm");
                string strbitis = BitisTarihi.ToString("yyyy/MM/dd") + " " + BitisTarihi.ToString("HH") + ":" + BitisTarihi.ToString("mm");
                executer.execute("Update YillikIzin set BaslangicTarihi='" + strbaslangic + "',BitisTarihi='"+ strbitis + "' where DoktorID=" + DoktorID);
                TempData["AlertMessage"] = "İzin ekleme Başarılı";
                return View();



            }
            catch
            {
                TempData["AlertMessage"] = "İzin ekleme Başarısız";
                return View();

            }
        }
        public IActionResult RandevuGRNThk()
        {


            return View();
        }
        [HttpPost]
        public IActionResult RandevuGRNThk([FromForm] RandevuDTO ds)
        {
            try {
                Hasta hasta = new Hasta();
                foreach (var item in hasta.Lists("select * from Hasta where TC='" + ds.TC+"'"))
                {

                    ds.HastaID = item.hastaid;

                }

           

                string strtarih = ds.Tarih.ToString("yyyy/MM/dd") + " " + ds.Tarih.ToString("HH") + ":" + ds.Tarih.ToString("mm");
                executer.execute("delete from Randevu where HastaID='" + ds.HastaID + "' and Tarih='" + strtarih + "'");
                TempData["AlertMessage"] = "Randevu silme başarılı";
                return View();
            
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
            return View(); 
        }
        [CustomValidationDoktor]
        [HttpPost]
        public IActionResult CikisVer([FromForm] IslemEkle ıslemEkle)
        {

            executer.execute("UPDATE IslemGrubu SET IslemDurumu = 0 WHERE HastaID = '" + ıslemEkle.hastaid + "'");
            TempData["AlertMessage"] = " Başarı ile Çıkış verildi";
            return View();
        }

    }
}
