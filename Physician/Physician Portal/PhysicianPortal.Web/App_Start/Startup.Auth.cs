using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Web.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using PhysicianPortal.Core.Helper;
using Microsoft.Owin.Extensions;
using System.Threading;

namespace PhysicianPortal.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            //MH_DWEntities db = new MH_DWEntities();
            //db.ApplicationSettings.Find();

            int timeOut = SettingManager.GetSettingValueInteger("TimeOut");
            if (timeOut == 0)
            {
                // Set TimeOut 15 minutes if it isn't available in Setting.
                timeOut = 15;
            }

            // Configure the db context, user manager and signin manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //ExpireTimeSpan = TimeSpan.FromMinutes(2),
                ExpireTimeSpan = TimeSpan.FromMinutes(timeOut),
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User>(
                    //    validateInterval: TimeSpan.FromMinutes(1), //This timer is used to validate the cookie after interval
                    //    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))

                    //We are using a custom OnValidateIdentity implementation so that we can get the Cookie Timeout Interval and add it to User claims
                    OnValidateIdentity = CustomValidateIdentity
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.Use(typeof(AuthenticationMiddleware));


            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        private static Task CustomValidateIdentity(CookieValidateIdentityContext context)
        {
            int timeOut = SettingManager.GetSettingValueInteger("TimeOut");
            if (timeOut == 0)
            {
                // Set TimeOut 15 minutes if it isn't available in Setting.
                timeOut = 15;
            }

            var stampValidator = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User>(
                 validateInterval: TimeSpan.FromMinutes(timeOut),
                 regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager, context.Identity));
            stampValidator.Invoke(context);

            // here we get the cookie expiry time
            var expireUtc = context.Properties.ExpiresUtc;
            var identity = context.Identity;

            if (expireUtc != null)
            {
                // add the expiry time back to cookie as one of the claims, called 'CookieExpirationTime'
                // to ensure that the claim has latest value, we must keep only one claim
                // otherwise we will be having multiple claims with same type but different values
                var claimType = "CookieExpirationTime";

                if (identity.HasClaim(c => c.Type == claimType))
                {
                    var existingClaim = identity.FindFirst(claimType);
                    identity.RemoveClaim(existingClaim);
                }
                var newClaim = new Claim(claimType, expireUtc.Value.UtcTicks.ToString());
                context.Identity.AddClaim(newClaim);
            }


            //Here we check if the current session is an 'Impersonation Session' by a super admin. If the super admin is currently impersonating anyother user
            //we check for the 'IsImpersonating' claim and add it back to the current user claims

            var impersonatingClaimType = "IsImpersonating";
            var impersonatingUserIdClaimType = "OriginalIdOfImpersonatingUser";

            if (identity.HasClaim(c => c.Type == impersonatingClaimType))
            {
                var existingClaim = identity.FindFirst(impersonatingClaimType);
                if (existingClaim != null)
                    identity.RemoveClaim(existingClaim);

                var existingUserIdClaim = identity.FindFirst(impersonatingUserIdClaimType);
                var impersonatingUserId = "";
                if (existingUserIdClaim != null)
                {
                    identity.RemoveClaim(existingUserIdClaim);
                    impersonatingUserId = existingUserIdClaim.Value;
                }
                var newClaim = new Claim(impersonatingClaimType, "true");
                context.Identity.AddClaim(newClaim);

                var newUserIdClaim = new Claim(impersonatingUserIdClaimType, impersonatingUserId);
                context.Identity.AddClaim(newUserIdClaim);
            }
            return Task.FromResult(0);
        }
    }


    //This Owin Middleware class is responsible for deleting the AuthenticationCookie from the response headers
    //See the CookieStripperAttribute class for more info
    public class AuthenticationMiddleware : OwinMiddleware
    {
        const string _authenticationCookie = CookieAuthenticationDefaults.CookiePrefix + DefaultAuthenticationTypes.ApplicationCookie;

        public AuthenticationMiddleware(OwinMiddleware next) :
            base(next)
        { }

        public override async Task Invoke(IOwinContext context)
        {
            var response = context.Response;
            response.OnSendingHeaders(state =>
            {
                var resp = (OwinResponse)state;

                if (resp.Environment.ContainsKey("StripAspCookie"))
                {
                    resp.Cookies.Delete(_authenticationCookie);
                }
            }, response);

            await Next.Invoke(context);
        }
    }    
}