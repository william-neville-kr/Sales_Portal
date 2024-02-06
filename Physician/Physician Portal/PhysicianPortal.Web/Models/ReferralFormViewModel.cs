using System;

namespace PhysicianPortal.Web.Models
{
    public class ReferralFormViewModel
    {
        public int ReferralFormId { get; set; }
        public int? PhysicianPortalPatientId { get; set; }
        public string ReferralFormCategoryName { get; set; }
        public string ReferralFormName { get; set; }
        public string ReferralFormLink { get; set; }
        public DateTime? FilledOn { get; set; }
        public string FilledBy { get; set; }

        public bool? IsSubmited { get; set; }
    }
}