using PRS.Models;
using PRSClassesManagement;
using PRSClassesManagement.Helpers;
using PRSClassesManagement.UsersManagement;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PRS.Controllers
{
    public class UsersController : Controller
    {
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
                    Session.Add(WebUtils.CurrentUser, user);
                else
                    myCookie.Expires = DateTime.Now;
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
            User u = new UserHandler().GetUser(viewModel.Email, HelperMethods.Sha256(viewModel.Password));
            if (u != null)
            {
                HttpCookie cookie = new HttpCookie("logsec");
                cookie.Values.Add("logid", u.UserName);
                cookie.Values.Add("psd", u.Password);

                if (viewModel.RememberMe)
                {
                    cookie.Expires = DateTime.Now.AddDays(7);
                }
                Response.SetCookie(cookie);
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
                var uh = new UserHandler();
                if (uh.AdminExists())
                    user.Role = uh.GetRoleById(2);
                else
                    user.Role = uh.GetRoleById(2);
                user.Password = HelperMethods.Sha256(user.Password);
                user.ConfirmPassword = HelperMethods.Sha256(user.ConfirmPassword);
                PRSContext db = PRSContext.GetInstance();
                using (db)
                {
                    db.Users.Add(user);
                    db.Entry(user.Role).State = EntityState.Unchanged;
                    db.SaveChanges();
                }
                return RedirectToAction("Login");
            }
            catch (DbEntityValidationException e)
            {
                string text = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    text = String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:{2}", eve.Entry.Entity.GetType().Name, eve.Entry.State, Environment.NewLine);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        text = String.Format("{0}{0}- Property: \"{1}\", Error: \"{2}\"", Environment.NewLine, ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return Content(text);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
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
                PRSContext db = PRSContext.GetInstance();
                if (ModelState.IsValid)
                {
                    User user = new UserHandler().GetUserByEmail(data.email);
                    string c = Path.GetRandomFileName().Replace(".", "");
                    user.Password = HelperMethods.Sha256(Convert.ToString(c));
                    user.ConfirmPassword = user.Password;
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(data.email));
                    message.Subject = "Your new password for PRS";
                    message.Body = $"Dear {user.UserName}\n" +
                        $"You password have been changed. Your new password is: {c}\n" +
                        $"Please change your password after logging in.\n" +
                        $"Thanks";
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
                ViewBag.Error = "Error Sending Email. Try Again Later.";
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
            PRSContext db = PRSContext.GetInstance();

            if (ModelState.IsValid)
            {
                using (db)
                {
                    if (!string.IsNullOrEmpty(u.Password)) u.Password = HelperMethods.Sha256(u.Password);
                    if (!string.IsNullOrEmpty(u.ConfirmPassword)) u.ConfirmPassword = HelperMethods.Sha256(u.ConfirmPassword);
                    if (u.ImageUrl.Contains("user-circle.png")) u.ImageUrl = new UserHandler().GetUserById(u.Id).ImageUrl;

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
            PRSContext db = PRSContext.GetInstance();
            using (db)
            {
                User user = db.Users.Find(id);
                if (user != null)
                {
                    user.Password = HelperMethods.Sha256(formdata["Password"]);
                    user.ConfirmPassword = user.Password;
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
