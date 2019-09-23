using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nexus.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [DisplayName("Parent Category")]
        public int? ParentId { get; set; }

        [DisplayName("Display Order")]
        [Range(0, int.MaxValue, ErrorMessage = "The value must be positive.")]
        public int DisplayOrder { get; set; }

        [DisplayName("Category Type")]
        public int CategoryTypeId { get; set; }

        public string Slug { get; set; }

        [Required]
        [DisplayName("Is Visible?")]
        public bool IsVisible { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max length ({1}) was exceeded.")]
        public string Title { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> CategoryTypes { get; set; }
    }
}