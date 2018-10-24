using ADIMS.App_Start;
using ADIMS.Extensions;
using ADIMS.Models;
using ADIMS.Services;
using ADIMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ADIMS.Controllers
{
    // GET: Account
        [Authorize]
        public class AccountController : Controller
        {
            private ApplicationUserManager _userManager;
            private readonly adimsEntities db = new adimsEntities();
            private ApplicationDbContext _context = new ApplicationDbContext();

        private string DEFAULT_PASSWORD = "Adims123#";

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login3()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult index()
        {
            return View();
        }
       
        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var profile = await getUserInfo(User.Identity.Name);
            ViewBag.userinfo = profile;
            return View();
        }

        private ApplicationSignInManager _signInManager;

            public ApplicationSignInManager SignInManager
            {
                get
                {
                    return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }
                private set { _signInManager = value; }
            }

            //
            // POST: /Account/Login
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
               
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                var _user = User.Identity;
                switch (result)
                {
                    case SignInStatus.Success:
                    var user = UserManager.FindByEmail(model.Email);
                        if (user.fresh_user)
                        {
                            return RedirectToAction("ChangePassword");
                        }
                        else
                        {

                        AuditService.Login(model.Email);

                            return RedirectToLocal(returnUrl);
                        }
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }

            //
            // GET: /Account/VerifyCode
            [AllowAnonymous]
            public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
            {
                // Require that the user has already logged in via username/password or external login
                if (!await SignInManager.HasBeenVerifiedAsync())
                {
                    return View("Error");
                }
                
                var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
                if (user != null)
                {
                    var code = await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
                }
                return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }

            
            // POST: /Account/VerifyCode
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // The following code protects for brute force attacks against the two factor codes. 
                // If a user enters incorrect codes for a specified amount of time then the user account 
                // will be locked out for a specified amount of time. 
                // You can configure the account lockout settings in IdentityConfig
                var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(model.ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid code.");
                        return View(model);
                }
            }

            //
            // GET: /Account/Register
            [AllowAnonymous]
            public ActionResult Register()
            {
                var counties = db.counties.OrderBy(x => x.name).ToList();
                var roles = _context.Roles.Select(x => x.Name).ToList();
                ViewBag.counties = counties;
                ViewBag.roles = roles;
                return View();
            }

            //
            // POST: /Account/Register
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Register(RegisterViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                                    {
                                        name = model.name,
                                        UserName = model.Email,
                                        Email = model.Email,
                                        PhoneNumber = model.phone_number,
                                        county = model.county,
                                        sub_county = model.sub_county,
                                        ward = model.ward,
                                        fresh_user = true
                                    };

                    var result = await UserManager.CreateAsync(user, DEFAULT_PASSWORD);
                    
                    if (result.Succeeded)
                    {
                        UserManager.AddToRole(user.Id, model.role);

                    //string message = $"Dear {user.name}, you have been added on ADIMS. Your ADIMS password number is {DEFAULT_PASSWORD}. Please login at {APP_URL} and change your password.";

                    string message = $"Dear {user.name}, you have been added on ADIMS. Your ADIMS password number is {DEFAULT_PASSWORD}. Please login and change your password.";
                    var smsService = new AfricasTalkingSmsService();

                    smsService.SendSms(Convert.ToString(user.PhoneNumber), message);
                   // SMSService.Send(Convert.ToString(user.PhoneNumber), message);

                    //Audit service
                    AuditService.AddEntry(user, User.Identity.GetUserName());

                    return RedirectToAction("users", "admin");
                    }
                    AddErrors(result);
                }

                var counties = db.counties.ToList();
                var roles = _context.Roles.Select(x => x.Name).ToList();
                ViewBag.counties = counties;
                ViewBag.roles = roles;
            
            // If we got this far, something failed, redisplay form
            return View(model);
            }

            //
            // GET: /Account/ConfirmEmail
            [AllowAnonymous]
            public async Task<ActionResult> ConfirmEmail(string userId, string code)
            {
                if (userId == null || code == null)
                {
                    return View("Error");
                }
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }

            //
            // GET: /Account/ForgotPassword
            [AllowAnonymous]
            public ActionResult ForgotPassword()
            {
                return View();
            }

            [HttpPost]
            [AllowAnonymous]
            public ActionResult ForgotPassword(RegisterViewModel model)
            {
                var user = UserManager.FindByEmail(model.Email);

            if (user == null)
                this.AddNotification("Sorry, that email that does not exist", NotificationType.ERROR);

                //generate a new password
                string newPassword = $"adm{Guid.NewGuid().ToString().Substring(0, 6)}";

                string _passwordToken = UserManager.GeneratePasswordResetToken(user.Id);

                UserManager.ResetPassword(user.Id, _passwordToken, newPassword);
            
                UserManager.Update(user);
            
                string message = $"Dear {user.name},\n at {DateTime.Now.ToShortTimeString()} you requested for a password reset.\n Your new login details are as follows\n Email : {user.Email}\nPassword : {newPassword}.\n Please login and change your password.";
                var smsService = new AfricasTalkingSmsService();

                smsService.SendSms(Convert.ToString(user.PhoneNumber), message);
                // SMSService.Send(Convert.ToString(user.PhoneNumber), message);
                //SMSService.Send(Convert.ToString(user.PhoneNumber), message);

                this.AddNotification("We have sent a new password to your mobile phone. Please do check.", NotificationType.SUCCESS);

                return RedirectToAction("Login");
            }

            [AllowAnonymous]
            public ActionResult ChangePassword()
            {
                return View();
            }
           
            [HttpPost]
            public async Task<ActionResult> ChangePassword(ChangeMyPasswordViewModel viewmodel)
            {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            if(viewmodel.NewPassword != viewmodel.ConfirmPassword)
            {
                ViewBag.error = "Password Mismatch";
            }

            var _token = UserManager.GeneratePasswordResetToken(User.Identity.GetUserId());

            var result = await UserManager.ResetPasswordAsync(User.Identity.GetUserId(), _token, viewmodel.NewPassword);

            if (result.Succeeded)
            {
                var user = UserManager.FindByName(User.Identity.GetUserName());
                user.fresh_user = false;
                UserManager.Update(user);
                
                //Send an sms with the New Password
                return RedirectToAction("Login", "Account");
            }
            AddErrors(result);
            return View();
        }
        public async Task<Tuple<string, county, subcounty>> getUserInfo(string name)
        {
            var user = await UserManager.FindByNameAsync(name);
            //get county
            var county = db.counties.Find(user.county);
            //sub-county
            var subCounty = db.subcounties.Find(user.sub_county);

            return Tuple.Create(user.name, county, subCounty);
        }


        // POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //        // Send an email with this link
        //        string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //        await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //    //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
            public ActionResult ForgotPasswordConfirmation()
            {
                return View();
            }

            //
            // GET: /Account/ResetPassword
            [AllowAnonymous]
            public ActionResult ResetPassword(string code)
            {
                return code == null ? View("Error") : View();
            }

            //
            // POST: /Account/ResetPassword
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                AddErrors(result);
                return View();
            }

            //
            // GET: /Account/ResetPasswordConfirmation
            [AllowAnonymous]
            public ActionResult ResetPasswordConfirmation()
            {
                return View();
            }

            //
            // POST: /Account/ExternalLogin
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public ActionResult ExternalLogin(string provider, string returnUrl)
            {
                // Request a redirect to the external login provider
                return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }

            //
            // GET: /Account/SendCode
            [AllowAnonymous]
            public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
            {
                var userId = await SignInManager.GetVerifiedUserIdAsync();
                if (userId == null)
                {
                    return View("Error");
                }
                var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
                var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
                return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }

            //
            // POST: /Account/SendCode
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> SendCode(SendCodeViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                // Generate the token and send it
                if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                {
                    return View("Error");
                }
                return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
            }
        [HttpGet]
        [Authorize]
        public ActionResult EditUser(string id)
        {
            ViewBag.roles = _context.Roles.Select(x => x.Name).ToList();

            var admin = _context.Roles.FirstOrDefault(x => x.Name == "Administrator");
            var extensionOfficer = _context.Roles.FirstOrDefault(x => x.Name == "Extension Officer");
            var ministryOfficial = _context.Roles.FirstOrDefault(x => x.Name == "Ministry Official");

            var user = UserManager.FindById(id) ?? null;
            
            var viewmodel = new EditUserViewModel()
                            {
                                id = user.Id,
                                name = user.name,
                                email = user.Email,
                                phonenumber = user.PhoneNumber,
                                Administrator = user.Roles.Any(x => x.RoleId == admin.Id),
                                ExtensionOfficer = user.Roles.Any(x => x.RoleId == extensionOfficer.Id),
                                MinistryOfficial = user.Roles.Any(x => x.RoleId == ministryOfficial.Id)
            };

            return View(viewmodel);
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(EditUserViewModel user)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            else
            {
                var _user = _context.Users.Find(user.id);
                
                _user.name = user.name;
                _user.PhoneNumber = user.phonenumber;
                _user.Email = user.email;

                _context.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                await _context.SaveChangesAsync();

                //Update user roles
                UpdateUserRoles(user.id, user.Administrator, user.ExtensionOfficer, user.MinistryOfficial);
                
                return Redirect("/admin/users");
            }
        }

        public void UpdateUserRoles(string user, bool Admin, bool ExtensionOfficer, bool MinistryOfficial)
        {
            var admin = _context.Roles.FirstOrDefault(x => x.Name == "Administrator");
            var extensionOfficer = _context.Roles.FirstOrDefault(x => x.Name == "Extension Officer");
            var ministryOfficial = _context.Roles.FirstOrDefault(x => x.Name == "Ministry Official");

            //admin check
            #region AdminRole
            switch (Admin)
            {
                case true:
                    AddRoleToUser(user, admin.Name);
                    break;
                case false:
                    RemoveRoleFromUser(user, admin.Name);
                    break;
                default:
                    break;
            }
            #endregion

            #region ExtensionOfficer
            switch (ExtensionOfficer)
            {
                case true:
                    AddRoleToUser(user, extensionOfficer.Name);
                    break;
                case false:
                    RemoveRoleFromUser(user, extensionOfficer.Name);
                    break;
                default:
                    break;
            }
            #endregion

            #region MinistryOfficial
            switch (MinistryOfficial)
            {
                case true:
                    AddRoleToUser(user, ministryOfficial.Name);
                    break;
                case false:
                    RemoveRoleFromUser(user, ministryOfficial.Name);
                    break;
                default:
                    break;
            }
            #endregion
        }

        public void RemoveRoleFromUser(string user, string role)
        {
            if (UserManager.IsInRole(user, role))
            {
                UserManager.RemoveFromRole(user, role);
            }
        }

        public void AddRoleToUser(string user, string role)
        {
            var roles =  UserManager.GetRoles(user).ToList();
            if (!UserManager.IsInRole(user, role))
            {
                UserManager.AddToRole(user, role);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteUser(string id)
        {
            var user = UserManager.FindById(id);

            UserManager.Delete(user);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
            public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (loginInfo == null)
                {
                    return RedirectToAction("Login");
                }

                // Sign in the user with this external login provider if the user already has a login
                var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                    case SignInStatus.Failure:
                    default:
                        // If the user does not have an account, then prompt the user to create an account
                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                }
            }

            //
            // POST: /Account/ExternalLoginConfirmation
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Manage");
                }

                if (ModelState.IsValid)
                {
                    // Get the information about the user from the external login provider
                    var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (info == null)
                    {
                        return View("ExternalLoginFailure");
                    }
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(result);
                }

                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

        ////
        //// POST: /Account/LogOff
        //[HttpGet]
        //public ActionResult LogOff()
        //{
        //    AuthenticationManager.SignOut();
        //    return RedirectToAction("login", "account");
        //}


        //
        // POST: /Account/LogOff
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public ActionResult LogOff()
            {
                AuthenticationManager.SignOut();
                return RedirectToAction("login", "account");
            }

            //
            // GET: /Account/ExternalLoginFailure
            [AllowAnonymous]
            public ActionResult ExternalLoginFailure()
            {
                return View();
            }

            #region Helpers
            // Used for XSRF protection when adding external logins
            private const string XsrfKey = "XsrfId";

            private IAuthenticationManager AuthenticationManager
            {
                get
                {
                    return HttpContext.GetOwinContext().Authentication;
                }
            }

            private void AddErrors(IdentityResult result)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            private ActionResult RedirectToLocal(string returnUrl)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            internal class ChallengeResult : HttpUnauthorizedResult
            {
                public ChallengeResult(string provider, string redirectUri)
                    : this(provider, redirectUri, null)
                {
                }

                public ChallengeResult(string provider, string redirectUri, string userId)
                {
                    LoginProvider = provider;
                    RedirectUri = redirectUri;
                    UserId = userId;
                }

                public string LoginProvider { get; set; }
                public string RedirectUri { get; set; }
                public string UserId { get; set; }

                public override void ExecuteResult(ControllerContext context)
                {
                    var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                    if (UserId != null)
                    {
                        properties.Dictionary[XsrfKey] = UserId;
                    }
                    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
                }
            }
            #endregion
        }
}