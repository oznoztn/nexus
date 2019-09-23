using System.Collections.Generic;
using Nexus.Service.Interfaces;

namespace Nexus.Service.DTOs
{
    public class CourseDto : IDto
    {
        private ICollection<CourseCategoryDto> _courseCategories;
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public bool IsCategoryVisible { get; set; }
        public int DisplayOrder { get; set; }
        public int Duration { get; set; }
        public int YearFinished { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public string Abstract { get; set; }
        public string Author { get; set; }
        public string Provider { get; set; }
        public bool IsOnlineCourse { get; set; }
        public bool IsVisible { get; set; }

        public ICollection<CourseCategoryDto> CourseCategories
        {
            get => _courseCategories ?? (_courseCategories = new List<CourseCategoryDto>());
            set => _courseCategories = value;
        }
    }
}