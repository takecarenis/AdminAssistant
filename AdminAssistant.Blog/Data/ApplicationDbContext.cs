using System;
using System.Collections.Generic;
using System.Text;
using AdminAssistant.Domain;
using AdminAssistant.Domain.Blog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AdminAssistant.Blog.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PostCategory>()
                .HasKey(bc => new { bc.PostId, bc.CategoryId });

            modelBuilder.Entity<PostCategory>()
                .HasOne(bc => bc.Post)
                .WithMany(b => b.PostCategories)
                .HasForeignKey(bc => bc.PostId);

            modelBuilder.Entity<PostCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.PostCategories)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<PostTag>()
               .HasKey(bc => new { bc.PostId, bc.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(bc => bc.Post)
                .WithMany(b => b.PostTags)
                .HasForeignKey(bc => bc.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(bc => bc.Tag)
                .WithMany(c => c.PostTags)
                .HasForeignKey(bc => bc.TagId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Newsletter> Newsletter { get; set; }
        public DbSet<NewsletterHistory> NewsletterHistory { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<PostCategory> PostCategory { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Page> Pages { get; set; }
    }
}
