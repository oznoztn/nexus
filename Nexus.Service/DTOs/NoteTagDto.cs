using Nexus.Service.Interfaces;

namespace Nexus.Service.DTOs
{
    public class NoteTagDto : IDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public bool IsHidden { get; set; }
        public int TagId { get; set; }
        public int NoteId { get; set; }
    }
}