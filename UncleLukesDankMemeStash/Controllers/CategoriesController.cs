using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UncleLukesDankMemeStash.Areas.Identity;
using UncleLukesDankMemeStash.Data;
using UncleLukesDankMemeStash.Models;

namespace UncleLukesDankMemeStash.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<MemeAuthor> _loginManager;
        private readonly UserManager<MemeAuthor> _userManager;

        public CategoriesController(ApplicationDbContext context, SignInManager<MemeAuthor> loginManager,
            UserManager<MemeAuthor> userManager)
        {
            _context = context;
            _loginManager = loginManager;
            _userManager = userManager;
        }

        private Task<MemeAuthor> GetUser()
            => _userManager.GetUserAsync(HttpContext.User);

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Category.ToListAsync();
            var user = await GetUser();

            ViewBag.User = user;
            ViewBag.CanAdd = user?.Admin ?? false;

            var tileViewModels = categories.Select(category => new TileViewModel
            {
                Displayable = category,
                CanEdit = user?.Admin ?? false
            });

            return View(tileViewModels);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var user = await GetUser();
            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.ID == id);

            ViewBag.User = user;

            if (category == null) return NotFound();

            return View(category);
        }

        // GET: Categories/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await GetUser();
            ViewBag.User = user;

            return !user.Admin ? View("NotAnAdmin") : View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,ImageURL")]
            Category category)
        {
            var user = await GetUser();

            ViewBag.User = user;

            if (!user.Admin) return View("NotAnAdmin");

            if (!ModelState.IsValid)
                return View(category);

            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await GetUser();
            ViewBag.User = user;

            if (id == null) return NotFound();

            if (!user.Admin) return View("NotAnAdmin");

            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,ImageURL")]
            Category category)
        {
            var user = await GetUser();
            ViewBag.User = user;

            if (!user.Admin) return View("NotAnAdmin");

            if (id != category.ID) return NotFound();

            if (!ModelState.IsValid)
                return View(category);

            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.ID)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await GetUser();
            ViewBag.User = user;

            if (!user.Admin) return View("NotAnAdmin");

            if (id == null) return NotFound();

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.ID == id);

            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await GetUser();
            ViewBag.User = user;

            if (!user.Admin) return View("NotAnAdmin");

            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.ID == id);
        }
    }
}