using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace CatForum.Models
{
    public class Post
    {
        // Primary key for the Post entity
        [Key]
        public int PostId { get; set; }

        // The title of the post (required)
        [Required(ErrorMessage = "Please provide a title.")]
        public string Title { get; set; } = string.Empty;

        // The main content/body of the post (required)
        [Required(ErrorMessage = "Post content is required.")]
        public string Body { get; set; } = string.Empty;

        // Stores the file name of the uploaded image (if any)
        public string? ImageFileName { get; set; }

        // This property is used for handling file uploads in forms but isn't stored in the database
        [NotMapped]
        public IFormFile? ImageUpload { get; set; }

        // Automatically sets the timestamp when a post is created
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Collection to hold replies associated with this post (optional)
        public ICollection<Reply>? Replies { get; set; }

        // Ensure ApplicationUserId is a string
        public string? UserId { get; set; } 

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
