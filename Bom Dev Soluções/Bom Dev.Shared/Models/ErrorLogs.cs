using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{    
    public class ErrorLogs
    {
        /// <summary>        
        /// Use this to easily delete future obsolete data
        /// </summary>
        public const int ExpirationDays = 180;

        [Key]
        [Column("ErrorLogsId")]
        public int ErrorLogsId { get; set; }

        [Required]
        [Column("Date")]
        public DateTime Date { get; private set; } = DateTime.Now;
        
        [Required]
        [Column("DateExpiration")]
        public DateTime DateExpiration { get; private set; } = DateTime.Now.AddDays(ExpirationDays);

        [Column("Title")]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Column("Message")]
        [StringLength(255)]
        public string Message { get; set; }

        [Column("StackTrace")]
        [MaxLength]
        public string StackTrace { get; set; }

        [Column("RequestMethod")]
        [StringLength(10)]
        public string RequestMethod { get; set; } = "GET";

        [Column("RequestUrl")]
        [StringLength(255)]
        public string RequestUrl { get; set; }

        [Column("UserAgent")]
        [StringLength(255)]
        public string UserAgent { get; set; }

        [Column("IpAddress")]
        [StringLength(45)]
        public string IpAddress { get; set; }

        [Column("Language")]
        [StringLength(15)]
        public string Language { get; set; }

        [Column("UserId")]        
        public string UserId { get; set; }
    }
}
