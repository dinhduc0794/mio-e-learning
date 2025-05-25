using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mio.ELearning.Core.ViewModels;
using Mio.ELearning.Service.Services;
using System.Threading.Tasks;

namespace Mio.ELearning.AdminSite.Controllers;

[Area("Section")]
public class SectionController : Controller
{
    private readonly ISectionService _sectionService;
    private readonly ICourseService _courseService;

    public SectionController(ISectionService sectionService, ICourseService courseService)
    {
        _sectionService = sectionService;
        _courseService = courseService;
    }

    public async Task<IActionResult> Index(int? courseId)
    {
        var result = await _sectionService.GetAllSectionsAsync(courseId);
        if (!result.IsSuccess)
        {
            ViewBag.ErrorMessage = result.Message;
            return View(new List<SectionViewModel>());
        }

        ViewBag.AllCourses = await GetCourseSelectList();
        ViewBag.SelectedCourseId = courseId;

        return View(result.Data ?? new List<SectionViewModel>());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSections(int? courseId)
    {
        var result = await _sectionService.GetAllSectionsAsync(courseId);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message, data = result.Data ?? new List<SectionViewModel>() });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.AllCourses = await GetCourseSelectList();
        return View("Form", new SectionViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(SectionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errorDetails = ModelState
                .Select(kvp => new { Field = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .Where(e => e.Errors.Any())
                .ToList();
            return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errorDetails });
        }

        var result = await _sectionService.AddSectionAsync(model);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.AllCourses = await GetCourseSelectList();
        var result = await _sectionService.GetSectionByIdAsync(id);
        if (!result.IsSuccess)
            return NotFound();

        return View("Form", result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(SectionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errorDetails = ModelState
                .Select(kvp => new { Field = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .Where(e => e.Errors.Any())
                .ToList();
            return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errorDetails });
        }

        var result = await _sectionService.UpdateSectionAsync(model);
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new { success = true, message = result.Message });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sectionService.DeleteSectionAsync(id);
        return Json(new { success = result.IsSuccess, message = result.Message });
    }

    private async Task<List<SelectListItem>> GetCourseSelectList()
    {
        var result = await _courseService.GetAllCoursesAsync(null);
        return result.IsSuccess && result.Data != null
            ? result.Data.Select(c => new SelectListItem { Value = c.CourseId.ToString(), Text = c.CourseName }).ToList()
            : new List<SelectListItem>();
    }
}