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
    
    public partial class vwReferralForm
    {
        public int ReferralFormId { get; set; }
        public string ReferralFormCategoryName { get; set; }
        public string ReferralFormName { get; set; }
        public string ReferralFormLink { get; set; }
        public string FilledBy { get; set; }
        public Nullable<System.DateTime> FilledOn { get; set; }
        public Nullable<int> PhysicianPortalPatientId { get; set; }
        public int ReferralFormCategoryId { get; set; }
    }
}