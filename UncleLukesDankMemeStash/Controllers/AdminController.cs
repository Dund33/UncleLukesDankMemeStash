using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<MemeAuthor> _loginManager;
        private readonly UserManager<MemeAuthor> _userManager;

        private const int UsersToDisplay = 10;

        public AdminController(ApplicationDbContext context,
            SignInManager<MemeAuthor> loginManager,
            UserManager<MemeAuthor> userManager)
        {
            _context = context;
            _loginManager = loginManager;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await GetUser();
            ViewBag.User = user;
            if (!user.Admin) return View("NotAnAdmin");

            return View();
        }

        [Authorize]
        public async Task<IActionResult> NonConfirmedUsers()
        {
            var user = await GetUser();
            ViewBag.User = user;

            if (!user.Admin) return View("NotAnAdmin");

            IEnumerable<MemeAuthor> users = _context.Users
                .Where(user => user.EmailConfirmed == false);

            return View(users);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Confirm(string id)
        {
            var user = await GetUser();
            ViewBag.User = user;

            if (!user.Admin) return View("NotAnAdmin");

            if (id == null)
                return NotFound("Invalid ID");

            var matchingUser = await _context.Users
                .Where(user => user.Id == id)
                .SingleAsync();
            matchingUser.EmailConfirmed = true;
            _context.SaveChanges();

            return RedirectToAction(nameof(NonConfirmedUsers));
        }

        [HttpGet]
        public async Task<IActionResult> FindUserByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return View(new List<UserDTO>());

            var nameTrimmed = name.Trim();

            var usersList = await _context.Users.ToListAsync();

            var users = usersList
                .OrderByDescending(user => GetMatchingCharactersLen(user, nameTrimmed))
                .Take(UsersToDisplay);

            var userDTOs = users.Select(user => new UserDTO
            {
                UserName = user.UserName,
                ID = user.Id
            });

            return View(userDTOs);
        }

        private static int GetMatchingCharactersLen(MemeAuthor user, string query)
        {
            return Regex.Match(user.UserName, query, RegexOptions.IgnoreCase).Length;
        }

        private Task<MemeAuthor> GetUser()
            => _userManager.GetUserAsync(HttpContext.User);
    }
}