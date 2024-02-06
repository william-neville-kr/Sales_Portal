using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Web.Models;
using System;
using PhysicianPortal.Core.Helpers;
using PhysicianPortal.Core.Repository;
using System.Security.Claims;
using System.Collections.Generic;
using PhysicianPortal.Core.Helper;
using PhysicianPortal.Trulioo;
using System.DirectoryServices.AccountManagement;
using PhysicianPortal.Web.Attributes;

namespace PhysicianPortal.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        public AccountController()
        {
        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager, signInManager)
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                if (Uri.IsWellFormedUriString(returnUrl, UriKind.Absolute))
                {
                    ViewBag.ReturnUrl = returnUrl;
                }
                else
                {
                    ViewBag.ReturnUrl = "";
                }

                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method Login(string returnUrl) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "Login", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ADLogin()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ADLogin() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ADLogin", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ADLogin(ADLoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }




#if DEBUG
                // authenticates against your local machine - for development time
                ContextType authenticationType = ContextType.Machine;
#else
            // authenticates against your Domain AD
            ContextType authenticationType = ContextType.Domain;
#endif
                PrincipalContext principalContext = new PrincipalContext(authenticationType);
                bool isAuthenticated = false;

                try
                {
                    isAuthenticated = principalContext.ValidateCredentials(model.Username, model.Password, ContextOptions.Negotiate);
                    if (isAuthenticated)
                    {

                    }
                }
                catch (Exception)
                {
                    isAuthenticated = false;

                }
                if (!isAuthenticated)
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                    return View(model);
                }

                var identity = User.Identity as ClaimsIdentity;

                var claimType = "SuperAdminADAuthentication";
                var newClaim = new Claim(claimType, "true");

                var currentClaim = identity.FindFirst(claimType);

                if (currentClaim != null)
                {
                    identity.RemoveClaim(currentClaim);
                }
                identity.AddClaim(newClaim);

                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });

                return RedirectToLocal("");

            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ADLogin() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ADLogin", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.CustomPasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);

                switch (result)
                {
                    case CustomSignInStatus.Success:
                        return RedirectToAction("SetUserRole", new { ReturnUrl = returnUrl });
                    case CustomSignInStatus.LockedOut:
                        return View("Lockout");
                    case CustomSignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case CustomSignInStatus.NotVerfied:
                        return View("NotVerified");
                    case CustomSignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method Login(LoginViewModel model, string returnUrl) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "Login", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        public ActionResult SetUserRole(string returnUrl = "\\")
        {
            try
            {
                //UserManager.AddToRole(CurrentUser.Id, CurrentUser.Role.Name);
                //AuditLogger.AuditTracker("Login Operation", "User with Id " + CurrentUser.Id + " has Login the Application Successfully", (int)AuditOperationType.Login);
                //var user = UserManager.FindById(User.Identity.GetUserId());
                int userId = User.Identity.GetUserId().ToInt().Value;
                var loggedInUser = unitOfWork.UserRepository.GetSingle(x => x.UserId == userId);
                loggedInUser.LastLogin = DateTime.UtcNow;
                loggedInUser.LastLoginIP = CommonHelper.GetUserIP(System.Web.HttpContext.Current.Request);
                unitOfWork.UserRepository.Update(loggedInUser);
                unitOfWork.Save();
                UserManager.AddToRole(loggedInUser.Id, loggedInUser.Role.Name);
                AuditLogger.AuditTracker("Login Operation", "User " + loggedInUser.FirstName + " " + loggedInUser.LastName + " has Login the Application Successfully.", (int)AuditOperationType.Login);


                //Adding claim for default home screen type

                //get this value from db
                //int menuType = loggedInUser.HomePageId ?? 0;                
                var claimType1 = "DefaultHomePage";
                var newClaim1 = new Claim(claimType1, (loggedInUser.HomePageId ?? 0).ToString());

                IList<string> userClaims = UserManager.GetClaims(CurrentUser.Id).Select(x => x.Type).ToList();
                if (userClaims.Contains(claimType1))
                {
                    UserManager.RemoveClaim(CurrentUser.Id, newClaim1);
                }
                UserManager.AddClaim(CurrentUser.Id, newClaim1);


                //string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                if (loggedInUser.Role.Name == "Pharmacist" && (loggedInUser.HomePage == null || loggedInUser.HomePage.HomePageValue == null))
                {
                    //Always redirect Pharmacist to the PharmacistHome
                    returnUrl = Request.ApplicationPath.TrimEnd('/') + "/Home/HomePharmacist";
                }
                else
                {
                    returnUrl = Request.ApplicationPath.TrimEnd('/') + "/Home/" + loggedInUser.HomePage.HomePageValue;
                }

                ///////////////////////////////////

                if (loggedInUser.Role.Name == "Physician" || loggedInUser.Role.Name == "Technician" || loggedInUser.Role.Name == "Admin")
                {
                    return RedirectToAction("CheckForOffice", "Account", new { returnUrl = returnUrl });
                }
                else if (loggedInUser.Role.Name == "SuperAdmin")
                {
                    var newClaimType = "SuperAdminADAuthentication";
                    var newClaim = new Claim(newClaimType, "false");

                    var identity = HttpContext.User.Identity as ClaimsIdentity;

                    // check for existing claim and remove it
                    var existingClaim = identity.FindFirst(newClaimType);
                    if (existingClaim != null)
                        identity.RemoveClaim(existingClaim);

                    // add new claim
                    identity.AddClaim(newClaim);
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });

                    //return RedirectToAction("ADLogin", "Account", new { returnUrl = returnUrl });
                }

                return RedirectToLocal(returnUrl);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method SetUserRole AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "SetUserRole", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        public ActionResult CheckForOffice(string returnUrl)
        {
            try
            {
                int userId = User.Identity.GetUserId().ToInt().Value;
                List<int> offices = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.UserId == userId).Select(x => x.OfficeId).ToList();

                if (offices.Count > 0)
                {
                    if (offices.Count == 1)
                    {
                        return CheckForOffice(new SelectOfficeViewModel { SelectedOffice = offices.First(), Offices = null, ReturnUrl = returnUrl });
                    }
                    var officeList = unitOfWork.OfficeRepository.GetAsQuerable(x => offices.Contains(x.OfficeId)).Select(t => new SelectListItem { Text = t.OfficeTitle, Value = t.OfficeId.ToString() }).ToList();
                    return View(new SelectOfficeViewModel { Offices = officeList, ReturnUrl = returnUrl });
                }
                else
                {
                    return RedirectToLocal(returnUrl);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method CheckForOffice AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "CheckForOffice", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckForOffice(SelectOfficeViewModel model)
        {
            try
            {

                //Adding the selected user office as a user claim. so that we can get it anywhere in the application

                var currentUser = User.Identity;
                var claimType = "UserOffice";
                var newClaim = new Claim(claimType, model.SelectedOffice.ToString());

                //var currentIdentity = User.Identity as ClaimsIdentity;
                //if (currentIdentity.HasClaim(c => c.Type == claimType))
                //{
                //    var existingClaim = currentIdentity.FindFirst(claimType);
                //    currentIdentity.RemoveClaim(existingClaim);
                //}
                //currentIdentity.AddClaim(newClaim);

                IList<string> userClaims = UserManager.GetClaims(currentUser.GetUserId()).Select(x => x.Type).ToList();
                if (userClaims.Contains(claimType))
                {
                    UserManager.RemoveClaim(currentUser.GetUserId(), newClaim);
                }
                UserManager.AddClaim(currentUser.GetUserId(), newClaim);

                return RedirectToLocal(model.ReturnUrl);

            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method CheckForOffice(SelectOfficeViewModel model) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "CheckForOffice(SelectOfficeViewModel model)", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [AllowAnonymous]
        public async Task<JsonResult> ResendCode(string provider)
        {
            try
            {
                if (!await SignInManager.SendTwoFactorCodeAsync(provider))
                {
                    //return View("Error");
                }
                return Json(new { message = "Verification Code Sent Successfully.", success = true });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ResendCode AccountController. " + exception.Message, exception);
                return Json(new { message = "Verification Code Not Sent.", success = false });
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            try
            {
                // Require that the user has already logged in via username/password or external login
                if (!await SignInManager.HasBeenVerifiedAsync())
                {
                    return View("Error");
                }
                return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe, RememberBrowser = false });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method VerifyCode(string provider, string returnUrl, bool rememberMe) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "VerifyCode", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            try
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
                        return RedirectToAction("SetUserRole");
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid code. Please try again");
                        return View(model);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method VerifyCode(VerifyCodeViewModel model) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "VeriyCode", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }


        [AllowAnonymous]
        public ActionResult RememberBrowser()
        {
            return PartialView("RememberBrowser");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method Register() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "Register", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                //Test Trulioo Verify User
                TruliooVerifyUser truliooVerifyUser = new TruliooVerifyUser();
                //truliooVerifyUser.TestApi();
                await truliooVerifyUser.TestApi2();


                if (ModelState.IsValid)
                {
                    var physician = unitOfWork.PhysiciansRepository.GetAsQuerable(
                        t =>
                                t.FirstName.ToLower().Equals(model.FirstName.ToLower())
                                && t.LastName.ToLower().Equals(model.LastName.ToLower())
                                && t.NationalProviderIdentifier.ToLower().Equals(model.NPI.ToLower())
                    //&& t.PhoneNumber.ToLower().Equals(model.PhoneNumber.ToLower())
                    ).ToList();
                    var homePage = unitOfWork.HomePagesRepository.GetSingle(t => t.HomePageValue.Equals("HomeDetail"));

                    if (physician?.Count() > 0)
                    {
                        Core.Data.User newUser = new Core.Data.User()
                        {
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PhoneNumber = model.PhoneNumber,
                            RoleId = 2,
                            PhoneNumberConfirmed = true,
                            TwoFactorEnabled = true,
                            IsActive = true,
                            IsVerified = false,
                            NPI = model.NPI,
                            LastLogin = DateTime.UtcNow,
                            LastLoginIP = CommonHelper.GetUserIP(System.Web.HttpContext.Current.Request),
                            DateOfBirth = model.DateOfBirth,
                            SSN = model.SSN,
                            DrivingLicenseNumber = model.DrivingLicenseNumber,
                            HomePageId = homePage?.HomePageId ?? 1
                        };

                        var result = await UserManager.CreateAsync(newUser, model.Password);
                        if (result.Succeeded)
                        {
                            // Add selected physicians for new user in UserPhysicianRelationship Table
                            unitOfWork.UserPhysicianRelationshippRepository.Insert(new UserPhysicianRelationship { UserId = newUser.UserId, PhysicianId = physician[0].PhysicianId });
                            unitOfWork.Save();

                            string code = newUser.UserId.ToString().Encrypt();
                            string callbackUrl = Url.Action("ConfirmEmail", "Admin", new { userId = newUser.UserId, code }, Request.Url.Scheme);
                            string template = SettingManager.GetSettingValue("Email.VerifyTemplate");
                            if (string.IsNullOrEmpty(template))
                            {
                                template =
                                    @"<div lang='x-wrapper' style='min-width: 320px;background-color: #f5f7fa;' class='wrapper'><div style='Margin: 0 auto;max-width: 560px;min-width: 280px; width: 280px;width: calc(28000% - 173040px);' class='preheader'><div style='border-collapse: collapse;display: table;width: 100%;'><!--[if (mso)|(IE)]><table align='center' class='preheader' cellpadding='0' cellspacing='0'><tr><td style='width: 280px' valign='top'><![endif]--><div style='Float: left;font-size: 12px;line-height: 19px;max-width: 280px;min-width: 140px; width: 140px;width: calc(14000% - 78120px);padding: 10px 0 5px 0;color: #b9b9b9;font-family: &quot;Open Sans&quot;,sans-serif;' class='snippet'></div><!--[if (mso)|(IE)]></td><td style='width: 280px' valign='top'><![endif]--><div style='Float: left;font-size: 12px;line-height: 19px;max-width: 280px;min-width: 139px; width: 139px;width: calc(14100% - 78680px);padding: 10px 0 5px 0;text-align: right;color: #b9b9b9;font-family: &quot;Open Sans&quot;,sans-serif;' class='webversion'></div><!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div><div id='emb-email-header-container' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 173000px);' class='header'><!--[if (mso)|(IE)]><table align='center' class='header' cellpadding='0' cellspacing='0'><tr><td style='width: 600px'><![endif]--><div align='center' style='font-size: 26px;line-height: 32px;Margin-top: 6px;Margin-bottom: 20px;color: #c3ced9;font-family: Roboto,Tahoma,sans-serif;Margin-left: 20px;Margin-right: 20px;' class='logo emb-logo-margin-box'><div align='center' id='emb-email-header' style='font-size:0px !important;line-height:0 !important;' class='logo-center'><img width='100px' alt='' src='" + System.Web.Configuration.WebConfigurationManager.AppSettings["ClientNameWebLogo"].ToString() + "' style='height: auto;width: 100%;border: 0;max-width: 100px;'></div></div><!--[if (mso)|(IE)]></td></tr></table><![endif]--></div><div style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 173000px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;' class='layout one-col fixed-width'><div emb-background-style='' style='border-collapse: collapse;display: table;width: 100%;background-color: #ffffff;' class='layout__inner'><!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0'><tr class='layout-fixed-width' emb-background-style><td style='width: 600px' class='w560'><![endif]--><div style='text-align: left;color: #60666d;font-size: 14px;line-height: 21px;font-family: &quot;Open Sans&quot;,sans-serif;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);' class='column'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 24px;'><div style='line-height:10px;font-size:1px'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;Margin-bottom: 24px;'><h2 lang='x-size-24' style='Margin-top: 0;Margin-bottom: 0;font-style: normal;font-weight: normal;color: #44a8c7;font-size: 20px;line-height: 28px;text-align: center;' class='size-24'>Welcome to" + ClientName3 + " Physician Portal</h2><p lang='x-size-16' style='Margin-top: 16px;Margin-bottom: 0;font-size: 16px;line-height: 24px;text-align: left;' class='size-16'>Hello @FirstName @LastName</p><p style='Margin-top: 20px;Margin-bottom: 0;text-align: left;'>Please confirm your account by clicking <b>@EmailVerify</b>.</p></div></div><!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div><div style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 173000px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;' class='layout email-footer'><div style='border-collapse: collapse;display: table;width: 100%;' class='layout__inner'><!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0'><tr class='layout-email-footer'><td style='width: 400px;' valign='top' class='w360'><![endif]--><div style='text-align: left;font-size: 12px;line-height: 19px;color: #b9b9b9;font-family: &quot;Open Sans&quot;,sans-serif;Float: left;max-width: 400px;min-width: 320px; width: 320px;width: calc(8000% - 47600px);' class='column wide'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 10px;Margin-bottom: 10px;'><div><div>" + ClientName3 + "</div></div><div style='Margin-top: 18px;'><div>You are receiving this email because you have registered on the " + ClientName3 + " Physician Portal</div></div></div></div><!--[if (mso)|(IE)]></td><td style='width: 200px;' valign='top' class='w160'><![endif]--><div style='text-align: left;font-size: 12px;line-height: 19px;color: #b9b9b9;font-family: &quot;Open Sans&quot;,sans-serif;Float: left;max-width: 320px;min-width: 200px; width: 320px;width: calc(72200px - 12000%);' class='column narrow'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 10px;Margin-bottom: 10px;'></div></div><!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div><div style='line-height:40px;font-size:40px;'>&nbsp;</div></div>";
                                SettingManager.Add("Email.VerifyTemplate", template);
                            }

                            template = template.Replace("@FirstName", model.FirstName)
                                .Replace("@LastName", model.LastName)
                                .Replace("@EmailVerify", "<a href=" + callbackUrl + ">here</a>")
                                .Replace("@Email", model.Email);

                            CommonHelper.SendEmail(ClientName3 + " Physician Portal Confirm your Account", template, model.Email, "");
                            CommonHelper.SendSms(model.PhoneNumber, "Your registration on Physician Portal was successful. An email has been sent to you containing verification information. Please check your email to proceed");

                            // Send Email to SuperAdmin when new user (physician) has be registered.
                            string msg = "Dear administrator a new physician is waiting account verification for login.";

                            var superAdminUserList = unitOfWork.UserRepository.GetAsQuerable(t => t.RoleId == 5);
                            foreach (var superAdminUser in superAdminUserList)
                            {
                                CommonHelper.SendEmail("New user has been registered", msg, superAdminUser.Email, "");
                            }

                            return Json(new { message = "Please click on the link sent to you via email to verify your account", success = true, title = "Success!" });
                        }
                        else
                        {
                            //AddErrors(result);
                            return Json(new { message = "User already exists", success = true, title = "Alert!" });
                        }
                    }
                    else
                    {
                        return Json(new { message = "Sorry, we couldn't find you in our pharmacy records. </ br>Please try again or contact us for further assistance.", success = true, title = "Alert!" });
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method Register(RegisterViewModel model) AccountController. " + exception.Message, exception);
            }
            return Json(new { message = "Exception thrown in Method Register(RegisterViewModel model) AccountController", success = false });
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return View("Error", new ErrorModel(new Exception(), "AccountController", "ConfirmEmail", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
                }
                var result = await UserManager.ConfirmEmailAsync(userId, code.Decrypt());
                if (result.Succeeded)
                {
                    var user = UserManager.FindById(userId);
                    user.EmailVarified = true;
                    var updateUserResult = await UserManager.UpdateAsync(user);
                    return View(updateUserResult.Succeeded ? "ConfirmEmail" : "Error");
                }
                else
                {
                    return View("Error", new ErrorModel(new Exception(result.Errors.ToString()), "AccountController", "ConfirmEmail", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ConfirmEmail(string userId, string code) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ConfirmEmail", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ForgotPassword() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ForgotPassword", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    {
                        return View("ForgotPasswordConfirmation");
                    }

                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    CommonHelper.SendEmail("Reset Password", "Please, reset your password of Physician Portal by clicking <a href=\"" + callbackUrl + "\">here.</a>", user.Email, null);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ForgotPassword(ForgotPasswordViewModel model) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ForgotPassword", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ForgotPasswordConfirmation() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ForgotPasswordConfirmation", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            try
            {
                return code == null ? View("Error") : View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ResetPassword() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ResetPassword", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
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
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ResetPassword(ResetPasswordViewModel model) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ResetPassword", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ResetPasswordConfirmation() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ResetPasswordConfirmation", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            try
            {
                // Request a redirect to the external login provider
                return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ExternalLogin() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ExternalLogin", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            try
            {
                var userId = await SignInManager.GetVerifiedUserIdAsync();
                //var user = CurrentUser;
                var user = UserManager.FindById(userId);
                Email.EmailAddress = user.Email;

                if (userId == null)
                {
                    return View("Error");
                }
                var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
                var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
                return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method SendCode(string returnUrl, bool rememberMe) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "SendCode", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            try
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
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method SendCode(SendCodeViewModel model) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "SendCode", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            try
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
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ExternalLoginCallback(string returnUrl) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ExternalLoginCallback", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            try
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
                    var user = new User { UserName = model.Email, Email = model.Email };
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
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl) AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ExternalLoginConfirmation", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                var user = CurrentUser;
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                AuditLogger.AuditTracker("LogOff Operation", "User " + user.FirstName + " " + user.LastName + ", UserID: " + user.Id + " has Logout from the Application Successfully.", (int)AuditOperationType.Logout);

                var identity = User.Identity as ClaimsIdentity;

                var claimType = "SuperAdminADAuthentication";
                var newClaim = new Claim(claimType, "true");

                var currentClaim = identity.FindFirst(claimType);

                if (currentClaim != null)
                {
                    identity.RemoveClaim(currentClaim);
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
                }


                Session.Abandon();

                return RedirectToAction("Login", "Account");
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method LogOff() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "LogOff", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ExternalLoginFailure() AccountController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AccountController", "ExternalLoginFailuer", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
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
            try
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddErrors AccountController. " + exception.Message, exception);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("HomeDetail", "Home");
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method RedirectToLocal AccountController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
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
                try
                {
                    var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                    if (UserId != null)
                    {
                        properties.Dictionary[XsrfKey] = UserId;
                    }
                    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
                }
                catch (Exception exception)
                {
                    Logger.InsertLog("Exception thrown in Method ExecuteResult AccountController. " + exception.Message, exception);
                }
            }
        }
        #endregion
    }

    //Class for getting email addresss from controller to EmailService 
    public static class Email
    {
        public static string EmailAddress;
    }

}