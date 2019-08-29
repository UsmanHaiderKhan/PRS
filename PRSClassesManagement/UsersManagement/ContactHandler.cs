using System.Collections.Generic;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class ContactHandler
    {
        private PRSContext db = PRSContext.GetInstance();

        public List<Contact> GetAllContactsList()
        {
            return (from c in db.Contacts select c).ToList();
        }

        public void DeleteUserMessages(int id)
        {
            Contact u = db.Contacts.Find(id);
            db.Contacts.Remove(u);
            db.SaveChanges();
        }
    }
}
