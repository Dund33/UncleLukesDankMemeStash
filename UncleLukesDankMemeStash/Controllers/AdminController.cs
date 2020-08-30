using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UncleLukesDankMemeStash.Areas.Identity;
using UncleLukesDankMemeStash.Data;

namespace UncleLukesDankMemeStash.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<MemeAuthor> _loginManager;
        private readonly UserManager<MemeAuthor> _userManager;

        public AdminController(ApplicationDbContext context, SignInManager<MemeAuthor> loginManager,
            UserManager<MemeAuthor> userManager)
        {
            _context = context;
            _loginManager = loginManager;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.Admin) return View("NotAnAdmin");

            return View();
        }

        [Authorize]
        public async Task<IActionResult> NonConfirmedUsers()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.Admin) return View("NotAnAdmin");

            IEnumerable<MemeAuthor> users = _context.Users
                .Where(user => user.EmailConfirmed == false);

            return View(users);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Confirm(string userID)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.Admin) return View("NotAnAdmin");

            if (userID == null)
                return NotFound("Invalid ID");

            var matchingUser = await _context.Users
                .Where(user => user.Id == userID)
                .SingleAsync();
            matchingUser.EmailConfirmed = true;
            _context.SaveChanges();
            return View("NonConfirmedUsers");
        }
    }
}