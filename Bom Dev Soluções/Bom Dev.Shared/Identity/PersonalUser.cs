using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Identity
{
    public class PersonalUser : IdentityUser
    {
        [PersonalData]
        [Required(ErrorMessage ="{0} é obrigatório")]
        [Display(Name ="Nome completo")]
        [StringLength(200)]
        public string FullName{get;set;}

        [PersonalData]        
        [Display(Name = "SubscribeToUpdates")]        
        public bool SubscribeToUpdates { get; set; }

        [PersonalData]
        [Display(Name = "AcceptedTerms")]
        [Required(ErrorMessage = "{0} required")]
        public bool AcceptedTerms { get; set; }
    }
}
