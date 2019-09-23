using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;

namespace Nexus.Data.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(NexusContext context) : base(context)
        {

        }

        public override Book Get(int id)
        {
            return Set.AsNoTracking().Include(b => b.BookCategories).SingleOrDefault(b => b.Id == id);
        }

        public override void Update(Book book)
        {
            var currentCategories = Context.Set<BookCategory>().Where(t => t.BookId == book.Id).ToList();
            var incomingCategories = book.BookCategories.ToList();

            // TODO: learn how to implement IEqualityComparer<T>
            var currentIds = currentCategories.Select(t => t.CategoryId).ToList();
            var incomingIds = incomingCategories.Select(t => t.CategoryId).ToList();

            var addedIds = incomingIds.Except(currentIds).ToArray();
            var removedIds = currentIds.Except(incomingIds).ToArray();

            Context.Set<BookCategory>().AddRange(addedIds.Select(addedCatId => new BookCategory { CategoryId = addedCatId, BookId = book.Id }));
            foreach (var removedId in removedIds)
            {
                var removed = Context.Set<BookCategory>().First(t => t.CategoryId == removedId && t.BookId == book.Id);
                Context.Set<BookCategory>().Remove(removed);
            }

            book.BookCategories = null;

            Set.Update(book);
            Context.SaveChanges();
        }

        public IEnumerable<Book> GetBooksByCategory(int categoryId)
        {
            var query =
                from book in Context.Set<Book>().AsNoTracking()
                join bcategory in Context.Set<BookCategory>().AsNoTracking() on book.Id equals bcategory.BookId
                where bcategory.CategoryId == categoryId
                select book;

            return query.AsEnumerable();
        }

        public IEnumerable<Book> GetBooksByCategories(int[] categoryIds)
        {
            var query =
                from course in Context.Set<Book>().AsNoTracking()
                join ccategory in Context.Set<BookCategory>().AsNoTracking().Where(bc => categoryIds.Any(id => id == bc.CategoryId))
                    on course.Id equals ccategory.BookId
                select course;

            return query.Distinct().AsEnumerable();
        }

        public void UpdateCoverImage(byte[] data, int bookId)
        {
            //// book is not tracked by the change tracker so the save call will not work
            //Book book = this.Get(bookId);
            //book.CoverImage = data;
            //this.UnitOfWork.SaveChanges();

            Book book = Context.Set<Book>().FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                book.CoverImage = data;
                Context.SaveChanges();
            }
        }

        public int GetNextDisplayOrder()
        {
            if (!Set.Any())
                return 1;

            return Set.Max(e => e.DisplayOrder) + 1;
        }
    }
}
