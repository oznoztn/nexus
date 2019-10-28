using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;

namespace Nexus.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(NexusContext context) : base(context)
        {

        }

        public CategoryRepository()
        {

        }

        public IEnumerable<NoteCategory> GetNoteCategories(int noteId)
        {
            return Context.Set<NoteCategory>().AsNoTracking().Where(nc => nc.NoteId == noteId).AsEnumerable();
        }

        public int GetNextDisplayOrder()
        {
            if (!Set.Any())
                return 1;

            return Set.Max(e => e.DisplayOrder) + 1;
        }

        public IEnumerable<Category> GetDefaultCategoriesOrdered()
        {
            return GetCategoriesOrderedInternal(CategoryType.Default).AsEnumerable();
        }

        private IQueryable<Category> GetCategoriesOrderedInternal(CategoryType type)
        {
            var query = type == CategoryType.Any ? Set : Set.Where(c => c.CategoryTypeId == (int)type);
            return query
                .OrderBy(c => c.CategoryTypeId)
                .ThenBy(c => c.ParentId)
                .ThenBy(c => c.DisplayOrder)
                .ThenBy(c => c.Id);
        }

        public Category GetCategoryBySlug(string slug)
        {
            return Set.FirstOrDefault(cat => cat.Slug == slug);
        }

        public IEnumerable<Category> GetAllOrdered(CategoryType type)
        {
            return GetCategoriesOrderedInternal(type).AsEnumerable();
        }

        public IEnumerable<Tuple<Category, int>> GetCategoryNoteCountPairs()
        {
            IQueryable<Category> categoriesQuery =
                Context.Set<Category>().Where(t => t.CategoryTypeId == (int)CategoryType.Default);
            DbSet<NoteCategory> noteCategoriesQuery = Context.Set<NoteCategory>();

            // category dediğimde neye göre grupluyor, def olarak Id alanına göre mi?
            var query =
                from category in categoriesQuery
                join notecategory in noteCategoriesQuery on category.Id equals notecategory.CategoryId
                orderby category.DisplayOrder
                group notecategory by category into grup
                select new Tuple<Category, int>(grup.Key, grup.Count());

            return query.AsEnumerable();
        }

        public IEnumerable<Tuple<Category, int>> GetCategoryNoteCountPairs(bool includeHiddenCategories, bool includeHiddenNotes)
        {
            IQueryable<Category> categoriesQuery =
                Context.Set<Category>().Where(t => t.CategoryTypeId == (int)CategoryType.Default);

            categoriesQuery = includeHiddenCategories ? categoriesQuery : categoriesQuery.Where(c => c.IsVisible);
            
            IQueryable<Note> notesQuery =
                includeHiddenNotes 
                    ? Context.Set<Note>() 
                    : Context.Set<Note>().Where(n => n.IsVisible);

            // LINQ queries are no longer evaluated on the client
            // https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes
            //var query =
            //    from category in categoriesQuery
            //    join notecategory in Context.Set<NoteCategory>() on category.Id equals notecategory.CategoryId
            //    join note in notesQuery on notecategory.NoteId equals note.Id
            //    orderby category.DisplayOrder
            //    group notecategory by category into grup
            //    select new Tuple<Category, int>(grup.Key, grup.Count());

            var query =
                from category in categoriesQuery
                join notecategory in Context.Set<NoteCategory>() on category.Id equals notecategory.CategoryId
                join note in notesQuery on notecategory.NoteId equals note.Id
                orderby category.DisplayOrder
                group notecategory by new { category.Id, category.Title, category.Slug } into grup
                select new Tuple<Category, int>(new Category { Id = grup.Key.Id, Title = grup.Key.Title, Slug = grup.Key.Slug }, grup.Count());

            return query.ToArray();
        }

        public IEnumerable<Category> GetInnerCategories(bool includeHiddenCategories)
        {
            var categories = 
                includeHiddenCategories 
                    ? GetCategoriesOrderedInternal(CategoryType.Default) 
                    : GetCategoriesOrderedInternal(CategoryType.Default).Where(t => t.IsVisible);
            var notes = 
                includeHiddenCategories 
                    ? Context.Set<Note>() 
                    : Context.Set<Note>().Where(no => no.IsVisible);

            var query =
                (from c in categories
                    join nc in Context.Set<NoteCategory>() on c.Id equals nc.CategoryId
                    join nt in notes on nc.NoteId equals nt.Id
                    select c)
                .OrderBy(c => c.CategoryTypeId)
                .ThenBy(c => c.ParentId)
                .ThenBy(c => c.DisplayOrder)
                .ThenBy(c => c.Id)
                .Distinct();

            return query.AsEnumerable();
        }

        public IEnumerable<Category> GetBookCategories()
        {
            return Set.Where(t => t.CategoryTypeId == (int)CategoryType.Book);
        }

        public IEnumerable<Category> GetCourseCategories()
        {
            return Set.Where(t => t.CategoryTypeId == (int)CategoryType.Course);
        }

        public IEnumerable<Category> GetDefaultCategories()
        {
            return Set.Where(t => t.CategoryTypeId == (int)CategoryType.Default);
        }
    }
}