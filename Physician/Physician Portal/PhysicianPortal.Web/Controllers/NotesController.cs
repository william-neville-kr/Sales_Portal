using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Repository;
using PhysicianPortal.Core.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhysicianPortal.Web.Models;
using PhysicianPortal.Web.Attributes;

namespace PhysicianPortal.Web.Controllers
{
    [CustomAuthorize]
    public class NotesController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();

        //PhysicianNotes
        public ActionResult GetMyNotes([DataSourceRequest] DataSourceRequest request, vwPhysicianNote _PhysicianNote = null)
        {
            try
            {
                IQueryable<vwPhysicianNote> noteList = Enumerable.Empty<vwPhysicianNote>().AsQueryable();

                noteList = _unitOfWork.vwNotesRepository.GetAsQuerable().OrderByDescending(t => t.CreateDate); ;

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    //When office admin log in.
                    var userOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice");
                    if (userOffice != null)
                    {
                        int currentOffice = userOffice.Value.ToInt0();
                        
                        var userIds = _unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                        var physicianIds = _unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).Distinct().ToList();
                        noteList = noteList.Where(t => physicianIds.Contains(t.PhysicianId.Value));
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    //Technicians and Physicians can only view the patients of the physcians who are registered against them
                    var physicianIds = _unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId).Distinct().ToList();
                    noteList = noteList.Where(t => physicianIds.Contains(t.PhysicianId.Value));
                }

                //Decoding Special Characters back to normal form
                foreach (vwPhysicianNote note in noteList.Cast<vwPhysicianNote>().ToList())
                {
                    note.PhysicianNote = DecodeHTML(note.PhysicianNote);
                    note.PatientIdEncrypted = SecurityHelper.Encrypt(note.PatientId.ToString());
                    note.PhysicianIdEncrypted = SecurityHelper.Encrypt(note.PhysicianId.ToString());
                    
                }
                return Json(noteList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetNotes NotesController. " + exception.Message, exception);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        // GET: Notes
        public ActionResult Index()
        {
            // To Do
            //vwPatient _vwPatient = _unitOfWork.PatientsRepository.GetAsQuerable(t => t.PatientId == 836).FirstOrDefault();
            //return View(_vwPatient);
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method Index() NotesController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "NotesController", "Index", ErrorTitle, ErrorMessage, null));
            }
        }


        // GET: Notes
        public ActionResult NotesIndex()
        {
            // To Do
            //vwPatient _vwPatient = _unitOfWork.PatientsRepository.GetAsQuerable(t => t.PatientId == 836).FirstOrDefault();
            //return View(_vwPatient);
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method NotesIndex() NotesController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "NotesController", "NotesIndex", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult GetPharmacyNotes([DataSourceRequest] DataSourceRequest request, vwPharmacyNote _vwPharmacyNote = null)
        {
            try
            {
                string patientId = null;
                int PatientId = 0;

                if (_vwPharmacyNote != null)
                {
                    patientId = _vwPharmacyNote.PatientIdEncrypted;
                    if (patientId != null)
                    {
                        PatientId = int.Parse(SecurityHelper.Decrypt(patientId));
                    }
                }

                IQueryable vwPharmacyNotesList = _unitOfWork.PharmacyNotesRepository.GetAsQuerable(t => t.PatientId == PatientId);
                return Json(vwPharmacyNotesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPharmacyNotes NotesController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //PhysicianNotes
        public ActionResult GetNotes([DataSourceRequest] DataSourceRequest request, vwPhysicianNote _PhysicianNote = null)
        {
            try
            {
                if (_PhysicianNote != null)
                {
                    string physicianId = null, patientId = null;
                    int PhysicianId = 0, PatientId = 0;
                    IQueryable NoteList;

                    physicianId = _PhysicianNote.PhysicianIdEncrypted;
                    patientId = _PhysicianNote.PatientIdEncrypted;

                    if (physicianId != null)
                    {
                        PhysicianId = int.Parse(SecurityHelper.Decrypt(physicianId));
                    }
                    if (patientId != null)
                    {
                        PatientId = int.Parse(SecurityHelper.Decrypt(patientId));
                    }
                    if (PhysicianId != 0)
                    {
                        NoteList = _unitOfWork.vwNotesRepository.GetAsQuerable(t => t.PhysicianId == PhysicianId && t.PatientId == PatientId);

                        //Decoding Special Characters back to normal form
                        foreach (vwPhysicianNote note in NoteList.Cast<vwPhysicianNote>().ToList())
                        {
                            note.PhysicianNote = DecodeHTML(note.PhysicianNote);
                        }
                        return Json(NoteList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetNotes NotesController. " + exception.Message, exception);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PatientSearch(string PatientId)
        {
            try
            {
                int patientid = string.IsNullOrEmpty(PatientId) ? 0 : int.Parse(SecurityHelper.Decrypt(PatientId));
                vwPatient patient = _unitOfWork.PatientsRepository.GetAsQuerable(t => (t.PatientId == patientid)).FirstOrDefault();

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
                Logger.InsertLog("Exception thrown in Method PatientSearch NotesController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "NotesController", "PatientSearch", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        public JsonResult CreateNote(PhysicianNote physicianNote, string npi)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { message = "Unexpected parameter values", success = false });
                }
                physicianNote.PhysicianId = physicianNote.PhysicianIdEncrypted != null ? int.Parse(SecurityHelper.Decrypt(physicianNote.PhysicianIdEncrypted)) : (int?)null;
                physicianNote.PatientId = physicianNote.PatientIdEncrypted != null ? int.Parse(SecurityHelper.Decrypt(physicianNote.PatientIdEncrypted)) : (int?)null;


                if (ModelState.IsValid && physicianNote != null && physicianNote.PhysicianId != null && physicianNote.PatientId != null)
                {
                    physicianNote.PhysicianNote1 = physicianNote.PhysicianNote1;
                    physicianNote.CreateDate = DateTime.UtcNow;
                    physicianNote.UpdateDate = DateTime.UtcNow;
                    physicianNote.PhysicianId = physicianNote.PhysicianId;
                    physicianNote.PatientId = physicianNote.PatientId;

                    _unitOfWork.NotesRepository.Insert(physicianNote);
                    _unitOfWork.Save();
                }
                else
                {
                    Logger.InsertLog("Exception thrown in Method CreateNote NotesController. Id was null", "");
                    return Json(new { message = "Failed to add note.", success = true });
                }


                AuditLogger.AuditTracker("New Note Created.", "A new note has been created for PatientId: " + physicianNote.PatientId + ", and PhysicianId: " + physicianNote.PhysicianId, (int)AuditOperationType.Addition);
                return Json(new { message = "New Note Added Successfully.", success = true });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method CreateNote NotesController. " + exception.Message, exception);
                return Json(new { message = exception.Message, success = false });
            }
        }

        public JsonResult DeleteNotes(PhysicianNote model)
        {
            try
            {
                PhysicianNote _physicianNote = _unitOfWork.NotesRepository.GetAsQuerable(t => t.PhysicianNoteId == model.PhysicianNoteId).FirstOrDefault();

                if (_physicianNote != null)
                {
                    _unitOfWork.NotesRepository.Delete(_physicianNote);
                    _unitOfWork.Save();
                    AuditLogger.AuditTracker("Note Deleted.", "Note wih Id: " + _physicianNote.PhysicianNoteId + " was deleted.", (int)AuditOperationType.Deletion);
                    return Json(new { message = "Notes Deleted Successfully.", success = true });
                }
                return Json(new { message = "Record not found.", success = false });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method DeleteNotes() NotesController. " + exception.Message, exception);
                return Json(new { message = exception.Message, success = false });
            }
        }

        private string DecodeHTML(string text)
        {
            try
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
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method DecodeHTML NotesController. " + exception.Message, exception);
            }
            return "";
        }

        public JsonResult RefillNotesSave(RefillNotesViewModel refillNotesViewModel)
        {
            try
            {
                string str = "";
                if (ModelState.IsValid && refillNotesViewModel != null && refillNotesViewModel.PhysicianNPI != null && refillNotesViewModel.PatientMRN != null)
                {

                    RefillFaxReportLog refillFaxReportLog = _unitOfWork.RefillFaxReportLogRepository.GetSingle(t => t.RefillFaxReportLogId == refillNotesViewModel.RefillFaxReportLogId);
                    if (refillFaxReportLog != null)
                    {
                        //RefillStatu refillStatus = _unitOfWork.RefillStatusRepository.GetSingle(t => t.Name.Contains("Deny"));
                        refillFaxReportLog.RefillStatusId = refillNotesViewModel.RefillStatusId;
                        refillFaxReportLog.IsSent = true;
                        refillFaxReportLog.SentOn = DateTime.UtcNow;
                        refillFaxReportLog.SentBy = CurrentUser.UserId;
                        refillFaxReportLog.StatusChanged = true;
                        if (refillNotesViewModel.RefillStatusId == 1)
                        {
                            //Denied
                            refillFaxReportLog.SentStatus = 1180;
                        }
                        else if (refillNotesViewModel.RefillStatusId == 3)
                        {
                            //Hold
                            refillFaxReportLog.SentStatus = 1179;
                        }
                        _unitOfWork.RefillFaxReportLogRepository.Update(refillFaxReportLog);
                        //_unitOfWork.Save();
                    }
                    if (refillNotesViewModel.RefillStatusId == 1)
                    {
                        str = "This Refill is denied successfully";
                    }
                    else if (refillNotesViewModel.RefillStatusId == 3)
                    {
                        str = "This Refill is holds successfully";
                    }

                    int PatientId = 0, PhysicianId = 0;
                    var patient = _unitOfWork.PatientsRepository.GetSingle(t => t.PatientCode.Equals(refillNotesViewModel.PatientMRN.ToString()));
                    if (patient != null)
                    {
                        PatientId = patient.PatientId;
                    }
                    var physician = _unitOfWork.PhysiciansRepository.GetSingle(t => t.NationalProviderIdentifier.Equals(refillNotesViewModel.PhysicianNPI));
                    if (physician != null)
                    {
                        PhysicianId = physician.PhysicianId;
                    }
                    PhysicianNote physicianNote = new PhysicianNote();

                    physicianNote.PhysicianNote1 = refillNotesViewModel.Note;
                    physicianNote.CreateDate = DateTime.Now;
                    physicianNote.UpdateDate = DateTime.Now;
                    physicianNote.PhysicianId = PhysicianId;
                    physicianNote.PatientId = PatientId;

                    _unitOfWork.NotesRepository.Insert(physicianNote);
                    _unitOfWork.Save();
                    AuditLogger.AuditTracker("New Note Created.", "A new note has been created for PatientId: " + PatientId + ", and PhysicianId: " + PhysicianId, (int)AuditOperationType.Addition);
                }
                return Json(new { message = str, success = true });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method RefillNotesSave NotesController. " + exception.Message, exception);
                return Json(new { message = "Failed to denied/hold Refill", success = false });
            }
        }

        public ActionResult PatientNotes(vwPhysicianNote _vwPhysicianNote)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_vwPhysicianNote?.PatientCode))
                {
                    var patient = _unitOfWork.PatientsRepository.GetSingle(t => t.PatientCode.Equals(_vwPhysicianNote.PatientCode));
                    _vwPhysicianNote.PatientId = patient?.PatientId ?? null;
                    _vwPhysicianNote.PhysicianId = patient?.PhysicianId ?? null;
                }

                _vwPhysicianNote.PatientId = _vwPhysicianNote.PatientIdEncrypted != null ? int.Parse(SecurityHelper.Decrypt(_vwPhysicianNote.PatientIdEncrypted)) : 0;
                _vwPhysicianNote.PhysicianId = _vwPhysicianNote.PhysicianIdEncrypted != null ? int.Parse(SecurityHelper.Decrypt(_vwPhysicianNote.PhysicianIdEncrypted)) : 0;

                return PartialView("_PhysicianNotes", _vwPhysicianNote);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method PatientNotes NotesController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "NotesController", "PatientNotes", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }
    }
}