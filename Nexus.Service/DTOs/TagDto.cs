using Nexus.Service.Interfaces;

namespace Nexus.Service.DTOs
{
    public class TagDto : IDto
    {
        public int Id { get; set; }
        public bool IsHidden { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
    }
}