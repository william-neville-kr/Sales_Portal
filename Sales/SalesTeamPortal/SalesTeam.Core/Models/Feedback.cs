using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesTeam.Core.Data
{
    [MetadataType(typeof(FeedbackMetaData))]
    public partial class Feedback
    {
    }

    public class FeedbackMetaData
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
