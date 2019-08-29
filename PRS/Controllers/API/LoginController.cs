using PRS.Models;
using PRSClassesManagement;
using PRSClassesManagement.Helpers;
using PRSClassesManagement.UsersManagement;
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
                return Ok(new TokenHandler().GenerateToken(user, vm.RememberMe).Key); // Generate token and return key
            }
            else
            {
                return BadRequest("Sorry Please Try With Correct Cardentials");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
