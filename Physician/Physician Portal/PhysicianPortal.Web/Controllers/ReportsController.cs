using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.Reporting.WebForms;
using PhysicianPortal.Core.Helpers;
using PhysicianPortal.Web.Models;
using System.Linq;
using PhysicianPortal.Core.Repository;
using PhysicianPortal.Web.Attributes;

namespace PhysicianPortal.Web.Controllers
{
    [CustomAuthorize]
    public class ReportsController : BaseController
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report(ReportViewModel reportViewModel)
        {
            try
            {
                ReportViewer rptViewer = new ReportViewer();

                // ProcessingMode will be Either Remote or Local  
                rptViewer.ProcessingMode = ProcessingMode.Remote;
                rptViewer.SizeToReportContent = true;
                rptViewer.ZoomMode = ZoomMode.PageWidth;
                rptViewer.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                rptViewer.Height = System.Web.UI.WebControls.Unit.Pixel(1000);
                rptViewer.AsyncRendering = true;
                rptViewer.ServerReport.ReportServerUrl = new Uri("http://irvvprpt01/ReportServer");  //http://irvvprpt01/reports/report/Physician/StatusCount
                rptViewer.ServerReport.ReportPath = @"/PhysicianPortal/" + reportViewModel.ReportName;
                ViewBag.ReportViewer = rptViewer;
                return View();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method Report() ReportsController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "ReportsController", "Report", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult GetReportsList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<ReportViewModel> ReportsList = new List<ReportViewModel>();

                //ReportsList.Add(new ReportViewModel { ReportName = "rptPhysicianPatientStatusCount", IsPowerBI = false, ReportURL = "" });
                //ReportsList.Add(new ReportViewModel { ReportName = "Test01", IsPowerBI = true, ReportURL = "https://www.google.com/search?q=%http://irvvprpt01/reports/powerbi/Test01" });
                ReportsList.Add(new ReportViewModel { ReportName = "Test01", IsPowerBI = true, ReportURL = "//irvvprpt01/reports/powerbi/Test01" });

                return Json(ReportsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method GetReportsList() ReportsController. " + e.Message, e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReportPowerBI(ReportViewModel reportViewModel)
        {
            try
            {
                return View(reportViewModel);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method Report() ReportsController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "ReportsController", "Report", ErrorTitle, ErrorMessage, null));
            }
        }


        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult DocumentsHistory()
        {
            return View();
        }


        public ActionResult GetDocumentsHistory([DataSourceRequest] DataSourceRequest request, string sreachString = null)
        {
            try
            {
                IQueryable AuditTrailList;
                string[] words = { " " };

                words = sreachString.Split(new string[] { " --" }, StringSplitOptions.RemoveEmptyEntries);

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result)
                {
                    if (words.Length >= 2)
                    {
                        DateTime startDate = DateTime.Parse(words[0]);
                        DateTime endDate = DateTime.Parse(words[1]);
                        endDate = endDate.AddDays(1).AddSeconds(-1);
                        AuditTrailList = unitOfWork.vwDocumentsUploadLogRepository.GetAsQuerable(t => t.AuditStamp >= startDate && t.AuditStamp <= endDate);
                        return Json(AuditTrailList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //AuditTrailList = unitOfWork.AuditTrailRepository.GetAsQuerable().Select(t => new { t.AuditStamp, t.LogMessage, t.Data, t.Url, t.Device });
                        AuditTrailList = unitOfWork.vwDocumentsUploadLogRepository.GetAsQuerable();//.Select(t => new { t.AuditStamp, t.LogMessage, t.Data, t.Url, t.Device });
                        return Json(AuditTrailList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetAuditTrail AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "GetAuditTrail", ErrorTitle, ErrorMessage, null));
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult GetDocumentsHistory([DataSourceRequest] DataSourceRequest request, DateTime? StartDate, DateTime? EndDate)//, string sreachString = null)
        //{
        //    try
        //    {
        //        IQueryable AuditTrailList;
        //        string[] words = { " " };

        //        if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result)
        //        {

        //            if (StartDate != null && EndDate != null)
        //            {

        //                //DateTime startDate = StartDate.Value.ToUniversalTime();
        //                //DateTime endDate = EndDate.Value.ToUniversalTime();


        //                DateTime startDate = StartDate.Value;
        //                DateTime endDate = EndDate.Value;
        //                endDate = endDate.AddDays(1).AddSeconds(-1);
        //                AuditTrailList = unitOfWork.vwDocumentsUploadLogRepository.GetAsQuerable(t => t.AuditStamp >= startDate && t.AuditStamp <= endDate);
        //                return Json(AuditTrailList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                AuditTrailList = unitOfWork.vwDocumentsUploadLogRepository.GetAsQuerable();
        //                return Json(AuditTrailList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.InsertLog("Exception thrown in Method GetDocumentsHistory AdminController. " + exception.Message, exception);
        //        return View("Error", new ErrorModel(exception, "AdminController", "GetDocumentsHistory", ErrorTitle, ErrorMessage, null));
        //    }
        //    return Json(null, JsonRequestBehavior.AllowGet);
        //}
    }
}