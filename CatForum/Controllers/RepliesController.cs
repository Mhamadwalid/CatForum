using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatForum.Data;
using CatForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CatForum.Controllers
{
    [Authorize]
    public class RepliesController : Controller
    {
        private readonly CatForumContext _context;

        // Constructor - Initializes the database context
        public RepliesController(CatForumContext context)
        {
            _context = context;
        }

        // GET: /Replies/Create?postId=#
        // This method renders the reply creation form for a specific post.
        public IActionResult Create(int? postId)
        {
            if (postId == null) // Check if postId was provided
            {
                return NotFound(); // Return 404 if postId is missing
            }

            ViewData["PostId"] = postId; // Pass postId to the view
            return View(); // Render the reply creation view
        }

        // POST: /Replies/Create
        // This method handles form submission to create a new reply.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("ReplyId,Text,PostId")] Reply reply)
        {
            if (ModelState.IsValid) // Ensure the model passes validation
            {
                reply.CreatedAt = DateTime.Now; // Set timestamp for the reply
                _context.Add(reply); // Add reply to the database context
                await _context.SaveChangesAsync(); // Save changes 

                // Redirect to the details page of the post after submission
                return RedirectToAction("Details", "Home", new { id = reply.PostId });
            }

            // If validation fails, reload the form with the entered data
            ViewData["PostId"] = reply.PostId;
            return View(reply);
        }

        // GET: /Replies/Delete/5
        // This method fetches and displays a confirmation page before deleting a reply.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // Check if reply ID is provided
            {
                return NotFound(); // Return 404 if no ID is provided
            }

            // Retrieve the reply from the database, including its related post
            var reply = await _context.Replies
                .Include(r => r.Post) // Include Post data 
                .FirstOrDefaultAsync(m => m.ReplyId == id);

            if (reply == null) // If reply not found, return 404
            {
                return NotFound();
            }

            return View(reply); // Render the delete confirmation page
        }

        // POST: /Replies/Delete/5
        // This method handles the actual deletion of a reply.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // Protects against attacks
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the reply by ID
            var reply = await _context.Replies.FindAsync(id);

            if (reply != null) // If the reply exists
            {
                int postId = reply.PostId; // Store the post ID before deleting
                _context.Replies.Remove(reply); // Remove reply from database
                await _context.SaveChangesAsync(); // Save changes

                // Redirect back to the details page of the related post
                return RedirectToAction("Details", "Home", new { id = postId });
            }

            return NotFound(); // Return 404 if the reply wasn't found
        }
    }
}
