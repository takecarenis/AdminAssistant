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

        public CategoryViewModel AddNewCategory(string name)
        {
            _dbContext.Category.Add(new Category
            {
                Name = name
            });

            _dbContext.SaveChanges();

            return new CategoryViewModel
            {
                Name = name
            };
        }


        public TagViewModel AddNewTag(string name)
        {
            Tag newTag = new Tag { Name = name };
            _dbContext.Tag.Add(newTag);

            _dbContext.SaveChanges();

            return new TagViewModel
            { Name = name, Id = newTag.Id };
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
