namespace Nexus.Service.DTOs
{
    public class NoteCategoryDto
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public int CategoryId { get; set; }
        public bool IsVisible { get; set; }
        public string CategoryTitle { get; set; }
        public string CategorySlug { get; set; }
    }
}
