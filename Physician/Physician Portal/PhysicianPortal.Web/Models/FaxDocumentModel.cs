using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class FaxDocumentModel
    {
        //public string DocumentPath { get; set; }
        public string DocumentIdEncrypted { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
    }
    public class RefillFaxModel
    {        
        public string HTMLText { get; set; }
        //public string FaxNumber { get; set; }
        //public string Email { get; set; }
        public byte[] Signature { get; set; }
        public bool IsSigned { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public string PatientMRN { get; set; }
        //public string NPI { get; set; }
    }
}