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
        INewsletterService _newsletterService;

        public BlogController(IPostService postService, INewsletterService newsletterService)
        {
            _postService = postService;
            _newsletterService = newsletterService;
        }

        public IActionResult Index()
        {
            BlogViewModel blog = new BlogViewModel();

            List<PostViewModel> posts = _postService.GetFiltered(new FilterModel { Take = 6 });

            blog.MainPosts.AddRange(posts.Take(2));
            blog.OtherPosts.AddRange(posts.Skip(2).Take(4));

            return View(blog);
        }

        public IActionResult PostByCategory(int category)
        {
            if (category == 0) return Redirect("/Blog/Index");

            List<PostViewModel> posts = _postService.GetPostByCategory(category);

            return View(posts);
        }

        public IActionResult Post(int? id)
        {
            if (id.HasValue && id != 0)
            {
                PostViewModel post = _postService.GetPost(id.Value);
                return View(post);
            }

            return View();
        }

        [HttpPost]
        public void Subscribe(string email)
        {
            _newsletterService.Subscribe(email);
        }

        [HttpGet]
        public IActionResult Unsubscribe(string email)
        {
            _newsletterService.Unsubscribe(email);

            return View();
        }
    }
}