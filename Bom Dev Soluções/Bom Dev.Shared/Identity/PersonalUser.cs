using Microsoft.AspNetCore.Identity;

namespace Data.Identity
{
    public class PersonalUser : IdentityUser
    {
        [PersonalData]
        public string FullName{get;set;}        
    }
}
