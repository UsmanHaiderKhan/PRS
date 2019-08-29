using System.Collections.Generic;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class ContactHandler
    {
        PRSContext db = PRSContext.GetInstance();
        public List<Contact> GetAllContactsList()
        {
            using (db)
            {
                return (from c in db.Contacts select c).ToList();
            }

        }
        public void DeleteUserMessages(int id)
        {
            using (db)
            {
                //if any error occured just Uncomment this code
                Contact u = db.Contacts.Find(id);
                db.Contacts.Remove(u);
                db.SaveChanges();

            }
        }


    }
}
