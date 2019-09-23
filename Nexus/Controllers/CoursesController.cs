using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nexus.Service;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            List<CourseGroup> model = _courseService.GetCourseGroups();
            return View(model);
        }
    }
}