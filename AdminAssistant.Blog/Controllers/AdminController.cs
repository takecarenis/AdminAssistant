using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace AdminAssistant.Blog.Controllers
{
    public class AdminController : Controller
    {
        IPostService _postService;
        private readonly IWebHostEnvironment _env;

        public AdminController(IPostService postService, IWebHostEnvironment env)
        {
            _postService = postService;
            _env = env;
        }

        public IActionResult Index()
        {
            _postService.GetPost(2);
            _postService.GetAllPosts();
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

        public void CreatePost(PostViewModel post)
        {
            _postService.CreatePost(new PostViewModel());
        }

        public void FileUpload(IFormFile file)
        {
            if (file != null)
            {
                string pic = Path.GetFileName(file.FileName);
                string path = Path.Combine(_env.WebRootPath, "~/img/posts", pic);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
        }
    }
}