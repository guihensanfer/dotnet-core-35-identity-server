using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Bom_Dev.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bom_Dev.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<Bom_Dev.Data.BomDevUser>(options => {
                    options.SignIn.RequireConfirmedAccount = true;
                    
                    // Senha
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;      

                    // Máximo tentativas login
                    options.Lockout.MaxFailedAccessAttempts = 5;              
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            // Login com Google
            services.AddAuthentication().AddGoogle(g => {
                g.ClientSecret = Configuration.GetValue<string>("GoogleLogin:ClientSecret");
                g.ClientId = Configuration.GetValue<string>("GoogleLogin:ClientId");                                
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddTransient<IEmailSender, EmailConfiguracao>();      

            services.ConfigureApplicationCookie(options =>  
            {  
                // Cookie settings  
                options.Cookie.HttpOnly = true;  
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);  
                options.LoginPath = "/Identity/Pages/Account/Login";  
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";  
                options.SlidingExpiration = true;  
            });  
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
