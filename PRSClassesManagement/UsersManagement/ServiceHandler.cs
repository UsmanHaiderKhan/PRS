﻿using System.Collections.Generic;
using System.Linq;

namespace PRSClassesManagement.UsersManagement
{
    public class ServiceHandler
    {
        private PRSContext db = PRSContext.GetInstance();

        public List<Service> GetAllServices()
        {
            using (db)
            {
                return db.Services.ToList();
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
