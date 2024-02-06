using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormTransplant1ViewModel : ReferralFormBaseViewModel
    {
        public int RefFormTransplantId { get; set; }
        public bool ChkBox_Z94_0KidneyTransplant { get; set; }
        public bool ChkBox_Z94_1HeartTransplant { get; set; }
        public bool ChkBox_Other { get; set; }
        public string Other_TxtBox { get; set; }
        public bool ChkBox_Demographics { get; set; }
        public bool ChkBox_HandP { get; set; }
        public bool ChkBox_PhysicianOrders { get; set; }
        public bool ChkBox_InsuranceInformation { get; set; }
        public bool ChkBox_Labs { get; set; }
        public string Diphenhydramine_TxtBox { get; set; }
        public string Acetaminophen_TxtBox { get; set; }
        public string Prednisone_TxtBox { get; set; }
        public string OtherMeds_TxtBox { get; set; }
        public string InfuseIVIG_TxtBox { get; set; }
        public string gramsor_TxtBox { get; set; }
        public string gm_kgIVover_TxtBox { get; set; }
        public string Frequency_TxtBox { get; set; }
        public string Duration_TxtBox { get; set; }
        public bool ChkBox_PharmacytoSelect { get; set; }
        public bool ChkBox_NoIVIGRequested { get; set; }
        public bool ChkBox_patientisgreater { get; set; }
        public bool ChkBox_patientisless { get; set; }
        public string patientisgreater_TxtBox { get; set; }
        public string patientisless_TxtBox { get; set; }
        public double Hours_TxtBox { get; set; }
        public bool ChkBox_Over { get; set; }
        public bool ChkBox_Initialrateof50mg_hr { get; set; }
        public bool ChkBox_NormalSaline { get; set; }
        public bool ChkBox_D5W { get; set; }
        public bool ChkBox_1mg_ml { get; set; }
        public bool ChkBox_2mg_ml { get; set; }
        public bool ChkBox_4mg_ml { get; set; }
        public bool ChkBox_NoRituxanRequested { get; set; }
        public bool ChkBox_BUN { get; set; }
        public bool ChkBox_Priortofirstinfusion1 { get; set; }
        public bool ChkBox_After1 { get; set; }
        public bool ChkBox_Others { get; set; }
        public bool ChkBox_Priortofirstinfusion2 { get; set; }
        public bool ChkBox_After2 { get; set; }
        public string PhysicianName_TxtBox { get; set; }
        public string License_TxtBox { get; set; }
        public string Address_TxtBox { get; set; }
        public string DEA_TxtBox { get; set; }
        public string City_TxtBox { get; set; }
        public string NPI_TxtBox { get; set; }
        public string Phone_TxtBox { get; set; }
        public string Fax_TxtBox { get; set; }
        public string OfficeContact_TxtBox { get; set; }
        public bool ChkBox_LungTransplant { get; set; }
        public string Others_TxtBox { get; set; }
        public string After_TxtBox1 { get; set; }
        public string After_TxtBox2 { get; set; }
        public string ZIP_TxtBox { get; set; }
        public string State_TxtBox { get; set; }
        public bool ChkBox_V42_0KidneyTransplant { get; set; }
        public bool ChkBox_V42_1HeartTransplant { get; set; }
        public bool ChkBox_V42_6LungTransplant { get; set; }


    }
}