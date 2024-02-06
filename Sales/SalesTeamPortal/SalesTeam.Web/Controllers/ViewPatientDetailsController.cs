using System;
using System.Linq;
using System.Web.Mvc;
using SalesTeam.Core.Repository;
using SalesTeam.Core.Data;
using SalesTeam.Core.Helpers;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using SalesTeam.Web.Models;
using System.Linq.Expressions;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class ViewPatientDetailsController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();
        // GET: ViewPatientDetails
        public ActionResult Index(string id)
        {
            string[] ids = id.Decrypt().Split('&');

            int? sTeamId = ids[0].ToInt0();
            int pId = ids[1].ToInt0();

            ViewBag.SalesTeamId = sTeamId;
            //ViewBag.PatientId = pId;
            vwPatient _vwPatient = _unitOfWork.vwPatientRepository.GetAsQuerable(t => t.PatientId == pId).FirstOrDefault();
            ViewBag.insurances = _unitOfWork.vwPatientInsurancesRepository.GetAsQuerable(t => t.PatientId == pId);
            ViewBag.physicianModel = _unitOfWork.vwPhysiciansRepository.GetAsQuerable(t => t.PhysicianId == _vwPatient.PhysicianId).FirstOrDefault();
            //ViewBag.physicianModel = _unitOfWork.vwSalesTeamPhysiciansRepository.GetAsQuerable(t => t.PhysicianId == _vwPatient.PhysicianId).FirstOrDefault();
            ViewBag.GetReferralFormCategories = GetReferralFormCategories();
            ViewBag.GetPhysicianAddressList = GetPhysicianAddressList(_vwPatient.PhysicianId);

            ViewBag.SalesRepresentativeFullName = "";
            AssociatesSalesTeam associatesSalesTeam = _unitOfWork.AssociatesSalesTeamRepository.GetSingle(t => t.PhysicianMasterId == _vwPatient.PhysicianId && t.PatientId == _vwPatient.PatientId && t.SalesTeamId == sTeamId);
            if (associatesSalesTeam != null)
            {
                ViewBag.SalesRepresentativeFullName = associatesSalesTeam.SalesRepresentativeFullName;
            }
            //var associatesSalesTeamList = _unitOfWork.AssociatesSalesTeamRepository.GetAsQuerable(t => t.PhysicianMasterId == _vwPatient.PhysicianId && t.PatientId == _vwPatient.PatientId).ToList();
            //if (associatesSalesTeamList != null)
            //{
            //    string str = "";
            //    foreach (var associatesSalesTeam in associatesSalesTeamList)
            //    {
            //        if (str != "")
            //        {
            //            str += ", ";
            //        }
            //        str += associatesSalesTeam.SalesRepresentativeFullName;
            //    }
            //    ViewBag.SalesRepresentativeFullName = str;
            //    LogInfo("ViewBag.SalesRepresentativeFullName: " + str , new Exception("Log SalesRepresentativeFullName"));
            //}

            DimProcessTime processTime = _unitOfWork.DimProcessTimesRepository.GetSingle();
            ViewBag.LatestNotesTime = processTime?.LatestNotesTime;
            ViewBag.LatestDocumentTime = processTime?.LatestDocumentTime;
            ViewBag.LatestDeliveryTime = processTime?.LatestDeliveryTime;

            var physicianConsent = _unitOfWork.PhysicianConsentRepository.GetAsQuerable(x => x.PhysicianId == _vwPatient.PhysicianId && x.SalesTeamId == MasterSalesTeamId).FirstOrDefault();
            if (physicianConsent != null)
            {
                //Consent Status
                // 1 = Granted, 2 = Denied, 3 = Pending
                ViewBag.PhysicianConsent = physicianConsent.ConsentGranted == null ? 3 : (physicianConsent.ConsentGranted.Value == true ? 1 : 2);
            }
            else
            {
                ViewBag.PhysicianConsent = 2;
            }

            return View(_vwPatient);
        }
        // GET: ViewPatientDetails
        //public ActionResult Index(int? sTeamId, int pId)
        //{
        //    ViewBag.SalesTeamId = sTeamId;
        //    ViewBag.PatientId = pId;
        //    vwPatient _vwPatient = _unitOfWork.vwPatientRepository.GetAsQuerable(t => t.PatientId == pId).FirstOrDefault();
        //    ViewBag.insurances = _unitOfWork.vwPatientInsurancesRepository.GetAsQuerable(t => t.PatientId == pId);
        //    ViewBag.physicianModel = _unitOfWork.vwPhysiciansRepository.GetAsQuerable(t => t.PhysicianId == _vwPatient.PhysicianId).FirstOrDefault();
        //    //ViewBag.physicianModel = _unitOfWork.vwSalesTeamPhysiciansRepository.GetAsQuerable(t => t.PhysicianId == _vwPatient.PhysicianId).FirstOrDefault();
        //    ViewBag.GetReferralFormCategories = GetReferralFormCategories();
        //    ViewBag.GetPhysicianAddressList = GetPhysicianAddressList(_vwPatient.PhysicianId);

        //    ViewBag.SalesRepresentativeFullName = "";
        //    AssociatesSalesTeam associatesSalesTeam = _unitOfWork.AssociatesSalesTeamRepository.GetSingle(t => t.PhysicianMasterId == _vwPatient.PhysicianId && t.PatientId == _vwPatient.PatientId && t.SalesTeamId == sTeamId);
        //    if (associatesSalesTeam != null)
        //    {
        //        ViewBag.SalesRepresentativeFullName = associatesSalesTeam.SalesRepresentativeFullName;
        //    }
        //    //var associatesSalesTeamList = _unitOfWork.AssociatesSalesTeamRepository.GetAsQuerable(t => t.PhysicianMasterId == _vwPatient.PhysicianId && t.PatientId == _vwPatient.PatientId).ToList();
        //    //if (associatesSalesTeamList != null)
        //    //{
        //    //    string str = "";
        //    //    foreach (var associatesSalesTeam in associatesSalesTeamList)
        //    //    {
        //    //        if (str != "")
        //    //        {
        //    //            str += ", ";
        //    //        }
        //    //        str += associatesSalesTeam.SalesRepresentativeFullName;
        //    //    }
        //    //    ViewBag.SalesRepresentativeFullName = str;
        //    //    LogInfo("ViewBag.SalesRepresentativeFullName: " + str , new Exception("Log SalesRepresentativeFullName"));
        //    //}

        //    DimProcessTime processTime = _unitOfWork.DimProcessTimesRepository.GetSingle();
        //    ViewBag.LatestNotesTime = processTime?.LatestNotesTime;
        //    ViewBag.LatestDocumentTime = processTime?.LatestDocumentTime;
        //    ViewBag.LatestDeliveryTime = processTime?.LatestDeliveryTime;

        //    return View(_vwPatient);
        //}

        public ActionResult ShowPatientPrescriptionInfo(int PrescriptionID)
        {
            try
            {
                vwPrescription prescription = _unitOfWork.vwPrescriptionRepository.GetAsQuerable(t => t.PrescriptionId == PrescriptionID).FirstOrDefault();
                return PartialView("_PrescriptionPopup", prescription);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
                return PartialView("Error", ex.Message);
            }
        }

        private IQueryable GetReferralFormCategories()
        {
            return _unitOfWork.ReferralFormCategoryRepository.GetAsQuerable().Select(t => new SelectListItem { Text = t.CategoryName, Value = t.ReferralFormCategoryId.ToString() });
        }

        public JsonResult GetReferralForms(string ReferralFormCategoryId)
        {
            try
            {
                int referralFormCategoryId = ReferralFormCategoryId.ToInt0();

                IQueryable referralForms = _unitOfWork.ReferralFormRepository.GetAsQuerable(t => t.ReferralFormCategoryId == referralFormCategoryId).Select(t => new SelectListItem { Text = t.ReferralFormName, Value = t.ReferralFormId.ToString() });
                return Json(new { message = referralForms, success = true });
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(new { message = "Error", success = false });
        }

        private IQueryable GetPhysicianAddressList(int physicianId)
        {
            return _unitOfWork.vwPhysicianAddressRepository.GetAsQuerable(t => t.PhysicianId == physicianId);
        }

        public ActionResult HippaConsent_Read([DataSourceRequest] DataSourceRequest request, string PatientCode)
        {
            try
            {
                var hIPAAConsent = _unitOfWork.spHIPAAConsent_ResultRepository.context.spHIPAAConsent(PatientCode).ToList();

                return Json(hIPAAConsent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPhysicianConsentRecords([DataSourceRequest] DataSourceRequest request, vwPhysician physicianModel)
        {
            try
            {
                List<PhysicianConsentModel> salesTeamConsentRecords = _unitOfWork.PhysicianConsentRepository.GetAsQuerable(x =>
                                                        x.PhysicianId == physicianModel.PhysicianId &&
                                                        x.SalesTeamId == MasterSalesTeamId
                                                        ).Select(x =>
                                                        new PhysicianConsentModel
                                                        {
                                                            PhysicianId = x.PhysicianId.Value,
                                                            SalesTeamId = x.SalesTeamId.Value,
                                                            ConsentGrantedStatus = x.ConsentGranted == null ? 3 : (x.ConsentGranted.Value == true ? 1 : 2),
                                                            ConsentRequestedOn = x.ModifiedOn == null ? x.CreatedOn.Value : x.ModifiedOn.Value,
                                                            ConsentChangedOn = x.ConsentChangedOn,
                                                            PhysicianEmail = x.PhysicianEmailAddress,
                                                            //SalesTeamName = _unitOfWork.vwSalesTeamsRepository.GetAsQuerable(y => y.SalesTeamId == (x.SalesTeamId ?? 0)).FirstOrDefault() != null ? _unitOfWork.vwSalesTeamsRepository.GetAsQuerable(y => y.SalesTeamId == x.SalesTeamId.Value).FirstOrDefault().SalesRepresentativeFullName : ""
                                                        }).ToList();

                foreach (PhysicianConsentModel salesteam in salesTeamConsentRecords)
                {
                    var record = _unitOfWork.vwSalesTeamsRepository.GetAsQuerable(x => x.SalesTeamId == salesteam.SalesTeamId).FirstOrDefault();
                    salesteam.SalesTeamName = record.SalesRepresentativeFullName;
                }

                return Json(salesTeamConsentRecords.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}