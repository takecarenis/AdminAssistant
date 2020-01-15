using System;
using System.Collections.Generic;
using System.Text;

namespace AdminAssistant.Domain.Blog
{
    public class Category : IEntity
    {
        public Category()
        {
            PostCategories = new HashSet<PostCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PostCategory> PostCategories { get; set; }
    }
}
