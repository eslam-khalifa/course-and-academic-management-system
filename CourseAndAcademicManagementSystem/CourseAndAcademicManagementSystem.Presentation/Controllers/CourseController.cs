using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels;
using CAMS.BusinessLogic.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace CAMS.Presentation.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public IUserService UserService { get; }

        public CourseController(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            UserService = userService;
        }
        public async Task<IActionResult> Index(string? searchTerm, string? category, int pageNumber = 1, int pageSize = 10)
        {
            var courseListViewModel = await _courseService.GetCoursesAsync(searchTerm, pageNumber, pageSize);

            return View(courseListViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        public async Task<IActionResult> Create()
        {
            var ins = await UserService.GetAllAsync(new QueryUser { Role = "Instructor" });

            var createdCourseViewModel = new CreatedCourseViewModel
            {
                Instructors = ins.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View(createdCourseViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatedCourseViewModel courseVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // استدعاء الخدمة لإنشاء الكورس
                    await _courseService.CreateCourseAsync(courseVM);

                    // رسالة نجاح
                    TempData["SuccessMessage"] = "Course created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // تسجيل الخطأ
                    ModelState.AddModelError("", "An error occurred while creating the course. Please try again.");
                    // ممكن في الـ logs تسجل تفاصيل الخطأ الفعلية ex.Message
                }
            }

            // مهم جدًا: إعادة تحميل قائمة الـ instructors في حالة الخطأ
            courseVM.Instructors = await UserService.GetAllAsync(new QueryUser { Role = "Instructor" })
                .ContinueWith(t => t.Result.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }));

            // العودة لنفس الـ View مع البيانات المدخلة
            return View(courseVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var courseVM = await _courseService.GetCourseByIdAsync(id);

            if (courseVM == null)
                return NotFound();

            var instructors = await UserService.GetAllAsync(new QueryUser { Role = "Instructor" });


            var updatedCourseVM = new UpdatedCourseViewModel
            {
                CourseId = courseVM.CourseId,
                Name = courseVM.Name,
                Code = courseVM.Code,
                Category = courseVM.Category,
                InstructorId = courseVM.InstructorId,
            };
            updatedCourseVM.Instructors = instructors.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(updatedCourseVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdatedCourseViewModel courseVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                  
                    // تحديث بيانات الكورس
                    await _courseService.UpdateCourseAsync(courseVM);

                    TempData["SuccessMessage"] = "Course updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // رسالة خطأ عامة للمستخدم
                    ModelState.AddModelError("", "An error occurred while updating the course. Please try again.");
                    // تسجيل التفاصيل الفعلية في الـ logs
                }
            }

            // مهم جدًا: تحميل قائمة الـ Instructors مرة أخرى لو حدث خطأ
            var instructors = await UserService.GetAllAsync(new QueryUser { Role = "Instructor" });

            courseVM.Instructors = instructors.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(courseVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.DeleteCourseAsync(id);

            if (course == null)
                return Json(new { success = false, message = "Course not found" });

            return Json(new { success = true, message = "Course deleted successfully" });
        }

    }
}
