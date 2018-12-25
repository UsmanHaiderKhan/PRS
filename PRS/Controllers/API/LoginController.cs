using PRS.Models;
using PRSClassesManagement;
using PRSClassesManagement.UsersManagement;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace PRS.Controllers.API
{
    public class LoginController : ApiController
    {
        private PRSContext db = new PRSContext();

        // GET: api/Login
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Login/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //[HttpGet]
        //public IHttpActionResult GetDataFromUser()
        //{
        //    return Ok();
        //}


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]

        public IHttpActionResult GetDataFromUser(LoginViewModel vm)
        {
            User user = new UserHandler().GetUser(vm.Email, vm.Password);
            if (user.Email.Equals(vm.Email) && user.Password.Equals(vm.Password))
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Sorry Please Try With Correct Cardentials");
            }

        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}