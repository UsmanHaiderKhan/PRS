using PRSClassesManagement.UsersManagement;
using System.Data.Entity;

namespace PRSClassesManagement
{
    public class PRSContext : DbContext
    {
        public PRSContext() : base("constr")
        {
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
