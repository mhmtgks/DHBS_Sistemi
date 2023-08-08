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
            if (kullanıcı == null) return NotFound("Kullanıcı Bulunamadı .");

            var token = TokenOluştur(kullanıcı);
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadToken(token);
            var tokens = decodedValue as JwtSecurityToken;
            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("Id", apikullanıcıBilgileri.KullaniciAdi.ToString());
            
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


    

