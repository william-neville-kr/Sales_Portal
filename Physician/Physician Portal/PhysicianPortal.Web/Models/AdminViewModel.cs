using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class UsersListModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string NPI { get; set; }
        public string Role { get; set; }
        public string LastLogin { get; set; }
        public string LastLoginIP { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public string IsActiveStr
        {
            get
            {
                return IsActive ? "Yes" : "No";
            }
        }
        public string IsVerifiedStr
        {
            get
            {
                return IsVerified ? "Yes" : "No";
            }
        }
    }

    public class EditPhysicianSignature
    {
        [Display(Name = "Signature")]
        public byte[] Signature { get; set; }

        public string SignatureBase64String { get; set; }

        public int PhysicianId { get; set; }
    }

    public class OfficesListModel
    {
        public int OfficeId { get; set; }
        public string OfficeTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }

    public class OfficeUsersViewModel
    {
        public int OfficeId { get; set; }
    }

    public class AddUserModel
    {
        [Display(Name = "Role")]
        [Required]
        public Nullable<int> RoleId { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = "The password must be atleast 8 characters in length", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must containt atleast one digit")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        [Required]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "NPI")]
        public string NPI { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact No")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Join Date")]
        public DateTime? JoinDate { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Office")]
        public List<int> OfficeList { get; set; }

        [Display(Name = "Signature")]
        public byte[] Signature { get; set; }

        //[Display(Name = "Physician")]
        [Display(Name = "Provider")]
        public List<int> PhysicianList { get; set; }
        public string PhysicianListStr { get; set; }
    }

    public class Physican_Id_NameViewModel
    {
        public int PhysicianId { get; set; }
        public string FullName { get; set; }
        public string NPI { get; set; }
    }

    public class AddOfficeModel
    {
        public int OfficeId { get; set; }

        [Display(Name = "Office Title")]
        [Required]
        public string OfficeTitle { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Fax Number")]
        [Required]
        public string FaxNumber { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required]
        public string State { get; set; }

        [Display(Name = "Zip")]
        [Required]
        public string Zip { get; set; }

        [Display(Name = "Document Path")]
        //[Required]
        public string SubmitChecklistDocumentPath { get; set; }
    }
}