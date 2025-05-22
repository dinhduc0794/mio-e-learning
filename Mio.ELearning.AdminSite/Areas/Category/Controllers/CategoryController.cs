using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mio.ELearning.Core.ViewModels;
using Mio.ELearning.Service.Services;

namespace Mio.ELearning.AdminSite.Controllers;
[Area("Category")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        if (!result.IsSuccess)
        {
            ViewBag.ErrorMessage = result.Message;
            return View(new CategoryViewModel());
        }

        var viewModel = new CategoryViewModel()
        {
            RecordCount = result.Data?.Count ?? 0
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }
        return Json(new { data = result.Data });
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);
        if (!result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }

        return View(result.Data);
    }

    public async Task<IActionResult> Form(int id)
    {
        CategoryViewModel viewModel = new CategoryViewModel { IsEdit = id > 0 };

        if (viewModel.IsEdit)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound();
            }
            viewModel = result.Data;
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }

        var result = await _categoryService.CreateCategoryAsync(categoryViewModel);
        if (result.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return Json(new { success = false, message = result.Message });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryViewModel categoryViewModel)
    {
        if (categoryViewModel.CategoryId == 0)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }

        var result = await _categoryService.UpdateCategoryAsync(categoryViewModel);
        if (result.IsSuccess && result.Data != null)
        {
            return RedirectToAction(nameof(Index));
        }

        return Json(new { success = false, message = result.Message });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);
        if (!result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }

        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            return Json(new { success = result.IsSuccess, message = result.Message });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
        }
    }
}