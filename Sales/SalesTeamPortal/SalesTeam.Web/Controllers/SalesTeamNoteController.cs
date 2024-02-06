using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SalesTeam.Core.Repository;
using SalesTeam.Web.Models;
using SalesTeam.Core.Data;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class SalesTeamNoteController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: SalesTeamNote
        public ActionResult Index()
        {
            DimProcessTime processTime = _unitOfWork.DimProcessTimesRepository.GetSingle();
            ViewBag.LatestNotesTime = processTime?.LatestNotesTime;
            return View();
        }

        public ActionResult vwSalesTeamNotes_Read([DataSourceRequest] DataSourceRequest request,
            vwSalesTeamNote _vwSalesTeamNote = null)
        {
            try
            {

                int patientId = 0, physicianId = 0;

                if (_vwSalesTeamNote != null)
                {
                    patientId = _vwSalesTeamNote.PatientId ?? 0;
                    physicianId = _vwSalesTeamNote.PhysicianId ?? 0;
                }

                IQueryable vwSalesTeamNoteList;
                // = _unitOfWork.vwSalesTeamNotesRepository.GetAsQuerable(t => t.CreatedSalesTeamId == MasterSalesTeamId);
                if (patientId != 0)
                    vwSalesTeamNoteList =
                        _unitOfWork.vwSalesTeamNotesRepository.GetAsQuerable(
                            t => (t.CreatedSalesTeamId == MasterSalesTeamId) && (t.PatientId == patientId));
                else if (physicianId != 0)
                    vwSalesTeamNoteList =
                        _unitOfWork.vwSalesTeamNotesRepository.GetAsQuerable(
                            t => (t.CreatedSalesTeamId == MasterSalesTeamId) && (t.PhysicianId == physicianId));
                else
                    vwSalesTeamNoteList =
                        _unitOfWork.vwSalesTeamNotesRepository.GetAsQuerable(t => t.CreatedSalesTeamId == MasterSalesTeamId);


                //Decoding Special Characters back to normal form

                var filteredList = vwSalesTeamNoteList.ToDataSourceResult(request);

                foreach (vwSalesTeamNote item in filteredList.Data)
                {
                    item.SalesTeamNote = decodeHTML(item.SalesTeamNote);
                }

                //foreach (var note in vwSalesTeamNoteList.Cast<vwSalesTeamNote>().ToList())
                //    note.SalesTeamNote = decodeHTML(note.SalesTeamNote);
                //var vwSalesTeamNoteListReturn = vwSalesTeamNoteList;

                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult vwPharmacyNotes_Read([DataSourceRequest] DataSourceRequest request,
            vwPharmacyNote _vwPharmacyNote = null)
        {
            var patientId = 0;

            if (_vwPharmacyNote != null)
                patientId = _vwPharmacyNote.PatientId ?? 0;
            var vwPharmacyNotesList = _unitOfWork.vwPharmacyNotesRepository.GetAsQuerable(t => t.PatientId == patientId);
            return Json(vwPharmacyNotesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPharmacyNotesInfo(int PharmacyNoteId)
        {
            try
            {
                var pharmacyNote =
                    _unitOfWork.vwPharmacyNotesRepository.GetAsQuerable(t => t.PharmacyNoteId == PharmacyNoteId).FirstOrDefault();

                return PartialView("_PharmacyNotesPopup", pharmacyNote);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
                return PartialView("Error", ex.Message);
            }
        }

        public ActionResult ShowNotes(int? SalesTeamId, int? PhysicianId, int? PatientId)
        {
            SalesTeamId = SalesTeamId ?? 0;
            PhysicianId = PhysicianId ?? 0;
            PatientId = PatientId ?? 0;
            try
            {
                return PartialView("_SalesTeamNotesPopup",
                    new PatientIdPhysicianIdModel { SalesTeamId = SalesTeamId, PhysicianId = PhysicianId, PatientId = PatientId });
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
                return PartialView("Error", ex.Message);
            }
        }

        public JsonResult CreateNote(SalesTeamNote salesTeamNote)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (salesTeamNote != null && salesTeamNote.SalesTeamId != null && salesTeamNote.PhysicianId != null)
                    {
                        salesTeamNote.SalesTeamNote1 = salesTeamNote.SalesTeamNote1;
                        salesTeamNote.CreateDate = DateTime.Now;
                        salesTeamNote.UpdateDate = DateTime.Now;
                        salesTeamNote.SalesTeamId = salesTeamNote.SalesTeamId;
                        salesTeamNote.PhysicianId = salesTeamNote.PhysicianId;
                        salesTeamNote.PatientId = salesTeamNote.PatientId;
                        salesTeamNote.CreatedSalesTeamId = MasterSalesTeamId;

                        _unitOfWork.SalesTeamNotesRepository.Insert(salesTeamNote);
                        _unitOfWork.Save();
                    }
                }
                return Json(new { message = "Notes Added Successfully.", success = true });
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
                return Json(new { message = ex.Message, success = false });
            }
        }
        public JsonResult DeleteNotes(SalesTeamNote model)
        {
            try
            {
                var _salesTeamNote =
                    _unitOfWork.SalesTeamNotesRepository.GetAsQuerable(t => t.SalesTeamNoteId == model.SalesTeamNoteId)
                        .FirstOrDefault();
                if (_salesTeamNote != null)
                {
                    _unitOfWork.SalesTeamNotesRepository.Delete(_salesTeamNote);
                    _unitOfWork.Save();
                    return Json(new { message = "Sales Team Notes Deleted Successfully.", success = true });
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

        public string decodeHTML(string text)
        {
            if ((text != "") && (text != "null") && (text != null))
            {
                text = text.Replace("&amp;", "&");
                text = text.Replace("&lt;", "<");
                text = text.Replace("&gt;", ">");
                text = text.Replace("&quot;", "\"");
                text = text.Replace("&#34;", '"'.ToString());
                text = text.Replace("&#39;", "'");
                text = text.Replace("&#44;", ",");
                text = text.Replace("&#58;", ":");
                return text;
            }
            return "";
        }
    }
}