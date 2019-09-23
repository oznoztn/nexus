using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nexus.Service.ServiceInterfaces;
namespace Nexus.Controllers
{
    public class BooksController : BaseController
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            var bookGroups = _bookService.GetBookGroups();

            return View(bookGroups);
        }
    }
}