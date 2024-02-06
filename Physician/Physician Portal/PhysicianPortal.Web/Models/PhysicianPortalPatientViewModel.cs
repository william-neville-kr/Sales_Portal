using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class PhysicianPortalPatientViewModel
    {
        public int PhysicianPortalPatientId { get; set; }
        public Nullable<int> PhysicianId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PreferredLanguage { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string PhysicianPortalPatientIdEncrypted { get; set; }
        public string PhysicianIdEncrypted { get; set; }
    }
}