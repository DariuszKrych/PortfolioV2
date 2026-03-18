using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dariusz_Krych___Portfolio.Models;

namespace Dariusz_Krych___Portfolio.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

public IActionResult PythonProjects()
    {
        // 1. Create a list of dummy data (Later, this could come from a database!)
        var myPythonProjects = new List<Github_Project>
        {
            new Github_Project { 
                Id = 1, 
                Title = "Bookbinding Signature Creator", 
                Description = "Takes an input of a 2 column pdf book file. Can add numbering to the pages. Cuts the pages in hald and shuffles them and divided them into seperate signatures so that when they are printed they can be directly bound into a book.", 
                GitHubLink = "https://github.com/DariuszKrych/Bookbinding_Signature_Creator" 
            },
            new Github_Project { 
                Id = 2, 
                Title = "Real Time Traffic Light Simulation System", 
                Description = "Realtime simulation of traffic lights with the SDL3 library using the PySDL3 wrapper. Written in Python.", 
                GitHubLink = "https://github.com/DariuszKrych/Real-Time-Traffic-Light-Simulation-System" 
            },
            new Github_Project { 
                Id = 3, 
                Title = "Py-Jv String Matching", 
                Description = "Implements various string matching algorithms and applies them onto data extracted from a steam reviews database.", 
                GitHubLink = "https://github.com/DariuszKrych/Py-Jv_StringMatching" 
            }
        };

        // 2. Pass the data into the View
        return View(myPythonProjects); 
    }

        public IActionResult CSharpProjects()
    {
        return View();
    }
        public IActionResult Education()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
