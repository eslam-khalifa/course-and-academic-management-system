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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CourseController(ICourseService courseService, IWebHostEnvironment webHostEnvironment)
        {
            _courseService = courseService;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Create()
        {
            var createdCourseViewModel = new CreatedCourseViewModel
            {
                Instructors = _courseService.GetInstructorsForDropdownAsync().Result
            };
            return View(createdCourseViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatedCourseViewModel courseVM, IFormFile? thumbnailFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // رفع الصورة لو موجودة
                    if (thumbnailFile != null && thumbnailFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(thumbnailFile.FileName);
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await thumbnailFile.CopyToAsync(stream);
                        }

                        courseVM.ThumbnailUrl = "/images/" + fileName;
                    }

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
            courseVM.Instructors = await _courseService.GetInstructorsForDropdownAsync();

            // العودة لنفس الـ View مع البيانات المدخلة
            return View(courseVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var courseVM = await _courseService.GetCourseByIdAsync(id);

            if (courseVM == null)
                return NotFound();

            courseVM.Instructors = await _courseService.GetInstructorsForDropdownAsync();

            return View(courseVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdatedCourseViewModel courseVM, IFormFile? thumbnailFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // لو فيه صورة جديدة، نرفعها ونخزن مسارها
                    if (thumbnailFile != null && thumbnailFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(thumbnailFile.FileName);
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await thumbnailFile.CopyToAsync(stream);
                        }

                        courseVM.ThumbnailUrl = "/images/" + fileName;
                    }

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
            courseVM.Instructors = await _courseService.GetInstructorsForDropdownAsync();

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
