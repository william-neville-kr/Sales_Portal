using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SalesTeam.Core.Repository;
using SalesTeam.Core.Data;
using System.Collections.Generic;
using SalesTeam.Web.Models;
using System.Web;
using SalesTeam.Core.Helpers;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class SalesTeamQueueController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: SalesTeamQueue
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getSalesTeamQueueRecords([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                List<PatientViewModel> patientViewModelList = _unitOfWork.vwSalesTeamQueuesRepository.GetAsQuerable(t => t.CreatedSalesTeamId == MasterSalesTeamId)
                                                                         .Select(t => new PatientViewModel
                                                                         {
                                                                             PatientId = t.PatientId ?? 0,
                                                                             PhysicianId = t.PhysicianId ?? 0,
                                                                             SalesTeamId = t.SalesTeamId,
                                                                             SalesTeamQueueId = t.SalesTeamQueueId,
                                                                             ReferralDateId = t.ReferralDateId,
                                                                             PatientCode = t.PatientCode,
                                                                             FirstName = t.FirstName,
                                                                             LastName = t.LastName,
                                                                             PhysicianName = t.PhysicianName,
                                                                             //PhyCity = t.phyCity,
                                                                             StateProvince = t.StateProvince,
                                                                             DrugShortName = t.DrugShortName,
                                                                             ActivityStatus = t.ActivityStatus,
                                                                             OriginatingPharmacy = t.OriginatingPharmacy,
                                                                             //NeedMoreInfo = t.NeedMoreInfo,
                                                                             RXStatus = t.RXStatus,
                                                                             DiseaseState = t.DiseaseState,
                                                                             Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                                                                         })
                                                                         .ToList();

                for (int i = 0; i < patientViewModelList.Count; i++)
                {
                    patientViewModelList[i].Url_SId_PId = HttpUtility.UrlEncode(patientViewModelList[i].Url_SId_PId.Encrypt());
                }

                return Json(patientViewModelList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteQueues(SalesTeamQueue model)
        {
            try
            {
                var salesTeamQueue =
                    _unitOfWork.SalesTeamQueuesRepository.GetAsQuerable(t => t.SalesTeamQueueId == model.SalesTeamQueueId)
                        .FirstOrDefault();
                if (salesTeamQueue != null)
                {
                    _unitOfWork.SalesTeamQueuesRepository.Delete(salesTeamQueue);
                    _unitOfWork.Save();
                    return Json(new { message = "Sales Team Queues Deleted Successfully.", success = true });
                }
                return Json(new { message = "Record not found.", success = true });
            }
            catch (Exception e)
            {
                LogInfo(e.Message, e);
                //Logger.LogException(e);
                return Json(new { message = e.Message, success = true });
            }
        }
    }
}