using AsifNutsNSeeds.Data;
using AsifNutsNSeeds.Data.Static;
using AsifNutsNSeeds.Data.ViewModels;
using AsifNutsNSeeds.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AsifNutsNSeeds.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly AppDbContext _context;
		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}

		public async Task<IActionResult> Users()
		{
			var users = await _context.Users.ToListAsync();
			return View(users);
		}

		public IActionResult Login() => View(new LoginVM());
		[HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
		{
			if (!ModelState.IsValid) return View(loginVM);

			var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
			if(user != null)
			{
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
				if (passwordCheck)
				{
					var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
					if(result.Succeeded)
					{
						return RedirectToAction("Index", "Products");
					}
				}

                TempData["Error"] = "Wrong credentials. Please try again!";
                return View(loginVM);
            }
			TempData["Error"] = "Wrong credentials. Please try again!";
			return View(loginVM);

		}
        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
		public async Task<IActionResult> Register(RegisterVM registerVM)
		{
			if (!ModelState.IsValid) return View(registerVM);

			var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
			if (user != null)
			{
				TempData["Error"] = "This email address is already in use";
				return View(registerVM);
			}

			var newUser = new ApplicationUser()
			{
				Fullname = registerVM.FullName,
				Email = registerVM.EmailAddress,
				UserName = registerVM.EmailAddress,
				Address = registerVM.Address,
                City = registerVM.City,
				PostalCode = registerVM.PostalCode

            };
			var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

			if (newUserResponse.Succeeded)
			{
				await _userManager.AddToRoleAsync(newUser, UserRoles.User);
				return View("RegisterCompleted");

			}
			else
			{
				foreach (var error in newUserResponse.Errors)
				{
					if (error.Code == "PasswordRequiresDigit")
						ModelState.AddModelError("Password", "Password must contain at least one digit.");
					else if (error.Code == "PasswordRequiresLower")
						ModelState.AddModelError("Password", "Password must contain at least one lowercase letter.");
					else if (error.Code == "PasswordRequiresUpper")
						ModelState.AddModelError("Password", "Password must contain at least one uppercase letter.");
					else if (error.Code == "PasswordRequiresNonAlphanumeric")
						ModelState.AddModelError("Password", "Password must contain at least one special character.");
					else if (error.Code == "PasswordTooShort")
						ModelState.AddModelError("Password", "Password must be at least 6 characters long.");
					else
						ModelState.AddModelError("", error.Description);
				}
				return View(registerVM);
			}
		}

		[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Products");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}
