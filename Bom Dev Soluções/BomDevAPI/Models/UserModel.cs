using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BomDevAPI.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAdress { get; set; }
        public string Role { get; set; }
    }
}
