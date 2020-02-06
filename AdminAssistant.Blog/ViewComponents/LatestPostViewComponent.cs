using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.ViewComponents
{
    public class LatestPostViewComponent : ViewComponent
    {
        public readonly IPostService _postService;
        
        public LatestPostViewComponent(IPostService service) 
        {
            _postService = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(string category)
        {
            int categoryId = _postService.GetCategoryId(category);
            List<PostViewModel> latestPosts = new List<PostViewModel>();

            if (categoryId != 0)
                latestPosts = _postService.GetPostByCategory(categoryId);

            ViewBag.Category = category == "Article" ? category + "s" : category;
            return View(latestPosts);
        }
    }
}
