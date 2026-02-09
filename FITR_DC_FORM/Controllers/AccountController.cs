using FITR_DC_FORM.Models.ViewModels;
using FITR_DC_FORM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITR_DC_FORM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _authService.Login(model.UserName, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.UserRole);

            HttpContext.Session.SetInt32("CompanyId", user.CompanyId);
            HttpContext.Session.SetInt32("LocationId", user.LocationId);

            HttpContext.Session.SetString("CompanyName", user.CompanyName);
            HttpContext.Session.SetString("LocationName", user.LocationName);

            HttpContext.Session.SetString("SignaturePath", user.SignaturePath ?? "");

            return RedirectToAction("List", "Fitr");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
