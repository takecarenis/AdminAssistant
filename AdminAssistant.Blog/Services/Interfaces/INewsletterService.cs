﻿using AdminAssistant.Blog.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Interfaces
{
    public interface INewsletterService
    {
        bool Subscribe(string email);
        bool IsSubscribed(string email);
        void Unsubscribe(string email);
        List<UserNewsletterViewModel> GetPaginated(int currentPage, int pageSize = 10);
        int GetSubscribersCount();
        bool DeleteSubscribers(List<string> users);
        bool SendEmail(SendMailViewModel sendMail);
        bool UpdateUserCategory(SendMailViewModel updateCategory);
    }
}
