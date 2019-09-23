using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            var projects = _projectService.GetAllWithFirstPictureIncluded().ToList();
            return View(projects);
        }

        public IActionResult Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return BadRequest();

            var dto = _projectService.GetBySlug(slug);

            return View(dto);
        }
    }
}