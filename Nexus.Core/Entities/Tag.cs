using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Core.Entities
{
    public class Tag : IEntity
    {
        public int Id { get; set; }
        public bool IsHidden { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; }
    }
}
