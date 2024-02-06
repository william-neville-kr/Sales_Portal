using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SalesTeam.Core.Repository;
using SalesTeam.Core.Data;
using SalesTeam.Web.Models;
using System.Collections.Generic;
using System.Web;
using SalesTeam.Core.Helpers;
using System.Net.Mail;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class SalesTeamPhysicianController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: SalesTeamPhysician
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getSalesTeamPhysiciansRecords([DataSourceRequest] DataSourceRequest request, string sreachString,
            vwSalesTeamPhysician _vwSalesTeamPhysician = null)
        {
            try
            {
                int physicianId = 0, salesTeamId = 0;
                IQueryable<PhysicianViewModel> physicianViewModelQueryable;

                if (_vwSalesTeamPhysician != null)
                {
                    physicianId = _vwSalesTeamPhysician.PhysicianId;
                    salesTeamId = _vwSalesTeamPhysician.SalesTeamId;
                }

                //if (physicianId != 0 && salesTeamId != 0)
                //{
                //    vwSalesTeamPhysicianList = _unitOfWork.vwSalesTeamPhysiciansRepository.GetAsQuerable(t => t.PhysicianId == physicianId && t.SalesTeamId == salesTeamId);
                //}
                //else
                if (salesTeamId != 0)
                {
                    physicianViewModelQueryable = _unitOfWork.vwSalesTeamPhysiciansRepository
                                                .GetAsQuerable
                                                (
                                                    t => t.MasterSalesTeamId == MasterSalesTeamId && t.SalesTeamId == salesTeamId
                                                )
                                                .Select(t => new PhysicianViewModel
                                                {
                                                    PhysicianId = t.PhysicianId,
                                                    SalesTeamId = t.SalesTeamId,
                                                    FirstName = t.FirstName,
                                                    LastName = t.LastName,
                                                    AddressLine1 = t.AddressLine1,
                                                    City = t.City,
                                                    StateProvince = t.StateProvince,
                                                    PostalCode = t.PostalCode,
                                                    NationalProviderIdentifier = t.NationalProviderIdentifier,
                                                    Pending = t.Pending,
                                                    Active = t.Active,
                                                    Cancelled = t.Cancelled,
                                                    Url_SId_PhyId = t.SalesTeamId + "&" + t.PhysicianId
                                                });
                    //.ToList();
                }
                else
                {
                    physicianViewModelQueryable = _unitOfWork.vwSalesTeamPhysiciansRepository
                                                .GetAsQuerable
                                                (
                                                    t => (t.MasterSalesTeamId == MasterSalesTeamId)
                                                    &&
                                                    (
                                                        t.FullName.ToLower().Contains(sreachString.ToLower())
                                                        || t.City.ToLower().Contains(sreachString.ToLower())
                                                        || t.AddressLine1.ToLower().Contains(sreachString.ToLower())
                                                        || t.NationalProviderIdentifier.ToLower().Contains(sreachString.ToLower())
                                                        || t.StateProvince.ToLower().Contains(sreachString.ToLower())
                                                    )
                                                )
                                                .Select(t => new PhysicianViewModel
                                                {
                                                    PhysicianId = t.PhysicianId,
                                                    SalesTeamId = t.SalesTeamId,
                                                    FirstName = t.FirstName,
                                                    LastName = t.LastName,
                                                    AddressLine1 = t.AddressLine1,
                                                    City = t.City,
                                                    StateProvince = t.StateProvince,
                                                    PostalCode = t.PostalCode,
                                                    NationalProviderIdentifier = t.NationalProviderIdentifier,
                                                    Pending = t.Pending,
                                                    Active = t.Active,
                                                    Cancelled = t.Cancelled,
                                                    Url_SId_PhyId = t.SalesTeamId + "&" + t.PhysicianId
                                                });
                    //.ToList();
                }

                var physicianViewModelList = physicianViewModelQueryable.ToDataSourceResult(request);
                foreach (PhysicianViewModel physician in physicianViewModelList.Data)
                {
                    physician.Url_SId_PhyId = HttpUtility.UrlEncode(physician.Url_SId_PhyId.Encrypt());
                    var consentGranted = _unitOfWork.PhysicianConsentRepository.GetAsQuerable(x => x.PhysicianId == physician.PhysicianId && x.SalesTeamId == MasterSalesTeamId).FirstOrDefault();
                    physician.ConsentGranted = consentGranted != null ? (consentGranted.ConsentGranted ?? false) : false;
                }
                return Json(physicianViewModelList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPhysiciansList([DataSourceRequest] DataSourceRequest request, vwPhysician _vwPhysician = null)
        {
            try
            {
                int salesTeamId = 0;
                List<PhysicianViewModel> physicianViewModelList;

                if (_vwPhysician != null)
                {
                    salesTeamId = _vwPhysician.SalesTeamId;
                }

                if (salesTeamId != 0)
                {
                    physicianViewModelList = _unitOfWork.vwPhysiciansRepository
                                                .GetAsQuerable(t => t.SalesTeamId == salesTeamId)
                                                .Select(t => new PhysicianViewModel
                                                {
                                                    PhysicianId = t.PhysicianId,
                                                    SalesTeamId = t.SalesTeamId,
                                                    FirstName = t.FirstName,
                                                    LastName = t.LastName,
                                                    AddressLine1 = t.AddressLine1,
                                                    City = t.City,
                                                    StateProvince = t.StateProvince,
                                                    PostalCode = t.PostalCode,
                                                    NationalProviderIdentifier = t.NationalProviderIdentifier,
                                                    Pending = t.Pending,
                                                    Active = t.Active,
                                                    Cancelled = t.Cancelled,
                                                    Url_SId_PhyId = t.SalesTeamId + "&" + t.PhysicianId
                                                })
                                                .ToList();

                    for (int i = 0; i < physicianViewModelList.Count; i++)
                    {
                        physicianViewModelList[i].Url_SId_PhyId = HttpUtility.UrlEncode(physicianViewModelList[i].Url_SId_PhyId.Encrypt());
                    }

                    return Json(physicianViewModelList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalesTeamPhysicianDrugTherapySearch([DataSourceRequest] DataSourceRequest request,
            string sreachString1, string sreachString2, string sreachString3)
        {
            try
            {
                IQueryable<SalesTeamPhysicianDrugTherapySearchViewModel> salesTeamPhysicianDrugTherapySearchViewModelList
                    = _unitOfWork.SalesTeamPhysicianDrugTherapySearchRepository
                                            .GetAsQuerable
                                            (
                                                t =>
                                                    t.MasterSalesTeamId == MasterSalesTeamId
                                            //&& (
                                            //    t.City.ToLower().Contains(sreachString1.ToLower())
                                            //    && t.DiseaseState.ToLower().Contains(sreachString2.ToLower())
                                            //    && t.CoreTherapyDrugShortName.ToLower().Contains(sreachString3.ToLower())
                                            //)
                                            )
                                            .Select(t => new SalesTeamPhysicianDrugTherapySearchViewModel
                                            {
                                                PhysicianId = t.PhysicianId,
                                                SalesTeamId = t.SalesTeamId,
                                                FirstName = t.FirstName,
                                                LastName = t.LastName,
                                                AddressLine1 = t.AddressLine1,
                                                City = t.City,
                                                StateProvince = t.StateProvince,
                                                PostalCode = t.PostalCode,
                                                NationalProviderIdentifier = t.NationalProviderIdentifier,
                                                Pending = t.Pending,
                                                Active = t.Active,
                                                Cancelled = t.Cancelled,
                                                Url_SId_PhyId = t.SalesTeamId + "&" + t.PhysicianId,
                                                LastPatientReferralDateId = t.LastPatientReferralDateId,
                                                DiseaseState = t.DiseaseState,
                                                CoreTherapyDrugShortName = t.CoreTherapyDrugShortName,
                                                LastTherepyReferralDate = t.LastTherepyReferralDate
                                            });

                if (!string.IsNullOrWhiteSpace(sreachString1))
                {
                    salesTeamPhysicianDrugTherapySearchViewModelList = salesTeamPhysicianDrugTherapySearchViewModelList.Where(t => t.City.ToLower().Contains(sreachString1.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(sreachString2))
                {
                    salesTeamPhysicianDrugTherapySearchViewModelList = salesTeamPhysicianDrugTherapySearchViewModelList.Where(t => t.DiseaseState.ToLower().Contains(sreachString2.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(sreachString3))
                {
                    salesTeamPhysicianDrugTherapySearchViewModelList = salesTeamPhysicianDrugTherapySearchViewModelList.Where(t => t.CoreTherapyDrugShortName.ToLower().Contains(sreachString3.ToLower()));
                }

                var filteredList = salesTeamPhysicianDrugTherapySearchViewModelList.ToDataSourceResult(request);

                foreach (SalesTeamPhysicianDrugTherapySearchViewModel item in filteredList.Data)
                {
                    item.Url_SId_PhyId = HttpUtility.UrlEncode(item.Url_SId_PhyId.Encrypt());
                }

                
                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendPhysicianConsentRequest(int physicianId, string physicianEmail)
        {
            try
            {
                var parameters = HttpUtility.UrlEncode((physicianId.ToString() + "&" + MasterSalesTeamId.ToString()).Encrypt());
                
                var consentGrantUrl = SettingManager.GetSettingByName("ProvidePhysicianConsentUrl").Value + parameters;
                var consentDenyUrl = SettingManager.GetSettingByName("DenyPhysicianConsentUrl").Value + parameters;

                var mailString = @"<p>Sales Member wants your consent to view your patients.</p>
                                   <br />
                                   <p> Click on the following link to provide your consent.</p>
                                   <br /> 
                                   <a href='" + consentGrantUrl + @"'>Provide Consent</a>
                                   <br /> 
                                   <a href='" + consentDenyUrl + "'>Deny Consent</a>";

                CommonHelper.SendEmail("Consent Required", mailString, physicianEmail, null);

                var physicianConsent = _unitOfWork.PhysicianConsentRepository.GetAsQuerable(x => x.PhysicianId == physicianId && x.SalesTeamId == MasterSalesTeamId).FirstOrDefault();

                if (physicianConsent != null)
                {
                    physicianConsent.ModifiedOn = DateTime.UtcNow;
                    physicianConsent.IPAdressSalesTeam = Request.UserHostAddress;
                    physicianConsent.PhysicianEmailAddress = physicianEmail;

                    _unitOfWork.PhysicianConsentRepository.Update(physicianConsent);
                }
                else
                {
                    physicianConsent = new PhysicianConsent
                    {
                        PhysicianId = physicianId,
                        SalesTeamId = MasterSalesTeamId,
                        ConsentGranted = null, // Null means consent is pending
                        CreatedOn = DateTime.UtcNow,
                        IPAdressSalesTeam = Request.UserHostAddress,
                        PhysicianEmailAddress = physicianEmail

                    };
                    _unitOfWork.PhysicianConsentRepository.Insert(physicianConsent);
                }

                _unitOfWork.Save();

                return Json(new { success = true, message = "Consent request email sent" });
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);

                return Json(new { success = false, message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ProvidePhysicianConsent(string parameters)
        {
            string[] ids = parameters.Decrypt().Split('&');

            int physicianId = ids[0].ToInt0();
            int salesTeamId = ids[1].ToInt0();

            var physicianConsent = _unitOfWork.PhysicianConsentRepository.GetAsQuerable(x => x.PhysicianId == physicianId && x.SalesTeamId == salesTeamId).FirstOrDefault();

            if (physicianConsent != null)
            {
                physicianConsent.ConsentGranted = true;
                physicianConsent.ConsentChangedOn = DateTime.UtcNow;

                _unitOfWork.PhysicianConsentRepository.Update(physicianConsent);
                _unitOfWork.Save();
            }

            return View();
            // Return a view here if showing the result to the physician
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult DenyPhysicianConsent(string parameters)
        {
            string[] ids = parameters.Decrypt().Split('&');

            int physicianId = ids[0].ToInt0();
            int salesTeamId = ids[1].ToInt0();

            var physicianConsent = _unitOfWork.PhysicianConsentRepository.GetAsQuerable(x => x.PhysicianId == physicianId && x.SalesTeamId == salesTeamId).FirstOrDefault();

            if (physicianConsent != null)
            {
                physicianConsent.ConsentGranted = false;
                physicianConsent.ConsentChangedOn = DateTime.UtcNow;

                _unitOfWork.PhysicianConsentRepository.Update(physicianConsent);
                _unitOfWork.Save();
            }

            return View();
            // Return a view here if showing the result to the physician
        }

    }
}