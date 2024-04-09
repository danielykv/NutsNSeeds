using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AsifNutsNSeeds.Controllers
{
    public class AboutController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }
    }
}
