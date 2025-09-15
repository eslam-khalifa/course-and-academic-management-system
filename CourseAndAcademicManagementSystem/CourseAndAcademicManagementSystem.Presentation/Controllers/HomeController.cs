using System.Diagnostics;
using CourseAndAcademicManagementSystem.Presentation.Models;
using DataAccessLayer.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace CourseAndAcademicManagementSystem.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LearningPlatformDbContext  context;


        public HomeController(ILogger<HomeController> logger, LearningPlatformDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Courses = context.Courses.Count();
            ViewBag.Users = context.Users.Count();
            ViewBag.Sessions = context.Sessions.Count();
            ViewBag.Grades = context.Grades.Count();
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
}
