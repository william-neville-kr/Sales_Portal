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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.AuditTrails = new HashSet<AuditTrail>();
            this.User_Office_Relationship = new HashSet<User_Office_Relationship>();
            this.UserClaims = new HashSet<UserClaim>();
        }
    
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<bool> PhoneNumberConfirmed { get; set; }
        public Nullable<bool> TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public Nullable<bool> LockoutEnabled { get; set; }
        public Nullable<int> AccessFailedCount { get; set; }
        public Nullable<int> PhysicianId { get; set; }
        public Nullable<int> ParentUserId { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string LastLoginIP { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsVerified { get; set; }
        public string NPI { get; set; }
        public byte[] Signature { get; set; }
        public Nullable<bool> EmailVarified { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string SSN { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public string FullName { get; set; }
        public Nullable<int> HomePageId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual Role Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Office_Relationship> User_Office_Relationship { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual HomePage HomePage { get; set; }
    }
}
