﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using static DHBS_Sistemi.Controllers.KimlikDoğrulamaController;
using System.IdentityModel.Tokens.Jwt;



namespace DHBS_Sistemi.Controllers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class CustomValidationUserAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public CustomValidationUserAttribute() { }  
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var user = context.HttpContext.Session;
                var tokenHandler = new JwtSecurityTokenHandler();
                string tstring = tokenHandler.ReadJwtToken(user.GetString("Token")).ToString();
                if (!tstring.Contains("\"74859012\""))
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