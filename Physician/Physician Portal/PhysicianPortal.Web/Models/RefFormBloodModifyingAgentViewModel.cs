using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormBloodModifyingAgentViewModel : ReferralFormBaseViewModel
    {
        public int RefFormBloodmodifyingagentsId { get; set; }
        public string Primary_TxtBox1 { get; set; }
        public string Primary_TxtBox2 { get; set; }
        public string Primary_ICD_10_TxtBox1 { get; set; }
        public string Primary_ICD_10_TxtBox2 { get; set; }
        public string Secondary_TxtBox1 { get; set; }
        public string Secondary_TxtBox2 { get; set; }
        public string Secondary_ICD_10_TxtBox1 { get; set; }
        public string Secondary_ICD_10_TxtBox2 { get; set; }
        public bool ChkBox_Aranesp { get; set; }
        public bool ChkBox_Aranesp_25mcg { get; set; }
        public bool ChkBox_Aranesp_40mcg { get; set; }
        public bool ChkBox_Aranesp_60mcg { get; set; }
        public bool ChkBox_Aranesp_100mcg { get; set; }
        public bool ChkBox_Aranesp_200mcg { get; set; }
        public bool ChkBox_Aranesp_300mcg { get; set; }
        public bool ChkBox_Aranesp_150mcg { get; set; }
        public string Aranesp_Qty_TxtBox { get; set; }
        public string Aranesp_Refills_TxtBox { get; set; }
        public bool ChkBox_PFS1_10mcg { get; set; }
        public bool ChkBox_PFS1_25mcg { get; set; }
        public bool ChkBox_PFS1_40mcg { get; set; }
        public bool ChkBox_PFS1_60mcg { get; set; }
        public bool ChkBox_PFS1_100mcg { get; set; }
        public bool ChkBox_PFS1_150mcg { get; set; }
        public bool ChkBox_PFS1_200mcg { get; set; }
        public bool ChkBox_PFS1_300mcg { get; set; }
        public bool ChkBox_PFS1_500mcg { get; set; }
        public string Qty_TxtBox { get; set; }
        public string Refills_TxtBox { get; set; }
        public bool ChkBox_Arixtra { get; set; }
        public bool ChkBox_Arixtra_2_5mg { get; set; }
        public bool ChkBox_Arixtra_5mg { get; set; }
        public bool ChkBox_Arixtra_7_5mg { get; set; }
        public bool ChkBox_Arixtra_10mg { get; set; }
        public string Arixtra_Qty_TxtBox { get; set; }
        public string Arixtra_Refills_TxtBox { get; set; }
        public bool ChkBox_Epogen { get; set; }
        public bool ChkBox_Epogen_SDV_2000 { get; set; }
        public bool ChkBox_Epogen_SDV_3000 { get; set; }
        public bool ChkBox_Epogen_SDV_4000 { get; set; }
        public bool ChkBox_Epogen_SDV_10000 { get; set; }
        public bool ChkBox_Epogen_MDV_20000 { get; set; }
        public bool ChkBox_Epogen_20000 { get; set; }
        public string Epogen_Qty_TxtBox { get; set; }
        public string Epogen_Refills_TxtBox { get; set; }
        public bool ChkBox_Procrit { get; set; }
        public bool ChkBox_Procrit_SDV_2000 { get; set; }
        public bool ChkBox_Procrit_SDV_3000 { get; set; }
        public bool ChkBox_Procrit_SDV_4000 { get; set; }
        public bool ChkBox_Procrit_SDV_10000 { get; set; }
        public bool ChkBox_Procrit_SDV_40000 { get; set; }
        public bool ChkBox_Procrit_MDV_20000_1 { get; set; }
        public bool ChkBox_Procrit_MDV_20000_2 { get; set; }
        public string Procit_Qty_TxtBox { get; set; }
        public string Procit_Refills_TxtBox { get; set; }
        public bool ChkBox_Granix { get; set; }
        public bool ChkBox_Granix_PFS300mcg { get; set; }
        public bool ChkBox_Granix_PFS480mcg { get; set; }
        public string Granix_Qty_TxtBox { get; set; }
        public string Granix_Refills_TxtBox { get; set; }
        public bool ChkBox_Leukine { get; set; }
        public bool ChkBox_Leukine_250mcg { get; set; }
        public bool ChkBox_Leukine_500mcg { get; set; }
        public string Leukine_Qty_TxtBox { get; set; }
        public string Leukine_Refills_TxtBox { get; set; }
        public bool ChkBox_Neulasta { get; set; }
        public bool ChkBox_Neulasta_6mg { get; set; }
        public bool ChkBox_Neulasta_onprokit { get; set; }
        public string Neulasta_Qty_TxtBox { get; set; }
        public string Neulasta_Refills_TxtBox { get; set; }
        public bool ChkBox_Neupogen { get; set; }
        public bool ChkBox_Neupogen_vial_300mcg { get; set; }
        public bool ChkBox_Neupogen__vial_480mcg { get; set; }
        public bool ChkBox_Neupogen_PFS_300mcg { get; set; }
        public bool ChkBox_Neupogen__PFS_480mcg { get; set; }
        public string Neupogen_Qty_TxtBox { get; set; }
        public string Neupogen_Refills_TxtBox { get; set; }
        public bool ChkBox_Zarxio { get; set; }
        public bool ChkBox_Zarxio_PFS_300mcg { get; set; }
        public bool ChkBox_Zarxio_PFS_480mcg { get; set; }
        public string Zarxio_Qty_TxtBox { get; set; }
        public string Zarxio_Refills_TxtBox { get; set; }
        public bool ChkBox_Other { get; set; }
        public string Other_TxtBox1 { get; set; }
        public string Other_TxtBox2 { get; set; }
        public string Other_Qty_TxtBox { get; set; }
        public string Other_Refills_TxtBox { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }
}