using System;
using System.Collections.Generic;

namespace AdminAssistant.Blog.Models.DomainModel
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleFormated { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public string PictureUrl { get; set; }
        public string PostedBy { get; set; }
        public string Intro { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }
}
