using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormOsteoporosisViewModel : ReferralFormBaseViewModel
    {
        public int RefFormOsteoporosisId { get; set; }
        public bool ChkBox_CLINICAL_M81_0 { get; set; }
        public bool ChkBox_CLINICAL_Other { get; set; }
        public string CLINICAL_DXCode { get; set; }
        public string CLINICAL_DrugAllergies { get; set; }
        public bool ChkBox_alendronate { get; set; }
        public string LengthofTreatment_TxtBox1 { get; set; }
        public bool ChkBox_ReasonforDiscontinuing1 { get; set; }
        public string ReasonforDiscontinuing1_TxtBox1 { get; set; }
        public bool ChkBox_bandronate { get; set; }
        public string LengthofTreatment_TxtBox2 { get; set; }
        public bool ChkBox_ReasonforDiscontinuing2 { get; set; }
        public string ReasonforDiscontinuing2_TxtBox2 { get; set; }
        public bool ChkBox_risedronate { get; set; }
        public string LengthofTreatment_TxtBox3 { get; set; }
        public bool ChkBox_ReasonforDiscontinuing3 { get; set; }
        public string ReasonforDiscontinuing3_TxtBox3 { get; set; }
        public bool ChkBox_prednisone { get; set; }
        public string LengthofTreatment_TxtBox4 { get; set; }
        public bool ChkBox_ReasonforDiscontinuing4 { get; set; }
        public string ReasonforDiscontinuing4_TxtBox4 { get; set; }
        public bool ChkBox_5 { get; set; }
        public string LengthofTreatment_TxtBox5 { get; set; }
        public bool ChkBox_ReasonforDiscontinuing5 { get; set; }
        public string ReasonforDiscontinuing5_TxtBox5 { get; set; }
        public string CLINICAL_BoneDensity_T_Score { get; set; }
        public string CLINICAL_BoneDensity_Type { get; set; }
        public System.DateTime CLINICAL_BoneDensity_Date { get; set; }
        public string CLINICAL_FractureHistorySite1 { get; set; }
        public System.DateTime CLINICAL_FractureHistorySite1Date { get; set; }
        public string CLINICAL_FractureHistorySite2 { get; set; }
        public System.DateTime CLINICAL_FractureHistorySite2Date { get; set; }
        public bool ChkBox_CLINICAL_patientbeenonForteo_Yes { get; set; }
        public bool ChkBox_CLINICAL_patientbeenonForteo_No { get; set; }
        public string CLINICAL_howlong_TxtBox { get; set; }
        public bool ChckBox_PatientAllergy_Yes { get; set; }
        public bool ChckBox_PatientAllergy_No { get; set; }
        public bool ChkBox_PRESCRIPTION_Boniva { get; set; }
        public string PRESCRIPTION_Refills_TxtBox1 { get; set; }
        public bool ChkBox_PRESCRIPTION_Forteo { get; set; }
        public string PRESCRIPTION_Refills_TxtBox2 { get; set; }
        public bool ChkBox_PRESCRIPTION_Prolia { get; set; }
        public string PRESCRIPTION_Refills_TxtBox3 { get; set; }
        public bool ChkBox_PRESCRIPTION_Reclast { get; set; }
        public string PRESCRIPTION_Refills_TxtBox4 { get; set; }
        public bool ChkBox_PRESCRIPTION_Other { get; set; }
        public string PRESCRIPTION_Other_TxtBox1 { get; set; }
        public string PRESCRIPTION_Other_TxtBox2 { get; set; }
        public string PRESCRIPTION_Other_TxtBox3 { get; set; }
        public string PRESCRIPTION_Other_Qty_TxtBox1 { get; set; }
        public string PRESCRIPTION_Refills_TxtBox5 { get; set; }
        public string CLINICAL_Other_TxtBox01 { get; set; }
        public string ChkBox_5_TxtBox { get; set; }
    }
}