using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Implementations
{
    public class SidebarService : ISidebarService
    {
        ApplicationDbContext _dbContext;

        public SidebarService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public List<CategoryViewModel> GetAllCategories()
        {
            List<CategoryViewModel> categories = _dbContext.Category
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();


            return categories;
        }

        public List<TagViewModel> GetAllTags()
        {
            List<TagViewModel> tags = _dbContext.Tag
                .Select(x => new TagViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            return tags;
        }
    }
}
