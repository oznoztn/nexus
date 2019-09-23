using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Core.Entities
{
    public class Note : IEntity
    {
        private ICollection<NoteCategory> _noteCategories;
        private ICollection<NoteTag> _noteTags;
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public bool IsVisible { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<NoteTag> NoteTags
        {
            get => _noteTags ?? (_noteTags = new HashSet<NoteTag>());
            set => _noteTags = value;
        }

        public ICollection<NoteCategory> NoteCategories
        {
            get => _noteCategories ?? (_noteCategories = new HashSet<NoteCategory>());
            set => _noteCategories = value;
        }
    }
}
