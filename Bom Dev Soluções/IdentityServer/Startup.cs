using IdentityServer.Quickstart;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;
using Data.Context;
using Data.Identity;

namespace IdentityServer
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
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<PersonalUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();            

            services.AddControllersWithViews();            

            services.AddIdentityServer(options => {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;                
                options.EmitStaticAudienceClaim = true;          
            })                
            .AddDeveloperSigningCredential()            
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<PersonalUser>();                                   

            // Login com Google
            services.AddAuthentication().AddGoogle(g =>
            {
                g.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                g.ClientSecret = Configuration.GetValue<string>("GoogleLogin:ClientSecret");
                g.ClientId = Configuration.GetValue<string>("GoogleLogin:ClientId");
            });

            var appSettingsUtil = new BomDevAppSettingsUtil(Configuration.GetValue<string>("Links:BomDevRegisterURL"),
                Configuration.GetValue<string>("Links:BomDevForgotPassword"),
                Configuration.GetValue<string>("Links:BomDevBaseURL"));

            services.AddSingleton(prov => appSettingsUtil);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDatabase(app);

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
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                ServerConfiguration serverConfiguration = new ServerConfiguration(Configuration.GetValue<string>("Links:BomDevBaseURL"));
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if(!context.ApiScopes.Any())
                {
                    foreach(var scope in serverConfiguration.ApiScopes)
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }

                    context.SaveChanges();
                }
                if (!context.Clients.Any())
                {
                    foreach (var client in serverConfiguration.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in serverConfiguration.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in serverConfiguration.ApiResources)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }                
            }
        }
    }
}
