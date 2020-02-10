using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace AdminAssistant.Blog.Controllers
{
    public class AdminController : Controller
    {
        IPostService _postService;
        INewsletterService _newsletterService;

        private readonly IWebHostEnvironment _env;
        private static string _currentPhotoPath { get; set; }

        public AdminController(IPostService postService, INewsletterService newsLetterService, IWebHostEnvironment env)
        {
            _postService = postService;
            _newsletterService = newsLetterService;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Posts()
        {
            List<PostViewModel> posts = _postService.GetAllPosts();

            return View(posts);
        }

        public ActionResult Newsletter()
        {
            UserNewsletterViewModel newsletter = new UserNewsletterViewModel();

            //List<PostViewModel> subscribers = _newsletterService.GetPaginated(page);

            //blog.MainPosts.AddRange(posts);
            //blog.CurrentPageIndex = page;
            //blog.Count = _postService.GetPostCount();

            return View();
        }

        public List<UserNewsletterViewModel> GetSubscribers()
        {
            UserNewsletterViewModel newsletter = new UserNewsletterViewModel();

            List<UserNewsletterViewModel> subscribers = _newsletterService.GetPaginated(1, 10);

            return subscribers;
        }

        public void CreatePost(PostViewModel post)
        {
            post.PictureUrl = _currentPhotoPath;

            _postService.CreatePost(post);
        }

        public bool DeletePost(PostViewModel post)
        {
            return _postService.DeletePost(post.Id);
        }

        public void DeleteSubscribers(List<string> users)
        {
            _newsletterService.DeleteSubscribers(users);
        }


        public ActionResult FileUpload()
        {
            var files = Request.Form.Files;

            if (files != null && files.Count > 0 && files[0] != null)
            {
                var file = files[0];

                int lastPostId = _postService.GetLastPostId();

                string pic = Path.GetFileName(file.FileName);
                string path = Path.Combine(_env.WebRootPath, "img\\posts", (lastPostId + 1).ToString() + Path.GetExtension(file.FileName));

                file.CopyTo(new FileStream(path, FileMode.CreateNew));

                _currentPhotoPath = "\\img\\posts\\" + (lastPostId + 1).ToString() + Path.GetExtension(file.FileName);

                return Json("Successfully added photo!");
            }

            return null;
        }
    }
}