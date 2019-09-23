using Microsoft.AspNetCore.Mvc;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}