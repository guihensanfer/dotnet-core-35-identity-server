using Data.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Data.Models
{
    public class Category
    {
     
        /// <summary>
        /// Adicione neste enum a quantidade desejada.
        /// </summary>
        public enum OrderView
        {
            [Display(Name = "1º")]
            First = 1,

            [Display(Name = "2º")]
            Second = 2,

            [Display(Name = "3º")]
            Third = 3,

            [Display(Name = "4º")]
            Fourth= 4,

            [Display(Name = "5º")]
            Fifth = 5,

            [Display(Name = "6º")]
            Sixth = 6,

            [Display(Name = "7º")]
            Seventh = 7,

            [Display(Name = "8º")]
            Eighth = 8,

            [Display(Name = "9º")]
            Ninth = 9,

            [Display(Name = "10º")]
            Tenth = 10
        }

        [Key]
        [Column("CategoryId")]
        [Display(Name = "Código")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Used for translation
        /// </summary>
        [Column("Guid")]        
        public Guid Guid { get; set; }

        [Column("Name")]
        [Required]
        [StringLength(600)]
        [Display(Name = "Nome")]
        public string Name { get; set; }        

        /// <summary>
        /// Translated name
        /// </summary>
        public string NameView
        {
            get
            {
                var translation = Utility.TryParseTranslation(Name);

                return translation.ToNullableStringOrDefault(Name);
            }
        }

        /// <summary>
        /// Translated name
        /// </summary>
        public string DescriptionView
        {
            get
            {
                var translation = Utility.TryParseTranslation(Description);

                return translation.ToNullableStringOrDefault(Description);
            }
        }

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
        [Display(Name = "Nível")]
        public OrderView Order { get; set; } = OrderView.First;

        [Column("Index", TypeName = "tinyint")]        
        [Display(Name = "Ordenaçao")]
        public int? Index { get; set; }

        [Column("Path")]
        [StringLength(300)]
        [Required]
        [Display(Name = "Endereço")]
        public string Path { get; set; }     

        public string PathView
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Path))
                {
                    return Path;
                }

                const char separator = '/';
                var vs = Path.Split(separator);

                if(vs != null && vs.Any())
                {
                    List<string> result = new List<string>();

                    foreach(var item in vs)
                    {
                        var translate = Utility.TryParseTranslation(item);

                        result.Add(translate);
                    }

                    return string.Join(separator, result);
                }

                return Path;
            }
        }
        
        public string GetPathByOrder(OrderView order)
        {
            var firstParentArray = Path.Split('/');
            string firstParentStr;

            if (firstParentArray != null && firstParentArray.Any())
            {
                int orderInt = (int)order;

                if(orderInt >= firstParentArray.Length)
                {
                    return null;
                }

                firstParentStr = firstParentArray[orderInt];

                if (!string.IsNullOrEmpty(firstParentStr))
                {
                    return firstParentStr;
                }
            }

            return null;
        }        
    }
}
