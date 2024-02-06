using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTeam.Web.Models
{
    public class SalesTeamViewModel
    {
        public int MasterSalesTeamId { get; set; }
        public string MasterSalesRepresentativeFirstName { get; set; }
        public string MasterSalesRepresentativeLastName { get; set; }
        public string SalesTerritory { get; set; }

        public string Url_SId { get; set; }

        //public string MasterSalesTerritory { get; set; }
        //public string MasterSalesRepresentativeFullName { get; set; }
        //public string MasterParentSaleTeamFullName { get; set; }
        //public Nullable<int> MasterParentSalesTeamId { get; set; }
        //public string MasterSalesRegion { get; set; }
        //public int SalesTeamId { get; set; }
        //public string SalesRepresentativeFirstName { get; set; }
        //public string SalesRepresentativeLastName { get; set; }
        //public string SalesRepresentativeFullName { get; set; }
        //public string ParentSaleTeamFullName { get; set; }
        //public Nullable<int> ParentSalesTeamId { get; set; }
        //public string SalesRegion { get; set; }
    }
}