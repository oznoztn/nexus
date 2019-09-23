using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
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
    public class BooksController : AdminBaseController
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IMessageProvider _messageProvider;
        private readonly BookViewModelFactory _bookViewModelFactory;

        public BooksController(IMapper mapper, IBookService bookService, ICategoryService categoryService, IMessageProvider messageProvider)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _messageProvider = messageProvider;
            _mapper = mapper;
            // TODO: DI, maybe?
            _bookViewModelFactory = new BookViewModelFactory(_bookService);
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

            var dto = _bookService.Get(id);

            if (dto == null)
                return NotFound();

            var vm = _mapper.Map<BookViewModel>(dto);
            vm.ReadingStatusSelections = _bookViewModelFactory.CreateReadingStatusList();

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel vm)
        {
            if (ModelState.IsValid)
            {
                BookDto bookDto = _bookService.Get(vm.Id);
                bookDto = _mapper.Map(vm, bookDto);

                //if (vm.CoverImageFile != null && vm.CoverImageFile.Length > 0)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        updated.CoverImageMime = vm.CoverImageFile.ContentType;
                //        vm.CoverImageFile.CopyTo(memoryStream);
                //        updated.CoverImage = memoryStream.ToArray();
                //    }
                //}

                _bookService.Update(bookDto);
                SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Update, "book"));
                return RedirectToAction("Edit", new { id = bookDto.Id });
            }

            vm.ReadingStatusSelections = _bookViewModelFactory.CreateReadingStatusList();
            return View(vm);
        }

        public IActionResult New()
        {
            return View(_bookViewModelFactory.Create());
        }

        [HttpPost]
        public IActionResult New(BookViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //if (vm.CoverImageFile != null && vm.CoverImageFile.Length > 0)
                //{
                //    // todo: validation practise can be introduced
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        vm.CoverImageFile.CopyTo(memoryStream);
                //        bookDto.CoverImage = memoryStream.ToArray();
                //        bookDto.CoverImageMime = vm.CoverImageFile.ContentType;
                //    }
                //}

                BookDto bookDto = _mapper.Map<BookDto>(vm);
                _bookService.Add(bookDto);

                if (bookDto.Id != default(int))
                {
                    SetClientSideNotificationMessage(_messageProvider.SuccessMessage(OperationType.Create, "book"));
                    return RedirectToAction("Edit", "Books", new { id = bookDto.Id });
                }
            }

            vm.ReadingStatusSelections = _bookViewModelFactory.CreateReadingStatusList();            
            return View(vm);
        }

        public ActionResult Books_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_bookService.GetAll().ToDataSourceResult(request));
        }

        //[HttpPost]
        //public ActionResult Submit(IFormFile formFile, int bookId)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        formFile.CopyTo(memoryStream);
        //        byte[] data = memoryStream.ToArray();

        //        var status = _bookService.UpdateCoverImage(data, bookId);

        //        if (!status.IsSuccessful)
        //            return Content(status.Error);

        //        return Content(string.Empty);
        //    }
        //}

        public JsonResult GetCategories()
        {
            return Json(_categoryService.GetBookCategories().Select(c => new { Id = c.Id, Title = c.Title }));
        }

        public JsonResult GetBookNoteCategories()
        {
            var bookCategories = _categoryService.GetBookCategories();

            var categories = new List<CategoryDto>();

            categories.Add(new CategoryDto() { Id = 0, CategoryTypeId = (int)CategoryType.Default, Title = "[None]" });
            categories.AddRange(bookCategories);

            return Json(categories.Select(c => new { Id = c.Id, Title = c.Title }));
        }

        [HttpPost]
        public ActionResult Grid_Delete(BookViewModel bookVm)
        {
            if (ModelState.IsValid)
            {
                var course = _bookService.Get(bookVm.Id);

                if (course == null)
                    ModelState.AddModelError("BookNotFound", "No book was found with the given Id.");

                if (ModelState.IsValid)
                    _bookService.Delete(bookVm.Id);
            }

            return Json(ModelState.ToDataSourceResult());
        }
    }
}