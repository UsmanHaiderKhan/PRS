using PRS.Models;
using PRSClassesManagement;
using PRSClassesManagement.UsersManagement;
using System;
using System.Data.Entity;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PRS.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index(int id)
        {
            User user = new UserHandler().GetUserById(id);
            return View(user);
        }
        [HttpGet]
        public ActionResult Login()
        {
            HttpCookie myCookie = Request.Cookies["logsec"];
            if (myCookie != null)
            {
                User user = new UserHandler().GetUser(myCookie.Values["logid"], myCookie.Values["psd"]);
                if (user != null)
                {
                    myCookie.Expires = DateTime.Now.AddDays(7);
                    Session.Add(WebUtils.CurrentUser, user);
                }
            }
            ViewBag.Controller = Request.QueryString["ctl"];
            ViewBag.Action = Request.QueryString["act"];
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            User u = new UserHandler().GetUser(viewModel.Email, viewModel.Password);
            if (u != null)
            {
                if (viewModel.RememberMe)
                {
                    HttpCookie cookie = new HttpCookie("logsec");
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.Values.Add("logid", u.UserName);
                    cookie.Values.Add("psd", u.Password);
                    Response.SetCookie(cookie);
                }
                Session.Add(WebUtils.CurrentUser, u);
                string ctl = Request.QueryString["c"];
                string act = Request.QueryString["a"];
                if (!string.IsNullOrEmpty(ctl) && string.IsNullOrEmpty(act))
                {
                    return RedirectToAction(ctl, act);
                }

                if (u.IsInRole(WebUtils.Admin))
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Your LoginId and Password are Wrong..Please try Again!";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            try
            {
                long uno = DateTime.Now.Ticks;
                int counter = 0;
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        string abc = uno + "_" + ++counter +
                                     file.FileName.Substring(file.FileName.LastIndexOf("."));
                        //dont save the url of the Images Save the 
                        string url = "~/Content/UserImages/" + abc;
                        string path = Request.MapPath(url);
                        user.ImageUrl = abc;
                        file.SaveAs(path);
                    }
                }
                user.Role = new Role { Id = 2 };
                PRSContext db = new PRSContext();
                using (db)
                {
                    db.Users.Add(user);
                    db.Entry(user.Role).State = EntityState.Unchanged;
                    db.SaveChanges();
                }
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            return View();
        }
        [HttpPost]

        public ActionResult PasswordRecovery(Email data)
        {
            try
            {
                PRSContext db = new PRSContext();
                if (ModelState.IsValid)

                {
                    User user = new UserHandler().GetUserByEmail(data.email);
                    var sub = user.UserName + " Password Recovered";
                    string c = Path.GetRandomFileName().Replace(".", "");
                    user.Password = Convert.ToString(c);
                    user.ConfirmPassword = Convert.ToString(c);
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(data.email));
                    message.Subject = "Reply For The Password Recovery From Personal Recomended System";
                    message.Body = $"Dear:{sub} Please Login next time with This Given Password:" + c;
                    message.IsBodyHtml = false;
                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(message);
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Success = "Email has been Sent To You Successfully!";
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Error Sending Email.  Try Again Later.Ooop's";
            }

            return View();
        }

        [HttpGet]
        public ActionResult ProfileSetting(int id)
        {
            User u = new UserHandler().GetUserById(id);
            return View(u);
        }
        [HttpPost]
        public ActionResult ProfileSetting(User u)
        {
            PRSContext db = new PRSContext();

            if (ModelState.IsValid)
            {
                using (db)
                {
                    db.Entry(u).State = EntityState.Modified;
                    db.SaveChanges();
                    Session.Add(WebUtils.CurrentUser, new UserHandler().GetUserById(u.Id));
                }
            }
            return RedirectToAction("ProfileSetting", "Users");
        }
        [HttpGet]
        public ActionResult ChangePassword(int id)
        {
            ViewBag.userId = id;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection formdata, int id)
        {
            PRSContext db = new PRSContext();
            using (db)
            {
                User user = db.Users.Find(id);
                if (user != null)
                {

                    user.Password = formdata["Password"];
                    user.ConfirmPassword = formdata["ConfirmPassword"];
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("login", "Users");
                }
            }
            return View();
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            HttpCookie hc = Request.Cookies["logsec"];
            if (hc != null)
            {
                hc.Expires = DateTime.Now;
                Response.SetCookie(hc);
            }

            return RedirectToAction("Login");
        }

    }
}
