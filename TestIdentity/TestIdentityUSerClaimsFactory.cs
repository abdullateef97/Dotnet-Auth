using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace TestIdentity
{
    public class TestIdentityUSerClaimsFactory :
        UserClaimsPrincipalFactory<TestIdentityUser>
    {
        public TestIdentityUSerClaimsFactory(UserManager<TestIdentityUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TestIdentityUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("locale",user.Locale));
            return identity;
        }
    }
}