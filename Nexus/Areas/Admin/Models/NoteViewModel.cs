using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Nexus.Service.DTOs;

namespace Nexus.Areas.Admin.Models
{
    public class NoteViewModel
    {
        private int[] _categories;
        //private ICollection<NoteTagDto> _noteTags;
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Abstract { get; set; }
        //public string AvailableTagsJsArray { get; set; }
        //public string TagsJsArray { get; set; }
        public string Content { get; set; }
        [Display(Name = "Is Visible?")]
        public bool IsVisible { get; set; }
        [Display(Name = "Last Update Date")]

        public DateTime? LastUpdateDate { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }
        public int[] Categories
        {
            get => _categories ?? new int[0];
            set => _categories = value;
        }
        //public ICollection<NoteTagDto> NoteTags
        //{
        //    get => _noteTags ?? new List<NoteTagDto>();
        //    set => _noteTags = value;
        //}
        public string Tags { get; set; }
        public string[] AvailableTags { get; set; }
    }
}
