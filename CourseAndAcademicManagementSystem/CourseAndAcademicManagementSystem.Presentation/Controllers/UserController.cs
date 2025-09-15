using CAMS.BusinessLogic.Services.Classes;
using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CAMS.Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IActionResult> Index(string? search, string? searchRole, int pageNumber = 1, int pageSize = 10)
        {
            var userVM = await userService.GetUsersAsync(role: searchRole, search: search, pageNumber: pageNumber, pageSize: pageSize);

            return View(userVM);
            /*
             userVM -> Users , SearchName, SearchRole, CurrentPage, TotalPages
             */
        }
        public IActionResult Create()
        {
            return View(new UserViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                await userService.CreateUserAsync(userVM);
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index");
            }

            return View(userVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            var userVM = new UserViewModel
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                State = user.State,
            };
            return View(userVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                await userService.UpdateUserAsync(userVM);
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return Json(new { success = false, message = "Error while deleting user" });
            }

            return Json(new { success = true, message = "User deleted successfully" });
        }
        // Remote validation for unique email
        public async Task<IActionResult> IsEmailUnique(string email, int userId)
        {
            bool isUnique = await userService.IsEmailUniqueAsync(email, userId);

            if (!isUnique)
            {
                return Json($"Email '{email}' is already taken.");
            }

            return Json(true);
        }
    }
}
