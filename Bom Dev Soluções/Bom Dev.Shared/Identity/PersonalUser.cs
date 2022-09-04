using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Identity
{
    public class PersonalUser : IdentityUser
    {
        [PersonalData]
        [Required]
        [Display(Name ="Nome completo")]
        [StringLength(200)]
        public string FullName{get;set;}        
    }
}
