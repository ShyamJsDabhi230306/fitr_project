using FITR_DC_FORM.Filters;
using FITR_DC_FORM.Models;
using FITR_DC_FORM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITR_DC_FORM.Controllers
{
    [RoleAuthorize("SUPERADMIN")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // LIST PAGE
        public IActionResult Index()
        {
            var list = _companyService.GetAll();
            return View(list);
        }

        // CREATE / EDIT PAGE
        public IActionResult Create(int id = 0)
        {
            if (id > 0)
            {
                var model = _companyService.GetById(id);
                return View(model);
            }

            return View(new CompanyMaster());
        }

        // SAVE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CompanyMaster model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _companyService.Save(model);
            return RedirectToAction("Index");
        }

        // DELETE
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _companyService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
