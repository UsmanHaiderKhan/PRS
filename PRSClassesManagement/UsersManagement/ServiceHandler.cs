using System.Collections.Generic;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class ServiceHandler
    {
        PRSContext db = new PRSContext();
        public List<Service> GetAllServices()
        {
            using (db)
            {
                return (from c in db.Services select c).ToList();
            }
        }

        public Service GetServiceById(int id)
        {
            using (db)
            {
                return (from c in db.Services where c.Id == id select c).FirstOrDefault();
            }
        }
        public void DeleteServices(int id)
        {
            using (db)
            {
                Service service = db.Services.Find(id);
                db.Services.Remove(service);
                db.SaveChanges();

            }
        }
    }
}
