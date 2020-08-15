using System;
using Microsoft.AspNetCore.Identity;

namespace Bom_Dev.Models
{
    public class UsuarioBomDev : IdentityUser
    {
        public string Nome{get;set;}
    }
}
