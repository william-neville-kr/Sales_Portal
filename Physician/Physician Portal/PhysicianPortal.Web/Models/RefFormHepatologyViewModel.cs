﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormHepatologyViewModel: ReferralFormBaseViewModel
    {
        public int RefFormHeptalogyId { get; set; }
        public bool ChkBox_CLINICAL_B18_2Chronic { get; set; }
        public bool ChkBox_CLINICAL_K72_90 { get; set; }
        public bool ChkBox_CLINICAL_K72_91 { get; set; }
        public bool ChkBox_CLINICAL_C22_0 { get; set; }
        public bool ChkBox_CLINICAL_C22_2 { get; set; }
        public bool ChkBox_CLINICAL_C22_7 { get; set; }
        public bool ChkBox_CLINICAL_C22_8 { get; set; }
        public bool ChkBox_CLINICAL_Other { get; set; }
        public string Clinical_Other_TxtBox { get; set; }
        public string Clinical_Drug_Allergies_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_Genotype_1 { get; set; }
        public bool ChkBox_CLINICAL_Genotype_1a { get; set; }
        public bool ChkBox_CLINICAL_NSSA_Yes { get; set; }
        public bool ChkBox_CLINICAL_NSSA_No { get; set; }
        public bool ChkBox_CLINICAL_1b { get; set; }
        public bool ChkBox_CLINICAL_2 { get; set; }
        public bool ChkBox_CLINICAL_3 { get; set; }
        public bool ChkBox_CLINICAL_4 { get; set; }
        public bool ChkBox_CLINICAL_5 { get; set; }
        public bool ChkBox_CLINICAL_6 { get; set; }
        public string CLINICAL_Viral_Load_TxtBox { get; set; }
        public System.DateTime CLINICAL_Viral_Load_TxtBox_Date { get; set; }
        public bool ChkBox_CLINICAL_Treatment_Naive { get; set; }
        public bool ChkBox_CLINICAL_Previously_Treated { get; set; }
        public string CLINICAL_Prior_treatment_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_Non_Responder { get; set; }
        public bool ChkBox_CLINICAL_Responder_Relapser { get; set; }
        public System.DateTime CLINICAL_previous_therapy_from_TxtBox_Date { get; set; }
        public System.DateTime CLINICAL_previous_therapy_to_TxtBox_Date1 { get; set; }
        public int CLINICAL_Total_of_months_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_HIV_Coinfected_Yes { get; set; }
        public bool ChkBox_CLINICAL_HIV_Coinfected_No { get; set; }
        public bool ChkBox_CLINICAL_HBV_Coinfected_Yes { get; set; }
        public bool ChkBox_CLINICAL_HBV_Coinfected_No { get; set; }
        public bool ChkBox_CLINICAL_Compensated_Liver_Disease_Yes { get; set; }
        public bool ChkBox_CLINICAL_Compensated_Liver_Disease_No { get; set; }
        public bool ChkBox_CLINICAL_Cirrhosis_Yes { get; set; }
        public bool ChkBox_CLINICAL_Cirrhosis_No { get; set; }
        public string CLINICAL_Metavir_Score_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_Solid_Organ_Yes { get; set; }
        public bool ChkBox_CLINICAL_Solid_Organ_No { get; set; }
        public bool ChkBox_CLINICAL_Awaiting_Liver_Transplant_Yes { get; set; }
        public bool ChkBox_CLINICAL_Awaiting_Liver_Transplant_No { get; set; }
        public bool ChkBox_PRESCRIPTION_Daklinza { get; set; }
        public bool ChkBox_PRESCRIPTION_60mg { get; set; }
        public bool ChkBox_PRESCRIPTION_30mg { get; set; }
        public bool ChkBox_PRESCRIPTION_90mg { get; set; }
        public string PRESCRIPTION_Refills_TxtBox1 { get; set; }
        public bool ChkBox_PRESCRIPTION_Epclusa { get; set; }
        public string PRESCRIPTION_Refills_TxtBox2 { get; set; }
        public bool ChkBox_PRESCRIPTION_Harvoni { get; set; }
        public string PRESCRIPTION_Refills_TxtBox3 { get; set; }
        public bool ChkBox_PRESCRIPTION_Olysio { get; set; }
        public string PRESCRIPTION_Refills_TxtBox4 { get; set; }
        public bool ChkBox_PRESCRIPTION_Sovaldi { get; set; }
        public string PRESCRIPTION_Refills_TxtBox5 { get; set; }
        public bool ChkBox_PRESCRIPTION_Technivie { get; set; }
        public string PRESCRIPTION_Refills_TxtBox6 { get; set; }
        public bool ChkBox_PRESCRIPTION_Viekira_Pak { get; set; }
        public string PRESCRIPTION_Refills_TxtBox7 { get; set; }
        public bool ChkBox_PRESCRIPTION_Viekira_XR { get; set; }
        public string PRESCRIPTION_Refills_TxtBox8 { get; set; }
        public bool ChkBox_PRESCRIPTION_Zepatier { get; set; }
        public string PRESCRIPTION_Refills_TxtBox9 { get; set; }
        public bool ChkBox_PRESCRIPTION_Moderiba { get; set; }
        public bool ChkBox_PRESCRIPTION_600mgAMand600mgPM1 { get; set; }
        public bool ChkBox_PRESCRIPTION_600mgAMand400mgPM1 { get; set; }
        public bool ChkBox_PRESCRIPTION_Ribavirin { get; set; }
        public bool ChkBox_PRESCRIPTION_200mg_Tabs1 { get; set; }
        public bool ChkBox_PRESCRIPTION_200mg_Caps1 { get; set; }
        public bool ChkBox_PRESCRIPTION_400mgAMand400mgPM1 { get; set; }
        public bool ChkBox_PRESCRIPTION_400mgAMand200mgPM1 { get; set; }
        public bool ChkBox_PRESCRIPTION_Ribasphere { get; set; }
        public bool ChkBox_PRESCRIPTION_200mg_Tabs2 { get; set; }
        public bool ChkBox_PRESCRIPTION_200mg_Caps2 { get; set; }
        public bool ChkBox_PRESCRIPTION_Other { get; set; }
        public string PRESCRIPTION_Take_mg_AM { get; set; }
        public string PRESCRIPTION_Take_mg_PM { get; set; }
        public string PRESCRIPTION_Refills_TxtBox10 { get; set; }
        public bool ChkBox_PRESCRIPTION_Riba_Pak { get; set; }
        public bool ChkBox_PRESCRIPTION_600mgAMand600mgPM2 { get; set; }
        public bool ChkBox_PRESCRIPTION_600mgAMand400mgPM2 { get; set; }
        public bool ChkBox_PRESCRIPTION_Moderiba_Pak { get; set; }
        public bool ChkBox_PRESCRIPTION_400mgAMand400mgPM2 { get; set; }
        public bool ChkBox_PRESCRIPTION_400mgAMand200mgPM2 { get; set; }
        public string PRESCRIPTION_Refills_TxtBox11 { get; set; }
        public bool ChkBox_PRESCRIPTION_Xifaxan { get; set; }
        public string PRESCRIPTION_Lactulose_TxtBox { get; set; }
        public string PRESCRIPTION_Refills_TxtBox12 { get; set; }
        public bool ChkBox_PRESCRIPTION_Other_Last { get; set; }
        public string PRESCRIPTION_Other_Last_TxtBox1 { get; set; }
        public string PRESCRIPTION_Other_Last_TxtBox2 { get; set; }
        public string PRESCRIPTION_Other_Quantity_TxtBox { get; set; }
        public string PRESCRIPTION_Refills_TxtBox13 { get; set; }
    }
}