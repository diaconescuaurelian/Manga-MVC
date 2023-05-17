
using Manga.DataAccess.Repository.IRepository;
using Manga.Models;
using Manga.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace MangaWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IUnitOfWork _unit;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unit)
        {
            _logger = logger;
            _unit = unit;
        }

        public IActionResult Index()
        {
           
            IEnumerable<Product> productList = _unit.Product.GetAll(includeProperties:"Category");
            return View(productList);
        }
        public IActionResult Details(int id)
        {
            ShoppingCart cart = new()
            {
                Product = _unit.Product.Get(u => u.Id == id, includeProperties: "Category"),
                Count = 1,
                ProductId = id
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDatabase = _unit.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);
            if (cartFromDatabase != null)
            {
                //shopping cart exists
                cartFromDatabase.Count += shoppingCart.Count;
                _unit.ShoppingCart.Update(cartFromDatabase);
                _unit.Save();
            }
            else
            {
                //add a cart record
                _unit.ShoppingCart.Add(shoppingCart);
                shoppingCart.Id = 0;

                _unit.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                                             _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == shoppingCart.ApplicationUserId).Count());
            }
            TempData["success"] = "Cart updated successfully";
           
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}