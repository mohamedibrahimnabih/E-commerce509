using E_commerce.Models;
using E_commerce.Models.ViewModels;
using E_commerce.Repository;
using E_commerce.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace E_commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager)
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = cartRepository.Get(includeProps: [e=>e.Product], filter: e=>e.ApplicationUserId == userManager.GetUserId(User));

            //ViewBag.Total = cart.Sum(e => e.Product.Price * e.Count);

            CartWithTotalPriceVM cartWithTotal = new CartWithTotalPriceVM()
            {
                Carts = cart.ToList(),
                TotalPrice = (double)cart.Sum(e => e.Product.Price * e.Count)
            };

            return View(cartWithTotal);
        }

        public IActionResult Increment(int productId)
        {
            var cart = cartRepository.GetOne(filter: e => e.ApplicationUserId == userManager.GetUserId(User) && e.ProductId == productId);

            cart.Count++;
            cartRepository.Commit();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddToCart(int count, int productId)
        {
            var userId = userManager.GetUserId(User);

            if (userId != null)
            {
                var cartInBD = cartRepository.GetOne(filter: e => e.ApplicationUserId == userId && e.ProductId == productId);

                if (cartInBD != null)
                {
                    cartInBD.Count += count;
                    cartRepository.Commit();
                }

                else
                {
                    cartRepository.Create(new()
                    {
                        ApplicationUserId = userId,
                        Count = count,
                        ProductId = productId,
                        Time = DateTime.Now
                    });

                }

                TempData["success"] = "تم اضافه المنتج الى السله بنجااح";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Pay()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Checkout/Success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/Cancel",
            };

            var cart = cartRepository.Get(includeProps: [e => e.Product], filter: e => e.ApplicationUserId == userManager.GetUserId(User)).ToList();

            foreach (var item in cart)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}
