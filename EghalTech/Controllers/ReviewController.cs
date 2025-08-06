using EghalTech.Models;
using EghalTech.Repository;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace EghalTech.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IPagedRepository<Review> reviewRepository;

        public ReviewController(UserManager<User> _userManager,
            IPagedRepository<Review> _reviewRepository)
        {
            userManager = _userManager;
            reviewRepository = _reviewRepository;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var user = await userManager.GetUserAsync(User);

            var reviews = reviewRepository.GetPaged
                (
                    page ?? 1,
                    pageSize: 8,
                    filter: r => r.UserId == user.Id,
                    includes: r => r.Product
                );

            var viewModel = reviews.Select(r => new ReviewViewModel
            {
                Id = r.Id,
                ProductId = r.ProductId,
                ProductName = r.Product.Name,
                Comment = r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt
            });

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ReviewFormViewModel reviewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);
                var user = userManager.Users.FirstOrDefault(u => u.Id == userId);

                var review = new Review
                {
                    UserId = userId,
                    ProductId = reviewModel.ProductId,
                    Rating = reviewModel.Rating,
                    Comment = reviewModel.Comment,
                    CreatedAt = DateTime.Now,
                    User = user
                };

                reviewRepository.Add(review);
                reviewRepository.SaveChanges();

                return Json(new
                {
                    success = true,
                    review = new
                    {
                        userName = user.Name,
                        rating = review.Rating,
                        comment = review.Comment,
                        date = review.CreatedAt.ToString("MMM dd, yyyy")
                    }
                });
            }

            var errors = ModelState
            .Where(kvp => kvp.Value.Errors.Any())
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return BadRequest(new { success = false, errors });
        }

        [HttpPost]
        public IActionResult Delete(int reviewId)
        {
            reviewRepository.Delete(reviewId);
            reviewRepository.SaveChanges();

            return Json(new { reviewId });
        }
    }
}
