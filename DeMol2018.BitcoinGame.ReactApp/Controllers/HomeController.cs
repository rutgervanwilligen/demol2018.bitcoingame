using Microsoft.AspNetCore.Mvc;

namespace DeMol2018.BitcoinGame.ReactApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
