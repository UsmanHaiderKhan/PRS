using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class UserHandler
    {
        private PRSContext db = PRSContext.GetInstance();

        public bool AdminExists()
        {
            return (from c in db.Users
                    .Include(m => m.Role)
                    where c.Role.Rank == 1
                    select c).Count() > 0;
        }

        public List<User> GetUsers()
        {
            var users = (from c in db.Users
                         .Include(m => m.Role)
                         select c).ToList();
            foreach (var user in users)
            {
                user.ConfirmPassword = user.Password;
            }
            return users;
        }

        public User GetUser(string email, string password)
        {
            var user = (from c in db.Users
                        .Include(m => m.Role)
                        where c.Email.Equals(email) && c.Password.Equals(password)
                        select c).FirstOrDefault();
            if (user != null) user.ConfirmPassword = user.Password;
            return user;
        }

        public User GetUserById(int id)
        {
            var user = (from c in db.Users
                        .Include(x => x.Role)
                        where c.Id == id
                        select c).FirstOrDefault();
            if (user != null) user.ConfirmPassword = user.Password;
            return user;
        }

        public User GetUserByEmail(string email)
        {
            var user = (from c in db.Users
                        .Include(x => x.Role)
                        where c.Email == email
                        select c).FirstOrDefault();
            if (user != null) user.ConfirmPassword = user.Password;
            return user;
        }

        public List<User> GetUsersById(int id)
        {
            var users = (from c in db.Users
                        .Include(x => x.Role)
                         where c.Id == id
                         select c).ToList();
            foreach (var user in users)
            {
                user.ConfirmPassword = user.Password;
            }
            return users;
        }

        public void GetDeleteUsers(int id)
        {
            User u = db.Users.Find(id);
            db.Users.Remove(u);
            db.SaveChanges();
        }

        public Role GetRoleById(int id)
        {
            return db.Roles.Find(id);
        }
    }
}
