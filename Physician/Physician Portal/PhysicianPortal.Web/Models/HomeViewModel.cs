using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class AddMessageViewModel
    {
        public Nullable<int> PhysicianId { get; set; }
        public Nullable<int> PatientId { get; set; }
        public string PatientIdEncrypted { get; set; }
        public string PhysicianIdEncrypted { get; set; }
        public int? PreMessageId { get; set; }
        [Display(Name = "New Message")]
        [Required]
        public string MessageBody { get; set; }
        [Display(Name = "Previous Message")]
        public string PreviousMessageBody { get; set; }

        [Display(Name = "Patient")]
        [Required]
        public string PatientName { get; set; }
        [Display(Name = "Physician")]
        [Required]
        public string PhysicianName { get; set; }
    }

    public class GridMessageViewModel
    {
        public int MessageId { get; set; }
        public Nullable<int> PhysicianId { get; set; }
        public Nullable<int> PatientId { get; set; }
        public string MessageBody { get; set; }
        public Nullable<int> PreMessageId { get; set; }
        public string PatientName { get; set; }
        public string PhysicianName { get; set; }
        public string CreatedOn { get; set; }
        public Nullable<int> SentBy { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string SenderName { get; set; }
        public int ThreadMessageCount { get; set; }
        //public Nullable<System.DateTime> ModifiedOn { get; set; }
        //public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
        public string PatientIdEncrypted { get; set; }
        public string PhysicianIdEncrypted { get; set; }
    }

    public class MessageThreadViewModel
    {
        public int ThredId { get; set; }
        public int? PatientId { get; set; }
        public string ReplyMessageBody { get; set; }
        public Nullable<bool> IsCompleted { get; set; }

    }

    public class RefillViewModel
    {
        public string PatientName { get; set; }
        public string FullName { get; set; }
        public string NationalProviderIdentifier { get; set; }
        public string RXNum { get; set; }
        public string Drug { get; set; }
        public Nullable<int> RXNumCount { get; set; }
        public string HTMLText { get; set; }
        [Required]
        public int Id { get; set; }
        public string PharmacyFaxNumber { get; set; }
        public bool IsSent { get; set; }
        public string SentBy { get; set; }
        public Nullable<System.DateTime> SentOn { get; set; }
        public string RefillStatus { get; set; }
        public Nullable<int> PatientId { get; set; }
        public string RefillDocumentPath { get; set; }
        public List<NewRefillViewModel> newRefillViewModelList { get; set; }
        public int NotesCount { get; set; }
    }

    public class NewRefillViewModel
    {
        //public int NewRefillId { get; set; }
        public string Qty { get; set; }
        public string AdditionalRefills { get; set; }
        public string DoNotSubstitute { get; set; }
        public Nullable<int> RefillFaxReportLogId { get; set; }
    }
}