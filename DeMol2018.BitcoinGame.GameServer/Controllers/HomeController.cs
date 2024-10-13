using Microsoft.AspNetCore.Mvc;

namespace DeMol2018.BitcoinGame.GameServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}
