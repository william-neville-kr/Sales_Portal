using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class ReferralFormBaseViewModel
    {

        // Patient Information
        public Nullable<int> PhysicianPortalPatientId { get; set; }
        public string PatientFullName { get; set; }
        public string PatientAddressLine1 { get; set; }
        public string PatientCity_StateProvince_PostalCode { get; set; }
        public string PatientPhoneNumber { get; set; }
        public Nullable<System.DateTime> PatientDateOfBirth { get; set; }
        public string PatientGender { get; set; }

        // Physician Information
        public string PhysicianFullName { get; set; }
        public string PhysicianDrugEnforcementAdministrationNumber { get; set; }
        public string PhysicianNationalProviderIdentifier { get; set; }
        public string PhysicianPhysicianLicenseNumber { get; set; }
        public string PhysicianAddressLine1 { get; set; }
        public string PhysicianCity_StateProvince_PostalCode { get; set; }
        public string PhysicianPhoneNumber { get; set; }
        public string PhysicianFaxNumber { get; set; }


        public Nullable<System.DateTime> FormDate { get; set; }
        public Nullable<System.DateTime> FormNeedsByDate { get; set; }
        public Nullable<bool> ChkBox_ShipToPatient { get; set; }
        public Nullable<bool> ChkBox_ShipToOffice { get; set; }
        public Nullable<bool> ChkBox_ShipToOther { get; set; }
        public string TxtBox_ShipToOther { get; set; }

        //Last Date
        public Nullable<System.DateTime> Date1 { get; set; }
        public Nullable<System.DateTime> Date2 { get; set; }

        //
        public int ReferralFormId { get; set; }
        public bool IsSignatured { get; set; }
    }
}