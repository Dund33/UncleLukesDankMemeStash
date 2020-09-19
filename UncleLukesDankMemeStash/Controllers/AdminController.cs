using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UncleLukesDankMemeStash.Areas.Identity;
using UncleLukesDankMemeStash.Data;
using UncleLukesDankMemeStash.Models;

namespace UncleLukesDankMemeStash.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<MemeAuthor> _userManager;
        //private readonly ILogger<AdminController> _logger;
        private const int UsersToDisplay = 10;

        public AdminController(ApplicationDbContext context,
            UserManager<MemeAuthor> userManager)
        {
            _context = context;
            _userManager = userManager;
            //_logger = logger;
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
        [Authorize]
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

        [Authorize]
        public async Task<IActionResult> Details(string Id)
        {
            var user = await GetUser();

            if (!user.Admin)
                return View("NotAnAdmin");

            if (string.IsNullOrEmpty(Id))
                return NotFound();

            var foundUser = await _userManager.FindByIdAsync(Id);

            return View(foundUser);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SetPassword(string Id)
        {
            Console.WriteLine(Id);
            var user = await GetUser();
            if (!user.Admin)
                return View("NotAnAdmin");

            var foundUser = await _userManager.FindByIdAsync(Id);

            return View(foundUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SetPassword([Bind] string Id, [Bind] string Password, [Bind] string PasswordConf)
        {
            var user = await GetUser();
            if (!user.Admin)
                return View("NotAnAdmin");

            var foundUser = await _userManager.FindByIdAsync(Id);

            if (Password != PasswordConf)
            {
                ViewBag.Success = false;
                return View(foundUser);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(foundUser);
            var passwordChangeResult = await _userManager.ResetPasswordAsync(foundUser, resetToken, Password);

            ViewBag.Success = passwordChangeResult.Succeeded;
            return View(foundUser);
        }

        private static int GetMatchingCharactersLen(MemeAuthor user, string query)
        {
            return Regex.Match(user.UserName, query, RegexOptions.IgnoreCase).Length;
        }

        private Task<MemeAuthor> GetUser()
            => _userManager.GetUserAsync(HttpContext.User);
    }
}