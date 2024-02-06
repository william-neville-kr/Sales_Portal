using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormOncology_1ViewModel : ReferralFormBaseViewModel
    {
        public int RefFormOncologyId { get; set; }
        public string Diagnosis_TxtBox { get; set; }
        public string ICD_10_TxtBox { get; set; }
        public string DrugAllergies_TxtBox { get; set; }
        public bool ChkBox_Thalomid { get; set; }
        public bool ChkBox_50mg { get; set; }
        public bool ChkBox_100mg { get; set; }
        public bool ChkBox_150mg { get; set; }
        public bool ChkBox_200mg { get; set; }
        public bool ChkBox_Take1capPOdaily1 { get; set; }
        public bool ChkBox_Empty1 { get; set; }
        public string Empty1_TxtBox { get; set; }
        public string Qty_TxtBox1 { get; set; }
        public bool ChkBox_AdultFemale { get; set; }
        public bool ChkBox_AdultFemaleNOT { get; set; }
        public bool ChkBox_AdultMale { get; set; }
        public bool ChkBox_Pomalyst { get; set; }
        public bool ChkBox_1mg { get; set; }
        public bool ChkBox_2mg { get; set; }
        public bool ChkBox_3mg { get; set; }
        public bool ChkBox_4mg { get; set; }
        public bool ChkBox_Take1capPOdailydays1_21of28daycycle1 { get; set; }
        public bool ChkBox_Empty2 { get; set; }
        public string Empty2_TxtBox { get; set; }
        public string Qty_TxtBox2 { get; set; }
        public bool ChkBox_FemaleChild { get; set; }
        public bool ChkBox_FemaleChildNOT { get; set; }
        public bool ChkBox_MaleChild { get; set; }
        public bool ChkBox_Revlimid { get; set; }
        public bool ChkBox_2_5mg { get; set; }
        public bool ChkBox_5mg { get; set; }
        public bool ChkBox_10mg { get; set; }
        public bool ChkBox_15mg { get; set; }
        public bool ChkBox_20mg { get; set; }
        public bool ChkBox_25mg { get; set; }
        public bool ChkBox_Take1capPOdaily2 { get; set; }
        public bool ChkBox_Take1capPOdailydays1_21of28daycycle2 { get; set; }
        public bool ChkBox_Empty3 { get; set; }
        public string Empty3_TxtBox { get; set; }
        public string Qty_TxtBox3 { get; set; }
        public string Authorization_TxtBox { get; set; }
        public System.DateTime Date1_TxtBox { get; set; }
        public string Confirmation_TxtBox { get; set; }
        public System.DateTime Date2_TxtBox { get; set; }
        public bool ChkBox_Ninlaro { get; set; }
        public bool ChkBox_4mg2 { get; set; }
        public bool ChkBox_3mg2 { get; set; }
        public bool ChkBox_2_3mg { get; set; }
        public string Refills_TxtBox1 { get; set; }
        public bool ChkBox_Dexamethasone { get; set; }
        public string Dexamethasone_Qty_TxtBox { get; set; }
        public string Dexamethasone_Refills_TxtBox { get; set; }
        public bool ChkBox_Empty4 { get; set; }
        public string Empty4_Qty_TxtBox { get; set; }
        public string Empty4_Refills_TxtBox { get; set; }
        public string Empty4_TxtBox { get; set; }
        public bool ChkBox_Others { get; set; }
        public string Others_TxtBox { get; set; }
    }
}