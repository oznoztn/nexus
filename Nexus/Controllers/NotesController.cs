using Microsoft.AspNetCore.Mvc;
using Nexus.Service;
using Nexus.Service.DTOs;
using Nexus.Service.ServiceInterfaces;
using Nexus.Shared;
using Nexus.Shared.Enums;

namespace Nexus.Controllers
{
    public class NotesController : BaseController
    {
        private readonly INoteService _noteService;
        private readonly ICategoryService _categoryService;

        public NotesController(INoteService noteService, ICategoryService categoryService)
        {
            _noteService = noteService;
            _categoryService = categoryService;
        }
        
        public IActionResult Index()
        {
            NoteViewModel vm = new NoteViewModel();
            //vm.NoteDates = _noteService.GetNoteDates();

            if (HttpContext.User.IsInRole(Administrator))
                vm.Notes = _noteService.GetNotes(Visibility.All, true, CurrentPage, PageSize);
            else
                vm.Notes = _noteService.GetNotes(Visibility.Visible, false, CurrentPage, PageSize);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Note(int id, string slug)
        {
            if (id == default(int))
                return NotFound();

            NoteDto note = _noteService.Get(id, honorVisibilityRule: !HttpContext.User.IsInRole(Administrator));

            if (note == null)
                return NotFound();

            if (!note.IsVisible)
                if (!HttpContext.User.IsInRole(Administrator))
                    return NotFound();

            if (slug != note.Slug)
                return RedirectToRoutePermanent("notes", new { id = note.Id, slug = note.Slug });

            return View(note);
        }

        public IActionResult Archive(int year, int month)
        {
            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;
            
            PagedDtoList<NoteDto> notes = 
                _noteService.GetNotes(year, month, CurrentPage, PageSize);

            return View(notes);
        }
    }
}