using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Areas.Admin.Models;
using Nexus.Extensions;
using Nexus.Service;
using Nexus.Service.DTOs;
using Nexus.Service.ServiceInterfaces;
using Nexus.Tools;

namespace Nexus.Areas.Admin.Controllers
{
    public static class FileHelper
    {
        public static string GenerateGuidFileName(string pathString)
        {
            return string.Concat(Guid.NewGuid(), Path.GetExtension(pathString));
        }
    }
    public class NotesController : AdminBaseController
    {
        private readonly IMapper _mapper;
        private readonly INoteService _noteService;
        private readonly ICategoryService _categoryService;
        private readonly INexusEnvironment _nexusEnvironment;
        private readonly IMessageProvider _messageProvider;
        private readonly IViewRenderService _viewRenderService;
        private readonly ITagService _tagService;

        private readonly NoteViewModelFactory _noteViewModelFactory;

        public NotesController(IMapper mapper, INoteService noteService, ICategoryService categoryService, INexusEnvironment nexusEnvironment, ITagService tagService, IMessageProvider messageProvider, IViewRenderService viewRenderService)
        {
            _mapper = mapper;
            _noteService = noteService;
            _categoryService = categoryService;
            _nexusEnvironment = nexusEnvironment;
            _messageProvider = messageProvider;
            _viewRenderService = viewRenderService;
            _tagService = tagService;

            _noteViewModelFactory = new NoteViewModelFactory(mapper, _noteService, _tagService);
        }

        public IActionResult Index()
        {
            return View();
        }

        //public PartialViewResult GetModal()
        //{
        //    return PartialView("CategoriesEdit");
        //    var factory = new CategoryViewModelFactory(_categoryService);
        //    var model = factory.Create();
        //    //var html = _viewRenderService.RenderToStringAsync("Categories/_createUpdateForm.cshtml", model).Result;
        //    string html2 = this.RenderViewAsync("New", _noteViewModelFactory.Create(), true).Result;
        //    return Content(html2);
        //}

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            if (id == default(int))
                return BadRequest();

            var vm = _noteViewModelFactory.Create(id);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(NoteViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var noteDto = _mapper.Map<NoteDto>(vm);

                _noteService.Update(noteDto);

                //Response.Headers.Add("X-XSS-Protection", "0"); // CHROME için, IE de bir şey olmuyor.
                SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Update, "note"));
                return RedirectToAction("Edit", new { id = vm.Id });
            }

            _noteViewModelFactory.PrepareTags(vm);            
            return View(vm);
        }

        public IActionResult New()
        {
            NoteViewModel vm = _noteViewModelFactory.Create();
            return View(vm);
        }

        [HttpPost]
        public IActionResult New(NoteViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var noteDto = _mapper.Map<NoteDto>(vm);

                _noteService.Add(noteDto);

                if (noteDto.Id != default(int))
                {
                    SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Create, "note"));
                    return RedirectToAction("Edit", "Notes", new { id = noteDto.Id });
                }
            }

            _noteViewModelFactory.PrepareTags(vm);
            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateNoteContent(int noteId, string content)
        {
            if (noteId == default(int))
                return Json(new {status = 0});

            bool result = _noteService.UpdateNoteContent(noteId, content);

            // failure
            if (!result) 
                return Json(new { status = 0 });

            // success
            return Json(new {status = 1});
        }

        public string GetNoteContent(int noteId)
        {
            return _noteService.GetNoteContent(noteId);
        }

        [HttpPost]
        public async Task<IActionResult> NotePictureUpload(IFormFile upload, int noteId)
        {
            //string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(upload.FileName)}";
            //string pyhsicalDir = Path.Combine(_hostingEnvironment.WebRootPath, "images", "notes", noteId.ToString());
            //string fileFullPath = Path.Combine(pyhsicalDir, fileName);
            //string fileRelativePath = $"/images/notes/{noteId}/{fileName}";

            string fileName = FileHelper.GenerateGuidFileName(upload.FileName);
            string pyhsicalDir = _nexusEnvironment.GetNoteImagesRootPath(noteId.ToString());
            string fileFullPath = Path.Combine(pyhsicalDir, fileName);
            string fileRelativePath = _nexusEnvironment.GetNoteImageRelativePathForWeb(noteId.ToString(), fileName);

            if (!Directory.Exists(pyhsicalDir))
                Directory.CreateDirectory(pyhsicalDir);
            try
            {
                using (var fileStream = new FileStream(fileFullPath, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }
                return Json(new
                {
                    uploaded = 1,
                    fileName = $"{fileName}",
                    url = "/" + fileRelativePath
                });
            }
            catch (Exception e)
            {
                return Json(new { uploaded = 0, error = e.Message });
            }
        }

        [HttpPost]
        public IActionResult NotePictureDelete(int noteId, string fileName)
        {
            try
            {
                string physicalDir = _nexusEnvironment.GetNoteImagesRootPath(noteId.ToString());
                string fileFullPath = Path.Combine(physicalDir, fileName);

                System.IO.File.Delete(fileFullPath);

                return Json(new { status = 1 });
            }
            catch(Exception ex)
            {
                return Json(new {status = 0, message = ex.Message});
            }
        }

        public ActionResult Notes_Read([DataSourceRequest]DataSourceRequest request, string searchTerm, int[] selectedCategories, int[] selectedTags)
        {
            if (searchTerm == null)
                searchTerm = string.Empty;
            
            PagedDtoList<NoteDto> filteredNotes = 
                _noteService.FindNotes(searchTerm, selectedCategories, selectedTags, request.Page, request.PageSize);

            return Json(new CustomDataSourceResult
            {
                Data = filteredNotes,
                Total = filteredNotes.TotalItemCount
            });
        }

        //public ActionResult NotePictures_Read([DataSourceRequest] DataSourceRequest request, int noteId)
        //{
        //    //var pics = _pictureService.GetNotePictureFilesNames(noteId).Select(str => new PicContainer
        //    //{
        //    //    FileName = str,
        //    //    FilePath = $"/images/notes/{noteId}/{str}"
        //    //});

        //    //return Json(pics.ToDataSourceResult(request));

        //    return null;
        //}       

        public JsonResult GetCategories()
        {
            return Json(_categoryService.GetAllOrdered().Select(c => new { Id = c.Id, Title = c.Title}));
        }

        public JsonResult GetTags()
        {
            return Json(_tagService.GetAll().ToArray());
        }

        [HttpPost]
        public IActionResult Grid_Delete(NoteViewModel noteVm)
        {
            if (ModelState.IsValid)
            {
                var course = _noteService.Get(noteVm.Id);

                if (course == null)
                    ModelState.AddModelError("NoteNotFound", "No note was found with the given Id.");

                if (ModelState.IsValid)
                {
                    _noteService.Delete(noteVm.Id);

                    if (_noteService.Get(noteVm.Id) == null)
                    {
                        // the note has been deleted successfully, now I need to delete the images of it

                    }
                }
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public IActionResult BrowseNoteImages(int noteId = 7)
        {
            var noteImagesPath  =_nexusEnvironment.GetNoteImagesRootPath(noteId.ToString());

            var imageUrls = new List<string>();

            foreach (var noteImagePath in Directory.GetFiles(noteImagesPath))
            {
                string fileName = Path.GetFileName(noteImagePath);
                imageUrls.Add(_nexusEnvironment.GetNoteImageRelativePathForWeb(noteId.ToString(), fileName));
            }
           
            return View(imageUrls);
        }
    }
    public class CustomDataSourceResult
    {
        public IEnumerable Data { get; set; }
        public int Total { get; set; }
        public object Errors { get; set; }
    }
}