//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SalesTeam.Core.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwPatientInsurance
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
    }
}
