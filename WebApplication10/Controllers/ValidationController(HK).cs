
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using static DHBS_Sistemi.Controllers.KimlikDoğrulamaController;

namespace DHBS_Sistemi.Controllers
{
    public class ValidationController_HK_ :Controller
    {
        public bool Validate(string tkn) {
            try
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                string tstring = tokenHandler.ReadJwtToken(tkn).ToString();
                if (tstring.Contains("\"927458907\""))
                {
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
