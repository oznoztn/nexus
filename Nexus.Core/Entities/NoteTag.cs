﻿namespace Nexus.Core.Entities
{
    public class NoteTag : IEntity
    {
        // TODO: Id is ignored, fix this problem.
        public int Id { get; set; }
        public int TagId { get; set; }
        public int NoteId { get; set; }
        public Tag Tag { get; set; }
        public Note Note { get; set; }
        
    }
}