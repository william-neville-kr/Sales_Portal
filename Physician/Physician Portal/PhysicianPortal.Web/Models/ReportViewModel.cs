using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class ReportViewModel
    {
        public string ReportName { get; set; }
        public bool IsPowerBI { get; set; }
        public string ReportURL { get; set; }
    }
}