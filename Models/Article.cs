using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SimpleWebsite.Models.Base;

namespace SimpleWebsite.Models
{
    public class Article : EntityBase
    {
        [Required]
        [Display(Name="Заголовок")]
        public string Header { get; set; }

        [Display(Name="Дата публикации")]
        public DateTime PublishDate {get; set;}

        [Display(Name="Текст")]
        public string Text { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}