using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CAMS.Presentation.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(string searchCourseName, int pageNumber = 1, int pageSize = 5)
        {
            var sessionVM = _sessionService.GetSessionsAsync(searchCourseName, pageNumber, pageSize);
            return View();
            /*
             * All Sessions include Courses And Instructors (Where search condition matches)
             * total pages, current page, search term
             */
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var sessionVM = new SessionViewModel
            {
                Courses = await _sessionService.GetCoursesForDropdownAsync()
                Instructors = await _sessionService.GetInstructorsForDropdownAsync()
            };
            return View(sessionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SessionViewModel sessionVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _sessionService.CreateSessionAsync(sessionVM);
                    TempData["SuccessMessage"] = "Session created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            sessionVM.Courses = await _sessionService.GetCoursesForDropdownAsync();
            sessionVM.Instructors = await _sessionService.GetInstructorsForDropdownAsync();
            return View(sessionVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var sessionVM = await _sessionService.GetSessionByIdAsync(id);
            if (sessionVM == null) return NotFound();

            sessionVM.Courses = await _sessionService.GetCoursesForDropdownAsync();
            sessionVM.Instructors = await _sessionService.GetInstructorsForDropdownAsync();
            return View(sessionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SessionViewModel sessionVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _sessionService.UpdateSessionAsync(sessionVM);
                    TempData["SuccessMessage"] = "Session updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            sessionVM.Courses = await _sessionService.GetCoursesForDropdownAsync();
            sessionVM.Instructors = await _sessionService.GetInstructorsForDropdownAsync();
            return View(sessionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            
            var deleted = await _sessionService.DeleteSessionAsync(id);
            if (!deleted)
            {
                return Json(new { success = false, message = "Error while deleting session" });
            }

            return Json(new { success = true, message = "Session deleted successfully" });
        }
        // Remote Validation for StartDate
        public async Task<IActionResult> ValidateStartDate(DateTime startDate)
        {
            if (_sessionService.ValidateStartDate(startDate))
            {
                return Json(true);
            }
            return Json("Start date cannot be in the past.");
        }
        // Remote Validation for EndDate
        public async Task<IActionResult> ValidateEndDate(DateTime startDate, DateTime endDate)
        {
            if (_sessionService.ValidateEndDate(startDate, endDate))
            {
                return Json(true);
            }
            return Json("End date must be after Start Date.");
        }
    }
}
