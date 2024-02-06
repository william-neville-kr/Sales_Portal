using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormImmunologyViewModel : ReferralFormBaseViewModel
    {
        public int RefFormImmunologyId { get; set; }
        public bool ChkBox_CLINICAL_J45_40Moderate { get; set; }
        public bool ChkBox_CLINICAL_J45_50Severe { get; set; }
        public bool ChkBox_CLINICAL_L50_1Chronic { get; set; }
        public bool ChkBox_CLINICAL_Other { get; set; }
        public string CLINICAL_Dxcode_TxtBox { get; set; }
        public string CLINICAL_Condition_TxtBox { get; set; }
        public string CLINICAL_DrugAllergies_TxtBox { get; set; }
        public bool ChkBox_Short_actingbeta { get; set; }
        public bool ChkBox_Long_actingbeta { get; set; }
        public bool ChkBox_Antihistamines { get; set; }
        public bool ChkBox_Decongestants { get; set; }
        public bool ChkBox_Immunotherapy { get; set; }
        public bool ChkBox_Inhaledcorticosteroid { get; set; }
        public bool ChkBox_Leukotrienemodifiers { get; set; }
        public bool ChkBox_Oralsteroids { get; set; }
        public bool ChkBox_Nasalsteroids { get; set; }
        public bool ChkBox_Other { get; set; }
        public string CLINICAL_Other_TxtBox { get; set; }
        public string CLINICAL_Pleaselisttherapies { get; set; }
        public bool ChkBox_Historyofpositiveskin { get; set; }
        public string CLINICAL_PretreatmentserumlgE_TxtBox { get; set; }
        public System.DateTime CLINICAL_Test_Date { get; set; }
        public double CLINICAL_Patientweight_TxtBox { get; set; }
        public System.DateTime CLINICAL_Dateweight_TxtBox { get; set; }
        public bool ChkBox_Allergist { get; set; }
        public bool ChkBox_Pulmonologist { get; set; }
        public bool ChkBox_ENT { get; set; }
        public bool ChkBox_Primarycare { get; set; }
        public bool ChkBox_Pediatrician { get; set; }
        public bool ChkBox_MDSpecialty_Other { get; set; }
        public string CLINICAL_MDSpecialty_Other_TxtBox { get; set; }
        public bool ChkBox_Naivenewstart { get; set; }
        public bool ChkBox_Restrat { get; set; }
        public bool ChkBox_ContinuedTherapy { get; set; }
        public System.DateTime CLINICAL_Lastinjectiondate_TxtBox { get; set; }
        public bool ChkBox_Xolair { get; set; }
        public bool ChkBox_75mgsubcutaneouslyevery4weeks { get; set; }
        public bool ChkBox_28daysupply { get; set; }
        public string PRESCRIPTION_REFILLS_TxtBox1 { get; set; }
        public bool ChkBox_150mgsubcutaneouslyevery4weeks { get; set; }
        public bool ChkBox_84daysupply { get; set; }
        public string PRESCRIPTION_REFILLS_TxtBox2 { get; set; }
        public bool ChkBox_225mgsubcutaneouslyevery2weeks { get; set; }
        public bool ChkBox_225mgsubcutaneouslyevery4weeks { get; set; }
        public bool ChkBox_300mgsubcutaneouslyevery2weeks { get; set; }
        public bool ChkBox_300mgsubcutaneouslyevery4weeks { get; set; }
        public bool ChkBox_375mgsubcutaneouslyevery2weeks { get; set; }
        public bool ChkBox_XolairPatientswithCIU { get; set; }
        public bool ChkBox_150mgsubcutaneouslyevery4weeks_2 { get; set; }
        public bool ChkBox_28daysupply2 { get; set; }
        public string PRESCRIPTION_REFILLS_TxtBox3 { get; set; }
        public bool ChkBox_300mgsubcutaneouslyevery4weeks_2 { get; set; }
        public bool ChkBox_84daysupply2 { get; set; }
        public string PRESCRIPTION_REFILLS_TxtBox4 { get; set; }
        public bool ChkBox_EpiPen { get; set; }
        public bool ChkBox_Injection0_3mg { get; set; }
        public bool ChkBox_EpiPen0_3mg { get; set; }
        public bool ChkBox_Injection0_15mg { get; set; }
        public bool ChkBox_EpiPenJr0_15mg { get; set; }
        public bool ChkBoxOther { get; set; }
        public string Other_TxtBox_1 { get; set; }
        public string Other_TxtBox_2 { get; set; }
        public string Other_Quantity_TxtBox { get; set; }
        public string Other_REFILLS_TxtBox { get; set; }
        public bool ChkBox_nola_L20_9AtopicDermatitis { get; set; }
        public bool ChkBox_nola_Dermatologist { get; set; }
        public bool ChkBox_nola_Dupixent { get; set; }
        public bool ChkBox_nola_Dupixent_300mg1 { get; set; }
        public bool ChkBox_nola_Dupixent_300mg2 { get; set; }
        public bool ChkBox_nola_Dupixent_Load { get; set; }
        public bool ChkBox_nola_Dupixent_Mantainance { get; set; }
        public string nola_Dupixent_Refills_TxtBox { get; set; }
    }
}