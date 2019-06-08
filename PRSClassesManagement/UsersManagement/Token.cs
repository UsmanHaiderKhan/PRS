using System;
using System.ComponentModel.DataAnnotations;

namespace PRSClassesManagement.UsersManagement
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public string Key { get; set; }
        public DateTime ExpiryDT { get; set; }
    }
}
