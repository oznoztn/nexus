using System.Collections.Generic;

namespace Nexus.Core.Entities
{
    public class Course : IEntity
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
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
        public Category Category { get; set; }
        public ICollection<CourseCategory> CourseCategories { get; set; }
    }
}