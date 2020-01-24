using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Interfaces
{
    public interface INewsletterService
    {
        void Subscribe(string email);
        bool IsSubscribed(string email);
        void Unsubscribe(string email);
    }
}
