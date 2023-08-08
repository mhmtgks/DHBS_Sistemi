using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DHBS_Sistemi.Models;
using static DHBS_Sistemi.Controllers.KimlikDoðrulamaController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtAyarlarý>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(options => {
    options.Cookie.Name = "Token"; // Cookie adý
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None; // Gerekirse sadece Cross-Site talepleriyle gönder
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest; // HTTPS üzerinde çalýþtýðýnda güvenli hale getir
    options.LoginPath = "/Login1";
}).AddJwtBearer(o =>
    {
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                context.Token = context.Request.Cookies["Token"];
                if (!String.IsNullOrEmpty(context.Request.Cookies["Token"]))
                {
                }
                return;
                
            }
        };
    });

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(5);
    option.Cookie.HttpOnly = true;
});


builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseSession();
app.Run();
