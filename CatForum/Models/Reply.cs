using System;
using System.ComponentModel.DataAnnotations;

namespace CatForum.Models
{
    public class Reply
    {
        // Primary key for the reply
        [Key]
        public int ReplyId { get; set; }

        // Required text content for the reply, ensuring it’s not empty
        [Required(ErrorMessage = "Reply cannot be empty.")]
        public string Text { get; set; } = string.Empty;

        // Timestamp for when the reply was created, defaulting to the current date and time
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign key linking this reply to a specific post
        public int PostId { get; set; }

        // Navigation property to reference the associated post
        // Helps in accessing the parent post details when needed
        public Post? Post { get; set; }
    }
}
