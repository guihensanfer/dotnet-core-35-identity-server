using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Category
    {        
        public enum OrderView
        {
            [Display(Name = "1º")]
            First = 1,

            [Display(Name = "2º")]
            Second = 2,

            [Display(Name = "3º")]
            Third = 3
        }

        [Key]
        [Column("CategoryId")]
        [Display(Name = "Código")]
        public int CategoryId { get; set; }

        [Column("Name")]
        [Required]
        [StringLength(60)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Column("Description")]
        [StringLength(300)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Column("DateCreated")]
        [Required]
        [Display(Name = "Data de criação")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [ForeignKey("ParentCategoryIdFk")]
        [Display(Name = "Categoria pai")]        
        public int? ParentCategoryId { get; set; }

        [Column("Url")]        
        [Display(Name = "Url")]
        [StringLength(2048)]
        public string Url { get; set; }

        [Column("Enabled")]
        [Required]
        [Display(Name = "Ativo")]        
        public bool Enabled { get; set; }

        [Column("Order", TypeName = "tinyint")]
        [Required]
        [Display(Name = "Ordem")]
        public OrderView Order { get; set; } = OrderView.First;

        [Column("Path")]
        [StringLength(300)]
        [Required]
        [Display(Name = "Endereço")]
        public string Path { get; set; }           
    }
}
