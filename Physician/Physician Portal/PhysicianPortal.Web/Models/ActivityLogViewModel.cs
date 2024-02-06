using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class ActivityLogReportViewModel
    {
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]        
        public Nullable<DateTime> StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public Nullable<DateTime> EndDate { get; set; }

    }
    
}