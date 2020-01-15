using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using AdminAssistant.Domain.Blog;
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
                Body = post.Body,
                Date = post.Date,
                PictureUrl = post.PictureUrl,
                PostedBy = post.PostedBy,
                Title = post.Title
            };

            _dbContext.Post.Add(newPost);
            _dbContext.SaveChanges();

            foreach(var tag in post.Tags)
            {
                Tag tagFromTable = _dbContext.Tag.FirstOrDefault(x => x.Id == tag.Id);
                _dbContext.PostTag.Add(new PostTag
                {
                    Post = newPost,
                    Tag = tagFromTable
                });

                _dbContext.SaveChanges();
            }

            foreach(var category in post.Categories)
            {
                Category categoryFromTable = _dbContext.Category.FirstOrDefault(x => x.Id == category.Id);
                _dbContext.PostCategory.Add(new PostCategory
                {
                    Post = newPost,
                    Category = categoryFromTable
                });

                _dbContext.SaveChanges();
            }

            return new PostViewModel
            {
                Body = newPost.Body,
                Id = newPost.Id,
                Date = newPost.Date,
                PictureUrl = newPost.PictureUrl,
                PostedBy = newPost.PostedBy,
                Title = newPost.Title,
                Categories = post.Categories,
                Tags = post.Tags
            };
        }

        public PostViewModel DeletePost(int id)
        {
            throw new NotImplementedException();
        }

        public List<PostViewModel> GetAllPosts()
        {
            List<PostViewModel> posts = _dbContext.Post.Select(x => new PostViewModel
            {
                Id = x.Id,
                Body = x.Body,
                Date = x.Date,
                PictureUrl = x.PictureUrl,
                PostedBy = x.PostedBy,
                Title = x.Title
            }).ToList();

            foreach(var post in posts)
            {

            }

        }

        public PostViewModel GetPost(int id)
        {
            throw new NotImplementedException();
        }

        public PostViewModel UpdatePost(PostViewModel post)
        {
            throw new NotImplementedException();
        }
    }
}
