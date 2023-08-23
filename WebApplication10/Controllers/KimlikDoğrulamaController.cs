using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DHBS_Sistemi.Models;
using Service;
using Service.DTO;

namespace DHBS_Sistemi.Controllers
{
    [Route("Login1")]


    public class KimlikDoğrulamaController : Controller
    {
       Router router = new Router();
        Giris giris = new Giris();
        Hasta hastak = new Hasta();
        public readonly JwtAyarları _jwtAyarları;
        public KimlikDoğrulamaController(IOptions<JwtAyarları> jwtAyarları)
        {
            //this.router = new Route();
            this._jwtAyarları = jwtAyarları.Value;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromForm] GirisDTO apikullanıcıBilgileri)
        {
            var kullanıcı = KimlikDoğrulamaYap(apikullanıcıBilgileri);
            if (kullanıcı == null) {

                ViewBag.style = "margin-left: 100px;margin-bottom: 20px; -webkit-text-fill-color:red;";
                ViewBag.Alert = "Kullanıcı Bulunamadı";
                return View(); 
            
            
            }
            ViewBag.style = "display:none";
            ViewBag.Alert = "";
            var token = TokenOluştur(kullanıcı);
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadToken(token);
            var tokens = decodedValue as JwtSecurityToken;
            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("Id", apikullanıcıBilgileri.KullaniciAdi.ToString());
            List<HastaDTO> dto = new List<HastaDTO>();
            string id="";
            string isim="";
            foreach (var item in hastak.Lists("select * from Hasta where TC='" + apikullanıcıBilgileri.KullaniciAdi.ToString() + "'"))
            {
                id= @item.hastaid.ToString();
                isim= @item.AdiSoyadi.ToString();
            }
            if (id == "")
            {
                List<CalisanDTO> dto1 = new List<CalisanDTO>();
                Calisan hastak1 = new Calisan();
                foreach (var item in hastak1.Lists("select * from Calisan where TC='" + apikullanıcıBilgileri.KullaniciAdi.ToString() + "'"))
                {
                    
                    id = @item.CalisanID.ToString();
                    isim = @item.AdiSoyadi.ToString();

                }
            }
            HttpContext.Session.SetString("userid", id);
            HttpContext.Session.SetString("username", isim);

            ViewBag.id= HttpContext.Session.GetString("userid");
            ViewBag.isim= HttpContext.Session.GetString("username");
            return RedirectToAction(router.FirstRouting(token), "Sayfalar");
        }

        private GirisDTO? KimlikDoğrulamaYap(GirisDTO apikullanıcıBilgileri)
        {

            return giris.Lists("select * from Giris").FirstOrDefault(x => x.KullaniciAdi == apikullanıcıBilgileri.KullaniciAdi && x.Sifre == apikullanıcıBilgileri.Sifre);
        }

        private string TokenOluştur(GirisDTO kullanıcı)
        {

            if (_jwtAyarları.Key == null)
            {
                throw new Exception("Jwt ayarlarındaki key null olamaz");

            }
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hdWADrPBOgMemtUm0aHzFFs5LxD5tRjHu1f"));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claimDizisi = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,kullanıcı.KullaniciAdi),
                new Claim(ClaimTypes.Role,kullanıcı.Yetki.ToString())

            };

            var token1 = new JwtSecurityToken(_jwtAyarları.Issuer,
                _jwtAyarları.Audience,
                claimDizisi,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            Response.Cookies.Append("Token", token1.ToString(), new CookieOptions
            {
                HttpOnly = true, // Tarayıcı tarafından erişilebilirliği kapatır
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None, // Gerekirse sadece Cross-Site talepleriyle gönder
                Secure = true, // HTTPS üzerinde çalıştığında güvenli hale getir
                Expires = DateTime.UtcNow.AddHours(1) // Cookie süresi
            });


            return new JwtSecurityTokenHandler().WriteToken(token1);
        }
        
    }
    
}


    

