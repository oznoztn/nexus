using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nexus.Areas.Admin.Models
{
    public class CourseViewModel
    {
        private int[] _categories;
        public int Id { get; set; }

        [Display(Name = "Note (Container) Category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        public int Duration { get; set; }

        [Display(Name = "Finished (in year)")]
        public int YearFinished { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max length (100) was exceeded.")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Course URL")]
        public string Url { get; set; }
        public string Abstract { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max length (100) was exceeded.")]
        public string Author { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max length (100) was exceeded.")]
        public string Provider { get; set; }

        public string CategorySlug { get; set; }

        [Display(Name = "Online Course?")]
        public bool IsOnlineCourse { get; set; }

        [Display(Name = "Visible")]
        public bool IsVisible { get; set; }

        [Display(Name = "Course Categories")]
        public int[] Categories
        {
            get => _categories ?? new int[0];
            set => _categories = value;
        }
    }
}
