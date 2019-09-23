using System;
using System.Collections.Generic;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;

namespace Nexus.Service.ServiceInterfaces
{
    public interface ICategoryService : IService<Category, CategoryDto>
    {

        IEnumerable<CategoryDto> GetInnerCategories(bool includeHiddenCategories);
        IEnumerable<CategoryDto> GetBookCategories();
        IEnumerable<CategoryDto> GetCourseCategories();
        IEnumerable<CategoryDto> GetDefaultCategories();
        IEnumerable<CategoryDto> GetDefaultCategoriesOrdered();
        IEnumerable<Tuple<CategoryDto, int>> GetCategoryNoteCountPairs();

        IEnumerable<Tuple<CategoryDto, int>> GetCategoryNoteCountPairs(bool includeHiddenCategories, bool includeHiddenNotes);
        CategoryDto GetCategoryBySlug(string slug);
        IEnumerable<CategoryDto> GetAllOrdered();
        IEnumerable<CategoryDto> GetCategoriesTree(bool hasDefaultSelectionItem = true);
        int GetNextDisplayOrder();
    }
}