using AdminAssistant.Blog.Models.DomainModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.ViewComponents
{
    public class TagViewComponent : ViewComponent
    {
        public TagViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TagViewModel> tags = new List<TagViewModel>() { new TagViewModel { Id = 1, Name = "Test" } };

            return View(tags);
        }
    }
}
