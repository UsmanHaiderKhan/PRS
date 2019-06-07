using System.ComponentModel.DataAnnotations;

namespace PRSClassesManagement.UsersManagement
{
    public class Service
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
