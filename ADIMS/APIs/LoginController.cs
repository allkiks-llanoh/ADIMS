using ADIMS.App_Start;
using ADIMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web.Http;

namespace ADIMS.APIs
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private ApplicationUserManager UserManager;

        public LoginController()
        {
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Login(APILoginViewModel viewmodel)
        {
            _context.Configuration.LazyLoadingEnabled = false;
            //Look for the user
            var _user = await UserManager.FindByEmailAsync(viewmodel.email);
            if (_user != null)
            {
                //check for password
                var _result = await UserManager.CheckPasswordAsync(_user, viewmodel.password);
                if (_result)
                {
                    return Ok(MakeUser(_user));
                }
                else
                {
                    return BadRequest("Incorrect password");
                }
            }
            else
            {
                return BadRequest("User does not exist");
            }
        }

        private UserViewModel MakeUser(ApplicationUser _user)
        {
            return new UserViewModel()
            {
                id = _user.Id,
                email = _user.Email,
                name = _user.name,
                phonenumber = _user.PhoneNumber,
                county = _user.county,
                subcounty = _user.sub_county,
                ward = _user.ward
            };
        }
    }

    public class APILoginViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class UserViewModel
    {
        public string id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string phonenumber { get; set; }
        public int county { get; set; }
        public int subcounty { get; set; }
        public int ward { get; set; }
    }
}
