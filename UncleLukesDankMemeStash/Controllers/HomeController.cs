using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UncleLukesDankMemeStash.Areas.Identity;
using UncleLukesDankMemeStash.Models;

namespace UncleLukesDankMemeStash.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<MemeAuthor> _loginManager;
        private readonly UserManager<MemeAuthor> _userManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<MemeAuthor> loginManager,
            UserManager<MemeAuthor> userManager)
        {
            _logger = logger;
            _loginManager = loginManager;
            _userManager = userManager;
        }

        private Task<MemeAuthor> GetUser()
            => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index()
        {
            var user = await GetUser();
            ViewBag.User = user;
            ViewBag.IsAdmin = user?.Admin ?? false;
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var user = await GetUser();
            ViewBag.User = user;
            return View();
        }

        public async Task<IActionResult> Register()
        {
            var user = await GetUser();
            ViewBag.User = user;
            return View();
        }

        public async Task<IActionResult> NotLogggedIn()
        {
            var user = await GetUser();
            ViewBag.User = user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel newUser)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Niepoprawne dane";
                return View();
            }

            newUser.Admin = false;
            var result = await _userManager.CreateAsync(newUser, newUser.Password);

            if (result.Succeeded) return RedirectToAction("Login");

            ViewBag.ErrorMessage = result.ToString();
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _loginManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Nie zalogowano - niepoprawne dane";
                return View();
            }

            var result =
                await _loginManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);

            if (!result.Succeeded)
            {
                ViewBag.ErrorMessage = "Nie zalogowano";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}