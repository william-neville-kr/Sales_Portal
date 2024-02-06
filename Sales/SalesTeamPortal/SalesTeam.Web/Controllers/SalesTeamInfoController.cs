using System;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SalesTeam.Core.Repository;
using System.Linq;
using SalesTeam.Web.Models;
using System.Collections.Generic;
using System.Web;
using SalesTeam.Core.Helpers;
using SalesTeam.Core.Data;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class SalesTeamInfoController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: SalesTeamInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult vwSalesTeamInfoes_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var salesTeamViewModelList = _unitOfWork.vwSalesTeamInfosRepository
                                                                 .GetAsQuerable(t => t.MasterSalesTeamId == MasterSalesTeamId)
                                                                 .Select(t => new SalesTeamViewModel
                                                                 {
                                                                     MasterSalesTeamId = t.MasterSalesTeamId,
                                                                     MasterSalesRepresentativeFirstName = t.MasterSalesRepresentativeFirstName,
                                                                     MasterSalesRepresentativeLastName = t.MasterSalesRepresentativeLastName,
                                                                     SalesTerritory = t.SalesTerritory,
                                                                     Url_SId = t.SalesTeamId.ToString()
                                                                 });

                var filteredList = salesTeamViewModelList.ToDataSourceResult(request);

                foreach (SalesTeamViewModel item in filteredList.Data)
                {
                    item.Url_SId = HttpUtility.UrlEncode(item.Url_SId.Encrypt());
                }

                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}