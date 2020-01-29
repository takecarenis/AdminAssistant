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
        private string _currentPhotoPath { get; set; }

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
            FileUpload();

            _postService.CreatePost(new PostViewModel());
        }

        public ActionResult FileUpload()
        {
            if (string.IsNullOrEmpty(_currentPhotoPath))
            {
                var files = Request.Form.Files;

                if (files != null && files.Count > 0 && files[0] != null)
                {
                    var file = files[0];

                    int lastPostId = _postService.GetLastPostId();

                    string pic = Path.GetFileName(file.FileName);
                    string path = Path.Combine(_env.WebRootPath, "img\\posts", (lastPostId + 1).ToString() + Path.GetExtension(file.FileName));

                    file.CopyTo(new FileStream(path, FileMode.CreateNew));

                    _currentPhotoPath = path;

                    return Json("Successfully added photo!");
                }
            }

            return null;
        }
    }
}