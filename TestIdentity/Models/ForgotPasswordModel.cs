using System.ComponentModel.DataAnnotations;

namespace TestIdentity.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}