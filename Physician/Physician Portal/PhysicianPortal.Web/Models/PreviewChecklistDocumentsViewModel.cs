using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class PreviewChecklistDocumentsViewModel
    {
        public string ReferralFormName { get; set; }
        //public List<Core.Data.spPatientChecklist_Result> spPatientChecklist_ResultList { get; set; }
        public List<spPatientChecklist_ResultViewModel> spPatientChecklist_ResultList { get; set; }
    }
    public class spPatientChecklist_ResultViewModel
    {
        public int PhysicianPortalPatientId { get; set; }
        public Nullable<int> ReferralFormChecklistRelationshipId { get; set; }
        public Nullable<bool> Isuploaded { get; set; }
        public string ChecklistName { get; set; }

        //PhysicianPortalPatientChecklistRelationship
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}