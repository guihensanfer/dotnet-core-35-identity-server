using Microsoft.AspNetCore.Identity;

namespace Bom_Dev.Data
{
    public class BomDevUser : IdentityUser
    {
        [PersonalData]
        public string Nome{get;set;}        
    }
}
