using EghalTech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EghalTech.ViewComponents
{
    public class WishListViewComponent : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public WishListViewComponent(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            bool hasWishListItems = user?.WishList?.WishlistItems?.Any() ?? false;

            return View(hasWishListItems);
        }
    }
}
