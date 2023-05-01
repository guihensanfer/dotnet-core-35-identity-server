using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class TranslationObject
    {        
        [Key]
        [Column("TranslationObjectId")]        
        public int TranslationObjectId { get; set; }

        [Required]
        [Column("ObjectGuid")]        
        public Guid ObjectGuid { get; set; }

        /// <summary>
        /// Example: pt-br, en-us
        /// </summary>
        [Column("Language")]
        [StringLength(15)]
        public string Language { get; set; }

        [Required]
        [Column("Value")]
        [StringLength(500)]
        public string Value { get; set; }

        [Required]
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
