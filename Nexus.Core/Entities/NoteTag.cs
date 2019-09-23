namespace Nexus.Core.Entities
{
    public class NoteTag : IEntity
    {
        public int TagId { get; set; }
        public int NoteId { get; set; }
        public Tag Tag { get; set; }
        public Note Note { get; set; }
        public int Id { get; set; }
    }
}