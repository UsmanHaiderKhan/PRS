using PRSClassesManagement.UsersManagement;
using System.Data.Entity;

namespace PRSClassesManagement
{
    public class PRSContext : DbContext
    {
        public static string ConnectionToUse { get; set; } = "constr";
        public PRSContext() : base(ConnectionToUse)
        {
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
