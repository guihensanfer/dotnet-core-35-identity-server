using IdentityServer.Data;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;

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

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            //services.AddIdentity<IdentityUser, IdentityRole>()            
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddTransient<IAuthRepository, AuthRepository>();

            services.AddControllersWithViews();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(ServerConfiguration.TestUsers)
                //.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                //.AddProfileService<ProfileService>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                });
                //.AddAspNetIdentity<IdentityUser>();
                    //.AddConfigurationStore(options => {
                    //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    //});                       

            // Login com Google
            services.AddAuthentication().AddGoogle(g =>
            {
                g.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                g.ClientSecret = Configuration.GetValue<string>("GoogleLogin:ClientSecret");
                g.ClientId = Configuration.GetValue<string>("GoogleLogin:ClientId");
            });
            
                //.AddTransient<IProfileService, ProfileService>();

            //services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>()
            //    .AddTransient<IProfileService, ProfileService>()
            //    .AddTransient<IAuthRepository, AuthRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDatabase(app);

            app.UseStaticFiles();
            app.UseRouting();            
            app.UseIdentityServer();
            //app.UseAuthentication();
            app.UseAuthorization();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if(!context.ApiScopes.Any())
                {
                    foreach(var scope in ServerConfiguration.ApiScopes)
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }

                    context.SaveChanges();
                }
                if (!context.Clients.Any())
                {
                    foreach (var client in ServerConfiguration.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in ServerConfiguration.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in ServerConfiguration.ApiResources)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }                
            }
        }
    }
}
