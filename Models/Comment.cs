using System;
using SimpleWebsite.Models.Base;

namespace SimpleWebsite.Models
{
    public class Comment : EntityBase
    {

        public DateTime PublishTime { get; set; }
        public string Text { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}