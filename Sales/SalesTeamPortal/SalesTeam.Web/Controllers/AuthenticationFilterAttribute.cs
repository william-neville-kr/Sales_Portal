using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.SessionState;

namespace SalesTeam.Web.Controllers
{
    public class AuthenticationFilterAttribute: ActionFilterAttribute, IAuthenticationFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WindowsIdentity clientId = (WindowsIdentity)HttpContext.Current.User.Identity;
            if (clientId.IsAuthenticated)
            {
                Console.WriteLine("Authenticated");
            }
            // The action filter logic.
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //var issueDate = filterContext.Principal.IssueDate;
            //WindowsIdentity currentUser = (WindowsIdentity)HttpContext.Current.User;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[0];
            HttpSessionState state = HttpContext.Current.Session;
            //state.Timeout = 1;
            WindowsIdentity clientId = (WindowsIdentity)HttpContext.Current.User.Identity;
            Console.WriteLine("Authenticated");
            //filterContext.Result = new HttpUnauthorizedResult();
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            Console.WriteLine("Not Authenticated");
        }
    }
}