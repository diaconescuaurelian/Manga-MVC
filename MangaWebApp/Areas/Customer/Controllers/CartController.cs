using Manga.DataAccess.Repository.IRepository;
using Manga.Models;
using Manga.Models.ViewModels;
using Manga.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Stripe.Checkout;
using System.Security.Claims;

namespace MangaWebApp.Areas.Customer.Controllers
{
    [Area ("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unit;
        [BindProperty]
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

			ShoppingCartViewModel.OrderHeader.ApplicationUser = _unit.ApplicationUser.Get(u => u.Id == userId);

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

        [HttpPost]
        [ActionName("Summary")]
		public IActionResult SummaryPOST()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel.ShoppingCartList = _unit.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");
			
            ShoppingCartViewModel.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartViewModel.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unit.ApplicationUser.Get(u=>u.Id == userId);

			//ShoppingCartViewModel.OrderHeader.ApplicationUser = _unit.ApplicationUser.Get(u => u.Id == userId);

			foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
			{
				cart.Price = cart.Product.Price;
				ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			ShoppingCartViewModel.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
			ShoppingCartViewModel.OrderHeader.OrderStatus = SD.StatusPending;
			_unit.OrderHeader.Add(ShoppingCartViewModel.OrderHeader);
			_unit.Save();

			foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
			{
				OrderDetail orderDetail = new()
				{
					ProductId = cart.ProductId,
					OrderId = ShoppingCartViewModel.OrderHeader.Id,
					Price = cart.Price,
					Count = cart.Count
				};
				_unit.OrderDetail.Add(orderDetail);
				_unit.Save();
			}
			var domain = "https://localhost:7241/";
			var options = new SessionCreateOptions
			{
				SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartViewModel.OrderHeader.Id}",
				CancelUrl = domain + "customer/cart/index",
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
			};

			foreach (var item in ShoppingCartViewModel.ShoppingCartList)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * 100),
						Currency = "ron",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Title
						}
					},
					Quantity = item.Count
				};
				options.LineItems.Add(sessionLineItem);
			}

			var service = new SessionService();
			Session session = service.Create(options);

			_unit.OrderHeader.UpdateStripePaymentID(ShoppingCartViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
			_unit.Save();
			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);

			//return RedirectToAction(nameof(OrderConfirmation), new {id = ShoppingCartViewModel.OrderHeader.Id});
		}

        public IActionResult OrderConfirmation(int id)
        {
			OrderHeader orderHeader = _unit.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");


			var service = new SessionService();
			Session session = service.Get(orderHeader.SessionId);

			if (session.PaymentStatus.ToLower() == "paid")
			{
				_unit.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
				_unit.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
				_unit.Save();
			}

			List<ShoppingCart> shoppingCartList = _unit.ShoppingCart.GetAll(u=>u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

			_unit.ShoppingCart.RemoveRange(shoppingCartList);
			_unit.Save();

            return View(id);
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
