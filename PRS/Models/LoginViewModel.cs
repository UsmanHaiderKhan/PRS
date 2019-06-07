using System.ComponentModel.DataAnnotations;

namespace PRS.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "The Email field is Required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessage = "Email Not Verified !!")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password Is Too Short..!!")]
        public string Password { get; set; }


        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
