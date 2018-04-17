using Microsoft.AspNetCore.Identity;

namespace TestIdentity {
    public class TestIdentityUser : IdentityUser
    {
        public string Locale { get; set; } = "en-NG";
        public string OrgId { get; set; }
    }
    
    public class Organization 
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}