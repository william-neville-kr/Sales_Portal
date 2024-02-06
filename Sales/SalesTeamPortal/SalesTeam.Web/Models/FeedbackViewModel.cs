using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTeam.Web.Models
{
    public class FeedbackViewModel
    {
        public int FeedbackId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

        //public string ColorCode { get; set; }
        public string ClassName { get; set; }
    }
}