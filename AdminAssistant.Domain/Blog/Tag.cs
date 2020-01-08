using System;
using System.Collections.Generic;
using System.Text;

namespace AdminAssistant.Domain.Blog
{
    public class Tag : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
