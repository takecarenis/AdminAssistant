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

        public async Task<IViewComponentResult> InvokeAsync()
        {
           // List<PostViewModel> latestPosts = _postService.GetFiltered();
            return View();
        }
    }
}
