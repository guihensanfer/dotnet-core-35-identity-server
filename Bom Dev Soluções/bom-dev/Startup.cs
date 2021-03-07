using Bom_Dev.Shared.Identity;
using Bom_Dev.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bom_Dev
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
            services.AddControllersWithViews();
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

            #region Identity            
            services.AddDefaultIdentity<Bom_Dev.Shared.Identity.BomDevUser>(options => {
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
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, "Bom Dev", o =>
            {                
                o.SignInScheme = IdentityConstants.ExternalScheme;
                o.SignOutScheme = IdentityConstants.ApplicationScheme;

                o.Authority = Configuration.GetValue<string>("Hosts:IdentityServerBaseURL");
                o.RequireHttpsMetadata = false;

                o.ClientId = "client1";
                o.ClientSecret = "client1_secret_code";
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");              
                endpoints.MapRazorPages();
            });                                   
        }       
    }
}
