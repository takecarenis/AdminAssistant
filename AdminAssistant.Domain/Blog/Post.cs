using System;
using System.Collections.Generic;

namespace AdminAssistant.Domain.Blog
{
    public class Post : IEntity
    {
        public Post()
        {
            PostTags = new HashSet<PostTag>();
            PostCategories = new HashSet<PostCategory>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public string PictureUrl { get; set; }
        public string PostedBy { get; set; }

        public virtual ICollection<PostCategory> PostCategories { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
