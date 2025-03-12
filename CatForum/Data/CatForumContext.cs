using Microsoft.EntityFrameworkCore;
using CatForum.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CatForum.Data
{
    // DbContext class that represents the database connection and tables for the CatForum application
    public class CatForumContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor that takes DbContextOptions and passes it to the base class
        public CatForumContext(DbContextOptions<CatForumContext> options) : base(options) { }

        // DbSet represents the Posts table in the database
        public DbSet<Post> Posts { get; set; }

        // DbSet represents the Replies table in the database
        public DbSet<Reply> Replies { get; set; }

        // This property seems to represent Photos, but it’s not a DbSet
        // If this is meant to store uploaded images, consider defining a Photo model and using DbSet Photo
        public object Photos { get; internal set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId) 
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
