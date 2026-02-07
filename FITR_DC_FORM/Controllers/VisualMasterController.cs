using FITR_DC_FORM.Filters;
using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITR_DC_FORM.Controllers
{
    [RoleAuthorize("SUPERADMIN")]
    public class VisualMasterController : Controller
    {
        private readonly IFitrVisualMasterService _service;

        public VisualMasterController(IFitrVisualMasterService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var list = _service.GetAll();
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new FitrVisualMaster());
        }

        [HttpPost]
        public IActionResult Create(FitrVisualMaster model)
        {
            _service.Save(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var model = _service.GetById(id);
            return View("Create", model); // 👈 force reuse
        }

        [HttpPost]
        public IActionResult Edit(FitrVisualMaster model)
        {
            _service.Save(model);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }

}
