using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Areas.Admin.Models
{
    public class BookViewModelFactory
    {
        private readonly IBookService _bookService;

        public BookViewModelFactory(IBookService bookService)
        {
            _bookService = bookService;
        }

        public BookViewModel Create()
        {
            var vm = new BookViewModel();
            vm.ReadingStatusSelections = CreateReadingStatusList();
            vm.DisplayOrder = _bookService.GetNextDisplayOrder();
            vm.IsVisible = true;
            return vm;
        }

        public List<SelectListItem> CreateReadingStatusList()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Read" },
                new SelectListItem { Value = "2", Text = "Currently Reading" },
                new SelectListItem { Value = "3", Text = "To Read / Queued" }
            };
        }
    }
}