using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.UserStore;
using PhysicianPortal.Web.Models;
using Twilio;
using System.Linq;
using PhysicianPortal.Web.Controllers;
using PhysicianPortal.Core.Helper;

namespace PhysicianPortal.Web
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            //Using CommonHelper Method for sending the emails
            Core.Helpers.CommonHelper.SendEmail(message.Subject, message.Body, Email.EmailAddress, null);
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            //Using CommonHelper Method for sending the sms
            Core.Helpers.CommonHelper.SendSms(message.Destination, message.Body);
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager()
            : base(new UserStoreService())
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager();
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 3;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            string ClientName3 = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientName3"].ToString();
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your one time verification code for" + ClientName3 + " Physician Portal is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your one time verification code for" + ClientName3 + " Physician Portal is {0}"
            });

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        //Using a custom SignIn Method because we had to include 'NotVerified' option in the result
        public Task<CustomSignInStatus> CustomPasswordSignIn(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            User user = this.UserManager.FindByEmail(userName);
            if (user == null)
            {
                return Task.FromResult(CustomSignInStatus.Failure);
            }
            if (!user.IsVerified.Value)
            {
                return Task.FromResult(CustomSignInStatus.NotVerfied);
            }
            else
            {
                Task<SignInStatus> siginStatus = PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
                if (siginStatus.Result == SignInStatus.Success)
                {
                    return Task.FromResult(CustomSignInStatus.Success);
                }
                else if (siginStatus.Result == SignInStatus.LockedOut)
                {
                    return Task.FromResult(CustomSignInStatus.LockedOut);
                }
                else if (siginStatus.Result == SignInStatus.RequiresVerification)
                {
                    return Task.FromResult(CustomSignInStatus.RequiresVerification);
                }
                else if (siginStatus.Result == SignInStatus.Failure)
                {
                    return Task.FromResult(CustomSignInStatus.Failure);
                }
                return Task.FromResult(CustomSignInStatus.Failure);
            }
        }


        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        internal object FindById(string v)
        {
            throw new NotImplementedException();
        }
    }

    //Using a custom SignIn Method because we had to include 'NotVerified' option in the result
    public enum CustomSignInStatus
    {
        //
        // Summary:
        //     Sign in was successful
        Success = 0,
        //
        // Summary:
        //     User is locked out
        LockedOut = 1,
        //
        // Summary:
        //     Sign in requires addition verification (i.e. two factor)
        RequiresVerification = 2,
        //
        // Summary:
        //     Sign in failed
        Failure = 3,
        //
        // Summary:
        //     Needs to be verified by SuperAdmin
        NotVerfied = 4
    }
}