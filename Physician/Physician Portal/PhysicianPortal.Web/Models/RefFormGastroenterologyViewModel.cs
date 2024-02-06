using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormGastroenterologyViewModel : ReferralFormBaseViewModel
    {
        public int RefFormGastroenterologyId { get; set; }
        public bool ChkBox_CLINICAL_K50_90 { get; set; }
        public bool ChkBox_CLINICAL_K51_90 { get; set; }
        public bool ChkBox_CLINICAL_Other1 { get; set; }
        public string CLINICAL_Other1_TxtBox { get; set; }
        public string CLINICAL_Other_Drug_Allergies_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_History { get; set; }
        public bool ChkBox_CLINICAL_NSAIDS { get; set; }
        public string CLINICAL_NSAIDS_Duration_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_Sulfasalazine { get; set; }
        public string CLINICAL_Sulfasalazine_Duration_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_Corticosteroid { get; set; }
        public string CLINICAL_Corticosteroid_Duration_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_MTX { get; set; }
        public string CLINICAL_MTX_Duration_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_5_ASA { get; set; }
        public string CLINICAL_5_ASA_Duration_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_6_MP { get; set; }
        public string CLINICAL_6_MP_Duration_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_Biologics { get; set; }
        public string CLINICAL_Biologics_Duration_TxtBox { get; set; }
        public bool ChkBox_CLINICAL_Azathioprine { get; set; }
        public string CLINICAL_Azathioprine_Duration { get; set; }
        public bool ChkBox_CLINICAL_Other2 { get; set; }
        public string CLINICAL_Other2_Duration_TxtBox { get; set; }
        public bool Radio_CLINICAL_patientcurrentlyonanytherapy { get; set; }
        public string CLINICAL_ListMeds_TxtBox { get; set; }
        public bool Radio_CLINICAL_patientstoptakingMedsbeforestarting { get; set; }
        public string CLINICAL_Howlongwillthepatientwaitbeforestarting_TxtBox { get; set; }
        public string CLINICAL_Othermedspatientison_TxtBox { get; set; }
        public bool Radio_CLINICAL_HaspatientreceivedPPD { get; set; }
        public string CLINICAL_Results_TxtBox { get; set; }
        public bool ChkBox_PRESCRIPTION_Cimzia { get; set; }
        public bool ChkBox_PRESCRIPTION_200_2PrefilledSyringe { get; set; }
        public bool ChkBox_PRESCRIPTION_200_2LYOPowder { get; set; }
        public bool ChkBox_PRESCRIPTION_StarterKit_Inject400mgsubcutaneously { get; set; }
        public bool ChkBox_PRESCRIPTION_Inject400mgsubcutaneouslyonce { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox1 { get; set; }
        public bool ChkBox_PRESCRIPTION_Creon { get; set; }
        public bool ChkBox_PRESCRIPTION_3000 { get; set; }
        public bool ChkBox_PRESCRIPTION_6000 { get; set; }
        public bool ChkBox_PRESCRIPTION_12000 { get; set; }
        public bool ChkBox_PRESCRIPTION_24000 { get; set; }
        public bool ChkBox_PRESCRIPTION_36000 { get; set; }
        public string PRESCRIPTION_capsulesthreetimes_TxtBox { get; set; }
        public string PRESCRIPTION_capsuleswith_TxtBox { get; set; }
        public string PRESCRIPTION_snacksdaily_TxtBox { get; set; }
        public string PRESCRIPTION_capsulesaday_TxtBox { get; set; }
        public string PRESCRIPTION_Quantity_TxtBox1 { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox2 { get; set; }
        public bool ChkBox_PRESCRIPTION_Dificid { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox3 { get; set; }
        public bool ChkBox_PRESCRIPTION_Entyvio { get; set; }
        public bool ChkBox_PRESCRIPTION_LoadingDose { get; set; }
        public bool ChkBox_PRESCRIPTION_Maintenance { get; set; }
        public string PRESCRIPTION_Quantity_TxtBox2 { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox4 { get; set; }
        public bool ChkBox_PRESCRIPTION_Humira { get; set; }
        public bool ChkBox_PRESCRIPTION_Crohns_UCStarter { get; set; }
        public bool ChkBox_PRESCRIPTION_40mgPrefilledPen { get; set; }
        public bool ChkBox_PRESCRIPTION_40mgPrefilledSyringe { get; set; }
        public bool ChkBox_PRESCRIPTION_Four40mgSubQday1OR { get; set; }
        public bool ChkBox_PRESCRIPTION_Two40mgSubQdays { get; set; }
        public bool ChkBox_PRESCRIPTION_Week4plusnject40mg { get; set; }
        public string PRESCRIPTION__Week4plusnject40mg_TextBox { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox5 { get; set; }
        public bool ChkBox_PRESCRIPTION_Remicade { get; set; }
        public string PRESCRIPTION_Wt_TxtBox { get; set; }
        public bool ChkBox_PRESCRIPTION_Infuse1 { get; set; }
        public bool ChkBox_PRESCRIPTION_Infuse2 { get; set; }
        public string PRESCRIPTION_mgIVonweek0week2week6then { get; set; }
        public string PRESCRIPTION_mgIVevery { get; set; }
        public string PRESCRIPTION_weeksfor { get; set; }
        public string PRESCRIPTION_infusions { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox6 { get; set; }
        public bool ChkBox_PRESCRIPTION_Simponi { get; set; }
        public bool ChkBox_PRESCRIPTION_100mgSmartJect { get; set; }
        public bool ChkBox_PRESCRIPTION_100mgPrefilledSyringe { get; set; }
        public bool ChkBox_PRESCRIPTION_Inject200mgsubcutaneouslyatweek0 { get; set; }
        public bool ChkBox_PRESCRIPTION_Inject100mgsubcutaneouslyonceevery4weeks { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox7 { get; set; }
        public bool ChkBox_PRESCRIPTION_Epipen { get; set; }
        public bool ChkBox_PRESCRIPTION_0_3mg { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox8 { get; set; }
        public bool ChkBox_PRESCRIPTION_Xifaxan { get; set; }
        public bool ChkBox_PRESCRIPTION_1tabletbymouthtwiceaday { get; set; }
        public bool ChkBox_PRESCRIPTION_1tabletbymouththreetimesaday { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox9 { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox10 { get; set; }
        public bool ChkBox_PRESCRIPTION_1Other { get; set; }
        public string PRESCRIPTION_TxtBox1 { get; set; }
        public string PRESCRIPTION_TxtBox2 { get; set; }
        public string PRESCRIPTION_Quantity_TxtBox3 { get; set; }
        public string PRESCRIPTION_REFILLS_None_TxtBox11 { get; set; }
        public bool ChkBox_Stelara { get; set; }
        public string Stelara_Refills_TxtBox { get; set; }
    }
}