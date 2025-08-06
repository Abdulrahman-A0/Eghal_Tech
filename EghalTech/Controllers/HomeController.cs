using EghalTech.Models;
using EghalTech.Repository;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using X.PagedList.Extensions;

namespace EghalTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPagedRepository<Category> categoryRepository;
        private readonly IProductRepository productRepository;
        private readonly UserManager<User> userManager;

        public HomeController(ILogger<HomeController> logger,
            IPagedRepository<Category> _categoryRepository,
            IProductRepository _productRepository,
            UserManager<User> _userManager)
        {
            _logger = logger;
            categoryRepository = _categoryRepository;
            productRepository = _productRepository;
            userManager = _userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            var wishListProductIds = new List<int>();

            if (user?.WishList != null)
            {
                wishListProductIds = user.WishList.WishlistItems.Select(i => i.ProductId).ToList();
            }

            var featuredProducts = productRepository.GetFeaturedProducts(6);

            var productCards = featuredProducts.Select(p => new ProductCard
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryName = p.Category.Name,
                AverageRating = p.Reviews != null && p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = p.Reviews?.Count ?? 0,
            }).ToPagedList();

            HomeViewModel viewModel = new HomeViewModel
            {
                Categories = categoryRepository.GetAll(),
                FeaturedProducts = new ProductCardViewModel
                {
                    Products = productCards,
                    WishListProductIds = wishListProductIds
                }
            };

            return View(viewModel);
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
