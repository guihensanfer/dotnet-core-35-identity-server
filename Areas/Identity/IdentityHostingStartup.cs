using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Bom_Dev.Areas.Identity.IdentityHostingStartup))]
namespace Bom_Dev.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            
        }
    }
}