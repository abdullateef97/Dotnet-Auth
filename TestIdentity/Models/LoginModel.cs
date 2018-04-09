using System.ComponentModel.DataAnnotations;

namespace TestIdentity.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        
       [DataType(DataType.Password)]
        public  string Password { get; set; }
    }
}