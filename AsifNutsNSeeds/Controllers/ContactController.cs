using Microsoft.AspNetCore.Mvc;

namespace AsifNutsNSeeds.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
