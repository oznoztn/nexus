using System.Collections.Generic;
using Nexus.Core;
using Nexus.Core.Entities;

namespace Nexus.Data.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetBooksByCategory(int categoryId);
        IEnumerable<Book> GetBooksByCategories(int[] categoryIds);
        void UpdateCoverImage(byte[] data, int bookId);
        int GetNextDisplayOrder();
    }
}