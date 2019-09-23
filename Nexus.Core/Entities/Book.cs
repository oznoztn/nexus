using System.Collections;
using System.Collections.Generic;

namespace Nexus.Core.Entities
{
    public class Book : IEntity
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public int ReadingStatusId { get; set; }
        public int PublicationYear { get; set; }
        public int Pages { get; set; }
        public int? YearFinished { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Author { get; set; }
        public byte[] CoverImage { get; set; }
        public string CoverImageMime { get; set; }
        public bool IsVisible { get; set; }
        public Category Category { get; set; }
        public ReadingStatus ReadingStatus { get; set; }
        public ICollection<BookCategory> BookCategories { get; set; }
    }
}