using Microsoft.AspNetCore.Identity;

namespace TestIdentity {
    public class TestIdentityUser : IdentityUser
    {
        public string Locale { get; set; } = "en-NG";
    }
}