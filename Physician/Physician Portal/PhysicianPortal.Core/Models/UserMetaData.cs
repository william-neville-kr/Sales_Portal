using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianPortal.Core.Data
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User : IUser
    {
        /* Add any attributes that do not exist in the User db table. Use NotMapped attribute */

        [NotMapped]
        public string Id
        {
            get
            {
                return UserId.ToString();
            }
        }

        [NotMapped]
        public bool EmailConfirmed
        {
            get
            {
                return true;
            }
        }


        [NotMapped]
        public string UserName
        {
            get
            {
                return Email;
            }

            set
            {
                Email = value;
            }
        }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, ClaimsIdentity identity = null)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            //Checking for impersonation claims here, if impersonation claims existed in the previous identity, we need to re add them to the current identity

            if (identity != null)
            {
                if (identity.FindFirstValue("IsImpersonating") == "true")
                {
                    if (userIdentity.HasClaim(c => c.Type == "IsImpersonating"))
                    {
                        var existingClaim = userIdentity.FindFirst("IsImpersonating");
                        userIdentity.RemoveClaim(existingClaim);
                    }
                    if (userIdentity.HasClaim(c => c.Type == "OriginalIdOfImpersonatingUser"))
                    {
                        var existingClaim = userIdentity.FindFirst("OriginalIdOfImpersonatingUser");
                        userIdentity.RemoveClaim(existingClaim);
                    }

                    var newImpersonatingClaim = new Claim("IsImpersonating", "true");
                    var newImpersonatingUserIdClaim = new Claim("OriginalIdOfImpersonatingUser", identity.FindFirst("OriginalIdOfImpersonatingUser").Value);

                    userIdentity.AddClaim(newImpersonatingClaim);
                    userIdentity.AddClaim(newImpersonatingUserIdClaim);
                }
            }
            return userIdentity;
        }
    }
    public class UserMetaData
    {
        [ScaffoldColumn(false)]
        public int UserId { get; set; }


        [Display(Name = "Role")]
        [Required]
        public Nullable<int> RoleId { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public string PasswordHash { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact No")]
        [Required]
        public string PhoneNumber;


        //[Display(Name = "Active")]
        //public bool Active { get; set; }


        //[ScaffoldColumn(false)]
        //public DateTime CreatedOn { get; set; }

        //[ScaffoldColumn(false)]
        //public int CreatedByUserId { get; set; }

        //[ScaffoldColumn(false)]
        //public DateTime LastUpdatedOn { get; set; }

        //[ScaffoldColumn(false)]
        //public DateTime? LastLoginDate { get; set; }

        //[ScaffoldColumn(false)]
        //public int LastUpdatedByUserId { get; set; }

    }
}
