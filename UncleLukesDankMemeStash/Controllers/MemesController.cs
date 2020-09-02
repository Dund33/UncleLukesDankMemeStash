using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UncleLukesDankMemeStash.Areas.Identity;
using UncleLukesDankMemeStash.Data;
using UncleLukesDankMemeStash.Models;

namespace UncleLukesDankMemeStash.Controllers
{
    public class MemesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<MemeAuthor> _loginManager;
        private readonly UserManager<MemeAuthor> _userManager;

        public MemesController(ApplicationDbContext context, SignInManager<MemeAuthor> loginManager,
            UserManager<MemeAuthor> userManager)
        {
            _context = context;
            _loginManager = loginManager;
            _userManager = userManager;
        }

        private Task<MemeAuthor> GetUser()
            => _userManager.GetUserAsync(HttpContext.User);

        private bool IsImage(string filename)
        {
            string[] imageFormats = {".jpg", ".jpeg", ".png", ".gif"};
            return imageFormats.Any(filename.EndsWith);
        }

        // GET: Memes
        public async Task<IActionResult> Index(int? category = null)
        {
            var memes = await _context.Memes
                .Include(m => m.Category)
                .Include(m => m.User)
                .ToListAsync();

            if (category != null)
                memes = memes
                    .Where(m => m.CategoryID == category)
                    .ToList();

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = currentUser;

            var tileViewModels = memes.Select(meme => new TileViewModel
            {
                Displayable = meme,
                CanEdit = meme.User == currentUser || (currentUser?.Admin ?? false)
            });

            return View(tileViewModels);
        }

        // GET: Memes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var meme = await _context.Memes
                .Include(m => m.Category)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (meme == null) return NotFound();

            var user = await GetUser();
            ViewBag.User = user;

            return View(meme);
        }

        // GET: Memes/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Title");

            var user = await GetUser();
            ViewBag.User = user;

            return View();
        }

        // POST: Memes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,ImageURL,Comment,CategoryID")]
            Meme meme)
        {
            var user = await GetUser();

            ViewBag.User = user;
            meme.User = user;

            if (ModelState.IsValid && IsImage(meme.ImageURL))
            {
                _context.Add(meme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Title", meme.CategoryID);
            ViewBag.Error = "Niewłaściwy format pliku lub uszkodzone dane";
            return View(meme);
        }

        // GET: Memes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var meme = await _context.Memes.FindAsync(id);
            if (meme == null) return NotFound();

            var user = await GetUser();
            if (meme.UserID != user.Id && !user.Admin) return View("NotAnOwner");

            ViewBag.User = user;
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Title", meme.CategoryID);

            return View(meme);
        }

        // POST: Memes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ImageURL,Comment,CategoryID,UserID")]
            Meme meme)
        {
            var user = await GetUser();
            var author = await _context.Memes.Where(m => m.ID == meme.ID).Select(m => m.User).FirstOrDefaultAsync();

            if (author.Id != user.Id && !user.Admin) return View("NotAnOwner");

            if (id != meme.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemeExists(meme.ID))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "ID", "Title", meme.CategoryID);
            ViewBag.User = user;
            return View(meme);
        }

        // GET: Memes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var meme = await _context.Memes
                .Include(m => m.Category)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (meme == null) return NotFound();

            var user = await GetUser();
            ViewBag.User = user;

            if (meme.UserID != user.Id && !user.Admin) return View("NotAnOwner");

            return View(meme);
        }

        // POST: Memes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meme = await _context.Memes.FindAsync(id);

            var user = await GetUser();
            if (meme.UserID != user.Id && !user.Admin) return View("NotAnOwner");

            ViewBag.User = user;

            _context.Memes.Remove(meme);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MemeExists(int id)
        {
            return _context.Memes.Any(e => e.ID == id);
        }

        private static int GetMatchingCharactersLen(IDisplayable meme, string query)
        {
            return Regex.Match(meme.Title, query, RegexOptions.IgnoreCase).Length;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var querySafe = query?.Trim() ?? "";
            var memes = await _context.Memes
                .Include(m => m.User)
                .ToListAsync();
            var sortedMemes = memes
                .OrderByDescending(meme => GetMatchingCharactersLen(meme, querySafe));

            var currentUser = await GetUser();

            var tileViewModels = sortedMemes.Select(meme => new TileViewModel
            {
                Displayable = meme,
                CanEdit = meme.User == currentUser || (currentUser?.Admin ?? false)
            });

            ViewBag.User = currentUser;

            return View("Index", tileViewModels);
        }
    }
}