using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
//using System.Web.Mvc;

namespace PhysicianPortal.Core.Data
{
    [MetadataType(typeof(PhysicianPortalFeedbackMetaData))]
    public partial class PhysicianPortalFeedback
    {
    }

    public class PhysicianPortalFeedbackMetaData
    {
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Feedback Status")]
        public Nullable<int> FeedbackStatusId { get; set; }
        [Required]
        [Display(Name = "Feedback Related")]
        public Nullable<int> FeedbackRelatedId { get; set; }
    }
}
