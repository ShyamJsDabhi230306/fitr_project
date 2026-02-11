using FITR_DC_FORM.Filters;
using FITR_DC_FORM.Models;
using FITR_DC_FORM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITR_DC_FORM.Controllers
{
    //[RoleAuthorize("SUPERADMIN")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly ILocationService _locationService;

        public UserController(
            IUserService userService,
            ICompanyService companyService,
            ILocationService locationService)
        {
            _userService = userService;
            _companyService = companyService;
            _locationService = locationService;
        }

        // ================= LIST =================
        public IActionResult Index()
        {
            ViewBag.CompanyList = _companyService.GetAll() ?? new List<CompanyMaster>();
            ViewBag.LocationList = _locationService.GetAll() ?? new List<LocationMaster>();

            var list = _userService.GetAll() ?? new List<UserMaster>();
            return View(list);
        }

        // ================= CREATE / EDIT (GET) =================
        public IActionResult Create(int? id)
        {
            ViewBag.CompanyList = _companyService.GetAll() ?? new List<CompanyMaster>();
            ViewBag.LocationList = _locationService.GetAll() ?? new List<LocationMaster>();

            if (id.HasValue && id.Value > 0)
            {
                var user = _userService.GetById(id.Value);
                if (user != null)
                {
                    return View(user);
                }
            }

            return View(new UserMaster());
        }

        // ================= CREATE / UPDATE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserMaster model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CompanyList = _companyService.GetAll();
                ViewBag.LocationList = _locationService.GetAll();
                return View(model);
            }

            // 🔥 Handle Signature Upload
            if (model.SignatureFile != null && model.SignatureFile.Length > 0)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/signatures");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string fileName = $"sign_{Guid.NewGuid()}{Path.GetExtension(model.SignatureFile.FileName)}";
                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    model.SignatureFile.CopyTo(stream);
                }

                model.SignaturePath = "/uploads/signatures/" + fileName;
                model.SignedOn = DateTime.Now;
            }
            else if (model.UserId > 0)
            {
                // Preserve existing signature if not changing
                var existing = _userService.GetById(model.UserId);
                if (existing != null)
                {
                    model.SignaturePath = existing.SignaturePath;
                    model.SignedOn = existing.SignedOn;
                }
            }

            _userService.Save(model);
            return RedirectToAction(nameof(Index));
        }


        // ================= DELETE =================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult GetLocationsByCompany(int companyId)
        {
            var locations = _locationService.GetByCompany(companyId);
            return Json(locations);
        }



    }
}
