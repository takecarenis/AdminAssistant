using System;
using System.Collections.Generic;
using System.Text;

namespace AdminAssistant.Domain.Blog
{
    public class Tag : IEntity
    {
        public Tag()
        {
            PostTags = new HashSet<PostTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
