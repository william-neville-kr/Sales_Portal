using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class PatientId_PhysicianIdViewModel
    {
        public int? PhysicianId { get; set; }
        public int? PatientId { get; set; }
        public string PatientIdEncrypted { get; set; }
        public string PhysicianIdEncrypted { get; set; }
    }
}