using System.Collections.Generic;
using Nexus.Service.DTOs;

namespace Nexus.Service
{
    public class BookGroup
    {
        public string CategoryTitle { get; set; }
        public List<BookDto> Books { get; set; }
    }
}