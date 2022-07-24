using Microsoft.AspNetCore.Identity;

namespace Bom_Dev.Shared.Identity
{
    public class BomDevUser : IdentityUser
    {
        [PersonalData]
        public string FullName{get;set;}        
    }
}
