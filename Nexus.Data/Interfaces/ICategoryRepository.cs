using System;
using System.Collections.Generic;
using Nexus.Core.Entities;

namespace Nexus.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Category, NoteCategory, Note tablolarının inner join'lenmesinin sonucu olan kategoriler,
        /// diğer bir deyişle en az bir tane nota sahip olan kategoriler.
        /// </summary>
        /// <param name="includeHiddenCategories"></param>
        /// <returns></returns>
        IEnumerable<Category> GetInnerCategories(bool includeHiddenCategories);
        IEnumerable<Category> GetBookCategories();
        IEnumerable<Category> GetCourseCategories();
        IEnumerable<Category> GetDefaultCategories();
        IEnumerable<Category> GetDefaultCategoriesOrdered();
        Category GetCategoryBySlug(string slug);
        IEnumerable<Category> GetAllOrdered(CategoryType type);
        IEnumerable<NoteCategory> GetNoteCategories(int noteId);
        IEnumerable<Tuple<Category, int>> GetCategoryNoteCountPairs();
        IEnumerable<Tuple<Category, int>> GetCategoryNoteCountPairs(bool includeHiddenCategories, bool includeHiddenNotes);
        int GetNextDisplayOrder();
    }
}