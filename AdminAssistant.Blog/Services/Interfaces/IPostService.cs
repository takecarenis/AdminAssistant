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
        PostViewModel GetPost(int id);
        PostViewModel CreatePost(PostViewModel post);
        PostViewModel UpdatePost(PostViewModel post);
        PostViewModel DeletePost(int id);
    }
}
