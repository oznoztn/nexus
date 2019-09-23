using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nexus.Areas.Admin.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }

        [Display(Name="Avocational")]
        public bool IsAvocational { get; set; }

        [Display(Name = "Is Visible")]
        public bool IsVisible { get; set; }

        [Display(Name = "Ongoing Project")]
        public bool IsOngoingProject { get; set; }
    
        [Required]
        [StringLength(250, ErrorMessage = "Max length ({1}) for {0} was exceeded.")]
        public string Title { get; set; }

        public string Slug { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }

        [Display(Name = "Project URL")]
        public string URL { get; set; }

        [Required]
        public int MonthFrom { get; set; }

        [Required]
        public int YearFrom { get; set; }

        public int? MonthTo { get; set; }
        public int? YearTo { get; set; }

        public List<SelectListItem> Months { get; set; }
        public List<SelectListItem> YearsFromList { get; set; }
        public List<SelectListItem> YearsToList { get; set; }
    }
}
