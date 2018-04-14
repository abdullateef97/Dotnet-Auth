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
        }
    }
}