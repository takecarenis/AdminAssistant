﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AdminAssistant.Blog.Models;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdminAssistant.Blog.Controllers
{
    public class BlogController : Controller
    {
        IPostService _postService;
        INewsletterService _newsletterService;
        private readonly ILogger<BlogController> _logger;

        public BlogController(ILogger<BlogController> logger, IPostService postService, INewsletterService newsletterService)
        {
            _postService = postService;
            _newsletterService = newsletterService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            BlogViewModel blog = new BlogViewModel();

            List<PostViewModel> posts = _postService.GetFiltered(new FilterModel { Take = 6 });

            blog.MainPosts.AddRange(posts.Take(2));

            return View(blog);
        }

        public IActionResult PostByCategory(int category)
        {
            if (category == 0) return Redirect("/Blog/Index");

            List<PostViewModel> posts = _postService.GetPostByCategory(category);

            return View(posts);
        }

        public IActionResult PostByTag(int tag)
        {
            if (tag == 0) return Redirect("/Blog/Index");

            List<PostViewModel> posts = _postService.GetPostByTag(tag);

            return View(posts);
        }

        public IActionResult Search(string searchTerm)
        {
            List<PostViewModel> posts = _postService.Search(searchTerm);
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
        public bool Subscribe(string email)
        {
            try
            {
                var addr = new MailAddress(email);

                if (addr.Address == email) 
                {
                    _logger.LogInformation("Calling Subscribe function.");
                    return _newsletterService.Subscribe(email);
                }

                _logger.LogInformation("Return false -> Email is invalid.");
                return false;
            }
            catch(Exception e)
            {
                _logger.LogError("Error: " + e.Message);
                return false;
            }
        }

        [HttpGet]
        public IActionResult Unsubscribe(string email)
        {
            _newsletterService.Unsubscribe(email);

            return View();
        }
    }
}