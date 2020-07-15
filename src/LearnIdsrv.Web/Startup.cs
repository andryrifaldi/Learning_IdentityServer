using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5001";

                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.Scope.Add("api1");

                options.SaveTokens = true;
            });
        }

        private void ConfigureIdentityServer(IServiceCollection services)
        {
            var builder = services.AddAuthentication(options => SetAuthOption(options));
            builder.AddCookie();
            builder.AddOpenIdConnect(options => SetOpenIdConnectionOption(options));
        }

        private void SetOpenIdConnectionOption(OpenIdConnectOptions options)
        {
            options.SignInScheme = "Cookies";
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
            options.Scope.Add("offline_access");
            options.ClaimActions.MapJsonKey("website", "website");

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
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                    .RequireAuthorization();
            });
        }
    }
}
