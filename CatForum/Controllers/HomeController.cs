using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatForum.Data;
using CatForum.Models;

namespace CatForum.Controllers
{
    public class HomeController : Controller
    {
        // Dependency injection for database context
        private readonly CatForumContext _context;

        // Constructor to initialize the database context
        public HomeController(CatForumContext context)
        {
            _context = context;
        }

        // GET: /Home/Index
        // This action fetches all posts along with their replies,
        // sorts them in descending order by creation date, and sends them to the view.
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.Replies) // Include replies for each post
                .OrderByDescending(p => p.CreatedAt) // Sort posts from newest to oldest
                .ToListAsync();

            return View(posts); // Pass the list of posts to the view
        }

        // GET: /Home/Details/5
        // Displays a specific post along with its replies.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // Check if the ID is provided
                return NotFound(); // Return 404 Not Found if no ID is given

            var post = await _context.Posts
                .Include(p => p.Replies.OrderByDescending(r => r.CreatedAt)) // Include replies, sorted by newest first
                .FirstOrDefaultAsync(p => p.PostId == id); // Find the post by ID

            if (post == null) // If the post doesn't exist, return 404
                return NotFound();

            return View(post); // Pass the post to the view
        }

        // GET: /Home/Privacy
        // Displays the Privacy Policy page
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
