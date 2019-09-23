using Microsoft.AspNetCore.Mvc;
using Nexus.Service;
using Nexus.Service.DTOs;
using Nexus.Service.ServiceInterfaces;
using Nexus.Shared.Enums;

namespace Nexus.Controllers
{
    public class TagsController : BaseController
    {
        private readonly INoteService _noteService;
        private readonly ITagService _tagService;

        public TagsController(INoteService noteService, ITagService tagService)
        {
            _noteService = noteService;
            _tagService = tagService;
        }

        public IActionResult Index(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return BadRequest();

            var tagDto = _tagService.GetBySlug(slug);

            if (tagDto == null)
                return NotFound();

            if (tagDto.IsHidden)
                if (!HttpContext.User.IsInRole(Administrator))
                    return NotFound();

            if (HttpContext.User.IsInRole(Administrator))
            {
                PagedDtoList<NoteDto> notes = _noteService.FindNotesByTagSlug(Visibility.All, slug, CurrentPage, PageSize);
                ViewBag.SelectedTag = tagDto.Title;
                return View(notes);
            }
            else
            {
                PagedDtoList<NoteDto> notes = _noteService.FindNotesByTagSlug(Visibility.Visible, slug, CurrentPage, PageSize);
                ViewBag.SelectedTag = tagDto.Title;
                return View(notes);
            }
        }
    }
}