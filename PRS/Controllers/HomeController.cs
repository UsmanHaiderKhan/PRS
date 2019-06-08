using PRSClassesManagement.UsersManagement;
using System;
using System.Web.Mvc;

namespace PRS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    //PRSContext db = new PRSContext();
                    //User user = new UserHandler().GetUserByEmail(contact.Email);
                    //var sub = contact.Name;
                    //var text = contact.Message;
                    //var message = new MailMessage();
                    //message.To.Add(new MailAddress(contact.Email));
                    //message.Subject = "Contact For the Query";
                    //message.Body = $"Dear:{sub} Your Message is :{message}. We will inform you as soon as possible";
                    //message.IsBodyHtml = true;
                    //using (var smtp = new SmtpClient())
                    //{
                    //    smtp.Send(message);
                    //    db.Contacts.Add(contact);
                    //    db.SaveChanges();
                    //}
                    return RedirectToAction("Message", "Home");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Message()
        {
            return View();
        }
    }
}
