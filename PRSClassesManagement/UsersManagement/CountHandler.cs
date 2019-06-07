using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class CountHandler
    {
        public int TotalServices()
        {
            PRSContext db = new PRSContext();
            using (db)
            {
                return (from c in db.Services select c).Count();
            }

        }
        public int GetUserByCount()
        {
            PRSContext db = new PRSContext();
            using (db)
            {
                return (from a in db.Users select a).Count();
            }
        }
        public int GetMessageCount()
        {
            PRSContext db = new PRSContext();
            using (db)
            {
                return (from c in db.Contacts select c).Count();
            }

        }
    }
}
