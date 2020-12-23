using Bom_Dev.Data;
using Bom_Dev.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Bom_Dev
{
    public class Startup
    {
        private const string IdentityServerHTTPSBaseURL = "https://localhost:44399";
        private const string MVCClientHTTPSBaseURL = "https://localhost:44378";
        private const string APIHTTPSBaseURL = "https://localhost:44302";

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
            services.AddTransient<IEmailSender, EmailConfiguracao>();                                                                         

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            #region Identity
            services.AddDefaultIdentity<BomDevUser>(options => {
                options.SignIn.RequireConfirmedAccount = true;

                // Senha
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // Máximo tentativas login
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion
            
            #region Identity Server            
            services.AddAuthentication(o =>
            {
                //o.DefaultScheme = IdentityConstants.ExternalScheme;                
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;                     
            })
            .AddCookie("Cookies", o =>
            {
                o.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, "Bom Dev", o =>
            {                
                o.SignInScheme = IdentityConstants.ExternalScheme;

                o.Authority = IdentityServerHTTPSBaseURL;
                o.RequireHttpsMetadata = false;

                o.ClientId = "client2";
                o.ClientSecret = "client2_secret_code";
                o.ResponseType = "code id_token";

                o.SaveTokens = true;
                o.GetClaimsFromUserInfoEndpoint = true;

                o.Scope.Add("employeesWebApi");
                o.Scope.Add("roles");

                o.ClaimActions.MapUniqueJsonKey("role", "role");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role"
                };
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
