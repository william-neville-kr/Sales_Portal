﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormHivAidViewModel : ReferralFormBaseViewModel
    {
        public int RefFormHivAidsId { get; set; }
        public Nullable<bool> B20_HIV_AIDS { get; set; }
        public Nullable<bool> B18_1_Chronic_Hepatitis_B { get; set; }
        public Nullable<bool> B18_2_Chronic_Hepatitis_C { get; set; }
        public Nullable<bool> ChkBox_DiagnosisOther { get; set; }
        public string TxtBox_DiagnosisOther { get; set; }
        public string DrugAllergiesTxt { get; set; }
        public string CD_4_T_cell { get; set; }
        public string HIV_RNA { get; set; }
        public string HCV_genotype { get; set; }
        public string Viral_Load { get; set; }
        public string ALT { get; set; }
        public string Liver_Biopsy_Results { get; set; }
        public string Weight { get; set; }
        public string BLOOD_RESULTS_Date_Drawn { get; set; }
        public string Hgb_Hct { get; set; }
        public string WBC { get; set; }
        public Nullable<bool> ChkBox_Edurant { get; set; }
        public string TxtBox_Edurant1 { get; set; }
        public string TxtBox_Edurant2 { get; set; }
        public string TxtBox_Edurant_Quantity { get; set; }
        public string TxtBox_Edurant_Refill { get; set; }
        public Nullable<bool> ChkBox_Emtriva { get; set; }
        public string TxtBox_Emtriva1 { get; set; }
        public string TxtBox_Emtriva2 { get; set; }
        public string TxtBox_Emtriva_Quantity { get; set; }
        public string TxtBox_Emtriva_Refill { get; set; }
        public Nullable<bool> ChkBox_Epivir { get; set; }
        public string TxtBox_Epivir1 { get; set; }
        public string TxtBox_Epivir2 { get; set; }
        public string TxtBox_Epivir_Quantity { get; set; }
        public string TxtBox_Epivir_Refill { get; set; }
        public Nullable<bool> ChkBox_Intelence { get; set; }
        public string TxtBox_Intelence1 { get; set; }
        public string TxtBox_Intelence2 { get; set; }
        public string TxtBox_Intelence_Quantity { get; set; }
        public string TxtBox_Intelence_Refill { get; set; }
        public Nullable<bool> ChkBox_Resciptor { get; set; }
        public string TxtBox_Resciptor1 { get; set; }
        public string TxtBox_Resciptor2 { get; set; }
        public string TxtBox_Resciptor_Quantity { get; set; }
        public string TxtBox_Resciptor_Refill { get; set; }
        public Nullable<bool> ChkBox_Retrovir { get; set; }
        public string TxtBox_Retrovir1 { get; set; }
        public string TxtBox_Retrovir2 { get; set; }
        public string TxtBox_Retrovir_Quantity { get; set; }
        public string TxtBox_Retrovir_Refill { get; set; }
        public Nullable<bool> ChkBox_Sustiva { get; set; }
        public string TxtBox_Sustiva1 { get; set; }
        public string TxtBox_Sustiva2 { get; set; }
        public string TxtBox_Sustiva_Quantity { get; set; }
        public string TxtBox_Sustiva_Refill { get; set; }
        public Nullable<bool> ChkBox_Videx { get; set; }
        public string TxtBox_Videx1 { get; set; }
        public string TxtBox_Videx2 { get; set; }
        public string TxtBox_Videx_Quantity { get; set; }
        public string TxtBox_Videx_Refill { get; set; }
        public Nullable<bool> ChkBox_Viramune { get; set; }
        public string TxtBox_Viramune1 { get; set; }
        public string TxtBox_Viramune2 { get; set; }
        public string TxtBox_Viramune_Quantity { get; set; }
        public string TxtBox_Viramune_Refill { get; set; }
        public Nullable<bool> ChkBox_Viread { get; set; }
        public string TxtBox_Viread1 { get; set; }
        public string TxtBox_Viread2 { get; set; }
        public string TxtBox_Viread_Quantity { get; set; }
        public string TxtBox_Viread_Refill { get; set; }
        public Nullable<bool> ChkBox_Zerit { get; set; }
        public string TxtBox_Zerit1 { get; set; }
        public string TxtBox_Zerit2 { get; set; }
        public string TxtBox_Zerit_Quantity { get; set; }
        public string TxtBox_Zerit_Refill { get; set; }
        public Nullable<bool> ChkBox_Ziagen { get; set; }
        public string TxtBox_Ziagen1 { get; set; }
        public string TxtBox_Ziagen2 { get; set; }
        public string TxtBox_Ziagen_Quantity { get; set; }
        public string TxtBox_Ziagen_Refill { get; set; }
        public Nullable<bool> ChkBox_Aptivus { get; set; }
        public string TxtBox_Aptivus1 { get; set; }
        public string TxtBox_Aptivus2 { get; set; }
        public string TxtBox_Aptivus_Quantity { get; set; }
        public string TxtBox_Aptivus_Refill { get; set; }
        public Nullable<bool> ChkBox_Invirase { get; set; }
        public string TxtBox_Invirase1 { get; set; }
        public string TxtBox_Invirase2 { get; set; }
        public string TxtBox_Invirase_Quantity { get; set; }
        public string TxtBox_Invirase_Refill { get; set; }
        public Nullable<bool> ChkBox_Kaletra { get; set; }
        public string TxtBox_Kaletra1 { get; set; }
        public string TxtBox_Kaletra2 { get; set; }
        public string TxtBox_Kaletra_Quantity { get; set; }
        public string TxtBox_Kaletra_Refill { get; set; }
        public Nullable<bool> ChkBox_Lexiva { get; set; }
        public string TxtBox_Lexiva1 { get; set; }
        public string TxtBox_Lexiva2 { get; set; }
        public string TxtBox_Lexiva_Quantity { get; set; }
        public string TxtBox_Lexiva_Refill { get; set; }
        public Nullable<bool> ChkBox_Norvir { get; set; }
        public string TxtBox_Norvir1 { get; set; }
        public string TxtBox_Norvir2 { get; set; }
        public string TxtBox_Norvir_Quantity { get; set; }
        public string TxtBox_Norvir_Refill { get; set; }
        public Nullable<bool> ChkBox_Prezista { get; set; }
        public string TxtBox_Prezista1 { get; set; }
        public string TxtBox_Prezista2 { get; set; }
        public string TxtBox_Prezista_Quantity { get; set; }
        public string TxtBox_Prezista_Refill { get; set; }
        public Nullable<bool> ChkBox_Reyataz { get; set; }
        public string TxtBox_Reyataz1 { get; set; }
        public string TxtBox_Reyataz2 { get; set; }
        public string TxtBox_Reyataz_Quantity { get; set; }
        public string TxtBox_Reyataz_Refill { get; set; }
        public Nullable<bool> ChkBox_Viracept { get; set; }
        public string TxtBox_Viracept1 { get; set; }
        public string TxtBox_Viracept2 { get; set; }
        public string TxtBox_Viracept_Quantity { get; set; }
        public string TxtBox_Viracept_Refill { get; set; }
        public Nullable<bool> ChkBox_Atripla { get; set; }
        public string TxtBox_Atripla1 { get; set; }
        public string TxtBox_Atripla2 { get; set; }
        public string TxtBox_Atripla_Quantity { get; set; }
        public string TxtBox_Atripla_Refill { get; set; }
        public Nullable<bool> ChkBox_Combivir { get; set; }
        public string TxtBox_Combivir1 { get; set; }
        public string TxtBox_Combivir2 { get; set; }
        public string TxtBox_Combivir_Quantity { get; set; }
        public string TxtBox_Combivir_Refill { get; set; }
        public Nullable<bool> ChkBox_Complera { get; set; }
        public string TxtBox_Complera1 { get; set; }
        public string TxtBox_Complera2 { get; set; }
        public string TxtBox_Complera_Quantity { get; set; }
        public string TxtBox_Complera_Refill { get; set; }
        public Nullable<bool> ChkBox_Epzicom { get; set; }
        public string TxtBox_Epzicom1 { get; set; }
        public string TxtBox_Epzicom2 { get; set; }
        public string TxtBox_Epzicom_Quantity { get; set; }
        public string TxtBox_Epzicom_Refill { get; set; }
        public Nullable<bool> ChkBox_Genvoya { get; set; }
        public string TxtBox_Genvoya1 { get; set; }
        public string TxtBox_Genvoya2 { get; set; }
        public string TxtBox_Genvoya_Quantity { get; set; }
        public string TxtBox_Genvoya_Refill { get; set; }
        public Nullable<bool> ChkBox_Odefsey { get; set; }
        public string TxtBox_Odefsey1 { get; set; }
        public string TxtBox_Odefsey2 { get; set; }
        public string TxtBox_Odefsey_Quantity { get; set; }
        public string TxtBox_Odefsey_Refill { get; set; }
        public Nullable<bool> ChkBox_Stribild { get; set; }
        public string TxtBox_Stribild1 { get; set; }
        public string TxtBox_Stribild2 { get; set; }
        public string TxtBox_Stribild_Quantity { get; set; }
        public string TxtBox_Stribild_Refill { get; set; }
        public Nullable<bool> ChkBox_Trizivir { get; set; }
        public string TxtBox_Trizivir1 { get; set; }
        public string TxtBox_Trizivir2 { get; set; }
        public string TxtBox_Trizivir_Quantity { get; set; }
        public string TxtBox_Trizivir_Refill { get; set; }
        public Nullable<bool> ChkBox_Truvada { get; set; }
        public string TxtBox_Truvada1 { get; set; }
        public string TxtBox_Truvada2 { get; set; }
        public string TxtBox_Truvada_Quantity { get; set; }
        public string TxtBox_Truvada_Refill { get; set; }
        public Nullable<bool> ChkBox_Isentress { get; set; }
        public string TxtBox_Isentress1 { get; set; }
        public string TxtBox_Isentress2 { get; set; }
        public string TxtBox_Isentress_Quantity { get; set; }
        public string TxtBox_Isentress_Refill { get; set; }
        public Nullable<bool> ChkBox_Selzentry { get; set; }
        public string TxtBox_Selzentry1 { get; set; }
        public string TxtBox_Selzentry2 { get; set; }
        public string TxtBox_Selzentry_Quantity { get; set; }
        public string TxtBox_Selzentry_Refill { get; set; }
        public Nullable<bool> ChkBox_Tivicay { get; set; }
        public string TxtBox_Tivicay1 { get; set; }
        public string TxtBox_Tivicay2 { get; set; }
        public string TxtBox_Tivicay_Quantity { get; set; }
        public string TxtBox_Tivicay_Refill { get; set; }
        public Nullable<bool> ChkBox_Egrifta { get; set; }
        public string TxtBox_Egrifta1 { get; set; }
        public string TxtBox_Egrifta2 { get; set; }
        public string TxtBox_Egrifta_Quantity { get; set; }
        public string TxtBox_Egrifta_Refill { get; set; }
        public Nullable<bool> ChkBox_Serostim { get; set; }
        public string TxtBox_Serostim1 { get; set; }
        public string TxtBox_Serostim2 { get; set; }
        public string TxtBox_Serostim_Quantity { get; set; }
        public string TxtBox_Serostim_Refill { get; set; }
    }
}