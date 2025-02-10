using Microsoft.EntityFrameworkCore;
using CatForum.Models;
using System.Collections.Generic;

namespace CatForum.Data
{
    // DbContext class that represents the database connection and tables for the CatForum application
    public class CatForumContext : DbContext
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
    }
}
