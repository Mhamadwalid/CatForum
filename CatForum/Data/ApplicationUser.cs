using CatForum.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CatForum.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? ImageFilename { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
