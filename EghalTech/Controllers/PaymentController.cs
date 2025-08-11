using EghalTech.Models;
using EghalTech.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace EghalTech.Controllers
{
    public class PaymentController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IRepository<Order> orderRepository;

        public PaymentController(UserManager<User> _userManager,
            IRepository<Order> _orderRepository)
        {
            userManager = _userManager;
            orderRepository = _orderRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            var user = await userManager.GetUserAsync(User);

            var lineItems = new List<SessionLineItemOptions>();

            foreach (var cartItem in user.Cart.CartItems)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(cartItem.Product.Price * 100),
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = cartItem.Product.Name
                        }
                    },
                    Quantity = cartItem.Quantity
                });
            }

            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 10 * 100,
                    Currency = "USD",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Shipping"
                    }
                },
            });

            var options = new SessionCreateOptions
            {
                CustomerEmail = user.Email,
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Payment", null, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Payment", null, Request.Scheme),
                LineItems = lineItems
            };

            var service = new SessionService();
            var session = service.Create(options);
            TempData["Session"] = session.Id;

            return Redirect(session.Url);
        }

        public async Task<IActionResult> Success()
        {
            var service = new SessionService();
            var session = service.Get(TempData["session"]?.ToString());
            if (session.PaymentStatus != "paid")
                return RedirectToAction("Cancel");

            var user = await userManager.GetUserAsync(User);

            var orderItems = new List<OrderItem>();

            foreach (var item in user.Cart.CartItems)
            {
                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price,
                });
                item.Product.StockQuantity--;
            }

            var Order = new Order
            {
                TotalAmount = user.Cart.CartItems.Sum(x => x.Product.Price * x.Quantity),
                Address = user.Address,
                UserId = user.Id,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderItems = orderItems
            };

            orderRepository.Add(Order);
            user.Cart.CartItems.Clear();
            orderRepository.SaveChanges();

            return View();
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Cart");
        }
    }
}
