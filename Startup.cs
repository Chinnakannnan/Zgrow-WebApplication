using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using NeoBankWebApp.API_Service;
using System;
using System.Text; 

namespace NeoBankWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
    
        public void ConfigureContainer(ContainerBuilder builder)
        {
        }
        public void ConfigureServices()
        {

            var builders = WebApplication.CreateBuilder();

            builders.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                // Configure cookie options if needed
            });
            builders.Services.AddControllersWithViews();
            builders.Services.AddHttpClient<IAPIService, APIService>();
            builders.Services.AddTransient<IAPIService, APIService>();            
            builders.Services.AddDistributedMemoryCache();
            builders.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(50);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builders.Build();
            app.UseAuthorization();
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting(); 
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.MapControllerRoute(
               name: "default",
               pattern: "{controller=Login}/{action=Login}/{id?}");
            app.Run();
        }

    }
}
