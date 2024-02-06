using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PhysicianPortal.Web.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User as ClaimsPrincipal;
            var claimType = "SuperAdminADAuthentication";

            if (user.HasClaim(x => x.Type == claimType))
            {
                var superAdminClaim = user.Claims.Where(x => x.Type == claimType).First();

                if (superAdminClaim.Value == "true")
                {
                    base.OnAuthorization(filterContext);
                }
                else
                {
                    //Redirect to ADLoginScreen here
                    base.HandleUnauthorizedRequest(filterContext);
                    filterContext.Result = new RedirectResult("~/Account/ADLogin");
                }
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}