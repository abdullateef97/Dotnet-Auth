using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class TestIdentityUserDbContext : IdentityDbContext<TestIdentityUser>
    {
        public TestIdentityUserDbContext(DbContextOptions<TestIdentityUserDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TestIdentityUser>(user => user.HasIndex(x => x.Locale).IsUnique(false));

            builder.Entity<Organization>(org =>
            {
                org.ToTable("Organizions");
                org.HasKey(x => x.Id);
                org.HasMany<TestIdentityUser>().WithOne().HasForeignKey(x => x.OrgId).IsRequired(false);
            });
        }
    }
}