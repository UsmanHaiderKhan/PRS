using PRSClassesManagement.UsersManagement;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;

namespace PRSClassesManagement
{
    public class PRSContext : DbContext
    {
        public static string ConnectionToUse { get; set; } = "constr";
        public PRSContext() : base(ConnectionToUse)
        {
            try
            {
                if (ConnectionToUse == "constr")
                {
                    ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["test"];
                    if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                    {
                        DbConnectionStringBuilder csb = new DbConnectionStringBuilder
                        {
                            ConnectionString = mySetting.ConnectionString
                        };
                    }
                }
            }
            catch (System.Exception)
            {
                ConnectionToUse = "constr2";
            }

            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
