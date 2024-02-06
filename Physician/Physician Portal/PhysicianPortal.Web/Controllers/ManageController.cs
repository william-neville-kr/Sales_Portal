using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PhysicianPortal.Web.Models;
using System;
using PhysicianPortal.Core.Helpers;
using PhysicianPortal.Core.Repository;
using System.Linq;
using PhysicianPortal.Web.Attributes;

namespace PhysicianPortal.Web.Controllers
{
    [CustomAuthorize]
    public class ManageController : BaseController
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        public ManageController()
        {
        }
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager, signInManager)
        {
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            try
            {
                ViewBag.StatusMessage =
                    message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                    : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                    : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                    : message == ManageMessageId.Error ? "An error has occurred."
                    : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                    : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                    : message == ManageMessageId.AddSignatureSuccess ? "Your Signature has been set."
                    : message == ManageMessageId.SetDefaultHomePage ? "Your Default Home Page has been set."
                    : "";

                var userId = User.Identity.GetUserId();
                var model = new IndexViewModel
                {
                    HasPassword = HasPassword(),
                    PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                    //Logins = await UserManager.GetLoginsAsync(userId),
                    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
                };
                return View(model);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method Index(ManageMessageId? message) ManageController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "ManageController", "Index", ErrorTitle, ErrorMessage, null));
            }
        }

        //
        // POST: /Manage/RemoveLogin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        //{
        //    ManageMessageId? message;
        //    var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
        //    if (result.Succeeded)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user != null)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //        }
        //        message = ManageMessageId.RemoveLoginSuccess;
        //    }
        //    else
        //    {
        //        message = ManageMessageId.Error;
        //    }
        //    return RedirectToAction("ManageLogins", new { Message = message });
        //}

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddPhoneNumber() ManageController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "ManageController", "AddPhoneNumber", ErrorTitle, ErrorMessage, null));
            }
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                // Generate the token and send it
                var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
                if (UserManager.SmsService != null)
                {
                    var message = new IdentityMessage
                    {
                        Destination = model.Number,
                        Body = "Your security code is: " + code
                    };
                    await UserManager.SmsService.SendAsync(message);
                }
                return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddPhoneNumber(AddPhoneNumberViewModel model) ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //[Authorize(Roles = "Physician")]
        //
        // GET: /Manage/AddSignature
        public ActionResult AddSignature()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddSignature() ManageController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "ManageController", "AddSignature", ErrorTitle, ErrorMessage, null));
            }
        }

        //
        // POST: /Manage/AddSignature
        [Authorize(Roles = "Physician")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSignature(AddSignatureViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                //var _user = unitOfWork.UserRepository.GetSingle(t => t.UserId == CurrentUser.UserId);
                //_user.Signature = model.Signature;
                //unitOfWork.UserRepository.Update(_user);
                //unitOfWork.Save();

                var userPhysicianRelationship = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(t => t.UserId == CurrentUser.UserId);
                Core.Data.PhysicianSignature physicianSignature;// = null;
                bool IsEdit = true;
                physicianSignature = userPhysicianRelationship != null ?
                    unitOfWork.PhysicianSignatureRepository.GetSingle(t => t.PhysicianMasterId == userPhysicianRelationship.PhysicianId) : null;

                if (physicianSignature == null)
                {
                    physicianSignature = new Core.Data.PhysicianSignature();
                    IsEdit = false;
                }

                physicianSignature.Signature = model.Signature;
                physicianSignature.PhysicianMasterId = userPhysicianRelationship?.PhysicianId ?? 0;
                physicianSignature.ModifiedBy = CurrentUser.UserId;
                physicianSignature.ModifiedOn = DateTime.UtcNow;

                if (IsEdit)
                {
                    unitOfWork.PhysicianSignatureRepository.Update(physicianSignature);
                }
                else
                {
                    unitOfWork.PhysicianSignatureRepository.Insert(physicianSignature);
                    physicianSignature.CreatedBy = CurrentUser.UserId;
                    physicianSignature.CreatedOn = DateTime.UtcNow;

                }
                unitOfWork.Save();

                return RedirectToAction("Index", new { Message = ManageMessageId.AddSignatureSuccess });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddSignature(AddSignatureViewModel model) ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /Manage/AddEditDefaultHomePage
        public ActionResult AddEditDefaultHomePage()
        {
            try
            {
                return View(CurrentUser.HomePage);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddEditDefaultHomePage() ManageController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "ManageController", "AddEditDefaultHomePage", ErrorTitle, ErrorMessage, null));
            }
        }

        // POST: /Manage/AddEditDefaultHomePage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditDefaultHomePage(Core.Data.HomePage model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = unitOfWork.UserRepository.GetSingle(t => t.UserId == CurrentUser.UserId);
                user.HomePageId = model.HomePageId;
                unitOfWork.UserRepository.Update(user);
                unitOfWork.Save();

                //return RedirectToAction("Index", new { Message = ManageMessageId.SetDefaultHomePage });

                return RedirectToAction("ChangeUserHomePageClaim", new { homePageId = model.HomePageId });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddEditDefaultHomePage(Core.Data.HomePage model) ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChangeUserHomePageClaim(int homePageId)
        {
            var claimType = "DefaultHomePage";
            var newClaim = new System.Security.Claims.Claim(claimType, homePageId.ToString());

            System.Collections.Generic.IList<string> userClaims = UserManager.GetClaims(CurrentUser.Id).Select(x => x.Type).ToList();
            if (userClaims.Contains(claimType))
            {
                UserManager.RemoveClaim(CurrentUser.Id, newClaim);
            }
            UserManager.AddClaim(CurrentUser.Id, newClaim);

            return RedirectToAction("Index", new { Message = ManageMessageId.SetDefaultHomePage });
        }

        public JsonResult GetHomePageList()
        {
            try
            {
                var homePageList = unitOfWork.HomePagesRepository.GetAsQuerable(x => x.HomePageId != 4).Select(t => new SelectListItem { Text = t.HomePageName, Value = t.HomePageId.ToString() }).ToList();
                return Json(homePageList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetHomePageList ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            try
            {
                await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", "Manage");
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method EnableTwoFactorAuthentication() ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            try
            {
                await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", "Manage");
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method DisableTwoFactorAuthentication() ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            try
            {
                var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
                // Send an SMS through the SMS provider to verify the phone number
                return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method VerifyPhoneNumber(string phoneNumber) ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
                }
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "Failed to verify phone");
                return View(model);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method VerifyPhoneNumber(VerifyPhoneNumberViewModel model) ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            try
            {
                var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
                if (!result.Succeeded)
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                }
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method RemovePhoneNumber() ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ChangePassword() ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    AuditLogger.AuditTracker("ChangePassword Operation", "User with Id: " + user.Id + ", changed password", (int)AuditOperationType.Login);
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ChangePassword(ChangePasswordViewModel model) ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method SetPassword(Get) ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        if (user != null)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        }
                        return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    AddErrors(result);
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method SetPassword(SetPasswordViewModel model) ManageController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        ////
        //// GET: /Manage/ManageLogins
        //public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : "";
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }
        //    var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
        //    var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
        //    ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
        //    return View(new ManageLoginsViewModel
        //    {
        //        CurrentLogins = userLogins,
        //        OtherLogins = otherLogins
        //    });
        //}

        ////
        //// POST: /Manage/LinkLogin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LinkLogin(string provider)
        //{
        //    // Request a redirect to the external login provider to link a login for the current user
        //    return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        //}

        ////
        //// GET: /Manage/LinkLoginCallback
        //public async Task<ActionResult> LinkLoginCallback()
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        //    }
        //    var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //    return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    try
        //    {
        //        if (disposing && _userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        base.Dispose(disposing);
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.InsertLog("Exception thrown in Method Dispose ManageController. " + exception.Message, exception);
        //    }
        //}

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
                Logger.InsertLog("Exception thrown in Method AddErrors ManageController. " + exception.Message, exception);
            }
        }

        private bool HasPassword()
        {
            try
            {
                var user = CurrentUser;
                if (user != null)
                {
                    return user.PasswordHash != null;
                }
                return false;
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method HasPassword ManageController. " + exception.Message, exception);
                return false;
            }
        }

        private bool HasPhoneNumber()
        {
            try
            {
                var user = CurrentUser;
                if (user != null)
                {
                    return user.PhoneNumber != null;
                }
                return false;
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method HasPhoneNumber ManageController. " + exception.Message, exception);
                return false;
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error,
            AddSignatureSuccess,
            SetDefaultHomePage
        }

        #endregion
    }
}