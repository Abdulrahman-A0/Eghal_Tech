using EghalTech.Models;
using EghalTech.Repository;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EghalTech.Controllers
{
    public class StoreController : Controller
    {
        private readonly IPagedRepository<Product> productRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly IWishlistRepository wishlistRepository;
        private readonly UserManager<User> userManager;

        public StoreController(IPagedRepository<Product> _productRepository,
            IReviewRepository _reviewRepository,
            IWishlistRepository _wishlistRepository,
            UserManager<User> _userManager)
        {
            productRepository = _productRepository;
            reviewRepository = _reviewRepository;
            wishlistRepository = _wishlistRepository;
            userManager = _userManager;
        }
        public async Task<IActionResult> Index(int? page)
        {
            var productList = productRepository.GetPaged(page ?? 1, pageSize: 9);

            var user = await userManager.GetUserAsync(User);

            var wishListProductIds = new List<int>();

            if (user?.WishList != null)
            {
                wishListProductIds = user.WishList.WishlistItems.Select(i => i.ProductId).ToList();
            }

            var viewModel = new ProductCardViewModel
            {
                Products = productList,
                WishListProductIds = wishListProductIds
            };

            return View(viewModel);
        }

        public IActionResult ProductDetails(int id)
        {
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product Not Fount";
                return RedirectToAction("Index");
            }
            List<Review> reviews = reviewRepository.GetByProductId(id);

            var userId = userManager.GetUserId(User);
            var isInWishList = wishlistRepository.Exists(userId, product.Id);

            ProductDetailsViewModel viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = reviews,
                AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0,
                ReviewCount = reviews.Count(),
                IsInWishList = isInWishList
            };
            return View(viewModel);
        }

        public IActionResult Search(string prodName, int? page)
        {
            if (!string.IsNullOrWhiteSpace(prodName))
            {
                prodName = prodName.Trim();
            }

            var products = productRepository.GetPaged
            (
                page ?? 1,
                pageSize: 9,
                filter: p => p.Name.Contains(prodName) || prodName == null
            );

            var userId = userManager.GetUserId(User);
            var user = userManager.Users.FirstOrDefault(u => u.Id == userId);

            var wishListProductIds = new List<int>();
            if (user?.WishList != null)
            {
                wishListProductIds = user.WishList.WishlistItems.Select(i => i.ProductId).ToList();
            }

            var viewModel = new ProductCardViewModel
            {
                Products = products,
                WishListProductIds = wishListProductIds
            };

            TempData["prodName"] = prodName;
            return View("Index", viewModel);
        }
    }
}
