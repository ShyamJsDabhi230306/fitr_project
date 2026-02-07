using FITR_DC_FORM.Models;
using FITR_DC_FORM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITR_DC_FORM.Controllers
{
    public class PrintCompanyController : Controller
    {
        private readonly IPrintCompanyService _service;

        public PrintCompanyController(IPrintCompanyService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View(_service.GetAll());
        }

        public IActionResult Create()
        {
            return View(new PrintCompanyModel { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PrintCompanyModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // HANDLE FILE UPLOAD (OPTIONAL)
            if (model.CompanyLogoFile != null && model.CompanyLogoFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads"
                );

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString()
                                  + Path.GetExtension(model.CompanyLogoFile.FileName);

                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.CompanyLogoFile.CopyTo(stream);
                }

                model.CompanyLogo = fileName;
            }

            _service.Save(model);

            // PRG PATTERN → NO REFRESH WARNING
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View("Create", _service.GetById(id));
        }

        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }

}
