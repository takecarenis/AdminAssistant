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

        DbSet<Newsletter> Newsletter { get; set; }
        DbSet<NewsletterHistory> NewsletterHistory { get; set; }
        DbSet<Post> Post { get; set; }
        DbSet<Category> Category { get; set; }
        DbSet<Tag> Tag { get; set; }
        DbSet<PostTag> PostTag { get; set; }
        DbSet<PostCategory> PostCategory { get; set; }
    }
}
