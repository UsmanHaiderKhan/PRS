using System;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public DateTime DateofBirth { get; set; }
        public Role Role { get; set; }
        public bool IsInRole(int id)
        {
            return (Role != null && Role.Id == id);
        }
    }
}
