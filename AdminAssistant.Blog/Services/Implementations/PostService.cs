﻿using AdminAssistant.Blog.Data;
using AdminAssistant.Blog.Models;
using AdminAssistant.Blog.Models.DomainModel;
using AdminAssistant.Blog.Services.Interfaces;
using AdminAssistant.Domain.Blog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
                    Date = post.Date,
                    PictureUrl = post.PictureUrl,
                    TitleFormated = post.Title.Replace(" ", "-"),
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

        public bool EditPost(PostViewModel post)
        {
            bool isDeleted = DeletePost(post.Id);

            if (!isDeleted) return false;

            PostViewModel newPost = new PostViewModel
            {
                Body = post.Body,
                Categories = post.Categories,
                Date = post.Date,
                Intro = post.Intro,
                PictureUrl = post.PictureUrl,
                PostedBy = post.PostedBy,
                Tags = post.Tags,
                Title = post.Title,
                TitleFormated = post.Title.Replace(" ", "-")
            };

            bool isCreated = CreatePost(newPost);

            if (!isCreated) return false;

            return true;
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
                    TitleFormated = x.TitleFormated == null ? x.Title.Replace(" ", "-") : x.TitleFormated,
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
                }).ToList().OrderByDescending(x => x.Date).ToList();

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
                   TitleFormated = x.TitleFormated == null ? x.Title.Replace(" ", "-") : x.TitleFormated,
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

            posts = posts.Where(x => x.Categories.Select(p => p.Id).Contains(categoryId)).ToList().OrderByDescending(x => x.Date).ToList();

            return posts;
        }

        public List<PostViewModel> GetPaginated(int currentPage, int pageSize = 5)
        {
            List<PostViewModel> posts = _dbContext.Post
               .Include(x => x.PostCategories)
               .Select(x => new PostViewModel
               {
                   Id = x.Id,
                   Body = x.Body,
                   Date = x.Date,
                   TitleFormated = x.TitleFormated == null ? x.Title.Replace(" ", "-") : x.TitleFormated,
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
               }).OrderByDescending(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList().OrderByDescending(x => x.Date).ToList();

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
                    TitleFormated = x.TitleFormated == null ? x.Title.Replace(" ", "-") : x.TitleFormated,
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
                }).Skip(filter.Skip).Take(filter.Take).ToList().OrderByDescending(x => x.Date).ToList();

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
                    Intro = x.Intro,
                    Date = x.Date,
                    PictureUrl = x.PictureUrl,
                    TitleFormated = x.TitleFormated == null ? x.Title.Replace(" ", "-") : x.TitleFormated,
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

        public PostViewModel GetPostByTitle(string title)
        {
            title = title.Replace(" ", "-");
            PostViewModel post = _dbContext.Post
                .Include(x => x.PostCategories).Where(x => x.TitleFormated == title || x.Title == title)
                .Select(x => new PostViewModel
                {
                    Id = x.Id,
                    Body = x.Body,
                    Intro = x.Intro,
                    Date = x.Date,
                    TitleFormated = x.TitleFormated == null ? x.Title.Replace(" ", "-") : x.TitleFormated,
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

        public void ReplaceAllTitles()
        {
            var titles = _dbContext.Post.Where(x => x.TitleFormated == null).ToList();

            foreach(var title in titles)
            {
                title.TitleFormated = title.Title.Replace(" ", "-");
                _dbContext.SaveChanges();
            }
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
                postFromDb.TitleFormated = post.Title.Replace(" ", "-");
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
                   TitleFormated = x.TitleFormated == null ? x.Title.Replace(" ", "-") : x.TitleFormated,
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
               }).ToList().OrderByDescending(x => x.Date).ToList();

            return posts;
        }

        public List<PostViewModel> GetPostByTag(int tag)
        {
            List<PostViewModel> posts = _dbContext.Post
               .Include(x => x.PostTags)
               .Select(x => new PostViewModel
               {
                   Id = x.Id,
                   Body = x.Body,
                   Intro = x.Intro,
                   Date = x.Date,
                   TitleFormated = x.TitleFormated == null ? x.Title.Replace(" ", "-") : x.TitleFormated,
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

            posts = posts.Where(x => x.Tags.Select(p => p.Id).Contains(tag)).ToList().OrderByDescending(x => x.Date).ToList();

            return posts;
        } 
    }
}