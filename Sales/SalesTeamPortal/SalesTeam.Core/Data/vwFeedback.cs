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
    
    public partial class vwFeedback
    {
        public int FeedbackId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string ClassName { get; set; }
        public string FeedbackRelatedName { get; set; }
        public string SalesRepresentativeFullName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    }
}