using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTeam.Web.Models
{
    public class PatientIdPhysicianIdModel
    {
        public int? SalesTeamId { get; set; }
        public int? PhysicianId { get; set; }
        public int? PatientId { get; set; }
    }
}