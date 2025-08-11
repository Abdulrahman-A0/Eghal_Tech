using EghalTech.Models;
using EghalTech.Repository;
using EghalTech.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace EghalTech.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IPagedRepository<Category> categoryRepository;

        public ProductController(IProductRepository _productRepository,
            IPagedRepository<Category> _categoryRepository)
        {
            productRepository = _productRepository;
            categoryRepository = _categoryRepository;
        }

        public IActionResult Index(int? page)
        {
            var productList = productRepository.GetPaged(
                page: page ?? 1,
                includes: p => p.Category
                );

            return View(productList);
        }

        [HttpGet]
        public IActionResult SearchProducts(string productName, int? page)
        {
            var productList = productRepository.GetPaged(
                filter: p => string.IsNullOrEmpty(productName) || p.Name.Contains(productName),
                page: page ?? 1,
                includes: p => p.Category
            );

            var result = productList.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                ImageUrl = $"/img/product/{p.ImageUrl}",
                brand = p.Brand,
                price = p.Price,
                stockQuantity = p.StockQuantity,
                categoryName = p.Category.Name
            });

            return Json(new { data = result });
        }

        public IActionResult ValidatePriceInput(decimal price)
        {
            if (price > 0)
                return Json(true);
            return Json(false);
        }
        public IActionResult ValidateStockQtyInput(int stockQuantity)
        {
            if (stockQuantity > 0)
                return Json(true);
            return Json(false);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            Product model = new Product();
            ViewBag.Categories = categoryRepository.GetAll();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Product product, IFormFile image)
        {
            if (image?.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetFileName(image.FileName);
                var filePath = Path.Combine("wwwroot/img/product", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                product.ImageUrl = fileName;

                productRepository.Add(product);
                productRepository.SaveChanges();

                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = categoryRepository.GetAll();
            return View(product);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product Not Found";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = categoryRepository.GetAll();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile image)
        {
            Product oldProduct = productRepository.GetById(product.Id);

            if (image?.Length > 0)
            {
                var oldImagePath = Path.Combine($"wwwroot/img/product", oldProduct.ImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                var filename = Guid.NewGuid() + Path.GetFileName(image.FileName);
                var filePath = Path.Combine("wwwroot/img/product", filename);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                product.ImageUrl = filename;
            }
            else
            {
                product.ImageUrl = oldProduct.ImageUrl;
            }
            oldProduct.Name = product.Name;
            oldProduct.Brand = product.Brand;
            oldProduct.Category = product.Category;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;
            oldProduct.StockQuantity = product.StockQuantity;
            oldProduct.ImageUrl = product.ImageUrl;

            productRepository.Update(oldProduct);
            productRepository.SaveChanges();

            TempData["SuccessMessage"] = "Product updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var imagePath = Path.Combine("wwwroot/img/product", product.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            productRepository.Delete(id);
            productRepository.SaveChanges();

            TempData["SuccessMessage"] = $"Product '{product.Name}' has been deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
