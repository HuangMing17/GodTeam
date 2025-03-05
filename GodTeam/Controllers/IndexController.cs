using GodTeam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace GodTeam.Controllers
{
    public class IndexController : Controller
    {

        private  ApplicationDbContext _db;
        public IndexController(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts.Include(p => p.Author).Include(p => p.Category).ToListAsync();
            return View(posts);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.PublishedDate = DateTime.Now;
                _db.Add(post);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

    }
}
