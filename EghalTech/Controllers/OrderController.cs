using EghalTech.Models;
using EghalTech.Repository;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EghalTech.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IPagedRepository<Order> orderRepository;
        private readonly UserManager<User> userManager;

        public OrderController(IPagedRepository<Order> _orderRepository,
            UserManager<User> _userManager)
        {
            orderRepository = _orderRepository;
            userManager = _userManager;
        }

        public IActionResult Index(int? page)
        {
            IPagedList<Order> orderList;
            var pageNumber = page ?? 1;
            if (User.IsInRole("Admin"))
            {
                orderList = orderRepository.GetPaged(pageNumber);
            }
            else
            {
                var userId = userManager.GetUserId(User);
                orderList = orderRepository.GetPaged
                    (
                        pageNumber,
                        filter: o => o.UserId == userId
                    );
            }
            return View(orderList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateStatus(int ordId, string status)
        {
            var order = orderRepository.GetById(ordId);
            if (order == null)
            {
                return NotFound();
            }

            if (Enum.TryParse<OrderStatus>(status, true, out var newStatus))
            {
                order.Status = newStatus;
                orderRepository.Update(order);
                orderRepository.SaveChanges();

                TempData["SuccessMessage"] = $"Order is {status}";
                return Json(new { message = TempData["SuccessMessage"] });
            }

            return BadRequest("Invalid status value");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int ordId)
        {
            orderRepository.Delete(ordId);
            orderRepository.SaveChanges();

            TempData["Message"] = $"Order {ordId} has been deleted";
            return Json(new { message = TempData["Message"] });
        }

        public IActionResult Details(int Id)
        {
            var order = orderRepository.GetById
                (
                    Id,
                    o => o.User,
                    o => o.OrderItems
                );

            var viewModel = new OrderDetailsViewModel
            {
                Id = order.Id,
                CustomerName = order.User.Name,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductName = oi.Product.Name,
                    ProductBrand = oi.Product.Brand,
                    ImageUrl = oi.Product.ImageUrl,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList(),
                Subtotal = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                Shipping = 10
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Cancel(int ordId)
        {
            var order = orderRepository.GetById(ordId);
            order.Status = OrderStatus.Cancelled;
            orderRepository.Update(order);
            orderRepository.SaveChanges();

            TempData["Message"] = "Order has been cancelled";
            return Json(new { message = TempData["Message"] });
        }
    }
}
