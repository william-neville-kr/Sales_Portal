//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhysicianPortal.Core.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class InsuranceEligibilityInfo
    {
        public int InsuranceEligibilityInfoId { get; set; }
        public string EligibilityValue { get; set; }
        public Nullable<int> PatientId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string PokitDok_TradingPartnerId { get; set; }
    
        public virtual PokitDok_TradingPartner PokitDok_TradingPartner { get; set; }
    }
}