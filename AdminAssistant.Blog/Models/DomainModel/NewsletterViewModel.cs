using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Models.DomainModel
{
    public class UserNewsletterViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime SubscribeDate { get; set; }
        public string SubscribeDateString { get; set; }
        public bool IsActive { get; set; }
        public bool Checked { get; set; } = false;
    }

    public class SubscriberViewModel
    {
        List<UserNewsletterViewModel> Subscribers { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Count { get; set; }

        public bool ShowPrevious => CurrentPageIndex > 1;
        public bool ShowNext => CurrentPageIndex < TotalPages;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
    }
}
