using Microsoft.AspNetCore.Mvc;
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
        }

        public List<PostViewModel> MainPosts { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<TagViewModel> Tags { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public int Count { get; set; }

        public bool ShowPrevious => CurrentPageIndex > 1;
        public bool ShowNext => CurrentPageIndex < TotalPages;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
    }
}
