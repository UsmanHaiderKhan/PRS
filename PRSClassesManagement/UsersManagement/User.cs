using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSClassesManagement.UsersManagement
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "User Name is Required...!")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email Not Verified !!")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped] // Will not create column for this property in database
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword {
            get => string.IsNullOrEmpty(_ConfirmPassword) ? "" : _ConfirmPassword;
            set => _ConfirmPassword = value;
        }
        public string ImageUrl
        {
            get => string.IsNullOrEmpty(_ImageUrl)? "../../Content/Images/user-circle.png" : _ImageUrl;
            set => _ImageUrl = value;
        }
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateofBirth { get; set; }
        [Required]
        public Role Role { get; set; }

        // Placeholders
        [NotMapped]
        private string _ConfirmPassword { get; set; }
        [NotMapped]
        private string _ImageUrl { get; set; }
    }
}
