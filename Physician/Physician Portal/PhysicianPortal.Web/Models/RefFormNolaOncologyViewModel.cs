using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class RefFormNolaOncologyViewModel : ReferralFormBaseViewModel
    {
        public int RefFormnolaOncologyId { get; set; }
        public string DiagnosisCode_TxtBox { get; set; }
        public string PriorFailed_TxtBox { get; set; }
        public bool ChkBox_Afinitor { get; set; }
        public string Afinitor_TxtBox1 { get; set; }
        public string Afinitor_TxtBox2 { get; set; }
        public string Afinitor_Qty_TxtBox { get; set; }
        public string Afinitor_Refills_TxtBox { get; set; }
        public bool ChkBox_Avastin { get; set; }
        public string Avastin_TxtBox1 { get; set; }
        public string Avastin_TxtBox2 { get; set; }
        public string Avastin_Qty_TxtBox { get; set; }
        public string Avastin_Refills_TxtBox { get; set; }
        public bool ChkBox_Erivedge { get; set; }
        public string Erivedge_TxtBox2 { get; set; }
        public string Erivedge_Qty_TxtBox { get; set; }
        public string Erivedge_Refills_TxtBox { get; set; }
        public bool ChkBox_Gleevec { get; set; }
        public string Gleevec_TxtBox1 { get; set; }
        public string Gleevec_TxtBox2 { get; set; }
        public string Gleevec_Qty_TxtBox { get; set; }
        public string Gleevec_Refills_TxtBox { get; set; }
        public bool ChkBox_Sprycel { get; set; }
        public string Sprycel_TxtBox1 { get; set; }
        public string Sprycel_TxtBox2 { get; set; }
        public string Sprycel_Qty_TxtBox { get; set; }
        public string Sprycel_Refills_TxtBox { get; set; }
        public bool ChkBox_Sutent { get; set; }
        public string Sutent_TxtBox1 { get; set; }
        public string Sutent_TxtBox2 { get; set; }
        public string Sutent_Qty_TxtBox { get; set; }
        public string Sutent_Refills_TxtBox { get; set; }
        public bool ChkBox_Tarceva { get; set; }
        public bool ChkBox_Tarceva25mg { get; set; }
        public bool ChkBox_Tarceva100mg { get; set; }
        public bool ChkBox_Tarceva150mg { get; set; }
        public string Tarceva_TxtBox2 { get; set; }
        public string Tarceva_Qty_TxtBox1 { get; set; }
        public string Tarceva_Refills_TxtBox1 { get; set; }
        public bool ChkBox_Targretin { get; set; }
        public string Targretin_TxtBox1 { get; set; }
        public string Targretin_TxtBox2 { get; set; }
        public string Targretin_Qty_TxtBox { get; set; }
        public string Targretin_Refills_TxtBox { get; set; }
        public bool ChkBox_Tasigna { get; set; }
        public string Tasigna_TxtBox1 { get; set; }
        public string Tasigna_TxtBox2 { get; set; }
        public string Tasigna_Qty_TxtBox { get; set; }
        public string Tasigna_Refills_TxtBox { get; set; }
        public bool ChkBox_Temodar { get; set; }
        public string Temodar_TxtBox1 { get; set; }
        public string Temodar_TxtBox2 { get; set; }
        public string Temodar_Qty_TxtBox { get; set; }
        public string Temodar_Refills_TxtBox { get; set; }
        public bool ChkBox_Xeloda { get; set; }
        public string Xeloda_TxtBox1 { get; set; }
        public string Xeloda_TxtBox2 { get; set; }
        public string Xeloda_Qty_TxtBox { get; set; }
        public string Xeloda_Refills_TxtBox { get; set; }
        public bool ChkBox_Zelboraf { get; set; }
        public string Zelboraf_TxtBox2 { get; set; }
        public string Zelboraf_Qty_TxtBox { get; set; }
        public string Zelboraf_Refills_TxtBox { get; set; }
        public bool ChkBox_Zytiga { get; set; }
        public string Zytiga_TxtBox2 { get; set; }
        public string Zytiga_Qty_TxtBox { get; set; }
        public string Zytiga_Refills_TxtBox { get; set; }
        public bool ChkBox_Aranesp { get; set; }
        public string Aranesp_TxtBox1 { get; set; }
        public string Aranesp_TxtBox2 { get; set; }
        public string Aranesp_Qty_TxtBox { get; set; }
        public string Aranesp_Refills_TxtBox { get; set; }
        public bool ChkBox_Neulasta { get; set; }
        public string Neulasta_TxtBox1 { get; set; }
        public string Neulasta_TxtBox2 { get; set; }
        public string Neulasta_Qty_TxtBox { get; set; }
        public string Neulasta_Refills_TxtBox { get; set; }
        public bool ChkBox_Neupogen { get; set; }
        public string Neupogen_TxtBox1 { get; set; }
        public string Neupogen_TxtBox2 { get; set; }
        public string Neupogen_Qty_TxtBox { get; set; }
        public string Neupogen_Refills_TxtBox { get; set; }
        public bool ChkBox_Procrit { get; set; }
        public string Procrit_TxtBox1 { get; set; }
        public string Procrit_TxtBox2 { get; set; }
        public string Procrit_Qty_TxtBox { get; set; }
        public string Procrit_Refills_TxtBox { get; set; }
        public bool ChkBox_Other1 { get; set; }
        public string Other1_TxtBox1 { get; set; }
        public string Other1_TxtBox2 { get; set; }
        public string Other1_Qty_TxtBox { get; set; }
        public string Other1_Refills_TxtBox { get; set; }
        public bool ChkBox_Other2 { get; set; }
        public string Other2_TxtBox1 { get; set; }
        public string Other2_TxtBox2 { get; set; }
        public string Other2_Qty_TxtBox { get; set; }
        public string Other2_Refills_TxtBox { get; set; }
        public bool ChkBox_Other3 { get; set; }
        public string Other3_TxtBox1 { get; set; }
        public string Other3_TxtBox2 { get; set; }
        public string Other3_Qty_TxtBox { get; set; }
        public string Other3_Refills_TxtBox { get; set; }
        public System.DateTime  CreatedOn { get; set; }
        public int  CreatedBy { get; set; }
        public System.DateTime  ModifiedOn { get; set; }
        public int  ModifiedBy { get; set; }
    }
}