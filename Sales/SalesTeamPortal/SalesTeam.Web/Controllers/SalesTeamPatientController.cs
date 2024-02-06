using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SalesTeam.Core.Helpers;
using SalesTeam.Core.Repository;
using SalesTeam.Core.Data;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using SalesTeam.Web.Models;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Web;
using System.Net;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class SalesTeamPatientController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: SalesTeamPatient
        public ActionResult Index()
        {
            DimProcessTime processTime = _unitOfWork.DimProcessTimesRepository.GetSingle();
            ViewBag.LatestOrderTime = processTime?.LatestOrderTime;
            return View();
        }

        public ActionResult getPatientsRecords([DataSourceRequest] DataSourceRequest request, string sreachString = null,
            vwPatient _vwPatient = null)
        {
            try
            {
                bool needMoreInfo = false;
                List<PatientViewModel> patientViewModelList;
                if (_vwPatient != null)
                {
                    needMoreInfo = _vwPatient.NeedMoreInfo ?? false;
                }

                if (needMoreInfo)
                {
                    patientViewModelList = _unitOfWork.vwPatientRepository.GetAsQuerable(
                            t =>
                                (
                                    t.PhysicianName.ToLower().Contains(sreachString.ToLower())
                                    || t.PhyCity.ToLower().Contains(sreachString.ToLower())
                                    || t.FullName.ToLower().Contains(sreachString.ToLower())
                                    || t.ActivityStatus.ToLower().Contains(sreachString.ToLower())
                                    || t.PatientCode.ToLower().Contains(sreachString.ToLower())
                                    || t.StateProvince.ToLower().Contains(sreachString.ToLower())
                                )
                                && t.NeedMoreInfo == needMoreInfo
                    )
                    .Select(t => new PatientViewModel
                    {
                        PatientId = t.PatientId,
                        PhysicianId = t.PhysicianId,
                        SalesTeamId = t.SalesTeamId,
                        ReferralDateId = t.ReferralDateId,
                        PatientCode = t.PatientCode,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        PhysicianName = t.PhysicianName,
                        PhyCity = t.PhyCity,
                        StateProvince = t.StateProvince,
                        DrugShortName = t.DrugShortName,
                        ActivityStatus = t.ActivityStatus,
                        OriginatingPharmacy = t.OriginatingPharmacy,
                        NeedMoreInfo = t.NeedMoreInfo,
                        Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                    })
                    .OrderBy(t => t.ActivityStatus)
                    .ToList();
                }
                else
                {
                    patientViewModelList = _unitOfWork.vwPatientRepository.GetAsQuerable(
                            t =>
                                t.PhysicianName.ToLower().Contains(sreachString.ToLower())
                                || t.PhyCity.ToLower().Contains(sreachString.ToLower())
                                || t.FullName.ToLower().Contains(sreachString.ToLower())
                                || t.ActivityStatus.ToLower().Contains(sreachString.ToLower())
                                || t.PatientCode.ToLower().Contains(sreachString.ToLower())
                                || t.StateProvince.ToLower().Contains(sreachString.ToLower())
                    )
                    .Select(t => new PatientViewModel
                    {
                        PatientId = t.PatientId,
                        PhysicianId = t.PhysicianId,
                        SalesTeamId = t.SalesTeamId,
                        ReferralDateId = t.ReferralDateId,
                        PatientCode = t.PatientCode,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        PhysicianName = t.PhysicianName,
                        PhyCity = t.PhyCity,
                        StateProvince = t.StateProvince,
                        DrugShortName = t.DrugShortName,
                        ActivityStatus = t.ActivityStatus,
                        OriginatingPharmacy = t.OriginatingPharmacy,
                        NeedMoreInfo = t.NeedMoreInfo,
                        Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                    })
                    .OrderBy(t => t.ActivityStatus)
                    .ToList();
                }
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

        public ActionResult getSalesTeamPatientsRecords([DataSourceRequest] DataSourceRequest request, string sreachString = null,
            vwSalesTeamPatient _vwSalesTeamPatient = null)
        {
            try
            {
                bool needMoreInfo = false;
                IQueryable patientViewModelList;
                if (_vwSalesTeamPatient != null)
                {
                    needMoreInfo = _vwSalesTeamPatient.NeedMoreInfo ?? false;
                }

                if (needMoreInfo == true)
                {
                    patientViewModelList = _unitOfWork.vwSalesTeamPatientsRepository.GetAsQuerable(
                        t =>
                            t.MasterSalesTeamId == MasterSalesTeamId
                            &&
                            (
                                t.PhysicianName.ToLower().Contains(sreachString.ToLower())
                                || t.phyCity.ToLower().Contains(sreachString.ToLower())
                                || t.FullName.ToLower().Contains(sreachString.ToLower())
                                || t.ActivityStatus.ToLower().Contains(sreachString.ToLower())
                                || t.PatientCode.ToLower().Contains(sreachString.ToLower())
                                || t.StateProvince.ToLower().Contains(sreachString.ToLower())
                            )
                            && t.NeedMoreInfo == needMoreInfo
                    )
                    .Select(t => new PatientViewModel
                    {
                        PatientId = t.PatientId,
                        PhysicianId = t.PhysicianId,
                        SalesTeamId = t.SalesTeamId,
                        ReferralDateId = t.ReferralDateId,
                        PatientCode = t.PatientCode,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        PhysicianName = t.PhysicianName,
                        PhyCity = t.phyCity,
                        StateProvince = t.StateProvince,
                        DrugShortName = t.DrugShortName,
                        ActivityStatus = t.ActivityStatus,
                        OriginatingPharmacy = t.OriginatingPharmacy,
                        NeedMoreInfo = t.NeedMoreInfo,
                        Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                    })
                    .OrderBy(t => t.ActivityStatus);
                }
                else
                {
                    patientViewModelList = _unitOfWork.vwSalesTeamPatientsRepository.GetAsQuerable(
                        t =>
                            t.MasterSalesTeamId == MasterSalesTeamId
                            &&
                            (
                                t.PhysicianName.ToLower().Contains(sreachString.ToLower())
                                || t.phyCity.ToLower().Contains(sreachString.ToLower())
                                || t.FullName.ToLower().Contains(sreachString.ToLower())
                                || t.ActivityStatus.ToLower().Contains(sreachString.ToLower())
                                || t.PatientCode.ToLower().Contains(sreachString.ToLower())
                                || t.StateProvince.ToLower().Contains(sreachString.ToLower())
                            )
                    )
                    .Select(t => new PatientViewModel
                    {
                        PatientId = t.PatientId,
                        PhysicianId = t.PhysicianId,
                        SalesTeamId = t.SalesTeamId,
                        ReferralDateId = t.ReferralDateId,
                        PatientCode = t.PatientCode,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        PhysicianName = t.PhysicianName,
                        PhyCity = t.phyCity,
                        StateProvince = t.StateProvince,
                        DrugShortName = t.DrugShortName,
                        ActivityStatus = t.ActivityStatus,
                        OriginatingPharmacy = t.OriginatingPharmacy,
                        NeedMoreInfo = t.NeedMoreInfo,
                        Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                    })
                    .OrderBy(t => t.ActivityStatus);
                }

                var filteredList = patientViewModelList.ToDataSourceResult(request);

                foreach (PatientViewModel item in patientViewModelList)
                {
                    item.Url_SId_PId = HttpUtility.UrlEncode(item.Url_SId_PId.Encrypt());
                }

                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPatients([DataSourceRequest] DataSourceRequest request, vwSalesTeamPatient _vwSalesTeamPatient = null)
        {
            try
            {
                int physicianId = 0, masterSalesTeamId = 0;
                bool needMoreInfo = false;
                List<PatientViewModel> patientViewModelList;
                if (_vwSalesTeamPatient != null)
                {
                    physicianId = _vwSalesTeamPatient.PhysicianId;
                    masterSalesTeamId = _vwSalesTeamPatient.MasterSalesTeamId;
                    needMoreInfo = _vwSalesTeamPatient.NeedMoreInfo ?? false;
                }
                if (physicianId != 0 && masterSalesTeamId != 0)
                {
                    if (needMoreInfo)
                    {
                        patientViewModelList = _unitOfWork.vwSalesTeamPatientsRepository.GetAsQuerable(t => t.PhysicianId == physicianId && t.MasterSalesTeamId == masterSalesTeamId && t.NeedMoreInfo == needMoreInfo)
                                                    .Select(t => new PatientViewModel
                                                    {
                                                        PatientId = t.PatientId,
                                                        PhysicianId = t.PhysicianId,
                                                        SalesTeamId = t.SalesTeamId,
                                                        ReferralDateId = t.ReferralDateId,
                                                        PatientCode = t.PatientCode,
                                                        FirstName = t.FirstName,
                                                        LastName = t.LastName,
                                                        PhysicianName = t.PhysicianName,
                                                        PhyCity = t.phyCity,
                                                        StateProvince = t.StateProvince,
                                                        DrugShortName = t.DrugShortName,
                                                        ActivityStatus = t.ActivityStatus,
                                                        OriginatingPharmacy = t.OriginatingPharmacy,
                                                        NeedMoreInfo = t.NeedMoreInfo,
                                                        Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                                                    })
                                                    .OrderBy(t => t.ActivityStatus)
                                                    .ToList();
                    }
                    else
                    {
                        patientViewModelList = _unitOfWork.vwSalesTeamPatientsRepository.GetAsQuerable(t => t.PhysicianId == physicianId && t.MasterSalesTeamId == masterSalesTeamId)
                                                    .Select(t => new PatientViewModel
                                                    {
                                                        PatientId = t.PatientId,
                                                        PhysicianId = t.PhysicianId,
                                                        SalesTeamId = t.SalesTeamId,
                                                        ReferralDateId = t.ReferralDateId,
                                                        PatientCode = t.PatientCode,
                                                        FirstName = t.FirstName,
                                                        LastName = t.LastName,
                                                        PhysicianName = t.PhysicianName,
                                                        PhyCity = t.phyCity,
                                                        StateProvince = t.StateProvince,
                                                        DrugShortName = t.DrugShortName,
                                                        ActivityStatus = t.ActivityStatus,
                                                        OriginatingPharmacy = t.OriginatingPharmacy,
                                                        NeedMoreInfo = t.NeedMoreInfo,
                                                        Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                                                    })
                                                    .OrderBy(t => t.ActivityStatus)
                                                    .ToList();
                    }
                    for (int i = 0; i < patientViewModelList.Count; i++)
                    {
                        patientViewModelList[i].Url_SId_PId = HttpUtility.UrlEncode(patientViewModelList[i].Url_SId_PId.Encrypt());
                    }
                    return Json(patientViewModelList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSalesTeamPatientsRecordsWithReferralDate([DataSourceRequest] DataSourceRequest request,
            DateTime? sreachStringReferralDateFrom = null, DateTime? sreachStringReferralDateTo = null,
            string sreachStringPhysician = null)
        {
            try
            {
                IQueryable patientViewModelList;

                if (sreachStringPhysician != "" && sreachStringReferralDateFrom != null && sreachStringReferralDateTo != null)
                {
                    patientViewModelList = _unitOfWork.vwSalesTeamPatientsRepository
                                                .GetAsQuerable
                                                (
                                                    t =>
                                                        t.MasterSalesTeamId == MasterSalesTeamId
                                                        && t.ReferralDateId.Value >= sreachStringReferralDateFrom
                                                        && t.ReferralDateId.Value <= sreachStringReferralDateTo
                                                        && t.PhysicianName.ToLower().Contains(sreachStringPhysician.ToLower())
                                                )
                                                .Select(t => new PatientViewModel
                                                {
                                                    PatientId = t.PatientId,
                                                    PhysicianId = t.PhysicianId,
                                                    SalesTeamId = t.SalesTeamId,
                                                    ReferralDateId = t.ReferralDateId,
                                                    PatientCode = t.PatientCode,
                                                    FirstName = t.FirstName,
                                                    LastName = t.LastName,
                                                    PhysicianName = t.PhysicianName,
                                                    PhyCity = t.phyCity,
                                                    StateProvince = t.StateProvince,
                                                    DrugShortName = t.DrugShortName,
                                                    ActivityStatus = t.ActivityStatus,
                                                    OriginatingPharmacy = t.OriginatingPharmacy,
                                                    NeedMoreInfo = t.NeedMoreInfo,
                                                    Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                                                });
                }
                else if (sreachStringReferralDateFrom != null && sreachStringReferralDateTo != null)
                {
                    patientViewModelList = _unitOfWork.vwSalesTeamPatientsRepository
                                                .GetAsQuerable
                                                (
                                                    t =>
                                                        t.MasterSalesTeamId == MasterSalesTeamId
                                                        && t.ReferralDateId.Value >= sreachStringReferralDateFrom
                                                        && t.ReferralDateId.Value <= sreachStringReferralDateTo
                                                )
                                                .Select(t => new PatientViewModel
                                                {
                                                    PatientId = t.PatientId,
                                                    PhysicianId = t.PhysicianId,
                                                    SalesTeamId = t.SalesTeamId,
                                                    ReferralDateId = t.ReferralDateId,
                                                    PatientCode = t.PatientCode,
                                                    FirstName = t.FirstName,
                                                    LastName = t.LastName,
                                                    PhysicianName = t.PhysicianName,
                                                    PhyCity = t.phyCity,
                                                    StateProvince = t.StateProvince,
                                                    DrugShortName = t.DrugShortName,
                                                    ActivityStatus = t.ActivityStatus,
                                                    OriginatingPharmacy = t.OriginatingPharmacy,
                                                    NeedMoreInfo = t.NeedMoreInfo,
                                                    Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                                                });
                }
                else
                {
                    patientViewModelList = null;
                }

                if (patientViewModelList != null)
                {
                    var filteredList = patientViewModelList.ToDataSourceResult(request);

                    foreach (PatientViewModel item in filteredList.Data)
                    {
                        item.Url_SId_PId = HttpUtility.HtmlEncode(item.Url_SId_PId.Encrypt());
                    }

                    return Json(filteredList, JsonRequestBehavior.AllowGet);
                }

                return Json(patientViewModelList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSalesTeamPatientsDrugsRecords([DataSourceRequest] DataSourceRequest request,
            string sreachString = null)
        {
            try
            {
                IQueryable patientViewModelList;

                if (sreachString != null)
                {
                    patientViewModelList = _unitOfWork.vwSalesTeamPatientsRepository
                                                .GetAsQuerable
                                                (
                                                    t =>
                                                        t.MasterSalesTeamId == MasterSalesTeamId
                                                        && t.DrugShortName.ToLower().Contains(sreachString.ToLower())
                                                )
                                                .Select(t => new PatientViewModel
                                                {
                                                    PatientId = t.PatientId,
                                                    PhysicianId = t.PhysicianId,
                                                    SalesTeamId = t.SalesTeamId,
                                                    ReferralDateId = t.ReferralDateId,
                                                    PatientCode = t.PatientCode,
                                                    FirstName = t.FirstName,
                                                    LastName = t.LastName,
                                                    PhysicianName = t.PhysicianName,
                                                    PhyCity = t.phyCity,
                                                    StateProvince = t.StateProvince,
                                                    DrugShortName = t.DrugShortName,
                                                    ActivityStatus = t.ActivityStatus,
                                                    OriginatingPharmacy = t.OriginatingPharmacy,
                                                    NeedMoreInfo = t.NeedMoreInfo,
                                                    Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                                                });
                }
                else
                {
                    patientViewModelList = _unitOfWork.vwSalesTeamPatientsRepository
                                               .GetAsQuerable
                                               (
                                                   t => t.MasterSalesTeamId == MasterSalesTeamId
                                               )
                                               .Select(t => new PatientViewModel
                                               {
                                                   PatientId = t.PatientId,
                                                   PhysicianId = t.PhysicianId,
                                                   SalesTeamId = t.SalesTeamId,
                                                   ReferralDateId = t.ReferralDateId,
                                                   PatientCode = t.PatientCode,
                                                   FirstName = t.FirstName,
                                                   LastName = t.LastName,
                                                   PhysicianName = t.PhysicianName,
                                                   PhyCity = t.phyCity,
                                                   StateProvince = t.StateProvince,
                                                   DrugShortName = t.DrugShortName,
                                                   ActivityStatus = t.ActivityStatus,
                                                   OriginatingPharmacy = t.OriginatingPharmacy,
                                                   NeedMoreInfo = t.NeedMoreInfo,
                                                   Url_SId_PId = t.SalesTeamId.ToString() + "&" + t.PatientId
                                               });
                }

                var filteredList = patientViewModelList.ToDataSourceResult(request);

                foreach (PatientViewModel item in filteredList.Data)
                {
                    item.Url_SId_PId = HttpUtility.UrlEncode(item.Url_SId_PId.Encrypt());
                }

                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddPatientToQueue(int? PatientId, int? PhysicianId, int? SalesTeamId)
        {
            try
            {
                var saleTeamQueue =
                    _unitOfWork.vwSalesTeamQueuesRepository.GetAsQuerable(
                        t => t.PatientId == PatientId && t.CreatedSalesTeamId == MasterSalesTeamId).FirstOrDefault();
                if (saleTeamQueue == null)
                {
                    var salesTeamQueue = new SalesTeamQueue();
                    salesTeamQueue.PhysicianId = PhysicianId;
                    salesTeamQueue.PatientId = PatientId;
                    salesTeamQueue.SalesTeamId = SalesTeamId;
                    salesTeamQueue.CreatedSalesTeamId = MasterSalesTeamId;
                    salesTeamQueue.CreateDate = DateTime.Now;
                    salesTeamQueue.UpdateDate = DateTime.Now;

                    _unitOfWork.SalesTeamQueuesRepository.Insert(salesTeamQueue);
                    _unitOfWork.Save();
                    return Json(new { message = "Patient Added in Queue Successfully.", success = true });
                    //return Json(new { addresses = listAddressesReturn, success = true });
                }
                return Json(new { message = "Patient already exits in Queue.", success = false });
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
                return Json(new { message = ex.Message, success = false });
            }
        }

        public ActionResult getPatientPrescriptionRecords([DataSourceRequest] DataSourceRequest request,
            vwPrescription _vwPrescription = null)
        {
            try
            {
                var patientID = 0;

                if (_vwPrescription != null)
                    patientID = _vwPrescription.PatientId ?? 0;
                var vwPrescriptionsList = _unitOfWork.vwPrescriptionRepository.GetAsQuerable(t => t.PatientId == patientID);
                return Json(vwPrescriptionsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPatientDocumentsRecords([DataSourceRequest] DataSourceRequest request,
            vwDocument _vwDocument = null)
        {
            try
            {
                var patientID = 0;

                if (_vwDocument != null)
                    patientID = _vwDocument.PatientId;
                var vwDocumentsList = _unitOfWork.vwDocumentRepository.GetAsQuerable(t => t.PatientId == patientID);

                var filteredList = vwDocumentsList.ToDataSourceResult(request);

                foreach (vwDocument document in filteredList.Data)
                {
                    document.FolderFileNameUNC = null;
                    document.DocumentIdEncrypted = document.DocumenstId.ToString().Encrypt();
                    document.DocumenstId = -1;
                }

                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowInsuranceInfo(int InsuranceID)
        {
            try
            {
                vwPatientInsurance patientInsurance = _unitOfWork.vwPatientInsurancesRepository.GetAsQuerable(t => t.InsuranceId == InsuranceID).FirstOrDefault();
                return PartialView("_InsuranceDetails", patientInsurance);
            }
            catch (Exception ex)
            {
                return PartialView("Error", ex.Message);
            }
        }

        #region GetPatientDocumentsTIF
        //public JsonResult GetPatientDocumentsTIF(string fileName)
        //{
        //    try
        //    {
        //        //if (fileName.Length > 0 && System.IO.File.Exists(Server.MapPath(fileName)))
        //        //{
        //        //    var byteArr = ShowImage(Server.MapPath(fileName));
        //        //    return Json(new { message = Convert.ToBase64String(byteArr), success = true });
        //        //}
        //        if (fileName.Length > 0 && System.IO.File.Exists(fileName))
        //        {
        //            var byteArr = ShowImage(fileName);
        //            return Json(new { message = Convert.ToBase64String(byteArr), success = true });
        //        }
        //        else
        //        {
        //            return Json(new { message = "Not Found", success = false });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogInfo(ex.Message, ex);
        //        return Json(new { message = "Error in GetPatientDocumentsTIF.", success = false });
        //    }
        //}


        public byte[] ShowImage(string fileUrl, string username, string password)
        {
            WebClient wc = new WebClient();
            wc.UseDefaultCredentials = true;
            wc.Credentials = new NetworkCredential(username, password);
            Stream newtworkStream = wc.OpenRead(fileUrl);

            Image imageBitmap = Image.FromStream(newtworkStream);
            //Image imageBitmap = Image.FromFile(fileName);

            int frameCount = imageBitmap.GetFrameCount(FrameDimension.Page);
            int runningHeight = 0;
            int maxWidth = 0;
            for (int measurementFrameIndex = 0; (measurementFrameIndex <= (frameCount - 1)); measurementFrameIndex++)
            {
                imageBitmap.SelectActiveFrame(FrameDimension.Page, measurementFrameIndex);
                runningHeight = (runningHeight + imageBitmap.Height);
                maxWidth = Math.Max(maxWidth, imageBitmap.Width);
            }

            var combinedBitmap = new Bitmap(maxWidth, runningHeight);
            int runningVerticalPosition = 0;
            for (int frameIndex = 0; frameIndex <= (frameCount - 1); frameIndex++)
            {
                imageBitmap.SelectActiveFrame(FrameDimension.Page, frameIndex);
                EmbedBitmap(imageBitmap, ref combinedBitmap, runningVerticalPosition);

                runningVerticalPosition = (runningVerticalPosition + (imageBitmap.Height + 5));
            }

            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                combinedBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }

            return byteArray;
        }

        private static void EmbedBitmap(Image SourceBitmap, ref Bitmap DestinationBitmap, int VerticalPosition)
        {
            var sourceRectangle = new Rectangle(new Point(0, 0), new Size(SourceBitmap.Width, SourceBitmap.Height));
            var destinationRectangle = new Rectangle(new Point(0, VerticalPosition),
                new Size(SourceBitmap.Width, SourceBitmap.Height));
            using (Graphics canvas = Graphics.FromImage(DestinationBitmap))
            {
                canvas.DrawImage(SourceBitmap, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }
        }
        #endregion

        #region GetPatientDocumentsPDF
        public ActionResult GetPatientDocumentsPDF(string fileName)
        {
            try
            {
                //if (fileName.Length > 0 && System.IO.File.Exists(Server.MapPath(fileName)))
                //{
                //    byte[] contents = System.IO.File.ReadAllBytes(Server.MapPath(fileName));
                //    Response.AddHeader("Content-Disposition", "inline; filename=test.pdf");
                //    return File(contents, "application/pdf");
                //}
                if (fileName.Length > 0 && System.IO.File.Exists(fileName))
                {
                    byte[] contents = System.IO.File.ReadAllBytes(fileName);
                    Response.AddHeader("Content-Disposition", "inline; filename=test.pdf");
                    return File(contents, "application/pdf");
                }
                else
                {
                    return Json(new { message = "Not Found", success = false });
                }
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
                return Json(new { message = "Error in GetPatientDocumentsPDF.", success = false });
            }
        }
        #endregion
        public JsonResult FaxDocument(FaxDocumentModel model)
        {
            try
            {
                int documentId = int.Parse(model.DocumentIdEncrypted.Decrypt());
                vwDocument document = _unitOfWork.vwDocumentRepository.GetAsQuerable(x => x.DocumenstId == documentId).FirstOrDefault();
                string resultMessage = "";

                var physician = _unitOfWork.vwPhysiciansRepository.GetAsQuerable(x => x.PhysicianId == model.PhysicianId).FirstOrDefault();

                var documentsServerUsername = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsServerUsername"].ToString(); 
                var documentsServerPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsServerPassword"].ToString(); 
                

                if (document != null)
                {
                    if (!string.IsNullOrEmpty(document.FolderFileNameUNC))
                    {
                        var index = document.FolderFileNameUNC.IndexOf("FAXES", StringComparison.OrdinalIgnoreCase) > -1 ? document.FolderFileNameUNC.IndexOf("FAXES", StringComparison.OrdinalIgnoreCase) :
                                (document.FolderFileNameUNC.IndexOf("DOCS", StringComparison.OrdinalIgnoreCase) > -1 ? document.FolderFileNameUNC.IndexOf("DOCS", StringComparison.OrdinalIgnoreCase) : -1);
                        byte[] fileData;
                        string fileType = "";

                        if (index > -1)
                        {
                            var subPath = document.FolderFileNameUNC.Substring(index);
                            subPath = subPath.Replace('\\', '/');
                            var documentsUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["PatientDocumentsUrl"].ToString();
                            var filePath = documentsUrl + subPath;
                            if (filePath.ToLower().IndexOf(".tif") > 0)
                            {
                                //tif document
                                fileData = ShowImage(filePath, documentsServerUsername, documentsServerPassword);
                                fileType = "tif";
                            }
                            else
                            {
                                //pdf document
                                WebClient wc = new WebClient();
                                wc.UseDefaultCredentials = true;
                                wc.Credentials = new NetworkCredential("kon8835", "*Tobasco2014");
                                Stream newtworkStream = wc.OpenRead(filePath);
                                MemoryStream ms = new MemoryStream();

                                newtworkStream.CopyTo(ms);
                                fileData = ms.ToArray();
                                fileType = "pdf";
                            }
                        }
                        else
                        {
                            return Json(new { message = "Document not found", success = false });
                        }

                        if (!string.IsNullOrEmpty(model.FaxNumber))
                        {
                            var result = RightFaxHelper.SharedInstance.SendFax_RightFax(model.FaxNumber, fileData, physician != null ? physician.FullName : "", fileType, physician != null ? physician.PhysicianId : 0);

                            if (result != "Success")
                            {
                                return Json(new { message = result, success = false });
                            }

                            resultMessage = "Fax sent successfully";
                        }
                        if (!string.IsNullOrEmpty(model.Email))
                        {
                            CommonHelper.SendEmail("Sales Team, Patient Document", "Please find attached the document", model.Email, null, fileData, fileType);

                            resultMessage = resultMessage == "" ? "Email sent successfully" : "Fax and Email sent successfully";
                        }
                    }
                }

                return Json(new { message = resultMessage, success = true });
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
                return Json(new { message = "Error in FaxDocument.", success = false });
            }
        }

        #region FillPDF
        private AcroFields form;
        //public JsonResult FillPDFForm(PDFFormModel pDFFormModel, FaxDocumentModel faxDocumentModel)
        public JsonResult FillPDFForm(PDFFormModel pDFFormModel)
        {
            try
            {
                vwPhysician _vwPhysician = _unitOfWork.vwPhysiciansRepository.GetAsQuerable(t => t.PhysicianId == pDFFormModel.PhysicianId).FirstOrDefault();
                vwPhysicianAddress _vwPhysicianAddress = _unitOfWork.vwPhysicianAddressRepository.GetAsQuerable(t => t.PID == pDFFormModel.PID).FirstOrDefault();
                int referralFormId = pDFFormModel.FileName.ToInt0();
                ReferralForm referralForm = _unitOfWork.ReferralFormRepository.GetSingle(t => t.ReferralFormId == referralFormId);

                if (referralForm != null)
                {
                    pDFFormModel.FileName = referralForm.ReferralFormValue;
                }
                if (_vwPhysician == null)
                {
                    _vwPhysician = new vwPhysician();
                }
                if (_vwPhysicianAddress == null)
                {
                    _vwPhysicianAddress = new vwPhysicianAddress();
                }

                if (pDFFormModel.FileName.Length > 0 && System.IO.File.Exists(Server.MapPath("~/Document/" + pDFFormModel.FileName + ".pdf")))
                {
                    string file = Server.MapPath("~/Document/" + pDFFormModel.FileName + ".pdf");
                    //path = System.Web.Configuration.WebConfigurationManager.AppSettings["myFilePath"].ToString();
                    //string fileName = Server.MapPath("~/Document/FilledPDF/" + pDFFormModel.FileName + DateTime.Now.Ticks.ToString() + ".pdf");
                    string fileName = System.Web.Configuration.WebConfigurationManager.AppSettings["FilledPDFPath"].ToString() + "/" + pDFFormModel.FileName + DateTime.Now.Ticks.ToString() + ".pdf";

                    //iTextSharp
                    FileStream outFile = new FileStream(fileName, FileMode.Create);
                    PdfReader pdfReader = new PdfReader(file);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, outFile);
                    form = pdfStamper.AcroFields;

                    //PATIENT INFO
                    // Patient Information not requried in Referral Forms
                    //FillPdfTextField("Patient Name.0", !string.IsNullOrEmpty(_vwPatient.FullName) ? _vwPatient.FullName : "");

                    //FillPdfTextField("Patient Name", !string.IsNullOrEmpty(_vwPatient.FullName) ? _vwPatient.FullName : "");
                    //FillPdfTextField("Address", !string.IsNullOrEmpty(_vwPatient.AddressLine1) ? _vwPatient.AddressLine1 : "");
                    //FillPdfTextField("City State Zip",
                    //    (!string.IsNullOrEmpty(_vwPatient.City) ? _vwPatient.City : "")
                    //    + ", " + (!string.IsNullOrEmpty(_vwPatient.StateProvince) ? _vwPatient.StateProvince : "")
                    //    + ", " + (!string.IsNullOrEmpty(_vwPatient.PostalCode) ? _vwPatient.PostalCode : "")
                    //    );

                    //FillPdfTextField("City", !string.IsNullOrEmpty(_vwPatient.City) ? _vwPatient.City : "");
                    //FillPdfTextField("State", !string.IsNullOrEmpty(_vwPatient.StateProvince) ? _vwPatient.StateProvince : "");
                    //FillPdfTextField("Zip", !string.IsNullOrEmpty(_vwPatient.PostalCode) ? _vwPatient.PostalCode : "");

                    //FillPdfTextField("Main Phone", !string.IsNullOrEmpty(_vwPatient.PhoneNumber) ? _vwPatient.PhoneNumber : "");
                    //FillPdfTextField("Alternate Phone", !string.IsNullOrEmpty(_vwPatient.PhoneNumber) ? _vwPatient.PhoneNumber : "");
                    //FillPdfTextField("Social Security", !string.IsNullOrEmpty("Not Found") ? "" : "");
                    //FillPdfTextField("Date of Birth", _vwPatient.DateOfBirth.HasValue ? String.Format("{0:MM/dd/yyyy}", _vwPatient.DateOfBirth) : "");

                    //FillPdfTextField("Patient Name.1", _vwPatient.DateOfBirth.HasValue ? String.Format("{0:MM/dd/yyyy}", _vwPatient.DateOfBirth) : "");

                    ////Gender
                    //if (!string.IsNullOrEmpty(_vwPatient.Gender) && _vwPatient.Gender.ToLower() == "m")
                    //{
                    //    FillPdfCheckBox("male check", true);
                    //}
                    //else if (!string.IsNullOrEmpty(_vwPatient.Gender) && _vwPatient.Gender.ToLower() == "f")
                    //{
                    //    FillPdfCheckBox("female check", true);
                    //}

                    //PRESCRIBER INFO
                    FillPdfTextField("Prescriber Name", !string.IsNullOrEmpty(_vwPhysician.FullName) ? _vwPhysician.FullName : "");
                    FillPdfTextField("DEA", !string.IsNullOrEmpty(_vwPhysician.DrugEnforcementAdministrationNumber) ? _vwPhysician.DrugEnforcementAdministrationNumber : "");
                    FillPdfTextField("NPI", !string.IsNullOrEmpty(_vwPhysician.NationalProviderIdentifier) ? _vwPhysician.NationalProviderIdentifier : "");
                    FillPdfTextField("License", !string.IsNullOrEmpty(_vwPhysician.PhysicianLicenseNumber) ? _vwPhysician.PhysicianLicenseNumber : "");

                    //Get Physician Address from vwPhysician
                    //FillPdfTextField("Prescriber Address", !string.IsNullOrEmpty(_vwPhysician.AddressLine1) ? _vwPhysician.AddressLine1 : "");
                    //FillPdfTextField("Prescriber City State Zip",
                    //    (!string.IsNullOrEmpty(_vwPhysician.City) ? _vwPhysician.City : "")
                    //    + ", " + (!string.IsNullOrEmpty(_vwPhysician.StateProvince) ? _vwPhysician.StateProvince : "")
                    //    + ", " + ((!string.IsNullOrEmpty(_vwPhysician.PostalCode) && _vwPhysician.PostalCode.Length >= 5) ? _vwPhysician.PostalCode.Substring(0, 5) : "")
                    //    );

                    ////mask("(999) 999-9999");

                    //FillPdfTextField("Prescriber Phone", (!string.IsNullOrEmpty(_vwPhysician.PhoneNumber) && _vwPhysician.PhoneNumber.Length >= 10) ? ("(" + _vwPhysician.PhoneNumber.Substring(0, 3) + ") " + _vwPhysician.PhoneNumber.Substring(3, 3) + "-" + _vwPhysician.PhoneNumber.Substring(6, 4)) : "");
                    //FillPdfTextField("Prescriber Fax", (!string.IsNullOrEmpty(_vwPhysician.FaxNumber) && _vwPhysician.FaxNumber.Length >= 10) ? ("(" + _vwPhysician.FaxNumber.Substring(0, 3) + ") " + _vwPhysician.FaxNumber.Substring(3, 3) + "-" + _vwPhysician.FaxNumber.Substring(6, 4)) : "");

                    //Get Physician Address from vwPhysicianAddress
                    FillPdfTextField("Prescriber Address", !string.IsNullOrEmpty(_vwPhysicianAddress.AddressLine1) ? _vwPhysicianAddress.AddressLine1 : "");
                    FillPdfTextField("Prescriber City State Zip",
                        (!string.IsNullOrEmpty(_vwPhysicianAddress.City) ? _vwPhysicianAddress.City : "")
                        + ", " + (!string.IsNullOrEmpty(_vwPhysicianAddress.StateProvince) ? _vwPhysicianAddress.StateProvince : "")
                        + ", " + ((!string.IsNullOrEmpty(_vwPhysicianAddress.PostalCode) && _vwPhysicianAddress.PostalCode.Length >= 5) ? _vwPhysicianAddress.PostalCode.Substring(0, 5) : "")
                        );

                    //FillPdfTextField("Prescriber Phone", !string.IsNullOrEmpty(_vwPhysicianAddress.PhoneNumber) ? _vwPhysicianAddress.PhoneNumber : "");
                    //FillPdfTextField("Prescriber Fax", !string.IsNullOrEmpty(_vwPhysicianAddress.FaxNumber) ? _vwPhysicianAddress.FaxNumber : "");

                    //mask("(999) 999-9999");

                    _vwPhysicianAddress.PhoneNumber = !string.IsNullOrEmpty(_vwPhysicianAddress.PhoneNumber) ? _vwPhysicianAddress.PhoneNumber.Replace("-", "") : "";
                    _vwPhysicianAddress.FaxNumber = !string.IsNullOrEmpty(_vwPhysicianAddress.FaxNumber) ? _vwPhysicianAddress.FaxNumber.Replace("-", "") : "";

                    FillPdfTextField("Prescriber Phone", (!string.IsNullOrEmpty(_vwPhysicianAddress.PhoneNumber) && _vwPhysicianAddress.PhoneNumber.Length >= 10) ? ("(" + _vwPhysicianAddress.PhoneNumber.Substring(0, 3) + ") " + _vwPhysicianAddress.PhoneNumber.Substring(3, 3) + "-" + _vwPhysicianAddress.PhoneNumber.Substring(6, 4)) : "");
                    FillPdfTextField("Prescriber Fax", (!string.IsNullOrEmpty(_vwPhysicianAddress.FaxNumber) && _vwPhysicianAddress.FaxNumber.Length >= 10) ? ("(" + _vwPhysicianAddress.FaxNumber.Substring(0, 3) + ") " + _vwPhysicianAddress.FaxNumber.Substring(3, 3) + "-" + _vwPhysicianAddress.FaxNumber.Substring(6, 4)) : "");

                    //FillPdfTextField("Contact Person", !string.IsNullOrEmpty(_vwPhysician.FaxNumber) ? _vwPhysician.FaxNumber : "");

                    //iTextSharp
                    // Readonly all fields
                    //foreach (var field in pdfReader.AcroFields.Fields)
                    //{
                    //    form.SetFieldProperty(field.Key.ToString(), "setfflags", PdfFormField.FF_READ_ONLY, null);
                    //}

                    pdfStamper.Close();
                    pdfReader.Close();

                    //CommonHelper.SendEmail("Kroger Form", "Please, check out the attachment.", pDFFormModel.Email, fileName);

                    //FaxDocumentModel faxDocumentModel = new FaxDocumentModel
                    //{
                    //    DocumentPath = fileName,
                    //    Email = pDFFormModel.Email,
                    //    FaxNumber = pDFFormModel.FaxNumber
                    //};
                    //return FaxDocument(faxDocumentModel);

                    return Json(new { message = fileName, success = true });

                    //Open PDF in web
                    //byte[] contents = System.IO.File.ReadAllBytes(Server.MapPath("~/Document/" + pDFFormModel.FileName));
                    //Response.AddHeader("Content-Disposition", "inline; filename=test.pdf");
                    //return File(contents, "application/pdf");
                }
                //if (fileName.Length > 0 && System.IO.File.Exists(fileName))
                //{
                //    byte[] contents = System.IO.File.ReadAllBytes(fileName);
                //    Response.AddHeader("Content-Disposition", "inline; filename=test.pdf");
                //    return File(contents, "application/pdf");
                //}
                else
                {
                    return Json(new { message = "PDF Form Not Found.", success = false });
                }
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
                return Json(new { message = "Error in FillPDFForm.", success = false });
            }
        }

        private void FillPdfTextField(string pdfFormFieldName, string strText)
        {
            // iTextSharp
            if (pdfFormFieldName != null)
            {
                form.SetField(pdfFormFieldName, !string.IsNullOrEmpty(strText) ? strText : " ");
            }
        }

        private void FillPdfCheckBox(string pdfFormFieldName, bool? value)
        {
            // iTextSharp
            if (pdfFormFieldName != null && value != null)
            {
                form.SetField(pdfFormFieldName, "Yes");
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetPatientDocumentData(string documentIdEncrypted)
        {
            string tempFile = "";
            try
            {
                int documentId = int.Parse(documentIdEncrypted.Decrypt());
                vwDocument patientDocument = _unitOfWork.vwDocumentRepository.GetAsQuerable(x => x.DocumenstId == documentId).FirstOrDefault();

                if (patientDocument != null)
                {
                    if (!string.IsNullOrEmpty(patientDocument.FolderFileNameUNC))
                    {
                        var index = patientDocument.FolderFileNameUNC.IndexOf("FAXES", StringComparison.OrdinalIgnoreCase) > -1 ? patientDocument.FolderFileNameUNC.IndexOf("FAXES", StringComparison.OrdinalIgnoreCase) :
                            (patientDocument.FolderFileNameUNC.IndexOf("DOCS", StringComparison.OrdinalIgnoreCase) > -1 ? patientDocument.FolderFileNameUNC.IndexOf("DOCS", StringComparison.OrdinalIgnoreCase) : -1);

                        var subPath = patientDocument.FolderFileNameUNC.Substring(index);
                        subPath = subPath.Replace('\\', '/');
                        var documentsUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["PatientDocumentsUrl"].ToString();
                        var filePath = documentsUrl + subPath;
                        tempFile = filePath;
                        if (index > -1)
                        {
                            if (filePath.ToLower().IndexOf(".tif") > 0)
                            {
                                //tif document
                                var documentsServerUsername = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsServerUsername"].ToString();
                                var documentsServerPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsServerPassword"].ToString();
                                var byteArr = ShowImage(filePath, documentsServerUsername, documentsServerPassword);

                                return Json(new { filetype = "tif", fileData = Convert.ToBase64String(byteArr), success = true }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                //pdf document
                                WebClient wc = new WebClient();
                                wc.UseDefaultCredentials = true;
                                wc.Credentials = new NetworkCredential("kon8835", "*Tobasco2014");
                                Stream newtworkStream = wc.OpenRead(filePath);
                                MemoryStream ms = new MemoryStream();

                                newtworkStream.CopyTo(ms);
                                byte[] contents = ms.ToArray();
                                //byte[] contents = System.IO.File.ReadAllBytes(filePath);

                                return Json(new { filetype = "pdf", fileData = Convert.ToBase64String(contents), success = true }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            var fileNotExistPath = SettingManager.GetSettingByName("PhysicianPortalDocumentsBaseFolder").Value + "File_Not_Exists.pdf";
                            byte[] contents = System.IO.File.ReadAllBytes(fileNotExistPath);
                            return Json(new { filetype = "pdf", fileData = Convert.ToBase64String(contents), success = true }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        var fileNotExistPath = SettingManager.GetSettingByName("PhysicianPortalDocumentsBaseFolder").Value + "File_Not_Exists.pdf";
                        byte[] contents = System.IO.File.ReadAllBytes(fileNotExistPath);
                        return Json(new { filetype = "pdf", fileData = Convert.ToBase64String(contents), success = true }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { message = "File not Found", success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //if (exception.GetType() == typeof(FileNotFoundException))
                //{
                //    var fileNotExistPath = SettingManager.GetSettingByName("PhysicianPortalDocumentsBaseFolder").Value + "File_Not_Exists.pdf";
                //    byte[] contents = System.IO.File.ReadAllBytes(fileNotExistPath);
                //    return Json(new { filetype = "pdf", fileData = Convert.ToBase64String(contents), success = true }, JsonRequestBehavior.AllowGet);
                //}
                LogInfo(ex.Message, ex);
                return Json(new { file = tempFile, message = ex.Message, innerMessage = ex.InnerException != null ? ex.InnerException.Message : "", success = false });
            }
        }
    }
}