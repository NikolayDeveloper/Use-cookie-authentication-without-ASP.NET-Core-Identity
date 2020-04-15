using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.App.Pattern.Models
{
    public class LoginModel
    {
        [Required, Display(Name = "Login")]
        public string Login { get; set; }
        
        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }
        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}
