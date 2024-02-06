using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    //public class DenyReasonViewModel
    //{
    //    public int RefillFaxReportLogId { get; set; }
    //    public string PhysicianNPI { get; set; }
    //    public Nullable<int> PatientMRN { get; set; }
    //    public string Note { get; set; }
    //    public int RefillStatusId { get; set; }
    //}
    public class RefillNotesViewModel
    {
        public int RefillFaxReportLogId { get; set; }
        public string PhysicianNPI { get; set; }
        public Nullable<int> PatientMRN { get; set; }
        public string Note { get; set; }
        public int RefillStatusId { get; set; }
    }
}