using System.Collections.Generic;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;

namespace Nexus.Service.ServiceInterfaces
{
    public interface IBookService : IService<Book, BookDto>
    {
        List<BookGroup> GetBookGroups();
        ImageUploadStatus UpdateCoverImage(byte[] data, int bookId);
        int GetNextDisplayOrder();
    }
}
