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
    
    public partial class User_Office_Relationship
    {
        public int RelationshipId { get; set; }
        public int UserId { get; set; }
        public int OfficeId { get; set; }
    
        public virtual Office Office { get; set; }
        public virtual User User { get; set; }
    }
}
