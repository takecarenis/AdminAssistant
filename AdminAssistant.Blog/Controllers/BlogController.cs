using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAssistant.Blog.Models;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminAssistant.Blog.Controllers
{
    public class BlogController : Controller
    {
        IPostService _postService;

        public BlogController(IPostService postService)
        {
            _postService = postService;
        }

        public IActionResult Index()
        {
            BlogViewModel blog = new BlogViewModel();

            List<PostViewModel> posts = _postService.GetFiltered(new FilterModel { Take = 6 });

            blog.MainPosts.AddRange(posts.Take(2));
            blog.OtherPosts.AddRange(posts.Skip(2).Take(4));

            return View(blog);
        }

        public IActionResult Post(int? id)
        {
            CategoryViewModel category = new CategoryViewModel() { Id = 1, Name = "Politika" };
            TagViewModel tag = new TagViewModel() { Id = 1, Name = "Stranka" };

            PostViewModel post = new PostViewModel()
            {
                Id = 1,
                Body = "<h2 style=\"color: #2e6c80;\">How to use the editor:</h2><p> Paste your documents in the visual editor on the left or your HTML code in the source editor in the right.",
                Categories = new List<CategoryViewModel>() { category },
                Tags = new List<TagViewModel>() { tag },
                Date = DateTime.Now,
                PictureUrl = "https://images.unsplash.com/photo-1511447333015-45b65e60f6d5?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjExMjU4fQ&w=1000&q=80",
                PostedBy = "Stefan",
                Title = "Ovo je neki naslov"
            };

            return View(post);
        }
    }
}