using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTeam.Web.Models
{
    public class PhysicianViewModel
    {
        public int PhysicianId { get; set; }
        public int SalesTeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string NationalProviderIdentifier { get; set; }
        public int Pending { get; set; }
        public int Active { get; set; }
        public int Cancelled { get; set; }
        public string Url_SId_PhyId { get; set; }
        public bool ConsentGranted { get; set; }


        //public string FullName { get; set; }
        //public string CompanyName { get; set; }
        //public string AddressLine2 { get; set; }
        //public string County { get; set; }
        //public string Country { get; set; }
        //public string PhoneNumber { get; set; }
        //public string UniquePhysicianIdentificationNumber { get; set; }
        //public string DrugEnforcementAdministrationNumber { get; set; }
        //public string PhysicianLicenseNumber { get; set; }
        //public string ProfessionalDegree { get; set; }
        //public string PhysicianSpecialty { get; set; }
        //public int SalesTeamId { get; set; }
        //public string FaxNumber { get; set; }
        //public int MasterSalesTeamId { get; set; }
        //public Nullable<System.DateTime> LastPatientReferralDateId { get; set; }
    }
}