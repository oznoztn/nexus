using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nexus.Areas.Admin.Models;
using Nexus.Core.Entities;
using Nexus.Extensions;
using Nexus.Service.DTOs;
using Nexus.Service.ServiceInterfaces;
using Nexus.Shared;
using Nexus.Tools;


namespace Nexus.Areas.Admin.Controllers
{
    public class CategoriesController : AdminBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMessageProvider _messageProvider;
        private readonly CategoryViewModelFactory _factory;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMessageProvider messageProvider, IMapper mapper)
        {
            _categoryService = categoryService;
            _messageProvider = messageProvider;
            _mapper = mapper;

            _factory = new CategoryViewModelFactory(_categoryService);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            return View(_factory.Create());
        }

        public IActionResult NewPartial()
        {
            return new PartialViewResult()
            {
                ViewName = "_createUpdateForm",
                ViewData = this.ViewData
            };
        }

        [HttpPost]
        public IActionResult New(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                CategoryDto category = _mapper.Map<CategoryViewModel, CategoryDto>(categoryViewModel);

                _categoryService.Add(category);

                if (category.Id != 0)
                {
                    SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Create, "category"));
                    return RedirectToAction("Edit", new { id = category.Id});
                }
            }

            categoryViewModel.CategoryTypes = _factory.GetCategoryTypes();
            return View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vm = _factory.Create(id);

            if (vm.Id == default(int))
                return BadRequest();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<CategoryViewModel, CategoryDto>(categoryViewModel);

                if (string.IsNullOrWhiteSpace(category.Slug))
                    category.Slug = Slug.Create(category.Title);

                _categoryService.Update(category);
                SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Update, "category"));
                return RedirectToAction("List");
            }

            return View(categoryViewModel);
        }

        public JsonResult TreeList_Update([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);
                _categoryService.Update(categoryDto);

                category.DisplayOrder = categoryDto.DisplayOrder;
                category.IsVisible = categoryDto.IsVisible;
                category.Title = categoryDto.Title;

                return Json(new[] { category }.ToTreeDataSourceResult(request, ModelState));
            }

            return Json(new[] { category }.ToTreeDataSourceResult(request, ModelState));
        }

        public JsonResult Kendo_GetCategories(int excludedCategoryId)
        {
            var categories = _categoryService.GetCategoriesTree();

            if (excludedCategoryId == default(int))
                return Json(categories.ToList());

            return Json(categories.Where(cat => cat.Id != excludedCategoryId).ToList());
        }

        public JsonResult TreeList_Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = _categoryService.GetAllOrdered().ToTreeDataSourceResult(request,
                e => e.Id,
                e => e.ParentId,
                e => e
            );

            return Json(result);
        }

        [HttpPost]
        public ActionResult TreeList_Delete(CategoryViewModel courseVm)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryService.Get(courseVm.Id);

                if(category == null)
                    ModelState.AddModelError("CategoryNotFound", "No category was found with the given Id.");

                if(ModelState.IsValid)
                    _categoryService.Delete(courseVm.Id);
            }

            return Json(ModelState.ToDataSourceResult()); // .. which means nothing
        }
    }
}