using EghalTech.Models;
using EghalTech.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EghalTech.Controllers
{
    public class WishListController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IRepository<WishList> wishListRepository;
        private readonly IRepository<WishListItem> wishItemRepository;

        public WishListController(UserManager<User> _userManager,
            IRepository<WishList> _wishListRepository,
            IRepository<WishListItem> _wishItemRepository)
        {
            userManager = _userManager;
            wishListRepository = _wishListRepository;
            wishItemRepository = _wishItemRepository;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var wishListItems = user?.WishList?.WishlistItems?.ToList() ?? new List<WishListItem>();

            return View(wishListItems);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ToggleWishList(int prodId)
        {
            var user = await userManager.GetUserAsync(User);

            if (user.WishList == null)
            {
                user.WishList = new WishList()
                {
                    UserId = user.Id,
                };
                wishListRepository.Add(user.WishList);
                wishListRepository.SaveChanges();
            }

            var existingItem = user.WishList.WishlistItems.FirstOrDefault(i => i.ProductId == prodId);

            if (existingItem != null)
            {
                wishItemRepository.Delete(existingItem.Id);
            }
            else
            {
                wishItemRepository.Add(new WishListItem
                {
                    ProductId = prodId,
                    WishlistID = user.WishList.Id
                });
            }
            wishItemRepository.SaveChanges();

            return Json(new { isInWishList = existingItem == null });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            wishItemRepository.Delete(id);
            wishItemRepository.SaveChanges();

            var user = await userManager.GetUserAsync(User);

            var wishListItems = user?.WishList?.WishlistItems;

            int totalItems = wishListItems?.Count() ?? 0;
            decimal totalValue = wishListItems?.Sum(i => i.Product.Price) ?? 0;
            int inStock = wishListItems?.Count(x => x.Product.StockQuantity > 0) ?? 0;

            return Json(new
            {
                itemId = id,
                itemsCount = totalItems,
                itemsPrice = totalValue,
                itemsStock = inStock
            });
        }

        [Authorize]
        public async Task<IActionResult> HasItems()
        {
            var user = await userManager.GetUserAsync(User);
            bool hasItems = user?.WishList?.WishlistItems?.Any() ?? false;
            return Json(new { hasItems });
        }
    }
}
