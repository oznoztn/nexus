using System.ComponentModel.DataAnnotations;

namespace Nexus.Areas.Admin.Models
{
    public class UserInfoViewModel
    {
        [Required]
        [MaxLength(25, ErrorMessage = "{0} should comprise {1} characters maximum.")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string StatusMessage { get; set; }
     }
}