using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicianPortal.Web.Models
{
    public class AddPatientViewModel
    {
        public int? PhysicianPortalPatientId { get; set; }
        public string PhysicianPortalPatientIdEncrypted { get; set; }
        [Display(Name = "Select Physician")]
        public int? physicianId { get; set; }
        //public string icd10 { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        //[Required]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Required]
        public Nullable<DateTime> DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public string Gender { get; set; }

        [Display(Name = "Language")]
        [Required]
        public string PreferredLanguage { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required]
        public string State { get; set; }

        [Display(Name = "Zip")]
        [Required]
        public string Zip { get; set; }

        [Display(Name = "SSN No")]
        public string SSN { get; set; }

        [Display(Name = "Insurance Policy Number")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Insurance Group Number")]
        public string GroupNumber { get; set; }


        public List<SelectListItem> PhysiciansList { get; set; }

        public bool IsEligibilityValueExist { get; set; }

        [Display(Name = "Insurance Carrier")]
        public string PokitDok_TradingPartnerId { get; set; }
    }

    public class OldPatientViewModel
    {
        public int PatientId { get; set; }

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

        public int PhysicianPortalPatientId { get; set; }
    }

    public class AddPrescriptionViewModel
    {
        [Display(Name = "Number of Fills")]
        [Required]
        public int FillNumber { get; set; }

        [Display(Name = "Number of Fills Allowed")]
        [Required]
        public int FillsAllowed { get; set; }

        [Display(Name = "Number of Days Supplied for")]
        [Required]
        public int DaysSupplied { get; set; }

        [Display(Name = "Number of Days Prescribed for")]
        [Required]
        public int DaysSupplyPrescribed { get; set; }

        public string ICD10 { get; set; }
        public string DiseaseName { get; set; }
    }


    public class PatientGridModel
    {
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }
        public string PatientCode { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string PreferredLanguage { get; set; }
        public string ActivityStatus { get; set; }
        public string PhysicianName { get; set; }
        public string DiseaseState { get; set; }
        public Nullable<System.DateTime> ReferralDateId { get; set; }
        public string ServiceArea { get; set; }
        public Nullable<int> StartOfCareDateId { get; set; }
        public string PatientRepresentativeGroup { get; set; }
        public string Diagnosis { get; set; }
        public string IntakeRep { get; set; }
        public string NurseCoordinator { get; set; }
        public Nullable<int> YearOfBirth { get; set; }
        public string ReferralSource { get; set; }
        public string OriginatingPharmacy { get; set; }
        public string DrugShortName { get; set; }
        public Nullable<System.DateTime> LastFillDate { get; set; }
        public string RXStatus { get; set; }
        public string LastOrderEvent { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PhyCity { get; set; }
        public Nullable<bool> NeedMoreInfo { get; set; }
        public string NeedMoreInfoStr { get; set; }
        public Nullable<int> PatientQueueId { get; set; }
        public int IsQueued { get; set; }
        public string IsQueuedStr { get; set; }
        public int NotesCount { get; set; }
        public string PatientIdEncrypted { get; set; }
        public string PhysicianIdEncrypted { get; set; }
    }

}