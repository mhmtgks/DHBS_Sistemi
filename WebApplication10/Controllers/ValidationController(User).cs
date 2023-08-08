using static DHBS_Sistemi.Controllers.KimlikDoğrulamaController;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace DHBS_Sistemi.Controllers
{
    public class ValidationController_User_ : Controller
    {
        public bool Validate(string tkn)
        {
            try
            {

                
                var tokenHandler = new JwtSecurityTokenHandler();
                string tstring = tokenHandler.ReadJwtToken(tkn).ToString();
                if (tstring.Contains("\"74859012\""))
                {
                    idExtractor.Extractor(tstring, 2);
                    return true;
                }

            }
            catch
            {

                return false;
            }

            return false;
        }




    }
}
