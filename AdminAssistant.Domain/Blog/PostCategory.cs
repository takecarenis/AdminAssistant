using System;
using System.Collections.Generic;
using System.Text;

namespace AdminAssistant.Domain.Blog
{
    public class PostCategory : IEntity
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public Category Category { get; set; }
    }
}
