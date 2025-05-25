using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mio.ELearning.Core.ViewModels;
using Mio.ELearning.Service.Services;
using System.Threading.Tasks;

namespace Mio.ELearning.AdminSite.Controllers;

[Area("Lesson")]
public class LessonController : Controller
{
    private readonly ILessonService _lessonService;
    private readonly ISectionService _sectionService;

    public LessonController(ILessonService lessonService, ISectionService sectionService)
    {
        _lessonService = lessonService;
        _sectionService = sectionService;
    }

    public async Task<IActionResult> Index(int? sectionId)
    {
        var result = await _lessonService.GetAllLessonsAsync(sectionId);
        if (!result.IsSuccess)
        {
            ViewBag.ErrorMessage = result.Message;
            return View(new List<LessonViewModel>());
        }

        ViewBag.AllSections = await GetSectionSelectList();
        ViewBag.SelectedSectionId = sectionId;

        return View(result.Data ?? new List<LessonViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLessons(int? sectionId)
    {
        var result = await _lessonService.GetAllLessonsAsync(sectionId);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message, data = result.Data ?? new List<LessonViewModel>() });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.AllSections = await GetSectionSelectList();
        return View("Form", new LessonViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(LessonViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errorDetails = ModelState
                .Select(kvp => new { Field = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .Where(e => e.Errors.Any())
                .ToList();
            return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errorDetails });
        }

        model.VideoUrl = await SaveFileAsync(model.VideoFile, "videos");
        model.DocumentUrl = await SaveFileAsync(model.DocumentFile, "documents");

        var result = await _lessonService.AddLessonAsync(model);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.AllSections = await GetSectionSelectList();
        var result = await _lessonService.GetLessonByIdAsync(id);
        if (!result.IsSuccess)
            return NotFound();

        return View("Form", result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(LessonViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errorDetails = ModelState
                .Select(kvp => new { Field = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .Where(e => e.Errors.Any())
                .ToList();
            return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errorDetails });
        }

        model.VideoUrl = await SaveFileAsync(model.VideoFile, "videos", model.VideoUrl);
        model.DocumentUrl = await SaveFileAsync(model.DocumentFile, "documents", model.DocumentUrl);

        var result = await _lessonService.UpdateLessonAsync(model);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _lessonService.DeleteLessonAsync(id);
        return Json(new { success = result.IsSuccess, message = result.Message });
    }

    private async Task<List<SelectListItem>> GetSectionSelectList()
    {
        var result = await _sectionService.GetAllSectionsAsync(null);
        return result.IsSuccess && result.Data != null
            ? result.Data.Select(s => new SelectListItem { Value = s.SectionId.ToString(), Text = s.SectionName }).ToList()
            : new List<SelectListItem>();
    }

    private async Task<string> SaveFileAsync(IFormFile file, string folder, string existingFilePath = null)
    {
        if (file == null || file.Length == 0)
            return existingFilePath;

        if (!string.IsNullOrEmpty(existingFilePath))
        {
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingFilePath.TrimStart('/'));
            if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);
        var directory = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/{folder}/{fileName}";
    }
}