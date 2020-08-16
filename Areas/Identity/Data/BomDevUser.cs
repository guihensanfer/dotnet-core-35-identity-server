using Microsoft.AspNetCore.Identity;

namespace BomDev.Data
{
    public class BomDevUser : IdentityUser
    {
        [PersonalData]
        public string Nome{get;set;}        
    }
}
