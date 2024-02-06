using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormPediatricGastroenterologyViewModel : ReferralFormBaseViewModel
    {
        public int RefFormPediatricgatroenterologyId { get; set; }
        public bool ChkBox_CLINICAL_Pediatric555_9 { get; set; }
        public bool ChkBox_CLINICAL_Pediatric556_0 { get; set; }
        public string CLINICAL_DrugAllergies_TxtBox { get; set; }
        public string CLINICAL_Prior_TxtBox1 { get; set; }
        public string CLINICAL_LengthofTreatment_TxtBox1 { get; set; }
        public string CLINICAL_ReasonforDiscontinuing_TxtBox1 { get; set; }
        public string CLINICAL_Prior_TxtBox2 { get; set; }
        public string CLINICAL_LengthofTreatment_TxtBox2 { get; set; }
        public string CLINICAL_ReasonforDiscontinuing_TxtBox2 { get; set; }
        public string CLINICAL_Prior_TxtBox3 { get; set; }
        public string CLINICAL_LengthofTreatment_TxtBox3 { get; set; }
        public string CLINICAL_ReasonforDiscontinuing_TxtBox3 { get; set; }
        public double CLINICAL_Patient_Weight_TxtBox { get; set; }
        public bool ChkBox_latexallergy_Yes { get; set; }
        public bool ChkBox_latexallergy_No { get; set; }
        public bool ChkBox_beforestart_Yes { get; set; }
        public bool ChkBox_beforestart_No { get; set; }
        public bool ChkBox_PRESCRIPTION_Humira { get; set; }
        public bool ChkBox_PRESCRIPTION_PediatricCrohnsDisease1 { get; set; }
        public bool ChkBox_PRESCRIPTION_LoadDay1 { get; set; }
        public bool ChkBox_PRESCRIPTION_Maintenancebegins1 { get; set; }
        public string PRESCRIPTION_REFILLS_TxtBox1 { get; set; }
        public bool ChkBox_PRESCRIPTION_PediatricCrohnsDisease2 { get; set; }
        public bool ChkBox_PRESCRIPTION_LoadDay2 { get; set; }
        public bool ChkBox_PRESCRIPTION_four40mginjections { get; set; }
        public bool ChkBox_PRESCRIPTION_two40mg { get; set; }
        public bool ChkBox_PRESCRIPTION_Maintenancebegins2 { get; set; }
        public bool ChkBox_PRESCRIPTION_CrohnsStarterPackage { get; set; }
        public bool ChkBox_PRESCRIPTION_20mgPreFilledSyringe { get; set; }
        public bool ChkBox_PRESCRIPTION_40mgPreFilledSyringe { get; set; }
        public bool ChkBox_PRESCRIPTION_40mgPreFilledPen { get; set; }
        public string PRESCRIPTION_REFILLS_TxtBox2 { get; set; }
        public bool ChkBox_PRESCRIPTION_Remicade { get; set; }
        public bool ChkBox_PRESCRIPTION_100mgoflyophilized { get; set; }
        public bool ChkBox_PRESCRIPTION_LoadDay3 { get; set; }
        public bool ChkBox_PRESCRIPTION_Maintenancebegins3 { get; set; }
        public string PRESCRIPTION_REFILLS_TxtBox3 { get; set; }
        public bool ChkBox_PRESCRIPTION_LoadDay4 { get; set; }
        public bool ChkBox_PRESCRIPTION_Maintenancebegins4 { get; set; }
        public string PRESCRIPTION_REFILLS_TxtBox4 { get; set; }
    }
}