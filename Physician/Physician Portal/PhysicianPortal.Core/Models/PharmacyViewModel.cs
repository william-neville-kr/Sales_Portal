using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianPortal.Core.Models
{
    public class PharmacyViewModel
    {
        [DisplayName("PharmacyId")]
        public int PharmacyId { get; set; }
        [DisplayName("ERP Code")]
        [Required]
        //[DataType(DataType.DateTime)]
        public string ERPCode { get; set; }
        [DisplayName("Pharmacy Name")]
        [Required]
        public string PharmacyName { get; set; }
        [DisplayName("RecordSourceId")]
        public Nullable<int> RecordSourceId { get; set; }
        [DisplayName("Company Code")]
        [Required]
        public string CompanyCode { get; set; }
        [DisplayName("NABP")]
        [Required]
        public string NABP { get; set; }
        [DisplayName("DEA")]
        [Required]
        public string DEANumber { get; set; }
        [DisplayName("NPI")]
        [Required]
        public string NationalProviderIdentifier { get; set; }
        [DisplayName("Address Line 1")]
        [Required]
        public string AddressLine1 { get; set; }
        [DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }
        [DisplayName("City")]
        [Required]
        public string City { get; set; }
        [DisplayName("County")]
        public string County { get; set; }
        [DisplayName("State")]
        [Required]
        public string StateProvince { get; set; }
        [DisplayName("Country")]
        [Required]
        public string Country { get; set; }
        [DisplayName("ZIP")]
        [Required]
        public string PostalCode { get; set; }
        [DisplayName("Phone Number")]
        [Required]
        //[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DisplayName("Fax Number")]
        //[DataType(DataType.PhoneNumber)]
        public string FaxNumber { get; set; }
    }
}
