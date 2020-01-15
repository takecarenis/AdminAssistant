using System;
using System.Collections.Generic;
using System.Text;
using AdminAssistant.Domain;
using AdminAssistant.Domain.Blog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminAssistant.Blog.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Newsletter> Newsletter { get; set; }
        public DbSet<NewsletterHistory> NewsletterHistory { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<PostTag> PostTag { get; set; }
        public DbSet<PostCategory> PostCategory { get; set; }
    }
}
