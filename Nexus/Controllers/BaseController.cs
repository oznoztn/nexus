using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nexus.Tools;

namespace Nexus.Controllers
{
    public class BaseController : Controller
    {
        public const string Administrator = "Administrator";
        public const int DefaultPage = 1;
        
        // TODO: need to introduce a blog settings mechanism to centralize such things
        protected int PageSize => SiteSettings.GetDefaultPageSize(); 

        protected int CurrentPage
        {
            get
            {                
                string page = HttpContext.Request.Query["page"].ToString();

                if (int.TryParse(page, out var result))
                    return Math.Max(DefaultPage, result);

                return DefaultPage;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.CurrentPage = CurrentPage;

            base.OnActionExecuted(context);
        }
    }
}