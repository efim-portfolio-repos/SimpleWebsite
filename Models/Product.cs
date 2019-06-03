using System.ComponentModel.DataAnnotations;
using SimpleWebsite.Models.Base;

namespace SimpleWebsite.Models
{
    public class Product : EntityBase
    {
        [Required]
        [Display(Name="Наименование")]
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name="Цена")]
        public double Price { get; set; }
        [Display(Name="Описание")]
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        [Display(Name="Категория")]
        public Category Category { get; set; }
    }
}