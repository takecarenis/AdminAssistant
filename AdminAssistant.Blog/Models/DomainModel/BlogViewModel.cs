using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Models.DomainModel
{
    public class BlogViewModel
    {
        public BlogViewModel()
        {
            MainPosts = new List<PostViewModel>();
            OtherPosts = new List<PostViewModel>();
        }

        public List<PostViewModel> MainPosts { get; set; }
        public List<PostViewModel> OtherPosts { get; set; }
    }
}
