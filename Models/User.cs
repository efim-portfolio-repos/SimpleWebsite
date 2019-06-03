using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SimpleWebsite.Models
{
    public class User : IdentityUser
    {
        public int MyProperty { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}