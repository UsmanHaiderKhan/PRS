using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class CountHandler
    {
        private PRSContext db = PRSContext.GetInstance();

        public int TotalServices()
        {
            return (from c in db.Services select c).Count();
        }
        public int GetUserByCount()
        {
            return (from a in db.Users select a).Count();
        }
        public int GetMessageCount()
        {
            return (from c in db.Contacts select c).Count();
        }
    }
}
