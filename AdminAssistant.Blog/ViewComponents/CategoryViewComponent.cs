using AdminAssistant.Blog.Models.DomainModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        public CategoryViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CategoryViewModel> categories = new List<CategoryViewModel>() { new CategoryViewModel { Id = 1, Name = "Test" } };

            return View(categories);
        }
    }
}
