using AdminAssistant.Blog.Models.DomainModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AdminAssistant.Blog.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Posts()
        {
            PostViewModel post1 = new PostViewModel
            {
                Title = "This is first post!",
                Date = DateTime.Now,
                PictureUrl = "/img/posts/post13122019.jpg"
            };

            PostViewModel post2 = new PostViewModel
            {
                Title = "This is second post!",
                Date = DateTime.Now,
                PictureUrl = "/img/posts/post13122019.jpg"
            };

            List<PostViewModel> posts = new List<PostViewModel> { post1, post2 };

            return View(posts);
        }
    }
}