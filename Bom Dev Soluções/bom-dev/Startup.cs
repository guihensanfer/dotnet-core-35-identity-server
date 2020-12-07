using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Bom_Dev.Data;
using Bom_Dev.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

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
            // Identity Server
            services.AddIdentityServer(o => {
                o.Events.RaiseErrorEvents = true;
                o.Events.RaiseInformationEvents = true;
                o.Events.RaiseFailureEvents = true;
                o.Events.RaiseSuccessEvents = true;                                
            })            
            .AddDeveloperSigningCredential()
            .AddInMemoryApiScopes(Config.GetApiScopes())            
            .AddInMemoryClients(Config.GetClients())
            .AddTestUsers(Config.GetUsers())
            .AddJwtBearerClientAuthentication()           
            .AddDeveloperSigningCredential() 
            .AddInMemoryApiResources(Config.GetApiResources());         
           
            // .AddAspNetIdentity<BomDevUser>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";                    
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";                                                                                                                               
                });        

            IdentityModelEventSource.ShowPII = true; //Add this line

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
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

            // Login com Google
            services.AddAuthentication().AddGoogle(g =>
            {
                g.ClientSecret = Configuration.GetValue<string>("GoogleLogin:ClientSecret");
                g.ClientId = Configuration.GetValue<string>("GoogleLogin:ClientId");
            });            
            
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddTransient<IEmailSender, EmailConfiguracao>();

                                           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {                        
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls12;

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
            
            app.UseIdentityServer();                                                                                                                  
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
