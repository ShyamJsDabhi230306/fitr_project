using FITR_DC_FORM.Models;
using FITR_DC_FORM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITR_DC_FORM.Controllers
{
    public class UserRightsController : Controller
    {
        private readonly IUserPageRightService _rightsService;
        private readonly IUserService _userService;

        public UserRightsController(
            IUserPageRightService rightsService,
            IUserService userService)
        {
            _rightsService = rightsService;
            _userService = userService;
        }

        // ✅ Load screen
        public IActionResult Index(int userId = 0)
        {
            // Load all users for dropdown
            ViewBag.Users = _userService.GetAll();
            ViewBag.SelectedUser = userId;

            if (userId == 0)
                return View(new List<UserPageRight>());

            // Load selected user rights
            var rights = _rightsService.GetRightsByUser(userId);

            return View(rights);
        }

        // ✅ Save updated rights
        [HttpPost]
        public IActionResult Update(List<UserPageRight> rights, int userId)
        {
            if (rights != null && rights.Any())
            {
                _rightsService.BulkUpdateRights(rights);
                TempData["SuccessMessage"] = "User rights updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "No changes detected.";
            }

            return RedirectToAction("Index", new { userId = userId });
        }
    }
}
