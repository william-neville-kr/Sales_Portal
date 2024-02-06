using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormRemicadeViewModel : ReferralFormBaseViewModel
    {
        public int RefFormRemicadeId { get; set; }
        public string CLINICAL_Diagnosis_TxtBox { get; set; }
        public string CLINICAL_ICD_10_TxtBox { get; set; }
        public string CLINICAL_DrugAllergies_TxtBox { get; set; }
        public System.DateTime DATEOFNEXTINFUSION_TxtBox { get; set; }
        public double CURRENTWEIGHT_TxtBox { get; set; }
        public bool ChkBox_5MG_KG { get; set; }
        public bool ChkBox_3MG_KG { get; set; }
        public bool ChkBox_MG_KG { get; set; }
        public string MG_KG_TxtBox { get; set; }
        public bool ChkBox_LOADINGDOSE { get; set; }
        public string ADMINISTER_TxtBox1 { get; set; }
        public bool ChkBox_MAINTENANCEDOSE { get; set; }
        public string ADMINISTER_TxtBox2 { get; set; }
        public string MGIVEVERYWEEKS_TxtBox { get; set; }
        public string REFILLS_TxtBox { get; set; }
        public bool ChkBox_MDOFFICE { get; set; }
        public bool ChkBox_INFUSIONCLINIC { get; set; }
        public string Name_TxtBox { get; set; }
        public string Phone_TxtBox { get; set; }
        public bool ChkBox_HOMEINFUSION { get; set; }
        public string HOMEHEALTHAGENCY_TxtBox { get; set; }
        public string Phone_TxtBox2 { get; set; }
        public bool ChkBox_KROGERSPECIALTY { get; set; }
        public bool ChkBox_INFUSIONSUPPLIES { get; set; }
        public bool ChkBox_ACETAMINOPHEN { get; set; }
        public bool ChkBox_DIPHENHYDRAMINE { get; set; }
        public bool ChkBox_DIPHENHYDRAMINE2 { get; set; }
        public bool ChkBox_PREDNISONE { get; set; }
        public bool ChkBox_SOLUMEDROL { get; set; }
        public bool CheckBox_1 { get; set; }
        public string TextBox1 { get; set; }
        public string Qty_TxtBox1 { get; set; }
        public string Qty_TxtBox2 { get; set; }
        public bool ChkBox_SOLUMEDROL2 { get; set; }
        public string Qty_TxtBox3 { get; set; }
        public bool ChkBox_PHENERGAN { get; set; }
        public bool ChkBox_25MG { get; set; }
        public bool ChkBox_PO { get; set; }
        public bool ChkBox_IVP { get; set; }
        public string Qty_TxtBox4 { get; set; }
        public bool CheckBox_2 { get; set; }
        public string TextBox2 { get; set; }
        public string Qty_TxtBox5 { get; set; }
        public bool ChkBox_PERIPHERALACCESS { get; set; }
        public bool ChkBox_CENTRALVENOUSACCESS { get; set; }
        public bool ChkBox_HEPARINFLUSH { get; set; }
        public string HEPARINFLUSH_Qty_TxtBox { get; set; }
        public bool ChkBox_HEPARINFLUSH2 { get; set; }
        public string HEPARINFLUSH_Qty_TxtBox2 { get; set; }
        public bool ChkBox_SALINEFLUSH { get; set; }
        public string SALINEFLUSH_Qty_TxtBox { get; set; }
        public bool ChkBox_Empty { get; set; }
        public string TxtBox_Empty { get; set; }
        public string TxtBox_Empty_Qty { get; set; }
        public bool ChkBox_EPINEPHRINE { get; set; }
        public string EPINEPHRINE_TxtBox { get; set; }
        public bool ChkBox1_SIG { get; set; }
        public string SIG_TxtBox1 { get; set; }
        public string SIG_Qty_TxtBox1 { get; set; }
        public bool ChkBox2_SIG { get; set; }
        public string SIG_TxtBox2 { get; set; }
        public string SIG_Qty_TxtBox2 { get; set; }
        public bool ChkBox3_SIG { get; set; }
        public string SIG_TxtBox3 { get; set; }
        public string SIG_Qty_TxtBox3 { get; set; }
        public bool ChkBox_SIG  { get; set; }
        public bool ChkBox_MAINTENANCEDOSE2 { get; set; }
        public string REF_PRN_TxtBox { get; set; }
        public bool ChkBox_Others { get; set; }
        public string Others_TxtBox { get; set; }
    }
}