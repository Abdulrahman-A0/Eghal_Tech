using EghalTech.Models;
using EghalTech.Repository;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EghalTech.Controllers
{
    public class ReviewController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IRepository<Review> reviewRepository;

        public ReviewController(UserManager<User> _userManager,
            IRepository<Review> _reviewRepository)
        {
            userManager = _userManager;
            reviewRepository = _reviewRepository;
        }

        [HttpPost]
        [Authorize]
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
    }
}
