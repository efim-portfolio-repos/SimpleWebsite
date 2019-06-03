using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SimpleWebsite.Models.Base;

namespace SimpleWebsite.Models
{
    public class Category : EntityBase
    {
        [Required]
        [Display(Name="Наименование")]
        public string Name { get; set; }

        [Display(Name="Описание")]
        public string Description {get; set;}

        public IEnumerable<Product> Products { get; set; }
    }
}