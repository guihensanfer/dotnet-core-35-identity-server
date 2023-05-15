using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.Context
{
    public class IdentityDbContext : IdentityDbContext<Identity.PersonalUser>
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<CacheObject> CacheObject { get; set; }
        public DbSet<TranslationObject> TranslationObject { get; set; }
        public DbSet<ErrorLogs> ErrorLogs { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
    }
}
