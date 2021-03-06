﻿using AdminAssistant.Blog.Models;
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
        List<PostViewModel> GetPostByCategory(int categoryId);
        int GetCategoryId(string categoryName);
        List<PostViewModel> GetPaginated(int currentPage, int pageSize = 5);
        int GetPostCount();
        PostViewModel GetPost(int id);
        void ReplaceAllTitles();
        PostViewModel GetPostByTitle(string title);
        bool CreatePost(PostViewModel post);
        PostViewModel UpdatePost(PostViewModel post);
        bool DeletePost(int id);
        int GetLastPostId();
        List<PostViewModel> Search(string searchTerm);
        List<PostViewModel> GetPostByTag(int tag);
        bool EditPost(PostViewModel post);
    }
}
