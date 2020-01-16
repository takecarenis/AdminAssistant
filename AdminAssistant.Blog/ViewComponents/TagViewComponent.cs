using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.ViewComponents
{
    public class TagViewComponent : ViewComponent
    {
        private readonly ISidebarService _service;

        public TagViewComponent(ISidebarService service) 
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TagViewModel> tags = _service.GetAllTags();

            return View(tags);
        }
    }
}
