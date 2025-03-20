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
                .Include(p => p.ApplicationUser)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(posts);
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
        public async Task<IActionResult> Profile(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

    }
}
