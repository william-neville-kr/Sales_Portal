using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class PatientDocumentModel
    {
        public int DocumentId { get; set; }
        public string DocumentIdEncrypted { get; set; }
        public string DocumentDescription { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public string PatientFullName { get; set; }
        public string PhysicianFullName { get; set; }
        public string NationalProviderIdentifier { get; set; }
        public string DocumentName { get; set; }
    }
}