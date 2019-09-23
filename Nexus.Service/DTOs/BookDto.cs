using System.Collections.Generic;
using Nexus.Service.Interfaces;

namespace Nexus.Service.DTOs
{
    public class BookDto : IDto
    {
        private ICollection<BookCategoryDto> _bookCategories;
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public bool IsCategoryVisible { get; set; }
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

        public ICollection<BookCategoryDto> BookCategories
        {
            get => _bookCategories ?? (_bookCategories = new List<BookCategoryDto>());
            set => _bookCategories = value;
        }                                         
    }
}
