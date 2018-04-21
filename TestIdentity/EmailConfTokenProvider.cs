using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace TestIdentity
{
    public class EmailConfTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class 

    {
        public EmailConfTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<EmailConfTokenProviderOptions> options) : base(dataProtectionProvider, options)
        {
        }
        
       
    }

    public class EmailConfTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        
    }
}