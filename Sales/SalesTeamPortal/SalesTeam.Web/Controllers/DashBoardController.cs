using SalesTeam.Core.Repository;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SalesTeam.Web.Controllers
{
    public class DashBoardController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();
        // GET: DashBoard
        public ActionResult Index()
        {
            ViewBag.GetSalesTeamList = _unitOfWork.vwSalesTeamInfosRepository.GetAsQuerable(t => t.MasterSalesTeamId == MasterSalesTeamId);
            return View();
        }

        public JsonResult GetPhysicianChartData(int SalesTeamId, DateTime? ReferralDateFrom, DateTime? ReferralDateTo)
        {
            try
            {
                if (ReferralDateFrom != null && ReferralDateTo != null)
                {
                    var PhysicianNameAndPatientcountList = _unitOfWork.spGetPhysicianNameAndPatientcountForChart_ResultRepository.context.spGetPhysicianNameAndPatientcountForChart(ReferralDateFrom, ReferralDateTo, SalesTeamId).Take(10).ToList();
                    return Json(new { message = PhysicianNameAndPatientcountList, success = true });
                }
                return Json(new { message = "", success = true });
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                return Json(new { message = "Error in GetPhysicianChartData Function in DashBoard Controller", success = false });
            }
        }

        public JsonResult DrillDownPhysicianChart(int SalesTeamId, int? PhysicianId, DateTime? ReferralDateFrom, DateTime? ReferralDateTo)
        {
            try
            {
                if (ReferralDateFrom != null && ReferralDateTo != null && PhysicianId != null)
                {
                    var DrugShortNameAndPatientcountList = _unitOfWork.spDrillDownDrugShortNameAndPatientcountForChart_ResultRepository.context.spDrillDownDrugShortNameAndPatientcountForChart(ReferralDateFrom, ReferralDateTo, SalesTeamId).Where(t => t.PhysicianId == PhysicianId).Take(10).ToList();
                    return Json(new { message = DrugShortNameAndPatientcountList, success = true });
                }
                return Json(new { message = "", success = true });
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                return Json(new { message = "Error in DrillDownPhysicianChart Function in DashBoard Controller", success = false });
            }
        }

        public JsonResult GetDrugChartData(int SalesTeamId, DateTime? ReferralDateFrom, DateTime? ReferralDateTo)
        {
            try
            {
                if (ReferralDateFrom != null && ReferralDateTo != null)
                {
                    var DrugShortNameAndPatientcountList = _unitOfWork.spGetDrugShortNameAndPatientcountForChart_ResultRepository.context.spGetDrugShortNameAndPatientcountForChart(ReferralDateFrom, ReferralDateTo, SalesTeamId).Take(10).ToList();
                    return Json(new { message = DrugShortNameAndPatientcountList, success = true });
                }
                return Json(new { message = "", success = true });
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                return Json(new { message = "Error in GetDrugChartData Function in DashBoard Controller", success = false });
            }
        }

        public JsonResult DrillDownDrugChart(int SalesTeamId, string DrugShortName, DateTime? ReferralDateFrom, DateTime? ReferralDateTo)
        {
            try
            {
                if (ReferralDateFrom != null && ReferralDateTo != null && DrugShortName != "")
                {
                    var DrugShortNameAndPatientcountList = _unitOfWork.spDrillDownDrugShortNameAndPatientcountForChart_ResultRepository.context.spDrillDownDrugShortNameAndPatientcountForChart(ReferralDateFrom, ReferralDateTo, SalesTeamId).Where(t => t.DrugShortName == DrugShortName).Take(10).ToList();
                    return Json(new { message = DrugShortNameAndPatientcountList, success = true });
                }
                return Json(new { message = "", success = true });
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                return Json(new { message = "Error in DrillDownDrugChart Function in DashBoard Controller", success = false });
            }
        }

        public JsonResult GetPhysicianList(int SalesTeamId)
        {
            try
            {
                IQueryable vwPhysicianList = _unitOfWork.vwPhysiciansRepository.GetAsQuerable(t => t.SalesTeamId == SalesTeamId).Take(500);
                return Json(new { message = vwPhysicianList, success = true });
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                return Json(new { message = "Error in GetPhysicianList Function in DashBoard Controller", success = false });
            }
        }

        public JsonResult GetActivityStatusChartData(int SalesTeamId, int PhysicianId, DateTime? ReferralDateFrom, DateTime? ReferralDateTo)
        {
            try
            {
                if (ReferralDateFrom != null && ReferralDateTo != null)
                {
                    var PatientActivityStatusList = _unitOfWork.spPatientActivityStatusForChart_ResultRepository.context.spPatientActivityStatusForChart(ReferralDateFrom, ReferralDateTo, SalesTeamId, PhysicianId).Take(10).ToList();
                    return Json(new { message = PatientActivityStatusList, success = true });
                }
                return Json(new { message = "", success = true });
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                return Json(new { message = "Error in GetActivityStatusChartData Function in DashBoard Controller", success = false });
            }
        }
    }
}