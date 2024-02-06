using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormCardiovascularViewModel : ReferralFormBaseViewModel
    {
        public int RefFormCardiovascularId { get; set; }
        public bool DiagnosisE78_0 { get; set; }
        public bool DiagnosisE78_2 { get; set; }
        public bool DiagnosisE78_4 { get; set; }
        public bool DiagnosisE78_5 { get; set; }
        public bool DiagnosisASCVD { get; set; }
        public string DiagnosisASCVDCodes { get; set; }
        public string DrugAllergies { get; set; }
        public bool ChkBox_I20_0 { get; set; }
        public bool ChkBox_I20_9 { get; set; }
        public bool ChkBox_I21 { get; set; }
        public string TxtBox_I21 { get; set; }
        public bool ChkBox_I22 { get; set; }
        public string TxtBox_I22 { get; set; }
        public bool ChkBox_I25 { get; set; }
        public string TxtBox_I25 { get; set; }
        public bool ChkBox_I63 { get; set; }
        public string TxtBox_I63 { get; set; }
        public bool ChkBox_I65 { get; set; }
        public string TxtBox_I65 { get; set; }
        public bool ChkBox_I66 { get; set; }
        public string TxtBox_I66 { get; set; }
        public bool ChkBox_I67 { get; set; }
        public string TxtBox_I67 { get; set; }
        public bool ChkBox_I70 { get; set; }
        public string TxtBox_I70 { get; set; }
        public bool ChkBox_I73_9 { get; set; }
        public bool ChkBox_G45_9 { get; set; }
        public bool ChkBox_G46 { get; set; }
        public string TxtBox_G46 { get; set; }
        public bool ChkBox_Other { get; set; }
        public string TxtBox_Other { get; set; }
        public string Most_recent_LDL_C_level_on_treatment { get; set; }
        public System.DateTime Most_recent_LDL_C_level_on_treatment_Date { get; set; }
        public bool ChkBox_Atorvastatin { get; set; }
        public bool ChkBox_Ezetimibe { get; set; }
        public bool ChkBox_Pravastatin { get; set; }
        public bool ChkBox_Rosuvastatin { get; set; }
        public bool ChkBox_Simvastatin { get; set; }
        public string Prior_and_or_Current_Treatments_Other { get; set; }
        public string Dose { get; set; }
        public string Length_of_Treatment { get; set; }
        public string Reason_for_Discontinuing { get; set; }
        public string Family_History_of_ACSVD_Yes { get; set; }
        public string Family_History_of_ACSVD_No { get; set; }
        public string Allergies { get; set; }
        public bool C75_mg_mL_Pre_filled_Pen_2_pack { get; set; }
        public bool C150_mg_mL_Pre_filled_Pen_2_pack { get; set; }
        public bool C75_mg_mL_Pre_filled_Syringe_2_pack { get; set; }
        public bool C150_mg_mL_Pre_filled_Syringe_2_pack { get; set; }
        public bool Inject_subcutaneously_once_every_2_weeks1 { get; set; }
        public string Refills1 { get; set; }
        public bool C140_mg_mL_SureClick_1_pack { get; set; }
        public bool C140_mg_mL_SureClick_2_pack { get; set; }
        public bool C140_mg_mL_SureClick_3_pack { get; set; }
        public bool C140_mg_mL_Pre_filled_Syringe_1_pack { get; set; }
        public bool Inject_subcutaneously_once_every_2_weeks2 { get; set; }
        public bool Inject_subcutaneously_monthly { get; set; }
        public string Refills2 { get; set; }
        public bool C420_mg_3_5_mL_single_use_Pushtronex_System { get; set; }
        public bool Administer_subcutaneously_once_monthly { get; set; }
        public bool ChckBox_PatientAllergy_Yes { get; set; }
        public bool ChckBox_PatientAllergy_No { get; set; }
    }
}