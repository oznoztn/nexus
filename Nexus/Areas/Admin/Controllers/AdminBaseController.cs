using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nexus.Tools;

namespace Nexus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminBaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewData[NavigationPageManager.ActiveControllerKey] = context.RouteData.Values["controller"] as string;
            ViewData[NavigationPageManager.ActivePageKey] = context.RouteData.Values["action"] as string;

            base.OnActionExecuted(context);
        }

        public void SetClientSideNotificationMessage(string message)
        {
            TempData["status_message"] = message;
        }
    }
}