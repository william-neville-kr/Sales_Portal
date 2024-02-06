using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace PhysicianPortal.Core.Models
namespace PhysicianPortal.Core.Data
{
    public partial class vwWorkflow
    {
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
    }
}
