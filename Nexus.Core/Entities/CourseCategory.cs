namespace Nexus.Core.Entities
{
    public class CourseCategory : IEntity
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int CategoryId { get; set; }
        public Course Course { get; set; }
        public Category Category { get; set; }
    }
}