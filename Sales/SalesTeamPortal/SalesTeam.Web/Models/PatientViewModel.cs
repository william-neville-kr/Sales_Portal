using System;

namespace SalesTeam.Web.Models
{
    public class PatientViewModel
    {
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }
        public Nullable<int> SalesTeamId { get; set; }
        public int SalesTeamQueueId { get; set; }

        public Nullable<System.DateTime> ReferralDateId { get; set; }
        public string PatientCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhysicianName { get; set; }
        public string PhyCity { get; set; }
        public string StateProvince { get; set; }
        public string DrugShortName { get; set; }
        public string ActivityStatus { get; set; }
        public string OriginatingPharmacy { get; set; }
        public Nullable<bool> NeedMoreInfo { get; set; }

        public string RXStatus { get; set; }
        public string DiseaseState { get; set; }

        public string Url_SId_PId { get; set; }

        //public string CompanyName { get; set; }
        //public string AddressLine1 { get; set; }
        //public string AddressLine2 { get; set; }
        //public string City { get; set; }
        //public string County { get; set; }
        //public string Country { get; set; }
        //public string PostalCode { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Gender { get; set; }
        //public string PreferredLanguage { get; set; }
        //public string DiseaseState { get; set; }
        //public string ServiceArea { get; set; }
        //public Nullable<int> StartOfCareDateId { get; set; }
        //public string PatientRepresentativeGroup { get; set; }
        //public string Diagnosis { get; set; }
        //public string IntakeRep { get; set; }
        //public string NurseCoordinator { get; set; }
        //public Nullable<int> YearOfBirth { get; set; }
        //public string ReferralSource { get; set; }
        //public Nullable<System.DateTime> LastFillDate { get; set; }
        //public string RXStatus { get; set; }
        //public string PhyStateProvince { get; set; }
        //public Nullable<System.DateTime> DateOfBirth { get; set; }
        //public string LastOrderEvent { get; set; }
    }
}