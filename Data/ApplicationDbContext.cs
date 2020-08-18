using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bom_Dev.Data
{
    public class ApplicationDbContextIdentity : IdentityDbContext<BomDevUser>
    {
        public ApplicationDbContextIdentity(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
