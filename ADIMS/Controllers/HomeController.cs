using ADIMS.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            var user = UserManager.FindByEmail(User.Identity.Name);
            if (user.fresh_user)
            {
                return Redirect("/account/changepassword");
            }
            return View();
        }
    }
}