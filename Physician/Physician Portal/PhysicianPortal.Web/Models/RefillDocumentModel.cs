using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefillDocumentModel
    {
        public int DocumentId { get; set; }
        public string DocumentIdEncrypted { get; set; }
        public Nullable<System.DateTime> SentOn { get; set; }
        public string PatientFullName { get; set; }
        public string PhysicianFullName { get; set; }
        public string NationalProviderIdentifier { get; set; }
        public string DocumentName { get; set; }
        public int PhysicianId { get; set; }
        public string PhysicianIdEncrypted { get; set; }
    }
}