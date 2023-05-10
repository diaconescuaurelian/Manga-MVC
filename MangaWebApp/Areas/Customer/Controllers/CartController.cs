using Manga.DataAccess.Repository.IRepository;
using Manga.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;

namespace MangaWebApp.Areas.Customer.Controllers
{
    [Area ("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unit;
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }
        public CartController(IUnitOfWork unit)
        {
                _unit = unit;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            foreach(var cart in  ShoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartViewModel.OrderHeader.ApplicationUser = _unit.ApplicationUser.Get(u=>u.Id== userId);

            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.ApplicationUser.Name;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.StreetAddress = ShoppingCartViewModel.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.County = ShoppingCartViewModel.OrderHeader.ApplicationUser.County;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartViewModel);
        }
        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unit.ShoppingCart.Get(u=>u.Id==cartId);
            cartFromDb.Count += 1;
            _unit.ShoppingCart.Update(cartFromDb);
            _unit.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unit.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                //remove from the cart
                _unit.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unit.ShoppingCart.Update(cartFromDb);
            }
            _unit.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unit.ShoppingCart.Get(u => u.Id == cartId);
            _unit.ShoppingCart.Remove(cartFromDb);
            _unit.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
