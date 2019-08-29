using PRS.Models;
using PRSClassesManagement;
using PRSClassesManagement.Helpers;
using PRSClassesManagement.UsersManagement;
using System.Linq;
using System.Web.Http;

namespace PRS.Controllers.API
{
    public class LoginController : ApiController
    {
        private PRSContext db = PRSContext.GetInstance();

        [AcceptVerbs("POST")]
        [HttpPost]
        public IHttpActionResult GetDataFromUser(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vm == null)
                return BadRequest();

            User user = new UserHandler().GetUser(vm.Email, HelperMethods.Sha256(vm.Password));
            if (user != null)
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
