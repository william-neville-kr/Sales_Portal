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
    
    public partial class PhysicianPortalPrescription
    {
        public int PrescriptionId { get; set; }
        public Nullable<int> FillNumber { get; set; }
        public Nullable<int> FillsAllowed { get; set; }
        public Nullable<int> FillsRemaining { get; set; }
        public Nullable<int> DaysSupplied { get; set; }
        public Nullable<int> DaysSuppllyPrescribed { get; set; }
        public string ICD10 { get; set; }
        public string ICD10Description { get; set; }
    }
}