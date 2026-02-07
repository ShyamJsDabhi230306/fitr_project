using FITR_DC_FORM.Filters;
using FITR_DC_FORM.Models;
using FITR_DC_FORM.Services.Interfaces;
using FITR_DC_FORM.Services.Repo_Services;
using Microsoft.AspNetCore.Mvc;
[RoleAuthorize("SUPERADMIN")]
public class LocationController : Controller
{
    private readonly ILocationService _src;
    private readonly ICompanyService _com;

    public LocationController(ILocationService src, ICompanyService ComSrc)
    {
        _src = src;
        _com = ComSrc;
    }
    public IActionResult Index()
    {
        return View(_src.GetAll());
    }
    public IActionResult Create()
    {
        ViewBag.CompanyList = _com.GetAll();
        return View(new LocationMaster());
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(LocationMaster model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .Where(m => !string.IsNullOrWhiteSpace(m))
                                   .ToList();

            throw new Exception(
                errors.Any()
                    ? string.Join(" | ", errors)
                    : "ModelState invalid but no error messages found"
            );
        }


        try
        {
            _src.Save(model);
        }
        catch (Exception ex)
        {
            // 🔥 THIS WILL EXPOSE THE REAL PROBLEM
            ModelState.AddModelError("", ex.Message);

            ViewBag.CompanyList = _com.GetAll();
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        ViewBag.CompanyList = _com.GetAll();
        return View("Create", _src.GetById(id));
    }

    public IActionResult Delete(int id)
    {
        _src.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    

}
