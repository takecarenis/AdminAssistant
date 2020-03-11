using AdminAssistant.Blog.Models.DomainModel;
using System.Collections.Generic;

namespace AdminAssistant.Blog.Services.Interfaces
{
    public interface IPageService
    {
        void AddNewPage(string name);
        void AddNewPageWithContent(string name, string content);
        bool UpdateContent(int id, string content);
        List<PageViewModel> GetAllPages();

        PageViewModel GetPage(int id);
    }
}