using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nexus.Areas.Admin.Models
{
    public class BookViewModel
    {
        private int[] _categories;

        public int Id { get; set; }

        [Display(Name = "Note (Container) Category")]
        public int? CategoryId { get; set; }

        [Required]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Reading Status")]
        public int ReadingStatusId { get; set; }

        [Required]
        [Display(Name = "Publication Year")]
        public int PublicationYear { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "{0} value must be between {1} and {2}")]
        public int Pages { get; set; }

        [Display(Name = "Year Finished")]
        public int? YearFinished { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max length (100) was exceeded.")]
        public string Title { get; set; }
        [Display(Name = "URL")]
        public string Url { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max length (100) was exceeded.")]
        public string Author { get; set; }

        [Display(Name = "Note Category Slug")]
        public string CategorySlug { get; set; }

        [Display(Name ="Cover Image")]
        public byte[] CoverImage { get; set; }

        // Don't need this in an Edit scenario
        [Display(Name = "Cover Image")]
        public IFormFile CoverImageFile { get; set; }

        [Display(Name = "Is Visible?")]
        public bool IsVisible { get; set; }

        public List<SelectListItem> ReadingStatusSelections { get; set; }

        [Display(Name = "Book Categories")]
        public int[] Categories
        {
            get => _categories ?? new int[0];
            set => _categories = value;
        }
    }
}
