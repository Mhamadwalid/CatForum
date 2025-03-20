using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CatForum.Data;
using CatForum.Models;

namespace CatForum.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly CatForumContext _context;

        public PostsController(CatForumContext context)
        {
            _context = context;
        }

        // GET: /Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            Console.WriteLine("Create method hit.");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model validation failed:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(post);
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User is not logged in.");
                return Unauthorized();
            }

            Console.WriteLine($"User ID: {userId}");
            post.UserId = userId;
            post.CreatedAt = DateTime.Now;

            if (post.ImageUpload != null && post.ImageUpload.Length > 0)
                {
                try
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(post.ImageUpload.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await post.ImageUpload.CopyToAsync(stream);
                    }

                    post.ImageFileName = "/uploads/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Image upload failed: " + ex.Message);
                    ModelState.AddModelError("", "Image upload failed: " + ex.Message);
                    return View(post);
                }
            }

            try
            {
                _context.Add(post);
                int changes = await _context.SaveChangesAsync();
                Console.WriteLine($"Database save successful! Rows affected: {changes}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database save failed: " + ex.Message);
                ModelState.AddModelError("", "Could not save post. Error: " + ex.Message);
                return View(post);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Replies)
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: /Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Replies)
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post != null)
            {
                if (!string.IsNullOrEmpty(post.ImageFileName))
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.ImageFileName.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                if (post.Replies != null)
                {
                    _context.Replies.RemoveRange(post.Replies);
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return NotFound();
        }
    }
}
