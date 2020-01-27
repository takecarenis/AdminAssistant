using AdminAssistant.Blog.Models;
using AdminAssistant.Blog.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Interfaces
{
    public interface IPostService
    {
        List<PostViewModel> GetAllPosts();
        List<PostViewModel> GetFiltered(FilterModel filter);
        PostViewModel GetPost(int id);
        PostViewModel CreatePost(PostViewModel post);
        PostViewModel UpdatePost(PostViewModel post);
        PostViewModel DeletePost(int id);
        int GetLastPostId();
    }
}
