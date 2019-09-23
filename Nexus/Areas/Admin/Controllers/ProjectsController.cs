using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;
using Nexus.Service.Extensions;
using Nexus.Service.ServiceInterfaces;
using Nexus.Tools;

namespace Nexus.Areas.Admin.Controllers
{
    public class ProjectsController : AdminBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IProjectPictureService _projectPictureService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMessageProvider _messageProvider;

        public ProjectsController(
            IMapper mapper,
            IProjectService projectService, 
            IProjectPictureService projectPictureService, 
            IHostingEnvironment hostingEnvironment,
            IMessageProvider messageProvider)
        {
            _mapper = mapper;
            _projectService = projectService;
            _projectPictureService = projectPictureService;
            _hostingEnvironment = hostingEnvironment;
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
                return BadRequest();

            ProjectDto dto = _projectService.Get(id);

            if (dto == null)
                return NotFound();

            ProjectModel model = _mapper.Map<ProjectModel>(dto);
            model.Months = GetMonths();
            model.YearsFromList = GetYearsSelectList(DateTime.Now.Year - 30, DateTime.Now.Year);
            model.YearsToList = GetYearsSelectList(DateTime.Now.Year - 30, DateTime.Now.Year + 30);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProjectModel model)
        {
            ValidateProjectMode(model);
            if (ModelState.IsValid)
            {
                ProjectDto dto = _mapper.Map<ProjectDto>(model);
                _projectService.Update(dto);
                SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Update, "project"));
                return RedirectToAction("Edit", new {id = model.Id});
            }

            model.Months = GetMonths();
            model.YearsFromList = GetYearsSelectList(DateTime.Now.Year - 30, DateTime.Now.Year);
            model.YearsToList = GetYearsSelectList(DateTime.Now.Year - 30, DateTime.Now.Year + 30);
            return View(model);
        }

        public IActionResult New()
        {
            var vm = new ProjectModel
            {
                Months = GetMonths(),
                YearsFromList = GetYearsSelectList(DateTime.Now.Year - 30, DateTime.Now.Year),
                YearsToList = GetYearsSelectList(DateTime.Now.Year - 30, DateTime.Now.Year + 30)
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult New(ProjectModel model)
        {
            ValidateProjectMode(model);
            if (ModelState.IsValid)
            {
                ProjectDto dto = _mapper.Map<ProjectDto>(model);
                
                _projectService.Add(dto);

                if (dto.Id != default(int))
                {
                    SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Create, "project"));
                    return RedirectToAction("Edit", new { id = dto.Id });
                }
             }
            
            model.Months = GetMonths();
            model.YearsFromList = GetYearsSelectList(DateTime.Now.Year - 30, DateTime.Now.Year);
            model.YearsToList = GetYearsSelectList(DateTime.Now.Year - 30, DateTime.Now.Year + 30);
            return View(model);
        }

        public ActionResult ProjectsGrid_Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<ProjectDto> projects = _projectService.GetAllWithFirstPictureIncluded();

            return Json(projects.ToDataSourceResult(request));
        }

        public JsonResult ProjectsGrid_Destroy(ProjectModel projectModel)
        {
            if (ModelState.IsValid)
            {
                var project = _projectService.Get(projectModel.Id);

                if (project == null)
                    ModelState.AddModelError("ProjectNotFound", "No project was found with the given Id.");

                if (ModelState.IsValid)
                    _projectService.Delete(project);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult ProjectPicturesGrid_Read([DataSourceRequest] DataSourceRequest request, int projectId)
        {
            var pictures = _projectPictureService.GetProjectPicturesOrdered(projectId);

            return Json(pictures.ToDataSourceResult(request));
        }

        [HttpPost]
        public JsonResult ProjectPicturesGrid_Update([DataSourceRequest] DataSourceRequest request, ProjectPictureDto projectPictureDto)
        {
            if (projectPictureDto != null && ModelState.IsValid)
            {
                _projectPictureService.Update(projectPictureDto);
            }

            return Json(new[] { projectPictureDto }.ToDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> FineUploader_Upload(IFormFile imageFile, int projectId)
        {
            if (projectId == default(int))
                return Json(new {success = false, error = $"Failure: Invalid project id: {projectId}" });

            if (imageFile == null || imageFile.Length == 0)
                return Json(new { success = false, error = $"Given file is invalid." });

            var validFormats = new [] {"image/jpeg", "image/png", "image/bmp", "image/gif"};
            if(validFormats.All(validFormat => validFormat != imageFile.ContentType))
                return Json(new { success = false, error = $"Invalid file format." });

            var project = _projectService.Get(projectId);
            if (project == null)
                return Json(new { success = false, error = $"No content found for given id: {projectId}." });
            
            string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(imageFile.FileName)}";
            string pyhsicalDir = Path.Combine(_hostingEnvironment.WebRootPath, "images", "projects");
            string fileFullPath = Path.Combine(pyhsicalDir, fileName);
            string fileRelativePath = $"/images/projects/{fileName}";

            if (!Directory.Exists(pyhsicalDir))
                Directory.CreateDirectory(pyhsicalDir);

            try
            {
                ProjectPictureDto projectPictureDto = new ProjectPictureDto
                {
                    ProjectId = projectId,
                    FileName = fileRelativePath,
                    IsVisible = true,
                    Alt = project.Title,
                    Title = project.Title
                };

                using (var memoryStream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(memoryStream);
                    projectPictureDto.Binary = memoryStream.ToArray();

                    await System.IO.File.WriteAllBytesAsync(fileFullPath, projectPictureDto.Binary);
                }

                _projectPictureService.Add(projectPictureDto);

                if (projectPictureDto.Id != 0)
                {
                    // OK
                    return Json(new { success = true, pictureId = projectPictureDto.Id });
                }
                else
                {
                    // FAILURE
                    return Json(new { success = false, error = $"Internal server error." });
                }
            }
            catch (Exception)
            {
                if (System.IO.File.Exists(fileFullPath))
                    System.IO.File.Delete(fileFullPath);
                else if (_projectPictureService.GetProjectPictures(projectId).Any(t => t.Title == fileRelativePath))
                {
                    var picture = _projectPictureService.GetProjectPictures(projectId).First(t => t.Title == fileRelativePath);
                    _projectPictureService.Delete(picture);
                }

                return Json(new { success = false, error = $"Internal server error." });
            }          
        }

        [HttpPost, HttpDelete]
        public JsonResult ProjectPicture_Delete(int id)
        {
            ProjectPictureDto projectPicture = _projectPictureService.Get(id);

            if (projectPicture == null)
                return Json(new { success = false, error = $"Failure: Invalid project picture id: {id}" });

            _projectPictureService.Delete(projectPicture);

            return Json(new { success = true });
        }

        private void ValidateProjectMode(ProjectModel model)
        {
            if (model.MonthTo.HasValue && model.YearTo.HasValue)
            {
                if (model.YearTo.Value < model.YearFrom)
                {
                    ModelState.AddModelError("", "End date can not be earlier than start date!");
                }
                else if (model.YearTo.Value == model.YearFrom)
                {
                    if (model.MonthTo < model.MonthFrom)
                    {
                        ModelState.AddModelError("", "End date can not be earlier than start date!");
                    }
                }
            }
            else
            {
                if ((model.MonthTo.HasValue && !model.YearTo.HasValue) || (!model.MonthTo.HasValue && model.YearTo.HasValue))
                    ModelState.AddModelError("", "End date month and year should be both selected or unselected!");
            }
        }

        private List<SelectListItem> GetMonths()
        {
            var returnList = new List<SelectListItem>();
            var monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;

            for (int i = 0; i < monthNames.Length; i++)
            {
                if (i == monthNames.Length - 1)
                    continue;

                returnList.Add(new SelectListItem
                {
                    Value = $"{i + 1}",
                    Text = monthNames[i]
                });
            }

            return returnList;
        }

        private List<SelectListItem> GetYearsSelectList(int start, int end)
        {
            return GetYears(start, end)
                .Select(year => new SelectListItem
                {
                    Value = year.ToString(),
                    Text = year.ToString()
                }).Reverse().ToList();
        }

        private List<int> GetYears(int start, int end)
        {
            var enumerable = Enumerable.Range(start, end - start + 1);

            return enumerable.ToList();
        }
    }
}