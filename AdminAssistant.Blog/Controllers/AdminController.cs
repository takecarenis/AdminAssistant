using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace AdminAssistant.Blog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IPostService _postService;
        INewsletterService _newsletterService;
        IPageService _pageService;
        ISidebarService _sidebarService;
        private readonly ILogger<AdminController> _logger;

        private readonly IWebHostEnvironment _env;
        private static string _currentPhotoPath { get; set; }

        public AdminController(IPostService postService, INewsletterService newsLetterService, 
            IPageService pageService, ISidebarService sidebar, IWebHostEnvironment env, ILogger<AdminController> logger)
        {
            _postService = postService;
            _newsletterService = newsLetterService;
            _pageService = pageService;
            _sidebarService = sidebar;
            _env = env;
            _logger = logger;
        }

        [Route("Admin/e3NHA57XCuMk7S2TnumF38Dy6k6P2hQh9urjzYyVNMpegujKy2")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Admin/YgVjzvtwyYkxdqvYX9ndRuWwcJXDkZPR5TLsrcUK83CsBGkE7S")]
        public ActionResult Posts()
        {
            List<PostViewModel> posts = _postService.GetAllPosts();
            List<CategoryViewModel> categories = _sidebarService.GetAllCategories();
            List<TagViewModel> tags = _sidebarService.GetAllTags();

            BlogViewModel blog = new BlogViewModel
            {
                MainPosts = posts,
                Categories = categories,
                Tags = tags
            };

            return View(blog);
        }

        [Route("Admin/jtndAHHLCgKd3EbJQy3jbZwXQZTTcnyy7aW58rkdeQrB8GdXAR")]
        public ActionResult Newsletter()
        {
            UserNewsletterViewModel newsletter = new UserNewsletterViewModel();

            return View();
        }

        [Route("Admin/qntAZtrzSmRE89NcGduQCefrqTr73BC939EzDzwPeHknQ7w2AE")]
        public ActionResult Pages()
        {
            List<PageViewModel> pages = _pageService.GetAllPages();

            return View(pages);
        }

        public List<UserNewsletterViewModel> GetSubscribers()
        {
            UserNewsletterViewModel newsletter = new UserNewsletterViewModel();

            _logger.LogInformation("Before GetSubscribbers");
            List<UserNewsletterViewModel> subscribers = _newsletterService.GetPaginated(1, 10);

            _logger.LogInformation("Subscribers Done");
            return subscribers;
        }

        public bool CreatePost(PostViewModel post)
        {
            if (post == null) return false;
            post.PictureUrl = _currentPhotoPath;

            return _postService.CreatePost(post);
        }

        public bool DeletePost(PostViewModel post)
        {
            if (post == null || post.Id == 0) return false;
            return _postService.DeletePost(post.Id);
        }

        public void DeleteSubscribers(List<string> users)
        {
            _newsletterService.DeleteSubscribers(users);
        }

        public void SendEmail(SendMailViewModel mail)
        {
            _newsletterService.SendEmail(mail);
        }

        public bool UpdateUserCategory(SendMailViewModel updateCategory)
        {
            return _newsletterService.UpdateUserCategory(updateCategory);
        }

        public TagViewModel AddNewTag(TagViewModel newTag)
        {
            return _sidebarService.AddNewTag(newTag.Name);
        }

        public bool FileUpload()
        {
            try
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

                    return true;
                }

                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        #region Pages

        public void AddNewPage(string name)
        {
            _pageService.AddNewPage(name);
        }

        public void AddNewPageWithContent(string name, string content)
        {
            _pageService.AddNewPageWithContent(name, content);
        }

        public bool UpdateContent(PageViewModel page)
        {
            return _pageService.UpdateContent(page.Id, page.Text);
        }

        #endregion
    }
}