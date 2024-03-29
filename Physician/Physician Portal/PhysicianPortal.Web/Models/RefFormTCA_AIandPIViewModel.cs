﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormTCA_AIandPIViewModel : ReferralFormBaseViewModel
    {
        public int RefFormTCA_AIandPIId { get; set; }
        public bool ChkBox_AcuteInfective { get; set; }
        public string AcuteInfective_ICD_10_TxtBox { get; set; }
        public bool ChkBox_MyastheniaGravis { get; set; }
        public string MyastheniaGravis_ICD_10_TxtBox { get; set; }
        public bool ChkBox_ChronicInflammatory { get; set; }
        public string ChronicInflammatory_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Pemphigus { get; set; }
        public string Pemphigus_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Dermatomyositis { get; set; }
        public string Dermatomyositis_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Pemphigoid { get; set; }
        public string Pemphigoid_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Inflammatory { get; set; }
        public string Inflammatory_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Polymyositis { get; set; }
        public string Polymyositis_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Multiple { get; set; }
        public string Multiple_ICD_10_TxtBox { get; set; }
        public bool ChkBox_StiffPerson { get; set; }
        public string StiffPerson_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Multifocal { get; set; }
        public string Multifocal_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Other { get; set; }
        public string Other_TxtBox { get; set; }
        public string Other_ICD_10_TxtBox { get; set; }
        public bool ChkBox_Myasthenia { get; set; }
        public string Myasthenia_ICD_10_TxtBox { get; set; }
        public bool ChkBox_SCIG { get; set; }
        public bool ChkBox_IV1G { get; set; }
        public bool ChkBox_Pharmacist { get; set; }
        public bool ChkBox_Formulation { get; set; }
        public string Formulation_TxtBox { get; set; }
        public bool ChkBox_LoadingDose { get; set; }
        public string LoadingDose_TxtBox { get; set; }
        public bool ChkBox_MaintenanceDose { get; set; }
        public string MaintenanceDose_over_TxtBox { get; set; }
        public string MaintenanceDose_day_TxtBox { get; set; }
        public string MaintenanceDose_week_TxtBox { get; set; }
        public string MaintenanceDose_cycle_TxtBox { get; set; }
        public bool ChkBox_OtherRegimen { get; set; }
        public string OtherRegimen_TxtBox { get; set; }
        public bool ChkBox_PharmacisttodetermineoStart { get; set; }
        public bool ChkBox_Startat { get; set; }
        public double Startat_TxtBox1 { get; set; }
        public double Startat_TxtBox2 { get; set; }
        public double Startat_TxtBox3 { get; set; }
        public double Startat_TxtBox4 { get; set; }
        public bool ChkBox_Peripheral { get; set; }
        public bool ChkBox_PICC { get; set; }
        public bool ChkBox_Port { get; set; }
        public bool ChkBox_Other2 { get; set; }
        public string Other2_TxtBox { get; set; }
        public bool ChkBox_Decline1 { get; set; }
        public bool ChkBox_Decline2 { get; set; }
        public bool ChkBox_Other3 { get; set; }
        public string Other3_TxtBox { get; set; }
        public string LabstobeDrawn_TxtBox { get; set; }
        public string FrequencyofLabs_TxtBox { get; set; }
        public string PhysicianName_TxtBox { get; set; }
        public string License_TxtBox { get; set; }
        public string Address_TxtBox { get; set; }
        public string DEA_TxtBox { get; set; }
        public string City_TxtBox { get; set; }
        public string ZIP_TxtBox { get; set; }
        public string State_TxtBox { get; set; }
        public string NPI_TxtBox { get; set; }
        public string Phone_TxtBox { get; set; }
        public string Fax_TxtBox { get; set; }
        public string OfficeContact_TxtBox { get; set; }
        public bool ChkBox_PI_CommonVariable { get; set; }
        public string PI_CommonVariable_ICD_10_TxtBox { get; set; }
        public bool ChkBox_PI_oImmunodeficiency { get; set; }
        public string PI_oImmunodeficiency_ICD_10_TxtBox { get; set; }
        public bool ChkBox_PI_CombinedImmunity { get; set; }
        public string PI_CombinedImmunity_ICD_10_TxtBox { get; set; }
        public bool ChkBox_PI_SelectiveIgM { get; set; }
        public string PI_SelectiveIgM_ICD_10_TxtBox { get; set; }
        public bool ChkBox_PI_Congenital { get; set; }
        public string PI_Congenital_ICD_10_TxtBox { get; set; }
        public bool ChkBox_PI_SelectiveIg { get; set; }
        public string PI_CSelectiveIg_ICD_10_TxtBox { get; set; }
        public bool ChkBox_PI_Hypogammaglobulinemia { get; set; }
        public string PI_Hypogammaglobulinemia_ICD_10_TxtBox { get; set; }
        public bool ChkBox_PI_Other { get; set; }
        public string PI_Other_ICD_10_TxtBox { get; set; }
        public string LoadingDose_TxtBox2 { get; set; }
    }
}