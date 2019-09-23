using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nexus.Core.Entities;
using Nexus.Service.ServiceInterfaces;


namespace Nexus.Areas.Admin.Models
{
    public class CategoryViewModelFactory
    {
        private readonly ICategoryService _categoryService;

        public CategoryViewModelFactory(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public CategoryViewModel Create()
        {
            return new CategoryViewModel()
            {
                CategoryTypes = GetCategoryTypes(),
                DisplayOrder = _categoryService.GetNextDisplayOrder()
            };
        }

        public CategoryViewModel Create(int categoryId)
        {
            var category = _categoryService.Get(categoryId);

            if (category == null || category.Id == default(int))
                return Create();

            var vm = new CategoryViewModel
            {
                Id = category.Id,
                CategoryTypeId = category.CategoryTypeId,
                ParentId = category.ParentId,
                DisplayOrder = category.DisplayOrder,
                IsVisible = category.IsVisible,
                Title = category.Title,
                Description = category.Description,
                CategoryTypes = GetCategoryTypes()
            };

            return vm;
        }

        public List<SelectListItem> GetCategoryTypes()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Note Category", Value = ((int)CategoryType.Default).ToString()},
                new SelectListItem(){ Text = "Book Category", Value = ((int)CategoryType.Book).ToString()},
                new SelectListItem(){ Text = "Course Category", Value = ((int)CategoryType.Course).ToString()},
            };
        }
    }
}