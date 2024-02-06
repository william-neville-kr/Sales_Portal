using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTeam.Web.Models
{
    public class PDFFormModel
    {
        public string FileName { get; set; }
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }

        //FaxDocumentModel
        public string DocumentPath { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        //Physician ID for getting physician unique address from vwPhysicianAddress
        public int PID { get; set; }
    }
}