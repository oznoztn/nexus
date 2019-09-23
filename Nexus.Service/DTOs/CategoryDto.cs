using Nexus.Service.Interfaces;

namespace Nexus.Service.DTOs
{
    public class CategoryDto : IDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int CategoryTypeId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
