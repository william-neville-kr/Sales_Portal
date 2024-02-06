using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTeam.Web.Models
{
    public class SalesTeamPhysicianDrugTherapySearchViewModel : PhysicianViewModel
    {
        public string PhysicianFullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public Nullable<System.DateTime> LastPatientReferralDateId { get; set; }
        public string DiseaseState { get; set; }
        public string CoreTherapyDrugShortName { get; set; }
        public Nullable<System.DateTime> LastTherepyReferralDate { get; set; }
    }
}