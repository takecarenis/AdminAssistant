using System;

namespace AdminAssistant.Domain.Blog
{
    public class Post : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public string PictureUrl { get; set; }
        public string PostedBy { get; set; }
    }
}
