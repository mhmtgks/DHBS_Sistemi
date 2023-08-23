using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DHBS_Sistemi.Models;
using Service.DTO;
using Service;

namespace DHBS_Sistemi.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return RedirectToAction("Login", "KimlikDoğrulama");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SignOut()
        {
            HttpContext.Session.SetString("Token", "");
            return RedirectToAction("Login", "KimlikDoğrulama");
        }
        public IActionResult CalismaSaatleri()
        {          
                    VW_Doktorlar w_Doktorlar = new VW_Doktorlar();
            List<VW_DoktorlarDTO> w_ = new List<VW_DoktorlarDTO>();
            w_ = w_Doktorlar.Lists("select * from vw_Doktorlar");

                return View(w_);

            
        }
        [HttpPost]
        public IActionResult CalismaSaatleri([FromForm] RandevuDTO randevuDTO, int doktorid, byte type)
        {//type 0 ilk giriş type 1 insert
            VW_Doktorlar w_Doktorlar = new VW_Doktorlar();
            List<VW_DoktorlarDTO> w_ = new List<VW_DoktorlarDTO>();
            w_ = w_Doktorlar.Lists("select * from vw_Doktorlar");
            ViewBag.id = HttpContext.Session.GetString("userid");
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
                return View(w_);
            }
            else if (type == 1)
            {
                string strtime = randevuDTO.Tarih.ToString("yyyy/MM/dd") + " " + randevuDTO.Tarih.ToString("HH") + ":" + randevuDTO.Tarih.ToString("mm");
                Randevu randevu1 = new Randevu();
                randevu1.insert("insert into Randevu values(" + randevuDTO.HastaID + "," + randevuDTO.DoktorID + ",1,'" + strtime + "','Online')");
                TempData["AlertMessage"] = "Randevu Alma İşlemi Başarılı";
                return View(w_);

            }
            else
            {
                TempData["AlertMessage"] = "Randevu Alma İşlemi Başarısız";
                return View(w_);

            }
            return View();


        }
    }
}