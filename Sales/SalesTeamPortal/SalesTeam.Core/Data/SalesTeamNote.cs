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
    
    public partial class SalesTeamNote
    {
        public int SalesTeamNoteId { get; set; }
        public Nullable<int> PhysicianId { get; set; }
        public Nullable<int> SalesTeamId { get; set; }
        public Nullable<int> PatientId { get; set; }
        public string SalesTeamNoteHeader { get; set; }
        public string SalesTeamNote1 { get; set; }
        public Nullable<bool> PrivateFlag { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public Nullable<bool> EmailSend { get; set; }
        public Nullable<bool> FaxSend { get; set; }
        public Nullable<bool> PharmacySend { get; set; }
        public Nullable<int> CreatedSalesTeamId { get; set; }
    }
}
