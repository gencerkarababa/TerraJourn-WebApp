using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TerraJourn.Models.Entities;

namespace TerraJourn.Data
{
    public class Context : DbContext 
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AboutUs> AboutUsSections { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER - COMMENT
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);

            // BLOG - COMMENT
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Blog)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId)
                .WillCascadeOnDelete(true);

            // USER - BLOG
            modelBuilder.Entity<Blog>()
                .HasRequired(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}