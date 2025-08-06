using EghalTech.Models;
using EghalTech.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace EghalTech.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IPagedRepository<Category> categoryRepository;

        public CategoryController(IPagedRepository<Category> _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }
        public IActionResult Index(int? page)
        {
            var catList = categoryRepository.GetPaged(page ?? 1);
            return View(catList);
        }
        [HttpGet]
        public IActionResult SearchCategories(string categoryName, int? page)
        {
            var catList = categoryRepository.GetPaged(
                page ?? 1,
                filter: c => c.Name.Contains(categoryName) || string.IsNullOrEmpty(categoryName)
                );

            var model = catList.Select(c => new
            {
                c.Id,
                c.Name,
            });

            return Json(new { Data = model });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(model);
                categoryRepository.SaveChanges();

                TempData["SuccessMessage"] = "Category Added Successfully";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var category = categoryRepository.GetById(id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Update(category);
                categoryRepository.SaveChanges();

                TempData["SuccessMessage"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Delete(int id)
        {
            categoryRepository.Delete(id);
            categoryRepository.SaveChanges();

            TempData["SuccessMessage"] = "Category Has Been Deleted";
            return RedirectToAction("Index");
        }
    }
}
