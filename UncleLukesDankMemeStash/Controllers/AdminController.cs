using System;
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

        public AdminController(ApplicationDbContext context, SignInManager<MemeAuthor> loginManager, UserManager<MemeAuthor> userManager)
        {
            _context = context;
            _loginManager = loginManager;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            MemeAuthor user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.Admin)
            {
                return View("NotAnAdmin");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> NonConfirmedUsers()
        {

            MemeAuthor user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.Admin)
            {
                return View("NotAnAdmin");
            }

            IEnumerable<MemeAuthor> users = await _context.Users.Where(user => user.EmailConfirmed == false).ToListAsync();

            return View(users);
        }
    }
}