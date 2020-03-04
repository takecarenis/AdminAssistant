using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Models;
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

        public bool CreatePost(PostViewModel post)
        {
            try
            {
                Post newPost = new Post
                {
                    Body = post.Body,
                    Date = DateTime.Now,
                    PictureUrl = post.PictureUrl,
                    PostedBy = "Administrator",
                    Title = post.Title,
                    Intro = post.Intro
                };

                _dbContext.Post.Add(newPost);
                if (_dbContext.SaveChanges() != 1) return false;

                if (post.Categories != null)
                {
                    foreach (var category in post.Categories)
                    {
                        _dbContext.PostCategory.Add(new PostCategory
                        {
                            PostId = newPost.Id,
                            CategoryId = category.Id
                        });

                        _dbContext.SaveChanges();
                    }
                }

                if (post.Tags != null)
                {
                    foreach (var tag in post.Tags)
                    {
                        _dbContext.PostTags.Add(new PostTag
                        {
                            PostId = newPost.Id,
                            TagId = tag.Id
                        });

                        _dbContext.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeletePost(int id)
        {
            try
            {
                List<PostCategory> postCategories = _dbContext.PostCategory.Where(x => x.PostId == id).ToList();

                _dbContext.PostCategory.RemoveRange(postCategories);
                _dbContext.SaveChanges();

                List<PostTag> postTags = _dbContext.PostTags.Where(x => x.TagId == id).ToList();

                _dbContext.PostTags.RemoveRange(postTags);
                _dbContext.SaveChanges();

                Post post = _dbContext.Post.FirstOrDefault(x => x.Id == id);

                PostViewModel deletedPost = new PostViewModel();

                if (post == null) return false;

                deletedPost.Id = post.Id;

                _dbContext.Post.Remove(post);
                return _dbContext.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<PostViewModel> GetAllPosts()
        {
            List<PostViewModel> posts = _dbContext.Post
                .Include(x => x.PostCategories)
                .Select(x => new PostViewModel
                {
                    Id = x.Id,
                    Body = x.Body,
                    Date = x.Date.Date,
                    PictureUrl = x.PictureUrl,
                    PostedBy = x.PostedBy,
                    Title = x.Title,
                    Intro = x.Intro,
                    Categories = x.PostCategories.Select(p => new CategoryViewModel
                    {
                        Id = p.CategoryId,
                        Name = p.Category.Name
                    }).ToList(),
                    Tags = x.PostTags.Select(p => new TagViewModel
                    {
                        Id = p.TagId,
                        Name = p.Tag.Name
                    }).ToList()
                }).ToList();

            return posts;
        }

        public List<PostViewModel> GetPostByCategory(int categoryId)
        {
            List<PostViewModel> posts = _dbContext.Post
               .Include(x => x.PostCategories)
               .Select(x => new PostViewModel
               {
                   Id = x.Id,
                   Body = x.Body,
                   Intro = x.Intro,
                   Date = x.Date,
                   PictureUrl = x.PictureUrl,
                   PostedBy = x.PostedBy,
                   Title = x.Title,
                   Categories = x.PostCategories.Select(p => new CategoryViewModel
                   {
                       Id = p.CategoryId,
                       Name = p.Category.Name
                   }).ToList(),
                   Tags = x.PostTags.Select(p => new TagViewModel
                   {
                       Id = p.TagId,
                       Name = p.Tag.Name
                   }).ToList()
               }).ToList();

            posts = posts.Where(x => x.Categories.Select(p => p.Id).Contains(categoryId)).ToList();

            return posts;
        }

        public List<PostViewModel> GetPaginated(int currentPage, int pageSize = 3)
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
                   Intro = x.Intro,
                   Title = x.Title,
                   Categories = x.PostCategories.Select(p => new CategoryViewModel
                   {
                       Id = p.CategoryId,
                       Name = p.Category.Name
                   }).ToList(),
                   Tags = x.PostTags.Select(p => new TagViewModel
                   {
                       Id = p.TagId,
                       Name = p.Tag.Name
                   }).ToList()
               }).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return posts;
        }

        public int GetPostCount()
        {
            return _dbContext.Post.Count();
        }

        public List<PostViewModel> GetFiltered(FilterModel filter)
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
                    Intro = x.Intro,
                    Title = x.Title,
                    Categories = x.PostCategories.Select(p => new CategoryViewModel
                    {
                        Id = p.CategoryId,
                        Name = p.Category.Name
                    }).ToList(),
                    Tags = x.PostTags.Select(p => new TagViewModel
                    {
                        Id = p.TagId,
                        Name = p.Tag.Name
                    }).ToList()
                }).Skip(filter.Skip).Take(filter.Take).ToList();

            return posts;
        }

        public int GetLastPostId()
        {
            Post post = _dbContext.Post.OrderByDescending(x => x.Date).FirstOrDefault();

            return post != null ? post.Id : 0;
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
                    }).ToList(),
                    Tags = x.PostTags.Select(p => new TagViewModel
                    {
                        Id = p.TagId,
                        Name = p.Tag.Name
                    }).ToList()
                }).FirstOrDefault();

            return post;
        }

        public PostViewModel UpdatePost(PostViewModel post)
        {
            Post postFromDb = _dbContext.Post.Include(x => x.PostCategories).FirstOrDefault(x => x.Id == post.Id);

            if (postFromDb != null)
            {
                postFromDb.Body = post.Body;
                postFromDb.PictureUrl = post.PictureUrl;
                postFromDb.PostedBy = post.PostedBy;
                postFromDb.Title = post.Title;
                postFromDb.PostCategories = post.Categories
                    .Select(x => new PostCategory
                    {
                        CategoryId = x.Id,
                        PostId = postFromDb.Id
                    }).ToList();
                postFromDb.PostTags = post.Tags
                    .Select(x => new PostTag
                    {
                        TagId = x.Id,
                        PostId = postFromDb.Id
                    }).ToList();

                _dbContext.SaveChanges();
            }

            return post;
        }

        public int GetCategoryId(string categoryName)
        {
            Category category = _dbContext.Category.FirstOrDefault(x => x.Name.ToLower() == categoryName.ToLower());

            if (category != null)
            {
                return category.Id;
            }

            return 0;
        }

        public List<PostViewModel> Search(string searchTerm)
        {
            List<PostViewModel> posts = _dbContext.Post
               .Include(x => x.PostCategories)
               .Where(x => x.Title.Contains(searchTerm) || x.Body.Contains(searchTerm))
               .Select(x => new PostViewModel
               {
                   Id = x.Id,
                   Body = x.Body,
                   Date = x.Date,
                   PictureUrl = x.PictureUrl,
                   PostedBy = x.PostedBy,
                   Intro = x.Intro,
                   Title = x.Title,
                   Categories = x.PostCategories.Select(p => new CategoryViewModel
                   {
                       Id = p.CategoryId,
                       Name = p.Category.Name
                   }).ToList(),
                   Tags = x.PostTags.Select(p => new TagViewModel
                   {
                       Id = p.TagId,
                       Name = p.Tag.Name
                   }).ToList()
               }).ToList();

            return posts;
        }
    }
}