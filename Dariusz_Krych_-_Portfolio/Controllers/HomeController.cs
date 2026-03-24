using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dariusz_Krych___Portfolio.Models;
using Dariusz_Krych___Portfolio.Services;

namespace Dariusz_Krych___Portfolio.Controllers;

public class HomeController : Controller
{
    private readonly IGithubService _githubService;

    public HomeController(IGithubService githubService)
    {
        _githubService = githubService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Education()
    {
        return View();
    }

    public IActionResult WebDevProjects()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetWebDevProjectsData()
    {
        var repoUrls = new List<string>
        {
            "https://github.com/DariuszKrych/Project_Kaida",
            "https://github.com/DariuszKrych/Portfolio",
            "https://github.com/DariuszKrych/PortfolioV2"
        };

        var projects = await _githubService.GetProjectsAsync(repoUrls);
        return Json(projects);
    }

    public IActionResult PythonProjects()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetPythonProjectsData()
    {
        var repoUrls = new List<string>
        {
            "https://github.com/DariuszKrych/Bookbinding_Signature_Creator",
            "https://github.com/DariuszKrych/Real-Time-Traffic-Light-Simulation-System",
            "https://github.com/DariuszKrych/Py-Jv_StringMatching"
        };

        var projects = await _githubService.GetProjectsAsync(repoUrls);
        return Json(projects);
    }

    public IActionResult CSharpProjects()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetCSharpProjectsData()
    {
        var repoUrls = new List<string>
        {
            "https://github.com/Sl11ck/Parking_Panic"
        };

        var projects = await _githubService.GetProjectsAsync(repoUrls);
        return Json(projects);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
