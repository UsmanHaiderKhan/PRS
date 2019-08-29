using PRSClassesManagement;
using PRSClassesManagement.Helpers;
using PRSClassesManagement.UsersManagement;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PRS.Controllers.API
{
    public class UsersController : ApiController
    {
        private PRSContext db = PRSContext.GetInstance();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            var r = Request;
            string key = r.Headers.Contains("key") ? r.Headers.GetValues("key").First() : "";

            if (string.IsNullOrEmpty(key))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Access key is required!") });

            if (new TokenHandler().IsTokenValidAdmin(key)) // Only admin can get all users
                return db.Users;
            else
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            var r = Request;
            var th = new TokenHandler();
            string key = r.Headers.Contains("key") ? r.Headers.GetValues("key").First() : "";
            User user = db.Users.Find(id);

            if (string.IsNullOrEmpty(key))
                return BadRequest("Access key is required!");

            var token = th.GetValidToken(key);

            if (user == null)
            {
                if (th.IsTokenValidAdmin(key))
                    return NotFound();
            }
            else if (th.IsTokenValidAdmin(key) || (token != null && token.User.Id == user.Id))
            {
                return Ok(user);
            }

            return Unauthorized();
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var r = Request;
            var th = new TokenHandler();
            string key = r.Headers.Contains("key") ? r.Headers.GetValues("key").First() : "";

            if (string.IsNullOrEmpty(key))
                return BadRequest("Access key is required!");

            if (user == null)
                return BadRequest();

            var token = th.GetValidToken(key);

            if (th.IsTokenValidAdmin(key) || (token != null && token.User.Id == user.Id))
            {
                db.Entry(user).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                        return NotFound();
                    else
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable) { Content = new StringContent("Failed to update database.") });
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            return Unauthorized();
        }

        // POST: api/Users
        // DateofBirth should be in format: yyyy-MM-dd
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (user == null)
                return BadRequest();

            user.Role = new UserHandler().GetRoleById(2);
            user.Password = HelperMethods.Sha256(user.Password);
            user.ConfirmPassword = HelperMethods.Sha256(user.ConfirmPassword);
            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            var r = Request;
            var th = new TokenHandler();
            string key = r.Headers.Contains("key") ? r.Headers.GetValues("key").First() : "";
            User user = db.Users.Find(id);

            if (string.IsNullOrEmpty(key))
                return BadRequest("Access key is required!");

            var token = th.GetValidToken(key);

            if (user == null)
            {
                if (th.IsTokenValidAdmin(key))
                    return NotFound();
            }
            else if (th.IsTokenValidAdmin(key) || (token != null && token.User.Id == user.Id))
            {
                db.Users.Remove(user);
                db.SaveChanges();

                return Ok(user);
            }

            return Unauthorized();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}
