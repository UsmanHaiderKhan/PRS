using System.Collections.Generic;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class ServiceHandler
    {
        private PRSContext db = PRSContext.GetInstance();

        public List<Service> GetAllServices()
        {
            return db.Services.ToList();
        }

        public Service GetServiceById(int id)
        {
            return (from c in db.Services where c.Id == id select c).FirstOrDefault();
        }

        public void DeleteServices(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
            db.SaveChanges();
        }
    }
}
