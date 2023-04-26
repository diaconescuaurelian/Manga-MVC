using Microsoft.AspNetCore.Mvc;

namespace MangaWebApp.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
