//using FITR_DC_FORM.Helpers;
//using FITR_DC_FORM.Models;
//using FITR_DC_FORM.Models.ViewModels;
//using FITR_DC_FORM.Services.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//namespace FITR_DC_FORM.Controllers
//{
//    public class FitrController : Controller
//    {
//        private readonly IFitrService _service;
//        private const string FITR_SESSION_KEY = "FITR_WIZARD";

//        public FitrController(IFitrService service)
//        {
//            _service = service;
//        }

//        // ================= START NEW FITR =================
//        public IActionResult New()
//        {
//            HttpContext.Session.RemoveObject(FITR_SESSION_KEY);

//            return RedirectToAction("Index", new
//            {
//                tab = "basic",
//                mode = "create"
//            });

//        }

//        // ================= LOAD PAGE =================
//        //public IActionResult Index(string tab = "basic", string mode = "")
//        //{
//        //    ViewBag.ActiveTab = tab;
//        //    ViewBag.IsCreate = mode == "create";
//        //    ViewBag.DcList = _service.GetDcList();

//        //    var model = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

//        //    if (model == null)
//        //    {
//        //        model = new FitrViewModel
//        //        {
//        //            Master = new FitrMaster()
//        //        };
//        //    }

//        //    return View(model);
//        //}
//        [HttpGet]
//        public IActionResult Index(int? id, string tab = "basic", string mode = null)
//        {
//            ViewBag.ActiveTab = tab;
//            ViewBag.IsCreate = mode == "create";
//            ViewBag.DcList = _service.GetDcList();

//            FitrViewModel model;

//            // 1️⃣ CREATE MODE
//            if (mode == "create")
//            {
//                model = new FitrViewModel
//                {
//                    Master = new FitrMaster()
//                };
//            }
//            // 2️⃣ SESSION / EDIT MODE
//            else
//            {
//                model = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

//                if (model == null)
//                {
//                    model = id.HasValue
//                        ? _service.GetFitr(id)
//                        : new FitrViewModel { Master = new FitrMaster() };
//                }
//            }

//            // 🔥 3️⃣ PREVIEW GENERATION (THIS IS THE KEY FIX)
//            if (model.Master.TCNo == null)
//            {
//                var preview = _service.GetNextTCPreview();
//                model.Master.PreviewTCNo = preview.tcNo;
//                model.Master.PreviewTCYear = preview.tcYear;
//            }

//            // 4️⃣ STORE BACK TO SESSION
//            HttpContext.Session.SetObject(FITR_SESSION_KEY, model);

//            return View(model);
//        }





//        // ================= BASIC =================
//        //[HttpPost]
//        //public IActionResult SaveBasic(FitrViewModel model)
//        //{
//        //    var sessionModel = new FitrViewModel
//        //    {
//        //        Master = model.Master
//        //    };
//        //    // 🔒 Preserve preview values
//        //    sessionModel.Master.PreviewTCNo = model.Master.PreviewTCNo
//        //                                      ?? sessionModel.Master.PreviewTCNo;

//        //    sessionModel.Master.PreviewTCYear = model.Master.PreviewTCYear
//        //                                        ?? sessionModel.Master.PreviewTCYear;
//        //    HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);

//        //    return RedirectToAction("Index", new { tab = "product" });
//        //}

//        [HttpPost]
//        public IActionResult SaveBasic(FitrViewModel model)
//        {
//            HttpContext.Session.SetObject(FITR_SESSION_KEY, model);
//            return RedirectToAction("Index", new { tab = "product" });
//        }


//        // ================= PRODUCT =================
//        [HttpPost]
//        public IActionResult SaveProduct(FitrViewModel model)
//        {
//            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

//            sessionModel.Master.ProductType = model.Master.ProductType;
//            sessionModel.Master.Model = model.Master.Model;
//            sessionModel.Master.Torque = model.Master.Torque;
//            sessionModel.Master.Ratio = model.Master.Ratio;
//            sessionModel.Master.HandwheelDia = model.Master.HandwheelDia;

//            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);

//            return RedirectToAction("Index", new { tab = "material" });
//        }

//        // ================= MATERIAL =================
//        [HttpPost]
//        public IActionResult SaveMaterial(FitrViewModel model)
//        {
//            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

//            sessionModel.Materials = model.Materials ?? new List<FitrMaterial>();

//            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);

//            return RedirectToAction("Index", new { tab = "sr" });
//        }

//        // ================= SR DATA =================
//        [HttpPost]
//        public IActionResult SaveSrData(FitrViewModel model)
//        {
//            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

//            sessionModel.SrData = model.SrData ?? new List<FitrSrData>();

//            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);

//            return RedirectToAction("Index", new { tab = "visual" });
//        }

//        // ================= VISUAL =================
//        [HttpPost]
//        public IActionResult SaveVisual(FitrViewModel model)
//        {
//            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

//            sessionModel.Master.Tapping = model.Master.Tapping;
//            sessionModel.Master.Hardware = model.Master.Hardware;
//            sessionModel.Master.Painting = model.Master.Painting;
//            sessionModel.Master.ShaftMovement = model.Master.ShaftMovement;

//            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);

//            return RedirectToAction("Index", new { tab = "final" });
//        }

//        // ================= FINAL (🔥 ONLY DB SAVE HERE) =================
//        [HttpPost]
//        public IActionResult SaveFinal(FitrViewModel model)
//        {
//            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

//            if (sessionModel == null || sessionModel.Master == null)
//                return RedirectToAction("New");

//            // Update final remarks from current screen
//            sessionModel.Master.FinalRemarks = model.Master.FinalRemarks;

//            // ✅ 1) Save BASIC FIRST (this will create FitrId if not created)
//            var fitrId = _service.SaveBasic(sessionModel.Master);
//            sessionModel.Master.FitrId = fitrId;

//            // ✅ 2) Save PRODUCT
//            _service.SaveProduct(sessionModel.Master);

//            // ✅ 3) Save MATERIALS
//            _service.SaveMaterials(fitrId, sessionModel.Materials ?? new List<FitrMaterial>());

//            // ✅ 4) Save SR DATA
//            _service.SaveSrData(fitrId, sessionModel.SrData ?? new List<FitrSrData>());

//            // ✅ 5) Save VISUAL
//            _service.SaveVisual(sessionModel.Master);

//            // ✅ 6) Save FINAL REMARKS
//            _service.SaveFinalRemark(fitrId, sessionModel.Master.FinalRemarks);

//            // 🔥 7️⃣ GENERATE TC (THIS FIXES NULL ISSUE)
//            _service.GenerateTCIfNotExists(fitrId);
//            // Clear session after final save
//            HttpContext.Session.RemoveObject(FITR_SESSION_KEY);

//            // ✅ FINAL STEP → GO TO LIST
//            return RedirectToAction("List");
//        }


//        // ================= DC LIST =================
//        public IActionResult List()
//        {
//            ViewBag.DcList = _service.GetDcList();
//            return View("List");
//        }

//        // ================= PRINT =================
//        public IActionResult Print(int id)
//        {
//            _service.GenerateTCIfNotExists(id); // 🔥 generate once
//            var model = _service.GetFitr(id);
//            return View("Print", model);
//        }
//        public IActionResult PrintMultiple(string ids)
//        {
//            if (string.IsNullOrEmpty(ids))
//                return RedirectToAction("List");

//            var idList = ids.Split(',')
//                            .Select(int.Parse)
//                            .ToList();

//            foreach (var id in idList)
//            {
//                _service.GenerateTCIfNotExists(id);
//            }
//            var models = idList
//                .Select(id => _service.GetFitr(id))
//                .ToList();

//            return View("PrintMultiple", models);
//        }




//        [AcceptVerbs("Get", "Post")]
//        public IActionResult CheckDuplicateDc(DateTime? dcDate, string dcNo, int fitrId)
//        {
//            if (dcDate == null || string.IsNullOrEmpty(dcNo))
//                return Json(true);

//            bool exists = _service.IsDcDuplicate(dcNo, dcDate.Value, fitrId);

//            if (exists)
//                return Json($"DC No {dcNo} with this date already exists");

//            return Json(true);
//        }

//    }
//}

using FITR_DC_FORM.Filters;
using FITR_DC_FORM.Helpers;
using FITR_DC_FORM.Models;
using FITR_DC_FORM.Models.ViewModels;
using FITR_DC_FORM.Services.Interfaces;
using FITR_DC_FORM.Services.Repo_Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace FITR_DC_FORM.Controllers
{
    [RoleAuthorize("SUPERADMIN", "ADMIN", "HOD", "USER")]
    public class FitrController : Controller
    {
        private readonly IFitrService _service;
        private readonly IPrintCompanyService _companyService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;
        private const string FITR_SESSION_KEY = "FITR_WIZARD";

        public FitrController(IFitrService service, IPrintCompanyService companyService, IUserService userService, IWebHostEnvironment env)
        {
            _service = service;
            _companyService = companyService;
            _userService = userService;
            _env = env;
        }

        // ================= FILE SAVE HELPER =================
        private string SaveFile(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadRoot = Path.Combine(_env.WebRootPath, "uploads", folder);

            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(uploadRoot, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("uploads", folder, fileName).Replace("\\", "/");
        }

        // ================= START NEW =================
        [RoleAuthorize("USER")]
        public IActionResult New()
        {
            HttpContext.Session.RemoveObject(FITR_SESSION_KEY);
            return RedirectToAction("Index", new { tab = "basic", mode = "create" });
        }

        // ================= LOAD PAGE =================
        //[HttpGet]
        //public IActionResult Index(int? id, string tab = "basic", string mode = null)
        //{
        //    ViewBag.ActiveTab = tab;
        //    ViewBag.IsCreate = mode == "create";
        //    ViewBag.DcList = _service.GetDcList();

        //    FitrViewModel model;

        //    if (mode == "create")
        //    {
        //        model = new FitrViewModel { Master = new FitrMaster() };
        //    }
        //    else
        //    {
        //        model = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY)
        //                ?? (id.HasValue
        //                    ? _service.GetFitr(id)
        //                    : new FitrViewModel { Master = new FitrMaster() });
        //    }

        //    if (model.Master.TCNo == null)
        //    {
        //        var preview = _service.GetNextTCPreview();
        //        model.Master.PreviewTCNo = preview.tcNo;
        //        model.Master.PreviewTCYear = preview.tcYear;
        //    }

        //    HttpContext.Session.SetObject(FITR_SESSION_KEY, model);
        //    return View(model);
        //}

        // ================= BASIC + ATTACHMENTS =================
        
        [HttpGet]
        public IActionResult Index(int? id, string tab = "basic", string mode = null)
        {
            ViewBag.ActiveTab = tab;
            ViewBag.IsCreate = mode == "create";
            //string role = HttpContext.Session.GetString("UserRole");
            //int companyId = HttpContext.Session.GetInt32("CompanyId") ?? 0;
            //int locationId = HttpContext.Session.GetInt32("LocationId") ?? 0;
            //int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            //ViewBag.DcList = _service.GetDcList(
            //    role,
            //    companyId,
            //    locationId,
            //    userId
            //);

            // 🔥 THIS WAS MISSING (ROOT CAUSE)
            ViewBag.VisualMaster = _service.GetAllVisualMaster();

            // 🔹 Fetch users for selection dropdown
            int companyId = HttpContext.Session.GetInt32("CompanyId") ?? 0;
            int locationId = HttpContext.Session.GetInt32("LocationId") ?? 0;
            ViewBag.UserList = _userService.GetAll(companyId, locationId);

            FitrViewModel model;

            if (mode == "create")
            {
                model = new FitrViewModel
                {
                    Master = new FitrMaster
                    {
                        CreatedByUserId = HttpContext.Session.GetInt32("UserId") // 🔹 Pre-select current user
                    },
                    Visuals = new List<FitrVisual>()
                };
            }
            else
            {
                model = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY)
                        ?? (id.HasValue
                            ? _service.GetFitr(id)
                            : new FitrViewModel
                            {
                                Master = new FitrMaster(),
                                Visuals = new List<FitrVisual>()
                            });
            }

            if (model.Master.TCNo == null)
            {
                var preview = _service.GetNextTCPreview();
                model.Master.PreviewTCNo = preview.tcNo;
                model.Master.PreviewTCYear = preview.tcYear;
            }

            HttpContext.Session.SetObject(FITR_SESSION_KEY, model);
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveBasic(
            FitrViewModel model,
            IFormFile DCAttachment,
            IFormFile POAttachment,
            IFormFile DrawingAttachment
        )
        {
            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY)
                               ?? new FitrViewModel { Master = new FitrMaster() };

            sessionModel.Master.CompanyName = model.Master.CompanyName;
            sessionModel.Master.DCNo = model.Master.DCNo;
            sessionModel.Master.DCDate = model.Master.DCDate;
            sessionModel.Master.PONo = model.Master.PONo;
            sessionModel.Master.PODate = model.Master.PODate;
            sessionModel.Master.Quantity = model.Master.Quantity;
            sessionModel.Master.CreatedByUserId = model.Master.CreatedByUserId;

            // 🔴 DC (MANDATORY)
            var dcPath = SaveFile(DCAttachment, "dc");
            if (!string.IsNullOrEmpty(dcPath))
                sessionModel.Master.DCAttachmentPath = dcPath;

            // 🔴 PO (MANDATORY)
            var poPath = SaveFile(POAttachment, "po");
            if (!string.IsNullOrEmpty(poPath))
                sessionModel.Master.POAttachmentPath = poPath;

            // 🟡 Drawing (OPTIONAL)
            var drawingPath = SaveFile(DrawingAttachment, "drawing");
            if (!string.IsNullOrEmpty(drawingPath))
                sessionModel.Master.DrawingAttachmentPath = drawingPath;

            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);
            return RedirectToAction("Index", new { tab = "product" });
        }

        // ================= PRODUCT =================
        [HttpPost]
        public IActionResult SaveProduct(FitrViewModel model)
        {
            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

            sessionModel.Master.ProductType = model.Master.ProductType;
            sessionModel.Master.Model = model.Master.Model;
            sessionModel.Master.Torque = model.Master.Torque;
            sessionModel.Master.Ratio = model.Master.Ratio;
            sessionModel.Master.HandwheelDia = model.Master.HandwheelDia;

            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);
            return RedirectToAction("Index", new { tab = "material" });
        }

        // ================= MATERIAL =================
        [HttpPost]
        public IActionResult SaveMaterial(FitrViewModel model)
        {
            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);
            sessionModel.Materials = model.Materials ?? new List<FitrMaterial>();
            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);
            return RedirectToAction("Index", new { tab = "sr" });
        }

        // ================= SR DATA =================
        [HttpPost]
        public IActionResult SaveSrData(FitrViewModel model)
        {
            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);
            sessionModel.SrData = model.SrData ?? new List<FitrSrData>();
            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);
            return RedirectToAction("Index", new { tab = "visual" });
        }

        // ================= VISUAL =================
        [HttpPost]
        public IActionResult SaveVisual(FitrViewModel model)
        {
            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);

            if (sessionModel == null)
                return RedirectToAction("New");

            sessionModel.Visuals = model.Visuals ?? new List<FitrVisual>(); // 🔥 SAFE

            HttpContext.Session.SetObject(FITR_SESSION_KEY, sessionModel);

            return RedirectToAction("Index", new { tab = "final" });
        }




        // ================= FINAL SAVE =================
        [HttpPost]
        public IActionResult SaveFinal(
         FitrViewModel model,
         IFormFile DCAttachment,
         IFormFile POAttachment,
         IFormFile DrawingAttachment,
         IFormFile TCAttachment
)
        {
            var sessionModel = HttpContext.Session.GetObject<FitrViewModel>(FITR_SESSION_KEY);
            if (sessionModel == null || sessionModel.Master == null)
                return RedirectToAction("New");

            /* ================= FINAL REMARK ================= */
            sessionModel.Master.FinalRemarks = model.Master.FinalRemarks;

            /* ================= ATTACHMENTS ================= */

            var dcPath = SaveFile(DCAttachment, "dc");
            if (!string.IsNullOrEmpty(dcPath))
                sessionModel.Master.DCAttachmentPath = dcPath;
            // 🔴 DUPLICATE DC NO CHECK
            if (_service.IsDcNoExists(sessionModel.Master.DCNo, sessionModel.Master.FitrId))
            {
                TempData["ErrorMessage"] = $"DC No '{sessionModel.Master.DCNo}' already exists.";
                return RedirectToAction("New");
            }
            var poPath = SaveFile(POAttachment, "po");
            if (!string.IsNullOrEmpty(poPath))
                sessionModel.Master.POAttachmentPath = poPath;

            var drawingPath = SaveFile(DrawingAttachment, "drawing");
            if (!string.IsNullOrEmpty(drawingPath))
                sessionModel.Master.DrawingAttachmentPath = drawingPath;

            var tcPath = SaveFile(TCAttachment, "tc");
            if (!string.IsNullOrEmpty(tcPath))
                sessionModel.Master.TCAttachmentPath = tcPath;

            /* ================= 🔥 MISSING PART (FIX) ================= */

            var userId = HttpContext.Session.GetInt32("UserId");
            var companyId = HttpContext.Session.GetInt32("CompanyId");
            var locationId = HttpContext.Session.GetInt32("LocationId");

            if (userId == null || companyId == null || locationId == null)
                return RedirectToAction("Login", "Account");
            /* ================= AUTO APPROVAL STATUS (✔ HERE) ================= */
            
            // 🔥 THIS WAS THE CORE ISSUE
            // 🔥 Only override if NOT explicitly set in the wizard
            if (sessionModel.Master.CreatedByUserId == null || sessionModel.Master.CreatedByUserId == 0)
            {
                sessionModel.Master.CreatedByUserId = userId.Value;
            }

            sessionModel.Master.CompanyId = companyId.Value;
            sessionModel.Master.LocationId = locationId.Value;
            sessionModel.Master.Status = "PREPARED";
            /* ================= DB SAVE ================= */

            // 1️⃣ Save BASIC (creates / updates FITR_Master)
            var fitrId = _service.SaveBasic(sessionModel.Master);
            sessionModel.Master.FitrId = fitrId;

            // 2️⃣ Save PRODUCT
            _service.SaveProduct(sessionModel.Master);

            // 3️⃣ Save MATERIALS
            _service.SaveMaterials(fitrId, sessionModel.Materials ?? new List<FitrMaterial>());

            // 4️⃣ Save SR DATA
            _service.SaveSrData(fitrId, sessionModel.SrData ?? new List<FitrSrData>());

            // 5️⃣ Save VISUALS
            var visuals = sessionModel.Visuals ?? new List<FitrVisual>();
            if (visuals.Any())
            {
                _service.SaveVisuals(fitrId, visuals);
            }

            // 6️⃣ Save FINAL REMARK
            _service.SaveFinalRemark(fitrId, sessionModel.Master.FinalRemarks);

            // 7️⃣ Generate TC
            _service.GenerateTCIfNotExists(fitrId);

            // 🧹 Clear session
            HttpContext.Session.RemoveObject(FITR_SESSION_KEY);

            return RedirectToAction("List");
        }

        // ================= TC ATTACHMENT =================
        [HttpPost]
        public IActionResult UploadTCAttachment(int fitrId, IFormFile TCAttachment)
        {
            var tcPath = SaveFile(TCAttachment, "tc");

            if (!string.IsNullOrEmpty(tcPath))
                _service.SaveTCAttachment(fitrId, tcPath);

            return RedirectToAction("Print", new { id = fitrId });
        }
        [HttpPost]
        public IActionResult UploadDrawingAttachment(int fitrId, IFormFile DrawingAttachment)
        {
            if (fitrId <= 0)
                return RedirectToAction("List");

            var drawingPath = SaveFile(DrawingAttachment, "drawing");

            if (!string.IsNullOrEmpty(drawingPath))
            {
                _service.SaveDrawingAttachment(fitrId, drawingPath);
            }

            return RedirectToAction("Index", new { id = fitrId, tab = "visual" });
        }

        // ================= LIST =================
        //[RoleAuthorize("SUPERADMIN", "ADMIN", "HOD","USER")]
        //public IActionResult List()
        //{
        //    ViewBag.DcList = _service.GetDcList();
        //    return View("List");
        //}
        //[RoleAuthorize("SUPERADMIN", "ADMIN", "HOD", "USER")]
        //public IActionResult List()
        //{
        //    string role = HttpContext.Session.GetString("UserRole");
        //    int companyId = HttpContext.Session.GetInt32("CompanyId") ?? 0;
        //    int locationId = HttpContext.Session.GetInt32("LocationId") ?? 0;
        //    int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        //    var dcList = _service.GetDcList(role, companyId, locationId, userId);

        //    ViewBag.DcList = dcList;

        //    return View();
        //}



        // ================= LIST =================
        [RoleAuthorize("SUPERADMIN", "ADMIN", "HOD", "USER")]
        public IActionResult List(string filter = "pending")
        {
            string role = (HttpContext.Session.GetString("UserRole") ?? "").ToUpper();
            int sessionCompanyId = HttpContext.Session.GetInt32("CompanyId") ?? 0;
            int sessionLocationId = HttpContext.Session.GetInt32("LocationId") ?? 0;
            int sessionUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            int effectiveCompanyId = sessionCompanyId;
            int effectiveLocationId = sessionLocationId;
            int effectiveUserId = sessionUserId;

            if (role == "SUPERADMIN")
            {
                // 🔥 Super Admin sees all data from all companies
                effectiveCompanyId = 0;
                effectiveLocationId = 0;
                effectiveUserId = 0;
            }
            else if (role == "ADMIN")
            {
                // Admin sees all data for their company
                effectiveUserId = 0;
            }

            // Using stable method
            var list = _service.GetListByRole(
                role,
                effectiveCompanyId,
                effectiveLocationId,
                effectiveUserId
            );

            // 🔥 PERFECT FILTER: Role-aware pending logic
            if (filter == "pending")
            {
                if (role == "USER")
                {
                    list = list.Where(x => string.Equals(x.Status, "Draft", StringComparison.OrdinalIgnoreCase) || 
                                          string.Equals(x.Status, "DRAFT", StringComparison.OrdinalIgnoreCase) ||
                                          string.Equals(x.Status, "PREPARED", StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else if (role == "HOD")
                {
                    list = list.Where(x => string.Equals(x.Status, "SUBMITTED", StringComparison.OrdinalIgnoreCase) || 
                                          string.Equals(x.Status, "HOD Approval Pending", StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else if (role == "ADMIN")
                {
                    list = list.Where(x => string.Equals(x.Status, "HOD_APPROVED", StringComparison.OrdinalIgnoreCase) || 
                                          string.Equals(x.Status, "Admin Approval Pending", StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else if (role == "SUPERADMIN")
                {
                    // SuperAdmin sees everything pending
                    list = list.Where(x => x.Status != null && (
                        x.Status.Contains("Pending", StringComparison.OrdinalIgnoreCase) || 
                        x.Status.Equals("SUBMITTED", StringComparison.OrdinalIgnoreCase) ||
                        x.Status.Equals("PREPARED", StringComparison.OrdinalIgnoreCase) ||
                        x.Status.Equals("DRAFT", StringComparison.OrdinalIgnoreCase)
                    )).ToList();
                }
            }

            // 🔥 FALLBACK: If SP missed SrData, fetch it now (only for visible records)
            foreach (var dc in list.Where(x => x.SrData == null || !x.SrData.Any()))
            {
                var full = _service.GetFitr(dc.FitrId);
                if (full != null && full.Master.SrData != null)
                {
                    dc.SrData = full.Master.SrData;
                }
            }

            ViewBag.DcList = list;
            ViewBag.Filter = filter;
            return View();
        }
        // ================= PRINT =================
        public IActionResult Print(int id, int? printCompanyId)
        {
            _service.GenerateTCIfNotExists(id);
            var model = _service.GetFitr(id);

            if (printCompanyId.HasValue)
            {
                ViewBag.PrintCompany = _companyService.GetById(printCompanyId.Value);
            }

            return View("Print", model);
        }

        [HttpGet]
        public IActionResult PrintMultiple(string ids, int? printCompanyId)
        {
            if (string.IsNullOrEmpty(ids))
                return BadRequest("No records selected");

            if (printCompanyId.HasValue)
            {
                ViewBag.PrintCompany = _companyService.GetById(printCompanyId.Value);
            }

            var idList = ids
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            var models = new List<FitrViewModel>();

            foreach (var id in idList)
            {
                _service.GenerateTCIfNotExists(id);
                var model = _service.GetFitr(id);

                if (model != null)
                    models.Add(model);
            }

            return View("BulkPrint", models);
        }

        [HttpGet]
        public IActionResult GetPrintCompanies()
        {
            var companies = _companyService.GetAll()
                .Where(c => c.IsActive)
                .Select(c => new { 
                    c.PrintCompanyId, 
                    c.CompanyName, 
                    c.CompanyAddress,
                    c.CompanyLogo 
                })
                .ToList();
            return Json(companies);
        }


        [HttpPost]
        [RoleAuthorize("USER")]
        public IActionResult Submit(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            _service.SubmitByUser(id, userId);

            TempData["Success"] = "FITR submitted successfully.";
            return RedirectToAction("List");
        }
        [HttpPost]
        [RoleAuthorize("HOD")]
        public IActionResult ApproveByHod(int id)
        {
            int hodUserId = HttpContext.Session.GetInt32("UserId").Value;

            _service.ApproveByHod(id, hodUserId);

            TempData["Success"] = "FITR approved by HOD.";
            return RedirectToAction("List");
        }
        [HttpPost]
        [RoleAuthorize("ADMIN")]
        public IActionResult ApproveByAdmin(int id)
        {
            int adminUserId = HttpContext.Session.GetInt32("UserId").Value;

            _service.ApproveByAdmin(id, adminUserId);

            TempData["Success"] = "FITR final approved.";
            return RedirectToAction("List");
        }

    }
}
