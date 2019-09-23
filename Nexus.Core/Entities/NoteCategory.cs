namespace Nexus.Core.Entities
{
    public class NoteCategory : IEntity
    {
        public int Id{ get; set; }
        public int NoteId { get; set; }
        public int CategoryId { get; set; }
        public Note Note { get; set; }
        public Category Category { get; set; }
    }
}