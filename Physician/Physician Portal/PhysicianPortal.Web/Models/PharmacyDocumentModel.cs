using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class PharmacyDocumentModel
    {
        public int DocumentId { get; set; }
        public string DocumentIdEncrypted { get; set; }
        public string DocumentDescription { get; set; }
        public Nullable<System.DateTime> DocumentCreatedDate { get; set; }
        public Nullable<System.DateTime> DocumentAssignedDate { get; set; }
    }
}