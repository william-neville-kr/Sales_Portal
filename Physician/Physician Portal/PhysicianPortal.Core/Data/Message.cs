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
    
    public partial class Message
    {
        public int MessageId { get; set; }
        public Nullable<int> PhysicianId { get; set; }
        public Nullable<int> PatientId { get; set; }
        public string MessageBody { get; set; }
        public Nullable<int> PreMessageId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string Action { get; set; }
        public Nullable<bool> IsAction { get; set; }
        public Nullable<int> SentBy { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
    }
}
