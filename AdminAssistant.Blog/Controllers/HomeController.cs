using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdminAssistant.Blog.Models;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;

namespace AdminAssistant.Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IPostService _postService;
        IPageService _pageService;

        public HomeController(ILogger<HomeController> logger, IPostService service, IPageService pageService)
        {
            _logger = logger;
            _postService = service;
            _pageService = pageService;

            _postService.ReplaceAllTitles();
        }

        public IActionResult Index(int page=1)
        {
            BlogViewModel blog = new BlogViewModel();

            List<PostViewModel> posts = _postService.GetPaginated(page);

            blog.MainPosts.AddRange(posts);
            blog.MainPosts.OrderByDescending(x => x.Date).ToList();
            blog.CurrentPageIndex = page;
            blog.Count = _postService.GetPostCount();

            return View(blog);
        }

        public IActionResult Privacy()
        {
            PageViewModel page = _pageService.GetPage(2);

            return View(page);
        }

        public IActionResult Terms()
        {
            PageViewModel page = _pageService.GetPage(1);

            return View(page);
        }

        public IActionResult About()
        {
            PageViewModel page = _pageService.GetPage(3);

            return View(page);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
