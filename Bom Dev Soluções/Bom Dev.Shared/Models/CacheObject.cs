using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class CacheObject
    {
        public CacheObject()
        {

        }
        public CacheObject(string key)
        {
            this.Key = key;
        }

        public enum Objects
        {
            MenuItensSite = 0
        }

        [Key]
        [Column("CacheObjectId")]        
        public int CacheObjectId { get; set; }

        [Required]
        [Column("Key")]
        [StringLength(50)]
        public string Key { get; set; }

        [Required]
        [Column("Value")]
        [MaxLength]
        public string Value { get; set; }

        [Column("Expiration")]
        public DateTime Expiration { get; set; } = DateTime.Now.AddHours(48);

        [Column("Language")]
        [StringLength(15)]
        public string Language { get; set; } = "pt-br";
    }
}
