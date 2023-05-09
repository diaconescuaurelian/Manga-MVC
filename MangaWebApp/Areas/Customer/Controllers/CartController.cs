using Microsoft.AspNetCore.Mvc;

namespace MangaWebApp.Areas.Customer.Controllers
{
    [Area ("Customer")]
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
