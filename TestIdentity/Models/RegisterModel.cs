﻿using System.ComponentModel.DataAnnotations;

namespace TestIdentity.Models
{
    public class RegisterModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        public string UserName { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Compare("Password")]
        [DataType(DataType.Password)]
        public  string ConfirmPassword { get; set; }
    }
}