using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormNeurologyViewModel : ReferralFormBaseViewModel
    {
        public int RefFormNeurologyId { get; set; }
        public bool ChkBox_G35Multiple { get; set; }
        public bool ChkBox_Other { get; set; }
        public string Other_TxtBox { get; set; }
        public bool ChkBox_RelapsingRemitting { get; set; }
        public bool ChkBox_PrimaryProgressive { get; set; }
        public bool ChkBox_SecondaryProgressive { get; set; }
        public bool ChkBox_ProgressiveRelapsing { get; set; }
        public bool ChkBox_patientbeenpreviouslyNo { get; set; }
        public bool ChkBox_patientbeenpreviouslyYes { get; set; }
        public string Medicationfailed_TxtBox1 { get; set; }
        public bool ChkBox_patientcurrentlyontherapyNo { get; set; }
        public bool ChkBox_patientcurrentlyontherapyYes { get; set; }
        public string Medicationfailed_TxtBox2 { get; set; }
        public bool ChkBox_stoptakingcurrenttherapyYes { get; set; }
        public bool ChkBox_stoptakingcurrenttherapyNo { get; set; }
        public string longwillpatientwaitbeforestarting_TxtBox { get; set; }
        public string othermedicationspatient_TxtBox { get; set; }
        public bool ChkBox_Avonex { get; set; }
        public bool ChkBox_AvonexPFS { get; set; }
        public bool ChkBox_AvonexPEN { get; set; }
        public bool ChkBox_AvonexPwd { get; set; }
        public bool ChkBox_Inject30mcg { get; set; }
        public bool ChkBox_Otherdosing { get; set; }
        public string Otherdosing_TxtBox { get; set; }
        public string Avonex_Qty_TxtBox { get; set; }
        public string Avonex_Refills_TxtBox { get; set; }
        public bool ChkBox_Betaseron { get; set; }
        public bool ChkBox_Betaseron_InitialWeek1and2 { get; set; }
        public bool ChkBox_Betaseron_MaintenanceDose { get; set; }
        public bool ChkBox_Copaxone { get; set; }
        public bool ChkBox_Copaxone_20mg { get; set; }
        public bool ChkBox_Copaxone_40mg { get; set; }
        public bool ChkBox_Copaxone_Inject20mg { get; set; }
        public bool ChkBox_Copaxone_Inject40mg { get; set; }
        public bool ChkBox_Extavia { get; set; }
        public bool ChkBox_ExtaviaInitialWeek1and2 { get; set; }
        public bool ChkBox_ExtaviaInitialMaintenanceDose { get; set; }
        public bool ChkBox_Gilenya { get; set; }
        public bool ChkBox_Glatopa { get; set; }
        public bool ChkBox_Plegridy { get; set; }
        public bool ChkBox_PlegridyStarterPack { get; set; }
        public bool ChkBox_PlegridyPEN { get; set; }
        public bool ChkBox_PlegridyPFS { get; set; }
        public bool ChkBox_Plegridy125mcgPEN { get; set; }
        public bool ChkBox_PlegridyInject63mcg { get; set; }
        public bool ChkBox_PlegridyInject125mcg { get; set; }
        public bool ChkBox_Rebif { get; set; }
        public bool ChkBox_RebifTitrationRebidose { get; set; }
        public bool ChkBox_RebifTitrationPack { get; set; }
        public bool ChkBox_Rebif22mcg_0_5mLRebidose { get; set; }
        public bool ChkBox_Rebif22mcg_0_5mLPFS { get; set; }
        public bool ChkBox_Rebif44mcg_0_5mLRebidose { get; set; }
        public bool ChkBox_Rebif44mcg_0_5mLPFS { get; set; }
        public bool ChkBox_RebifTitrationDose { get; set; }
        public bool ChkBox_RebifMaintenanceDose { get; set; }
        public string Rebif_Inject_TxtBox { get; set; }
        public bool ChkBox_Tecfidera { get; set; }
        public bool ChkBox_TecfideraTitrationStarterPack { get; set; }
        public bool ChkBox_Tecfidera240mg { get; set; }
        public bool ChkBox_Tecfidera120mg { get; set; }
        public bool ChkBox_TecfideraTitrationDose { get; set; }
        public bool ChkBox_TecfideraTake240mg { get; set; }
        public bool ChkBox_TecfideraTake120mg { get; set; }
        public bool ChkBox_TecfideraOther { get; set; }
        public string TecfideraOther_TxtBox { get; set; }
        public bool ChkBox_Epipen { get; set; }
        public string Epipen_REFILLS_TxtBox { get; set; }
        public bool ChkBox_EpipenJr { get; set; }
        public bool ChkBox_Others { get; set; }
        public string Others_TxtBox { get; set; }
        public string DrugAllergies_TxtBox { get; set; }
        public bool ChkBox_Plegridy125mcgPFS { get; set; }
    }
}