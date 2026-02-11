using FITR_DC_FORM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FITR_DC_FORM.Filters
{
    public class PermissionFilter : IAsyncActionFilter
    {
        private readonly IPermissionService _permissionService;

        public PermissionFilter(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task OnActionExecutionAsync(
     ActionExecutingContext context,
     ActionExecutionDelegate next)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            // 🔥 VERY IMPORTANT — SKIP ACCOUNT CONTROLLER
            if (controller == "Account")
            {
                await next();
                return;
            }

            var userId = context.HttpContext.Session.GetInt32("UserId");
            var role = context.HttpContext.Session.GetString("UserRole");

            // 🔥 If not logged in → redirect to Login
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // 🔥 SUPERADMIN FULL ACCESS
            if (role == "SUPERADMIN")
            {
                await next();
                return;
            }

            string pageName = controller switch
            {
                "Company" => "CompanyMaster",
                "Location" => "LocationMaster",
                "Fitr" => "Fitr",
                "VisualMaster" => "VisualMaster",
                "User" => "UserMaster",
                "UserRights" => "UserRights",
                "PrintCompany" => "PrintCompany",
                _ => null
            };

            if (pageName == null)
            {
                await next();
                return;
            }

            bool allowed = action switch
            {
                "Index" => _permissionService.CanView(userId.Value, pageName),
                "List" => _permissionService.CanView(userId.Value, pageName),
                "Create" => _permissionService.CanCreate(userId.Value, pageName),
                "Edit" => _permissionService.CanEdit(userId.Value, pageName),
                "Delete" => _permissionService.CanDelete(userId.Value, pageName),
                _ => true
            };

            if (!allowed)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                return;
            }

            await next();
        }

    }
}