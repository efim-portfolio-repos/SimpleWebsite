using System.Collections.Generic;
using SimpleWebsite.Models;

namespace SimpleWebsite.Models.ViewModels
{
    public class BlogListViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public int CurrentPage { get; set; }
        public int TotalElements { get; set; }

        public int ElementOnPage { get; set; }
    }
}