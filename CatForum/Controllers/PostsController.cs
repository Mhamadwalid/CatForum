using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatForum.Data;
using CatForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CatForum.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly CatForumContext _context;

        // Constructor to initialize the database context
        public PostsController(CatForumContext context)
        {
            _context = context;
        }

        // GET: /Posts/Create
        // Displays the form for creating a new post
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Posts/Create
        // Handles form submission for creating a new post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Body,ImageUpload")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                post.CreatedAt = DateTime.Now; // Set post creation timestamp

                if (post.ImageUpload != null)
                {
                    string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(post.ImageUpload.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profilepics");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, uniqueName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await post.ImageUpload.CopyToAsync(stream);
                    }

                    post.ImageFileName = uniqueName;
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(post); // If validation fails, return to the form
        }


        // GET: /Posts/Delete/5
        // Displays a confirmation page before deleting a post
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Ensure a valid post ID is provided
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Replies) // Include replies to show related content
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null) // If the post doesn't exist, return a 404 response
            {
                return NotFound();
            }

            return View(post); // Render the delete confirmation view
        }

        // POST: /Posts/Delete/5
        // Handles the actual deletion of the post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Replies) // Include replies to ensure they are deleted with the post
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post != null)
            {
                // Delete all replies associated with this post before removing it
                if (post.Replies != null)
                {
                    _context.Replies.RemoveRange(post.Replies);
                }

                _context.Posts.Remove(post); // Remove the post from the database
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home"); // Redirect to the home page after deletion
            }

            return NotFound(); // If post was not found, return a 404 response
        }


    }
}
