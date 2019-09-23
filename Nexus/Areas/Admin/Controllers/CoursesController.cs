using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Nexus.Areas.Admin.Models;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.ServiceInterfaces;
using Nexus.Tools;

namespace Nexus.Areas.Admin.Controllers
{
    public class CoursesController : AdminBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IMessageProvider _messageProvider;

        public CoursesController(IMapper mapper, ICourseService courseService, ICategoryService categoryService, IMessageProvider messageProvider)
        {
            _mapper = mapper;
            _courseService = courseService;
            _categoryService = categoryService;
            _messageProvider = messageProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            if (id == default(int))
                return RedirectToAction("List", "Courses");

            var course = _courseService.Get(id);

            if (course == null)
                return NotFound($"Course for given id ({id}) was not found.");

            var vm = _mapper.Map<CourseViewModel>(course);

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(CourseViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(vm);

            CourseDto course = _mapper.Map<CourseDto>(vm);
            _courseService.Update(course);
            SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Update, "course"));
            return RedirectToAction("Edit", new { id = vm.Id });
        }

        public IActionResult New()
        {
            return View(new CourseViewModel() {
                DisplayOrder = _courseService.GetNextDisplayOrder(),
                YearFinished = DateTime.Now.Year,
                IsOnlineCourse = true,
                IsVisible = true
            });
        }

        [HttpPost]
        public IActionResult New(CourseViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<CourseDto>(vm);
                _courseService.Add(course);

                if (course.Id != default(int))
                {
                    SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Create, "course"));
                    return RedirectToAction("Edit", "Courses", new { id = course.Id });
                }
            }

            return View(vm);
        }

        public JsonResult Courses_Read(int excludedCategoryId)
        {
            return Json(_courseService.GetAll()
                .Where(cat => cat.Id != excludedCategoryId).ToList());
        }

        public ActionResult CoursesGrid_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(_courseService.GetAll().ToDataSourceResult(request));
        }

        public JsonResult GetCategories()
        {
            return Json(_categoryService.GetDefaultCategories().Select(c => new { Id = c.Id, Title = c.Title }));
        }

        public JsonResult GetCourseNoteCategories()
        {
            var categories = new List<CategoryDto>();
            categories.Add(new CategoryDto { Id = 0, CategoryTypeId = (int)CategoryType.Default, Title = "[None]"});
            categories.AddRange(_categoryService.GetCourseCategories());

            return Json(categories.Select(c => new { Id = c.Id, Title = c.Title }));
        }

        [HttpPost]
        public IActionResult Grid_Delete(CourseViewModel courseVm)
        {
            if (ModelState.IsValid)
            {
                var course = _courseService.Get(courseVm.Id);

                if (course == null)
                    ModelState.AddModelError("CourseNotFound", "No course was found with the given Id.");

                if (ModelState.IsValid)
                    _courseService.Delete(courseVm.Id);
            }

            return Json(ModelState.ToDataSourceResult());
        }
    }
}