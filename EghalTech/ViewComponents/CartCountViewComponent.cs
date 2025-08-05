using EghalTech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EghalTech.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly UserManager<User> userManager;

        public CartCountViewComponent(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int count = 0;
            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(HttpContext.User);
                count = user?.Cart?.CartItems?.Count ?? 0;
            }
            return View(count);
        }
    }
}
