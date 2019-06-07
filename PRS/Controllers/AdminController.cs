using PRSClassesManagement;
using PRSClassesManagement.UsersManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PRS.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllClients()
        {
            User user = (User)Session[WebUtils.CurrentUser];
            if (!(user != null && user.IsInRole(WebUtils.Admin)))
                return RedirectToAction("Login", "Users", new { ctl = "Slider", act = "AddImages" });
            UserHandler h = new UserHandler();
            List<User> u = h.GetUsers();
            return View(u);
        }
        public ActionResult UserDetails(int id)
        {
            User user1 = (User)Session[WebUtils.CurrentUser];
            if (!(user1 != null && user1.IsInRole(WebUtils.Admin)))
                return RedirectToAction("Login", "Users", new { ctl = "Admin", act = "UserDetails" });

            UserHandler user = new UserHandler();
            //List<User> u = user.GetUsers();
            //ViewBag.udetails = u;
            // PRSContext db = new PRSContext();
            List<User> u = user.GetUsersById(id);
            return View(u);
        }
        public ActionResult DeleteUser(int id)
        {
            UserHandler user = new UserHandler();
            user.GetDeleteUsers(id);

            return Json("Delete", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllMessages()

        {
            ContactHandler contactHandler = new ContactHandler();
            List<Contact> contacts = contactHandler.GetAllContactsList();
            return View(contacts);
        }
        public ActionResult DeleteMessage(int id)
        {
            ContactHandler contactHandler = new ContactHandler();
            contactHandler.DeleteUserMessages(id);

            return Json("Delete", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddServices()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddServices(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                PRSContext db = new PRSContext();
                if (ModelState.IsValid)
                {
                    db.Services.Add(service);
                    db.SaveChanges();
                    return RedirectToAction("Thanks", "Admin");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return View();
        }
        [HttpGet]

        public ActionResult GetAllServices()
        {
            try
            {
                ServiceHandler serviceHandler = new ServiceHandler();
                List<Service> services = serviceHandler.GetAllServices();
                return View(services);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public ActionResult DeleteServicePrice(int id)
        {
            ServiceHandler serviceHandler = new ServiceHandler();
            serviceHandler.DeleteServices(id);

            return Json("Delete", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Thanks()
        {
            return View();
        }
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
        //public ActionResult GetCount()
        //{
        //    CountHandler hc = new CountHandler();
        //    int count = hc.GetUserByCount();
        //    return RedirectToAction("Index", "Admin", count);
        //}




    }
}
