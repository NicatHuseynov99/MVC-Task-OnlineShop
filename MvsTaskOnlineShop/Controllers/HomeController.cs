using Microsoft.AspNetCore.Mvc;

namespace MvsTaskOnlineShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
