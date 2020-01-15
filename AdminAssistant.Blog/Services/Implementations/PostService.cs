using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using AdminAssistant.Domain.Blog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Services.Implementations
{
    public class PostService : IPostService
    {
        ApplicationDbContext _dbContext;

        public PostService(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public PostViewModel CreatePost(PostViewModel post)
        {
            Post newPost = new Post
            {
                Body = "This is body................",
                Date = DateTime.Now,
                PictureUrl = "This is url",
                PostedBy = "Stefan Djokic",
                Title = "This is Title"
            };

            CategoryViewModel category1 = new CategoryViewModel { Id = 1, Name = "Category" };
            CategoryViewModel category2 = new CategoryViewModel { Id = 2, Name = "Category2" };
            post.Categories = new List<CategoryViewModel>
            {  category1, category2
            };


            _dbContext.Post.Add(newPost);
            _dbContext.SaveChanges();

            foreach(var category in post.Categories)
            {
                Category categoryFromTable = _dbContext.Category.FirstOrDefault(x => x.Id == category.Id);

                PostCategory postCategory = new PostCategory
                {
                    Category = categoryFromTable,
                    CategoryId = categoryFromTable.Id,
                    Post = newPost,
                    PostId = newPost.Id
                };

                _dbContext.PostCategory.Add(postCategory);
                _dbContext.SaveChanges();
            }

            return new PostViewModel();
        }

        public PostViewModel DeletePost(int id)
        {
            //List<PostCategory>   

            return new PostViewModel();
        }

        public List<PostViewModel> GetAllPosts()
        {
            List<PostViewModel> posts = _dbContext.Post
                .Include(x => x.PostCategories)
                .Select(x => new PostViewModel
                {
                    Id = x.Id,
                    Body = x.Body,
                    Date = x.Date,
                    PictureUrl = x.PictureUrl,
                    PostedBy = x.PostedBy,
                    Title = x.Title,
                    Categories = x.PostCategories.Select(p => new CategoryViewModel
                    {
                        Id = p.CategoryId,
                        Name = p.Category.Name
                    }).ToList()
                }).ToList();

            return posts;
        }

        public PostViewModel GetPost(int id)
        {
            PostViewModel post = _dbContext.Post
                .Include(x => x.PostCategories).Where(x => x.Id == id)
                .Select(x => new PostViewModel
                {
                    Id = id,
                    Body = x.Body,
                    Date = x.Date,
                    PictureUrl = x.PictureUrl,
                    PostedBy = x.PostedBy,
                    Title = x.Title,
                    Categories = x.PostCategories.Select(p => new CategoryViewModel
                    {
                        Id = p.CategoryId,
                        Name = p.Category.Name
                    }).ToList()
                }).FirstOrDefault();

            return post;
        }

        public PostViewModel UpdatePost(PostViewModel post)
        {
            throw new NotImplementedException();
        }
    }
}
