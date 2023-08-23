using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using static DHBS_Sistemi.Controllers.KimlikDoğrulamaController;
using System.IdentityModel.Tokens.Jwt;
using Service;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CustomValidationHKAttribute : Attribute, IAsyncAuthorizationFilter
{

    public CustomValidationHKAttribute()
    {


    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var user = context.HttpContext.Session;
            var tokenHandler = new JwtSecurityTokenHandler();
            string tstring = tokenHandler.ReadJwtToken(user.GetString("Token")).ToString();
         
            if (!tstring.Contains("\"927458907\""))
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