using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTeam.Web.Models
{
    public class vwPatientInsuranceModel
    {
        public int PatientId { get; set; }
        public int InsuranceId { get; set; }
        public string InsuranceName { get; set; }
        public Nullable<int> RecordSourceId { get; set; }
        public string InsuranceType { get; set; }
        public string InsuranceLevelNumber { get; set; }
        public string InsuranceLevelName { get; set; }
        public string InsuranceBankIdentificationNumber { get; set; }
        public string InsuranceProcessControlNumber { get; set; }
        public System.DateTime LastUsedDate { get; set; }
        public string LastUsedDateProp {
            get
            {
                return this.LastUsedDate.ToShortDateString();
            }
        }
    }
}