using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;
using Nexus.Service.Extensions;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Areas.Admin.Controllers
{
    public class TagsController : AdminBaseController
    {
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public TagsController(IMapper mapper, ITagService tagService)
        {
            _mapper = mapper;
            _tagService = tagService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult TagsGrid_Read([DataSourceRequest] DataSourceRequest request)
        {
            var tagModels = _tagService
                .GetTopNTagsAlongWithUsageInfo(true, true)
                .Select(tuple => new TagViewModel
                {
                    Id = tuple.Item1.Id,
                    Slug = tuple.Item1.Slug,
                    Title = tuple.Item1.Title,
                    UsageCount = tuple.Item2,
                    IsHidden = tuple.Item1.IsHidden
                }).ToArray();

            return Json(tagModels.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult TagsGrid_Destroy(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = _tagService.GetById(model.Id);

                if (tag == null)
                    ModelState.AddModelError("TagNotFound", "No tag found with the given Id.");

                if (ModelState.IsValid)
                    _tagService.Delete(tag);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
        public JsonResult TagsGrid_Update([DataSourceRequest] DataSourceRequest request, TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tagDto = _tagService.GetById(model.Id);
                if (tagDto == null)
                {
                    ModelState.AddModelError("TagNotFound", "No tag found with the given Id.");
                    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
                }

                if (tagDto.IsHidden != model.IsHidden && 
                    tagDto.Slug == model.Slug && 
                    tagDto.Title == model.Title)
                {
                    // Only the IsHidden field was changed.
                }
                else if (tagDto.Title != model.Title &&
                         tagDto.Slug == model.Slug)
                {
                    if (_tagService.GetByTitle(model.Title) != null)
                    {
                        ModelState.AddModelError("TagAlreadyExists", "There is already a tag with the same title!");
                    }
                }
                else if (tagDto.Slug != model.Slug &&
                         tagDto.Title == model.Title)
                {
                    if (_tagService.GetBySlug(model.Slug) != null)
                    {
                        ModelState.AddModelError("TagAlreadyExists", "There is already a tag with the same slug!");
                    }
                }
                
                if (ModelState.IsValid)
                {
                    var dto = _mapper.Map<TagDto>(model);
                    _tagService.Update(dto);
                }
            }

            return Json(new[] {model}.ToDataSourceResult(request, ModelState));
        }
    }
}
