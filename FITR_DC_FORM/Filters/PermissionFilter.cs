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

            // 🔥 Skip Account controller
            if (controller == "Account")
            {
                await next();
                return;
            }

            var userId = context.HttpContext.Session.GetInt32("UserId");
            var role = context.HttpContext.Session.GetString("UserRole");

            // 🔥 Not logged in
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // 🔥 SuperAdmin full access
            if (role == "SUPERADMIN")
            {
                await next();
                return;
            }

            // 🔥 PageName = Controller Name (must match PageMaster)
            string pageName = controller;

            // 🔥 Determine permission based on action
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