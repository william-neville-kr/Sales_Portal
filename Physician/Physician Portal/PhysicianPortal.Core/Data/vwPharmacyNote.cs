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
    
    public partial class vwPharmacyNote
    {
        public Nullable<int> PatientId { get; set; }
        public int PhysicianId { get; set; }
        public string FullName { get; set; }
        public int PharmacyNoteId { get; set; }
        public string NoteHeader { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> NoteDate { get; set; }
        public string StaffFullName { get; set; }
        public bool IsTechnician { get; set; }
    }
}
