using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mio.ELearning.AdminSite.Models;
using Mio.ELearning.Service.Services;

namespace Mio.ELearning.AdminSite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryService _categoryService;

    public HomeController(ILogger<HomeController> logger, ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        // var result = await _categoryService.GetAllCategoriesAsync();
        //
        // if (result.IsSuccess && result.Data != null)
        // {
        //     ViewBag.AllCategories = result.Data
        //         .Select(c => new SelectListItem
        //         {
        //             Value = c.CategoryId.ToString(),
        //             Text = c.Name
        //         })
        //         .ToList();
        // }

        return View();
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