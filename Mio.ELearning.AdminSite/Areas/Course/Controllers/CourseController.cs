using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mio.ELearning.AdminSite.Models;
using System.Threading.Tasks;
using System.IO;
using Mio.ELearning.Core.ViewModels;
using Mio.ELearning.Service.Services;

namespace Mio.ELearning.AdminSite.Controllers;

[Area("Course")] 
public class CourseController : Controller
{
    private readonly ICourseService _courseService;
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CourseController(ICourseService courseService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment)
    {
        _courseService = courseService;
        _categoryService = categoryService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var result = await _courseService.GetAllCoursesAsync(categoryId);
        if (!result.IsSuccess)
        {
            ViewBag.ErrorMessage = result.Message;
            return View(new List<CourseViewModel>());
        }

        ViewBag.AllCategories = await GetCategorySelectList();
        ViewBag.SelectedCategoryId = categoryId;

        return View(result.Data ?? new List<CourseViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses(int? categoryId)
    {
        var result = await _courseService.GetAllCoursesAsync(categoryId);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message, data = result.Data ?? new List<CourseViewModel>() });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.AllCategories = await GetCategorySelectList();
        return View("Form", new CourseViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errorDetails = ModelState
                .Select(kvp => new { Field = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .Where(e => e.Errors.Any())
                .ToList();
            return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errorDetails });
        }

        // Handle file uploads
        model.ImageUrl = await SaveFileAsync(model.ImageFile, "images");
        foreach (var section in model.Sections)
        {
            foreach (var lesson in section.Lessons)
            {
                lesson.VideoUrl = await SaveFileAsync(lesson.VideoFile, "videos");
                lesson.DocumentUrl = await SaveFileAsync(lesson.DocumentFile, "documents");
            }
        }

        var result = await _courseService.AddCourseAsync(model);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.AllCategories = await GetCategorySelectList();
        var result = await _courseService.GetCourseByIdAsync(id);
        if (!result.IsSuccess)
            return NotFound();

        return View("Form", result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CourseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errorDetails = ModelState
                .Select(kvp => new { Field = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .Where(e => e.Errors.Any())
                .ToList();
            return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errorDetails });
        }

        // Handle file uploads
        model.ImageUrl = await SaveFileAsync(model.ImageFile, "images", model.ImageUrl);
        foreach (var section in model.Sections)
        {
            foreach (var lesson in section.Lessons)
            {
                lesson.VideoUrl = await SaveFileAsync(lesson.VideoFile, "videos", lesson.VideoUrl);
                lesson.DocumentUrl = await SaveFileAsync(lesson.DocumentFile, "documents", lesson.DocumentUrl);
            }
        }

        var result = await _courseService.UpdateCourseAsync(model);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _courseService.DeleteCourseAsync(id);
        return Json(new { success = result.IsSuccess, message = result.Message });
    }

    private async Task<string?> SaveFileAsync(IFormFile? file, string folder, string? existingPath = null)
    {
        if (file == null || file.Length == 0)
            return existingPath; // Return existing path if no new file is uploaded

        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folder);
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Delete old file if exists (for edit scenarios)
        if (!string.IsNullOrEmpty(existingPath))
        {
            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingPath.TrimStart('/'));
            if (System.IO.File.Exists(oldFilePath))
                System.IO.File.Delete(oldFilePath);
        }

        return $"/Uploads/{folder}/{fileName}";
    }

    private async Task<List<SelectListItem>> GetCategorySelectList()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return result.IsSuccess && result.Data != null
            ? result.Data.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }).ToList()
            : new List<SelectListItem>();
    }
}