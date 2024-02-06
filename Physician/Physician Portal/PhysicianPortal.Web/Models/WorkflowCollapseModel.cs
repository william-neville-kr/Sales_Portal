using PhysicianPortal.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class WorkflowCollapseModel
    {
        public string CollapseTitle { get; set; }
        public DateTime? WorkflowStartDateId { get; set; }
        public string WorkflowStartDateIdStr
        {
            get
            {
                if (WorkflowStartDateId.HasValue)
                {
                    return WorkflowStartDateId.Value.ToString("MMM-dd-yyyy");
                }
                return "";
            }
        }
        public string WorkflowName { get; set; }
        public List<vwWorkflow> items { get; set; }
    }
}