using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class AddCPRPatientToPhysicianPortalModel
    {

        public int? PhysicianPortalPatientId { get; set; }
        public int? physicianId { get; set; }

        public string Email { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        
        public Nullable<DateTime> DateOfBirth { get; set; }
        
        public string Gender { get; set; }
        
        public string PreferredLanguage { get; set; }
        
        public string Address { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string Zip { get; set; }
        
        public string SSN { get; set; }
        
        public string PolicyNumber { get; set; }
        
        public string GroupNumber { get; set; }

        public int PatientId { get; set; }
        [Required]
        public string PatientIdEncrypted { get; set; }
        [Required]
        public string PhysicianIdEncrypted { get; set; }
    }
}