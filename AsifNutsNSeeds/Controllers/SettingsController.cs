using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AsifNutsNSeeds.Models;

namespace AsifNutsNSeeds.Controllers
{
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SettingsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            return View(user); // Pass the user object to the view
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSettings(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    // Update user's information
                    user.Address = model.Address;
                    user.City = model.City;
                    user.PostalCode = model.PostalCode;

                    // Save changes to the database
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        // Redirect to a success page or back to settings page
                        TempData["ItemMessage"] = "Information upadated";

                        return RedirectToAction("Index"); // Change to your desired redirect route
                    }
                    else
                    {
                        // If update fails, add model errors and return to settings page
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            
            // If ModelState is invalid, return to the settings page with validation errors
            return View("Settings", model);
        }
    }
}
