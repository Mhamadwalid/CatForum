using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CatForum.Models;

namespace CatForum.Data
{
    public class CatForumContext : IdentityDbContext<ApplicationUser>
    {
        public CatForumContext(DbContextOptions<CatForumContext> options) : base(options) { }

        // Define the database tables
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure the Post model correctly maps to the database
            modelBuilder.Entity<Post>()
                .HasKey(p => p.PostId); // Ensure PostId is the primary key

            modelBuilder.Entity<Post>()
                .Property(p => p.PostId)
                .HasColumnName("PostId"); // Ensures the database uses "PostId" and not "Id"

            // Define foreign key relationship between Posts and Users
            modelBuilder.Entity<Post>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
