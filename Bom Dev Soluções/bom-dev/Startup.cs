using Bom_Dev.Models;
using Data.Context;
using Data.Identity;
using Data.Interface;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Bom_Dev
{
    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public const string IdentityServerName = "Bom Dev";
        public const string IdentityServerScheme = OpenIdConnectDefaults.AuthenticationScheme;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLocalization(options => options.ResourcesPath = SupportedCultures.ResourcesFolder);
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
                

            services.Configure<RequestLocalizationOptions>(options =>
            {                
                var supportedCultures = SupportedCultures.GetCultures();

                options.DefaultRequestCulture = new RequestCulture(culture: SupportedCultures.DefaultLanguage, uiCulture: SupportedCultures.DefaultLanguage);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;                                     
            });


            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();                
            services.AddRazorPages();
            services.AddTransient<IEmailSender, EmailSender>();

            // Atualiza as informações de conexão com e-mail conforme appsettings
            EmailConfig.host = Configuration.GetValue<string>("Email:Host");
            EmailConfig.enableSsl = Configuration.GetValue<bool>("Email:EnableSsl");
            EmailConfig.email = Configuration.GetValue<string>("Email:Email");
            EmailConfig.password = Configuration.GetValue<string>("Email:Password");
            EmailConfig.port = Configuration.GetValue<short>("Email:Port");
            EmailConfig.signatureHTML = Configuration.GetValue<string>("Email:SignatureHTML");

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRepository, Repository>();

            #region Identity            
            services.AddDefaultIdentity<PersonalUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

                // Senha
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // Máximo tentativas login
                options.Lockout.MaxFailedAccessAttempts = 3;
                
            })
            .AddEntityFrameworkStores<IdentityDbContext>();
            #endregion
            
            #region Identity Server            
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                o.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, IdentityServerName, o =>
            {                
                o.SignInScheme = IdentityConstants.ExternalScheme;
                o.SignOutScheme = IdentityConstants.ApplicationScheme;

                o.Authority = Configuration.GetValue<string>("Hosts:IdentityServerBaseURL");
                o.RequireHttpsMetadata = false;

                o.ClientId = "WebClient";
                o.ClientSecret = "89C9FD35E23FA2E1A63EE8A59FB9F";
                o.ResponseType = "code id_token";

                o.SaveTokens = true;
                o.GetClaimsFromUserInfoEndpoint = true;

                o.Scope.Add("employeesWebApi");
                o.Scope.Add("roles");              
            });
            #endregion
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();    
                        
            app.UseAuthentication();             
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                // adm
                endpoints.MapAreaControllerRoute(
                    name: "admArea",
                    areaName: "adm",
                    pattern: "/adm/{controller=Category}/{action=Index}/{id?}");

                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Home}/{action=Index}/{id?}");
            });
        }       
    }
}
