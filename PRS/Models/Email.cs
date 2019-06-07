using System.ComponentModel.DataAnnotations;

namespace PRS.Models
{
    public class Email
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }
    }
}
