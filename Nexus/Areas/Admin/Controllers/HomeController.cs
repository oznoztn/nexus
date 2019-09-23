using Microsoft.AspNetCore.Mvc;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}