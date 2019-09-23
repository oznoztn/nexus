using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Identity.Models;
using Nexus.Service;
using Nexus.Service.DTOs;
using Nexus.Service.ServiceInterfaces;
using Nexus.Shared.Enums;

namespace Nexus.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly INoteService _noteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICategoryService categoryService, INoteService noteService, UserManager<ApplicationUser> userManager)
        {
            _categoryService = categoryService;
            _noteService = noteService;
            _userManager = userManager;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Category(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();

            var category = _categoryService.GetCategoryBySlug(slug);

            if (category == null)
                return NotFound();

            if (!category.IsVisible)
                if (!HttpContext.User.IsInRole(Administrator))
                    return NotFound();

            if (HttpContext.User.IsInRole(Administrator))
            {
                PagedDtoList<NoteDto> notes = _noteService.GetNotesByCategorySlug(Visibility.All, slug, CurrentPage, PageSize);
                ViewBag.CategoryTitle = category.Title;
                return View(notes);
            }
            else
            {
                PagedDtoList<NoteDto> notes = _noteService.GetNotesByCategorySlug(Visibility.Visible, slug, CurrentPage, PageSize);
                ViewBag.CategoryTitle = category.Title;
                return View(notes);
            }
        }
    }
}