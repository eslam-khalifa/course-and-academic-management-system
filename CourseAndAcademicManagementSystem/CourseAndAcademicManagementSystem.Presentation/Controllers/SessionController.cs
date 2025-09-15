using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels;
using CAMS.BusinessLogic.ViewModels.SessionViewModels;
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
        public async Task<IActionResult> Index(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var sessions = await _sessionService.GetSessionsAsync(search: search, pageNumber: pageNumber, pageSize: pageSize);

            var vm = new SessionListViewModel
            {
                SearchTerm = search,
                PagedCourses = sessions
            };

            return View(vm);
        }
        public async Task<IActionResult> Details(int id)
        {
            var session = await _sessionService.GetSessionByIdAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var sessionVM = new CreatedSessionViewModel
            {
                Courses = await _sessionService.GetCoursesForDropdownAsync(),
                Instructors = await _sessionService.GetInstructorsForDropdownAsync()
            };
            return View(sessionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatedSessionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Courses = await GetCoursesForDropdownAsync();
                vm.Instructors = await GetInstructorsForDropdownAsync();
                return View(vm);
            }

            try
            {
                await _sessionService.CreateSessionAsync(vm);
                TempData["SuccessMessage"] = "Session created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                vm.Courses = await GetCoursesForDropdownAsync();
                vm.Instructors = await GetInstructorsForDropdownAsync();
                return View(vm);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var session = await _sessionService.GetSessionByIdAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            var vm = new UpdatedSessionViewModel
            {
                SessionId = session.SessionId,
                CourseId = session.CourseId,
                Title = session.Title,
                SessionCode = session.SessionCode,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                EnrollmentStartDate = session.EnrollmentStartDate,
                EnrollmentEndDate = session.EnrollmentEndDate,
                Location = session.Location,
                Capacity = session.Capacity,
                InstructorId = session.InstructorId,
                Status = session.Status,
                Mode = session.Mode,
                Courses = await GetCoursesForDropdownAsync(),
                Instructors = await GetInstructorsForDropdownAsync()
            };

            return View(vm);
        }
        public async Task<IActionResult> Edit(UpdatedSessionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Courses = await GetCoursesForDropdownAsync();
                vm.Instructors = await GetInstructorsForDropdownAsync();
                return View(vm);
            }

            try
            {
                await _sessionService.UpdateSessionAsync(vm);
                TempData["SuccessMessage"] = "Session updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                vm.Courses = await GetCoursesForDropdownAsync();
                vm.Instructors = await GetInstructorsForDropdownAsync();
                return View(vm);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _sessionService.DeleteSessionAsync(id);
            if (!result.Success)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = result.Message });
        }
        // Remote Validation for StartDate
        //public async Task<IActionResult> ValidateStartDate(DateTime startDate)
        //{
        //    if (_sessionService.ValidateStartDate(startDate))
        //    {
        //        return Json(true);
        //    }
        //    return Json("Start date cannot be in the past.");
        //}
        //// Remote Validation for EndDate
        //public async Task<IActionResult> ValidateEndDate(DateTime startDate, DateTime endDate)
        //{
        //    if (_sessionService.ValidateEndDate(startDate, endDate))
        //    {
        //        return Json(true);
        //    }
        //    return Json("End date must be after Start Date.");
        //}
    }
}
