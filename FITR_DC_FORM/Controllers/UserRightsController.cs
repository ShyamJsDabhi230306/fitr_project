using FITR_DC_FORM.Models;
using FITR_DC_FORM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITR_DC_FORM.Controllers
{
    public class UserRightsController : Controller
    {
        private readonly IUserPageRightService _service;
        private readonly IUserService _userService;

        public UserRightsController(
            IUserPageRightService service,
            IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        public IActionResult Index(int userId = 0)
        {
            ViewBag.Users = _userService.GetAll();

            if (userId == 0)
                return View(new List<UserPageRight>());

            var rights = _service.GetRightsByUser(userId);

            ViewBag.SelectedUser = userId;

            return View(rights);
        }

        [HttpPost]
        public IActionResult Update(List<UserPageRight> rights, int userId)
        {
            if (rights != null && rights.Any())
            {
                _service.BulkUpdateRights(rights);
                TempData["SuccessMessage"] = "User rights updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "No changes to save.";
            }

            return RedirectToAction("Index", new { userId = userId });
        }

    }
}
