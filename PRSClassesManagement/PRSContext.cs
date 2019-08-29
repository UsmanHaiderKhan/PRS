using PRSClassesManagement.UsersManagement;
using System.Data.Entity;
using System.Data.SqlClient;

namespace PRSClassesManagement
{
    public class PRSContext : DbContext
    {
        public static string ConnectionToUse { get; set; } = "constr";

        // Using a factory pattern to make constructor private and return instance only by calling GetInstance()
        private PRSContext() : base(ConnectionToUse)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public static PRSContext GetInstance()
        {
            PRSContext db = new PRSContext();
            try
            {
                db.Database.CreateIfNotExists();
            }
            catch (SqlException)
            {
                PRSContext.ConnectionToUse = "constr2"; // change to constr2
                db = new PRSContext();
                db.Database.CreateIfNotExists();
            }

            return db;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
