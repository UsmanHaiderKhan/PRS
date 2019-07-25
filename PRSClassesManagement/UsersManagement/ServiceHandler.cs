using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class ServiceHandler
    {
        private PRSContext _db { get; set; } // placeholder for db variable
        private PRSContext db
        {
            get => _db;
            set {
                _db = value;
                try
                {
                    _db.Database.CreateIfNotExists();
                }
                catch (SqlException e)
                {
                    if (PRSContext.ConnectionToUse.Equals("constr"))
                    {
                        PRSContext.ConnectionToUse = "constr2"; // change to constr2
                        _db = new PRSContext();
                    }
                    else
                        throw e;
                }
            }
        }

        public ServiceHandler()
        {
            db = new PRSContext();
        }

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
