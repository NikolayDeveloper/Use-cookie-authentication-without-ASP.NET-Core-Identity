using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.App.Pattern.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class ApplicationUsers
    {
        public List<User> Users { get; set; }
    }
}
