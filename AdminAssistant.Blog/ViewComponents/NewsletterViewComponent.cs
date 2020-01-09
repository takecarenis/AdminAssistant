using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.ViewComponents
{
    public class NewsletterViewComponent : ViewComponent
    {
        public NewsletterViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
