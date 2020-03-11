using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using AdminAssistant.Domain.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Implementations
{
    public class PageService : IPageService
    {
        private ApplicationDbContext _context;
        public PageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddNewPage(string name)
        {
            _context.Pages.Add(new Page
            {
                Name = name
            });

            _context.SaveChanges();
        }

        public void AddNewPageWithContent(string name, string content)
        {
            _context.Pages.Add(new Page
            {
                Name = name,
                Text = content
            });

            _context.SaveChanges();
        }

        public bool UpdateContent(int id, string content)
        {
            Page existPage = _context.Pages.FirstOrDefault(x => x.Id == id);

            if (existPage != null)
            {
                existPage.Text = content;

                return _context.SaveChanges() == 1;
            }

            return false;
        }

        public List<PageViewModel> GetAllPages()
        {
            List<PageViewModel> pages = _context.Pages.Select(x => new PageViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Text = x.Text
            }).ToList();

            return pages;
        }

        public PageViewModel GetPage(int id)
        {
            PageViewModel page = _context.Pages
                .Where(x => x.Id == id)
                .Select(x => new PageViewModel
                {
                    Name = x.Name,
                    Text = x.Text,
                    Id = x.Id
                }).FirstOrDefault();

            return page;
        }
    }
}
