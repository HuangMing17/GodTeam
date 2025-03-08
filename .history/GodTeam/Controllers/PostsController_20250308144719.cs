using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GodTeam.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GodTeam.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? categoryId = null)
        {
            IQueryable<Post> postsQuery = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category);

            if (categoryId.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.PostCategories.Any(pc => pc.CategoryId == categoryId));
            }

            postsQuery = postsQuery.OrderByDescending(p => p.PublishedDate);

            var posts = await postsQuery.ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategoryId = categoryId;
            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Users = _context.Users.ToList();
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,UserId")] Post post, int[] selectedCategories)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    post.PublishedDate = DateTime.Now;
                    _context.Add(post);
                    await _context.SaveChangesAsync();

                    if (selectedCategories != null && selectedCategories.Length > 0)
                    {
                        foreach (var categoryId in selectedCategories)
                        {
                            _context.PostCategories.Add(new PostCategory
                            {
                                PostId = post.Id,
                                CategoryId = categoryId
                            });
                        }
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo bài viết: " + ex.Message);
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Users = _context.Users.ToList();
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.PostCategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Users = _context.Users.ToList();
            ViewBag.SelectedCategories = post.PostCategories.Select(pc => pc.CategoryId).ToArray();
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserId")] Post post, int[] selectedCategories)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var existingPost = await _context.Posts
                        .Include(p => p.PostCategories)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (existingPost == null)
                    {
                        return NotFound();
                    }

                    existingPost.Title = post.Title;
                    existingPost.Content = post.Content;
                    existingPost.UserId = post.UserId;

                    // Remove existing categories
                    _context.PostCategories.RemoveRange(existingPost.PostCategories);

                    // Add selected categories
                    if (selectedCategories != null && selectedCategories.Length > 0)
                    {
                        foreach (var categoryId in selectedCategories)
                        {
                            _context.PostCategories.Add(new PostCategory
                            {
                                PostId = post.Id,
                                CategoryId = categoryId
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật bài viết: " + ex.Message);
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Users = _context.Users.ToList();
            ViewBag.SelectedCategories = selectedCategories;
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts
                .Include(p => p.PostCategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post != null)
            {
                _context.PostCategories.RemoveRange(post.PostCategories);
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}