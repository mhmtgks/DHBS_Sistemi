using Service;
using System.IdentityModel.Tokens.Jwt;

namespace DHBS_Sistemi.Controllers
{
    public class ValidationController_Doktor_
    {
        public bool Validate(string tkn)
        {
            try
            {


                var tokenHandler = new JwtSecurityTokenHandler();
                string tstring = tokenHandler.ReadJwtToken(tkn).ToString();
                if (tstring.Contains("\"43274205\""))
                {
                    idExtractor.Extractor(tstring,1);
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
