using EghalTech.Models;
using EghalTech.Repository;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EghalTech.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository<Cart> cartRepository;
        private readonly UserManager<User> userManager;
        private readonly IRepository<CartItem> cartItemRepository;
        private readonly IWishlistRepository wishlistRepository;

        public CartController(IRepository<Cart> _cartRepository,
            UserManager<User> _userManager,
            IRepository<CartItem> cartItemRepository,
            IWishlistRepository _wishlistRepository)
        {
            cartRepository = _cartRepository;
            userManager = _userManager;
            this.cartItemRepository = cartItemRepository;
            wishlistRepository = _wishlistRepository;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            List<CartItem> cartItems = new List<CartItem>();
            if (user?.Cart != null)
            {
                cartItems = user.Cart.CartItems?.ToList();
            }
            decimal subtotal = cartItems?.Sum(c => c.Product.Price * c.Quantity) ?? 0;
            decimal shipping = 10;
            decimal total = subtotal + shipping;

            var wishListProductIds = new List<int>();
            if (user?.WishList != null)
            {
                wishListProductIds = user.WishList.WishlistItems.Select(i => i.ProductId).ToList();
            }

            var viewModel = new CartViewModel
            {
                CartItems = cartItems,
                Subtotal = subtotal,
                Shipping = shipping,
                Total = total,
                WishListProductIds = wishListProductIds
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int prodId)
        {
            var user = await userManager.GetUserAsync(User);

            if (user!.Cart == null)
            {
                user.Cart = new Cart()
                {
                    UserId = user.Id
                };
                cartRepository.Add(user.Cart);
                cartRepository.SaveChanges();
            }

            var existingItem = user.Cart.CartItems.FirstOrDefault(c => c.ProductId == prodId);

            if (existingItem != null)
            {
                ++existingItem.Quantity;
                cartItemRepository.Update(existingItem);
                cartItemRepository.SaveChanges();
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = prodId,
                    CartId = user.Cart.Id,
                    Quantity = 1
                };

                cartItemRepository.Add(cartItem);
                cartItemRepository.SaveChanges();
            }
            TempData["SuccessMessage"] = $"Added to cart";
            return Json(new { count = user.Cart.CartItems.Count, message = TempData["SuccessMessage"] });
        }

        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var cartItem = cartItemRepository.GetById(cartItemId);
            cartItem.Quantity = quantity;
            cartItemRepository.Update(cartItem);
            cartItemRepository.SaveChanges();

            var user = await userManager.GetUserAsync(User);

            var summary = GetCartSummary(user);

            return Json(new { summary, price = cartItem.Product.Price * quantity });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int cartItemId)
        {
            cartItemRepository.Delete(cartItemId);
            cartItemRepository.SaveChanges();

            var user = await userManager.GetUserAsync(User);

            var summary = GetCartSummary(user);

            return Json(new { summary, cartItemId });
        }

        private object GetCartSummary(User user)
        {
            var cartItems = user.Cart.CartItems.ToList();

            decimal subtotal = cartItems?.Sum(c => c.Product.Price * c.Quantity) ?? 0;
            decimal shipping = 10;
            decimal total = subtotal + shipping;

            return new
            {
                CartCount = cartItems.Count,
                SubTotal = subtotal,
                Total = total,
            };
        }
    }
}
