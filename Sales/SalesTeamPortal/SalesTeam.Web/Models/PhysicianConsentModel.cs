using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTeam.Web.Models
{
    public class PhysicianConsentModel
    {
        public int PhysicianId { get; set; }
        public int SalesTeamId { get; set; }
        public string SalesTeamName { get; set; }
        public int ConsentGrantedStatus { get; set; } // 1 = Granted, 2 = Denied, 3 = Pending
        public DateTime ConsentRequestedOn { get; set; }
        public DateTime? ConsentChangedOn { get; set; }
        public string PhysicianEmail { get; set; }
        public string ConsentGrantedString
        {
            get
            {
                switch (this.ConsentGrantedStatus)
                {
                    case 1:
                        return "Granted";
                    case 2:
                        return "Denied";
                    case 3:
                        return "Pending";
                    default:
                        return "Unknown";
                }
            }
        }

    }
}