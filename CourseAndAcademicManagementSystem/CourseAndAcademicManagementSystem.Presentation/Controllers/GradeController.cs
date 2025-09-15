using CAMS.BusinessLogic.Services.Classes;
using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels.GradeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CAMS.Presentation.Controllers
{
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        public async Task<IActionResult> Index(string? traineeName, int? sessionId, int? traineeId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _gradeService.GetGradesAsync(
                TraineeName: traineeName,
                sessionId: sessionId,
                traineeId: traineeId,
                pageNumber: pageNumber,
                pageSize: pageSize
            );

            var model = new GradeListViewModel
            {
                PagedGrades = result,
                SearchTerm = traineeName
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
                return NotFound();

            return View(grade);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreatedGradeViewModel
            {
                Sessions = await _gradeService.GetSessionDropDownAsync(),
                Trainees = await _gradeService.GetTraineeDropDownAsync()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatedGradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _gradeService.CreatedGradeAsync(model);
            TempData["Success"] = "Grade created successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
                return NotFound();

            var model = new UpdatedGradeViewModel
            {
                GradeId = grade.GradeId,
                SessionId = grade.SessionId,
                TraineeId = grade.TraineeId,
                Value = grade.Value,
                Weight = grade.Weight,
                AttemptNumber = grade.AttemptNumber,
                IsFinal = grade.IsFinal,
                Comments = grade.Comments,
                Sessions = await _gradeService.GetSessionDropDownAsync(),
                Trainees = await _gradeService.GetTraineeDropDownAsync()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdatedGradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _gradeService.UpdateGradeAsync(model);
            TempData["Success"] = "Grade updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var grade = await _gradeService.DeleteGradeAsync(id);

            if (grade == null)
                return Json(new { success = false, message = "Course not found" });

            return Json(new { success = true, message = "Course deleted successfully" });
        }
    }
}
