using AdminAssistant.Blog.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Interfaces
{
    public interface ISidebarService
    {
        List<CategoryViewModel> GetAllCategories();
        List<TagViewModel> GetAllTags();

        TagViewModel AddNewTag(string name);

        CategoryViewModel AddNewCategory(string name);
        bool DeleteTags(List<int> tags);
    }
}
