using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace DHBS_Sistemi.Controllers
{
     internal class CustomValidationDoktorAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public CustomValidationDoktorAttribute() { }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var user = context.HttpContext.Session;
                var tokenHandler = new JwtSecurityTokenHandler();
                string tstring = tokenHandler.ReadJwtToken(user.GetString("Token")).ToString();
                if (!tstring.Contains("\"43274205\""))
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                    return;
                }

                return;
            }
            catch
            {
                context.Result = new StatusCodeResult(StatusCodes.Status511NetworkAuthenticationRequired);
                return;
            }
        }
    }
}