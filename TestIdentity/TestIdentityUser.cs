using Microsoft.AspNetCore.Identity;

namespace TestIdentity {
    public class TestIdentityUser { 
        public string Id {get;set;}
        public string UserName {get;set;}
        public string NormalizedUserName {get;set;}
        public string PasswordHash {get;set;}
    }
}