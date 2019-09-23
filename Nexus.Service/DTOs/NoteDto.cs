using System;
using System.Collections.Generic;
using Nexus.Service.Interfaces;

namespace Nexus.Service.DTOs
{
    public class NoteDto : IDto
    {
        ICollection<NoteCategoryDto> _noteCategories;
        ICollection<NoteTagDto> _noteTags;
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public bool IsVisible { get; set; }
        public SlimNoteDto PreviousNote { get; set; }
        public SlimNoteDto NextNote { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<NoteCategoryDto> NoteCategories
        {
            get => _noteCategories ?? (_noteCategories = new List<NoteCategoryDto>());
            set => _noteCategories = value;
        }
        public ICollection<NoteTagDto> NoteTags
        {
            get => _noteTags ?? (_noteTags = new List<NoteTagDto>());
            set => _noteTags = value;
        }
    }
}