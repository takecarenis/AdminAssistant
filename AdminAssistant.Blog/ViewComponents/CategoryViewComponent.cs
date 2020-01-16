using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ISidebarService _service;

        public CategoryViewComponent(ISidebarService service) 
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CategoryViewModel> categories = _service.GetAllCategories();

            return View(categories);
        }
    }
}
