using System;
using System.ComponentModel.DataAnnotations;

namespace PRSClassesManagement.UsersManagement
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public DateTime ExpiryDT { get; set; }
    }
}
