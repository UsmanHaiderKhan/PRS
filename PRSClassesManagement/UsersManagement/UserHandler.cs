using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class UserHandler
    {
        PRSContext db = new PRSContext();
        public List<User> GetUsers()
        {
            using (db)
            {
                return (from c in db.Users.Include(m => m.Role) select c).ToList();
            }
        }

        public User GetUser(string email, string password)
        {
            using (db)
            {
                return (from c in db.Users.Include(m => m.Role)
                        where c.Email.Equals(email) && c.Password.Equals(password)
                        select c).FirstOrDefault();
            }
        }
        public User GetUserById(int id)
        {

            using (db)
            {
                return (from c in db.Users
                        .Include(x => x.Role)
                        where c.Id == id
                        select c).FirstOrDefault();
            }
        }
        public User GetUserByEmail(string email)
        {
            using (db)
            {
                return (from c in db.Users where c.Email == email select c).FirstOrDefault();
            }
        }
        public List<User> GetUsersById(int id)
        {

            using (db)
            {
                return (from c in db.Users
                        .Include(x => x.Role)
                        where c.Id == id
                        select c).ToList();
            }
        }
        public void GetDeleteUsers(int id)
        {
            using (db)
            {
                //if any error occured just Uncomment this code

                User u = db.Users.Find(id);
                db.Users.Remove(u);
                db.SaveChanges();

            }
        }
        public Role GetRoleById(int id)
        {
            using (db)
            {
                //if any error occured just Uncomment this code

                Role u = db.Roles.Find(id);
                return u;


            }
        }

    }
}
