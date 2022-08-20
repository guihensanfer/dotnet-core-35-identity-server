using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bom_Dev.Shared
{
    public class Category
    {
        public Category(DateTime dateCreated)
        {
            this.DateCreated = dateCreated;
        }

        [Key]
        [Column("CategoryId")]
        [Display(Name = "Código")]
        public int CategoryId { get; set; }

        [Column("Name")]
        [StringLength(60)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Column("Description")]
        [StringLength(300)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Column("DateCreated")]
        [Display(Name = "Data de criação")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; private set; }

        [ForeignKey("ParentCategoryIdFk")]
        [Display(Name = "Categoria pai")]        
        public Category ParentCategoryId { get; set; }

        [Column("Url")]
        [Display(Name = "Url")]
        [StringLength(2048)]
        public string Url { get; set; }
    }
}
