using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicianPortal.Web.Attributes
{
    //This attribute will remove the owin authentication cookie from the response.
    //Apply this attribute to any method you want to not extend the session timeout
    public class CookieStripperAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            filterContext.HttpContext.GetOwinContext().Environment.Add("StripAspCookie", true);
        }
    }
}