using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Helpers;
using PhysicianPortal.Core.Repository;
using PhysicianPortal.PokitDokIntegration.Models;
using PhysicianPortal.Web.Attributes;
using PhysicianPortal.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PhysicianPortal.Web.Controllers
{
    [CustomAuthorize]
    public class InsurancesController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: Insurances
        public ActionResult Index()
        {
            try
            {
                //ViewBag.IsEligibilityValueExist = !string.IsNullOrEmpty(_unitOfWork.InsuranceEligibilityInfoRepository.GetSingle(t => t.PatientId == patId)?.EligibilityValue);
                return View();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method Index() InsurancesController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "InsurancesController", "Index", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult InsuranceSearch(int PatientId)
        {
            try
            {
                vwPatient patient = _unitOfWork.PatientsRepository.GetAsQuerable(t => (t.PatientId == PatientId)).FirstOrDefault();
                ViewBag.PatientModel = patient;

                if (patient != null)
                {
                    patient.PatientIdEncrypted = SecurityHelper.Encrypt(patient.PatientId.ToString());
                    patient.PhysicianIdEncrypted = SecurityHelper.Encrypt(patient.PhysicianId.ToString());
                    return PartialView("_PatientInsurance", patient);
                }
                else
                {
                    return Redirect(null);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method InsuranceSearch InsurancesController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "InsurancesController", "InsuranceSearch", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        public ActionResult PatientSearch(int PatientId)
        {
            try
            {
                vwPatient patient = _unitOfWork.PatientsRepository.GetAsQuerable(t => (t.PatientId == PatientId)).FirstOrDefault();

                if (patient != null)
                {
                    return PartialView("_PatientDetail", patient);
                }
                else
                {
                    return Redirect(null);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method PatientSearch InsurancesController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "InsuranceSearch", "PatientSearch", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        public ActionResult CheckPatientEligibility(int PatientId, string PokitDok_TradingPartnerId, bool IsInitial = false)
        {
            try
            {
                var patient = _unitOfWork.PatientsRepository.GetSingle(t => t.PatientId == PatientId);
                vwPhysician physician = _unitOfWork.PhysiciansRepository.GetSingle(t => t.PhysicianId == patient.PhysicianId);
                PokitDokModel pokitDokModel = null;

                if (patient != null && physician != null)
                {
                    bool IsEdit = true;
                    var insuranceEligibilityInfo = _unitOfWork.InsuranceEligibilityInfoRepository.GetSingle(t => t.PatientId == PatientId);

                    if (insuranceEligibilityInfo == null)
                    {
                        insuranceEligibilityInfo = new InsuranceEligibilityInfo();
                        IsEdit = false;
                    }

                    if ((string.IsNullOrEmpty(insuranceEligibilityInfo.EligibilityValue) || (!insuranceEligibilityInfo.PokitDok_TradingPartnerId?.Equals(PokitDok_TradingPartnerId) ?? true)) && !IsInitial)
                    {
                        //ViewBag.PokitDok_TradingPartnerId = !string.IsNullOrEmpty(insuranceEligibilityInfo?.PokitDok_TradingPartnerId);
                        EligibilityRecord eligibilityRecord = new EligibilityRecord();

                        string birthDate = patient.DateOfBirth.Value.ToString("yyyy-MM-dd");
                        string memFirstName = patient.FirstName;
                        string memLastName = patient.LastName;

                        string npi = physician.NationalProviderIdentifier;
                        string prodLastName = physician.LastName;
                        string prodFirstName = physician.FirstName;

                        string tradingPartnerId = "MOCKPAYER";

                        pokitDokModel = eligibilityRecord.RetrieveData(birthDate, memFirstName, memLastName, npi, prodFirstName, prodLastName, tradingPartnerId);

                        insuranceEligibilityInfo.EligibilityValue = pokitDokModel.ToXml();
                        insuranceEligibilityInfo.PokitDok_TradingPartnerId = PokitDok_TradingPartnerId;
                        insuranceEligibilityInfo.PatientId = patient.PatientId;
                        insuranceEligibilityInfo.ModifiedBy = CurrentUser.UserId;
                        insuranceEligibilityInfo.ModifiedOn = DateTime.UtcNow;

                        if (IsEdit)
                        {
                            _unitOfWork.InsuranceEligibilityInfoRepository.Update(insuranceEligibilityInfo);
                        }
                        else
                        {
                            insuranceEligibilityInfo.CreatedBy = CurrentUser.UserId;
                            insuranceEligibilityInfo.CreatedOn = DateTime.UtcNow;
                            _unitOfWork.InsuranceEligibilityInfoRepository.Insert(insuranceEligibilityInfo);
                        }
                        _unitOfWork.Save();
                    }
                    else if (!string.IsNullOrEmpty(insuranceEligibilityInfo.EligibilityValue))
                    {
                        string str = insuranceEligibilityInfo.EligibilityValue;
                        pokitDokModel = str.FromXml<PokitDokModel>();
                    }
                    else if (string.IsNullOrEmpty(insuranceEligibilityInfo.EligibilityValue) && IsInitial)
                    {
                        return View();
                    }
                }
                return View(pokitDokModel);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method CheckPatientEligibility InsurancesController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "InsurancesController", "CheckPatientEligibility", ErrorTitle, ErrorMessage, null));
            }
        }

        public JsonResult GetPokitDok_TradingPartnerId(int PatientId)
        {
            try
            {
            string pokitDok_TradingPartnerId = _unitOfWork.InsuranceEligibilityInfoRepository.GetSingle(t => t.PatientId == PatientId)?.PokitDok_TradingPartnerId;
            return Json(new { message = pokitDok_TradingPartnerId, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPokitDok_TradingPartnerId() InsurancesController. " + exception.Message, exception);
                return Json(new { message = exception.ToString(), success = false });
            }
        }
    }
}
