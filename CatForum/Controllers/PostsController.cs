using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatForum.Data;
using CatForum.Models;
using Microsoft.EntityFrameworkCore;

namespace CatForum.Controllers
{
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
            if (ModelState.IsValid) // Ensure submitted data is valid
            {
                post.CreatedAt = DateTime.Now; // Set the current timestamp for post creation

                // If the user uploaded an image, generate a unique file name
                if (post.ImageUpload != null)
                {
                    string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(post.ImageUpload.FileName);
                    post.ImageFileName = uniqueName; // Store the generated file name in the post model
                }

                // Add the new post to the database
                _context.Add(post);
                await _context.SaveChangesAsync();

                // If an image was uploaded, save it to the server
                if (post.ImageUpload != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "catimages", post.ImageFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await post.ImageUpload.CopyToAsync(stream); // Copy image data to the file
                    }
                }

                return RedirectToAction("Index", "Home"); // Redirect to the home page after successful post creation
            }

            // If validation fails, return the form with entered data
            return View(post);
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
