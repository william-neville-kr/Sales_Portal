using System;
using System.Web.Mvc;
using SalesTeam.Web.Models;
using System.Web.Security;
using System.Text;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateForgeryToken]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                // FormsAuthentication.SetAuthCookie(model.UserName,false);
                // Session["userNamePassword"] = model.UserName + model.Password;   
                Session["userName"] = model.UserName;
                //LoginInformation(model.UserName, "Login", model.Password);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        public ActionResult LogOff()
        {
            Session.Abandon();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForceLogout(string id)
        {
            string httpAuth = this.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(httpAuth) && httpAuth.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                byte[] enc = Encoding.UTF8.GetBytes(id + ':' + id);
                string expected = "basic " + Convert.ToBase64String(enc);

                if (string.Equals(httpAuth, expected, StringComparison.OrdinalIgnoreCase))
                {
                    return Content("You are logged out.");
                }
            }
            this.Response.StatusCode = 401;
            this.Response.StatusDescription = "Unauthorized";
            this.Response.AppendHeader("WWW-Authenticate", "basic realm=\"My Realm\"");

            return Content("Force AJAX component to sent header");
        }

        public ActionResult TimedOut()
        {
            Session.Abandon();
            return View();
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "SalesTeamPatient");
        }
        #endregion
    }
}