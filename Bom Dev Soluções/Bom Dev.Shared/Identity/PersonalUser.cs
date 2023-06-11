using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Identity
{
    public class PersonalUser : IdentityUser
    {
        [PersonalData]
        [Required(ErrorMessage ="{0} � obrigat�rio")]
        [Display(Name ="Nome completo")]
        [StringLength(200)]
        public string FullName{get;set;}

        [PersonalData]        
        [Display(Name = "Desejo receber informa��es sobre produtos, servi�os e eventos.")]                
        public bool SubscribeToUpdates { get; set; }

    }
}
