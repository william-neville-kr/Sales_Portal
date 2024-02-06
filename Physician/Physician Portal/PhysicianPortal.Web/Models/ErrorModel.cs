using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicianPortal.Web.Models
{
    public class ErrorModel : HandleErrorInfo
    {
        public string ErrorTitle { get; set; }
        public string ErrorReason { get; set; }
        public string Layout { get; set; }
        public ErrorModel(Exception exception, string controllerName, string actionName, string title, string errorReason, string layout) : base(exception, controllerName, actionName)
        {
            ErrorTitle = title;
            ErrorReason = errorReason;
            Layout = layout;
        }
    }
}