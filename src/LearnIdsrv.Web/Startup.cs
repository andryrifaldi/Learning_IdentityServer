using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LearnIdsrv.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });


            services.AddControllersWithViews();
            services.AddRazorPages();

            // Identity Server Configuration
            ConfigureIdentityServer(services);

        }

        private void ConfigureIdentityServer(IServiceCollection services)
        {
            var builder = services.AddAuthentication(options => SetAuthOption(options));
            builder.AddCookie();
            builder.AddOpenIdConnect(options => SetOpenIdConnectionOption(options));
        }

        private void SetOpenIdConnectionOption(OpenIdConnectOptions options)
        {
            options.Authority = "https://localhost:5001";
            options.ClientId = "LearnIdsrv.Web";
            options.ClientSecret = "0b4168e4-2832-48ea-8fc8-7e4686b3620b";
            options.RequireHttpsMetadata = false;

            options.ResponseType = "code";
            options.UsePkce = true;

            options.Scope.Clear();
            options.Scope.Add("profile");
            options.Scope.Add("openid");
            options.Scope.Add("LearnIdsrv.Api");

            // keeps id_token smaller
            options.GetClaimsFromUserInfoEndpoint = true;
            options.SaveTokens = true;
        }

        private void SetAuthOption(AuthenticationOptions options)
        {
            options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                //.RequireAuthorization() The RequireAuthorization method disables anonymous access for the entire application.
                ;
                endpoints.MapRazorPages();
            });
        }
    }
}
