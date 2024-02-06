using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using PhysicianPortal.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Web.Models;
using PhysicianPortal.Core.Helpers;
using Microsoft.AspNet.SignalR;
using System.Data.Entity;
using Microsoft.AnalysisServices.AdomdClient;
using System.Text;
using System.Data;
using System.Collections;
using System.Security.Claims;
using TuesPechkin;
using System.Drawing.Printing;
using SelectPdf;
using PhysicianPortal.Core.Helper;
using System.IO;
using System.Text.RegularExpressions;
using PhysicianPortal.Web.Attributes;

namespace PhysicianPortal.Web.Controllers
{
    [CustomAuthorize]
    public class HomeController : BaseController
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index(PatientId_PhysicianIdViewModel patientId_PhysicianIdViewModel = null)
        {
            try
            {
                var currentUser = CurrentUser;
                ViewBag.userRole = currentUser.RoleId;
                List<int> physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == currentUser.UserId).Select(x => x.PhysicianId).ToList();

                //List<string> physicianIdsEncrypted = new List<string>();
                //foreach (int id in physicianIds)
                //{
                //    physicianIdsEncrypted.Add(SecurityHelper.Encrypt(id.ToString()));
                //}

                ViewBag.physicianIds = physicianIds;
                //ViewBag.physicianId = currentUser.PhysicianId == null ? -1 : currentUser.PhysicianId;
                ViewBag.currentUserId = currentUser.UserId;

                if (patientId_PhysicianIdViewModel != null && patientId_PhysicianIdViewModel.PhysicianIdEncrypted != null && patientId_PhysicianIdViewModel.PhysicianIdEncrypted != null)
                {
                    return View(new { PatientId = patientId_PhysicianIdViewModel.PatientIdEncrypted, PhysicianId = patientId_PhysicianIdViewModel.PhysicianIdEncrypted });
                }
                return View();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method Index() HomeController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "HomeController", "Index", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin,Admin,Physician,Technician")]
        public ActionResult HomeCompact()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method HomeCompact() HomeController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "HomeController", "HomeCompact", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin,Admin,Physician,Technician")]
        public ActionResult HomeCompact2()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method HomeCompact2() HomeController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "HomeController", "HomeCompact2", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin,Admin,Physician,Technician")]
        public ActionResult HomeDetail()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method HomeDetail() HomeController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "HomeController", "HomeDetail", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin,Pharmacist")]
        public ActionResult HomePharmacist()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method HomePharmacist() HomeController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "HomeController", "HomePharmacist", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult Refills()
        {
            try
            {
                //ViewBag.CurrentUser = CurrentUser;
                var userPhysicianRelationship = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(t => t.UserId == CurrentUser.UserId);
                var physicianSignature = userPhysicianRelationship != null ?
                    unitOfWork.PhysicianSignatureRepository.GetSingle(t => t.PhysicianMasterId == userPhysicianRelationship.PhysicianId) : null;

                if (physicianSignature?.Signature != null)
                {
                    ViewBag.IsHaveSignature = true;
                }
                else
                {
                    ViewBag.IsHaveSignature = false;
                }
                return View();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method Refills() HomeController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "HomeController", "Refills", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult MessagesThread(int threadID)
        {
            try
            {
                Message parentMessage = unitOfWork.MessagesRepository.GetSingle(x => x.MessageId == threadID);
                MessageThreadViewModel threadModel = new MessageThreadViewModel()
                {
                    ThredId = threadID,
                    PatientId = parentMessage.PatientId,
                    ReplyMessageBody = "",
                    IsCompleted = parentMessage.IsCompleted
                };
                ViewBag.currentUser = CurrentUser;
                //The message thread was opened so we change the 'IsRead' to true
                //Only set the 'IsRead' to true if the message was opened by a Physician or a Technician
                if (UserManager.IsInRole(CurrentUser.Id, "Physician") || UserManager.IsInRole(CurrentUser.Id, "Technician"))
                {
                    parentMessage.IsRead = true;
                    unitOfWork.MessagesRepository.Update(parentMessage);
                    unitOfWork.Save();
                }

                return View(threadModel);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method MessagesThread() HomeController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "HomeController", "MessagesThread", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        public ActionResult GetMessages([DataSourceRequest] DataSourceRequest request, string PatientIdEncrypted = null)
        {
            //var currentUser = CurrentUser;
            try
            {

                List<vwLastThreadMessage> messagesList;
                if (PatientIdEncrypted != null)
                {
                    int patientId = int.Parse(SecurityHelper.Decrypt(PatientIdEncrypted));
                    if (patientId < 0)
                    {
                        //Unexpected behavior, just return null

                        return null;
                    }
                    // Get messages for specific Patient
                    messagesList = unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(t => t.PatientId == patientId).ToList();
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Pharmacist").Result)
                {
                    //If admin or pharmacistis logged in, get all the messages
                    messagesList = unitOfWork.vwLastThreadMessageRepository.GetAsQuerable().ToList();
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    //If admin logged in, get all the messages of admin office
                    //To Do need some null checks here.
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    messagesList = unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId ?? 0)).ToList();
                }
                else
                {
                    //If physician or technician is logged in, only get his messages
                    List<int> physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId).ToList();
                    messagesList = unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId ?? 0)).ToList();
                }

                //var filteredList = messagesList.ToDataSourceResult(request);

                List<GridMessageViewModel> gridMessagesList = new List<GridMessageViewModel>();

                foreach (vwLastThreadMessage message in messagesList)
                {
                    gridMessagesList.Add(new GridMessageViewModel
                    {
                        MessageId = message.MessageId,
                        PhysicianId = message.PhysicianId,
                        PatientId = message.PatientId,
                        MessageBody = message.LastMessage.Length < 100 ? message.LastMessage : message.LastMessage.Substring(0, 100) + "...",
                        PreMessageId = message.ThreadId,
                        PatientName = message.PatientName,
                        PhysicianName = message.PhysicianName,
                        CreatedOn = message.CreatedOn.Value.ToLocalTime().ToString("M/d/yyyy, hh:mm:ss tt"),
                        IsRead = message.IsRead,
                        ThreadMessageCount = message.ThreadCount.Value,
                        IsCompleted = message.IsCompleted,
                        PhysicianIdEncrypted = SecurityHelper.Encrypt(message.PhysicianId.ToString()),
                        PatientIdEncrypted = SecurityHelper.Encrypt(message.PatientId.ToString())
                    });
                }
                return Json(gridMessagesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.InsertLog("Exception thrown in Method GetMessages() HomeController. " + e.Message, e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRefill(RefillViewModel model)
        {
            try
            {
                var vwRefillFaxReportLog = unitOfWork.vwRefillFaxReportLogsRepository.GetSingle(t => t.id == model.Id);

                if (vwRefillFaxReportLog != null)
                {
                    RefillViewModel refillViewModel = new RefillViewModel();
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(vwRefillFaxReportLog.HTMLText);
                    var htmlNodes = doc.DocumentNode.SelectNodes("//table[@id='tblPrescription']/tr/td/input");
                    //This xpath query find all "input" elements from item.HTMLText where table id is "tblPrescription"

                    refillViewModel.newRefillViewModelList = new List<NewRefillViewModel>();
                    for (var i = 0; i / 3 < vwRefillFaxReportLog.RXNumCount;)
                    {
                        var htmlNodes_Qty = htmlNodes[i++];
                        var htmlNodes_AdditionalRefill = htmlNodes[i++];
                        var htmlNodes_DoNotSub = htmlNodes[i++];
                        NewRefillViewModel newRefillViewModel = new NewRefillViewModel();

                        if (htmlNodes_Qty != null)
                        {
                            newRefillViewModel.Qty = htmlNodes_Qty.Attributes["value"].Value;
                        }
                        if (htmlNodes_AdditionalRefill != null)
                        {
                            newRefillViewModel.AdditionalRefills = htmlNodes_AdditionalRefill.Attributes["value"].Value;
                        }
                        if (htmlNodes_DoNotSub != null)
                        {
                            newRefillViewModel.DoNotSubstitute = htmlNodes_DoNotSub.Attributes["value"].Value;
                        }
                        refillViewModel.newRefillViewModelList.Add(newRefillViewModel);
                    }
                    //If data found in our ph.[NewRefill] table then get this one
                    var newRefillList = unitOfWork.NewRefillRepository.GetAsQuerable(t => t.RefillFaxReportLogId == vwRefillFaxReportLog.id).ToList();

                    if (newRefillList != null && newRefillList.Count() > 0)
                    {
                        for (int i = 0; i < newRefillList.Count(); i++)
                        {
                            refillViewModel.newRefillViewModelList[i].Qty = newRefillList[i].Qty;
                            refillViewModel.newRefillViewModelList[i].AdditionalRefills = newRefillList[i].AdditionalRefills;
                            refillViewModel.newRefillViewModelList[i].DoNotSubstitute = newRefillList[i].DoNotSubstitute;
                        }
                    }

                    //refillViewModel.Drug = vwRefillFaxReportLog.Drug;
                    refillViewModel.FullName = vwRefillFaxReportLog.FullName;
                    refillViewModel.HTMLText = vwRefillFaxReportLog.HTMLText;
                    refillViewModel.NationalProviderIdentifier = vwRefillFaxReportLog.NationalProviderIdentifier;
                    //refillViewModel.PatientName = vwRefillFaxReportLog.PatientName;
                    //refillViewModel.RXNum = vwRefillFaxReportLog.RXNum;
                    refillViewModel.RXNumCount = vwRefillFaxReportLog.RXNumCount;
                    refillViewModel.Id = vwRefillFaxReportLog.id;
                    refillViewModel.PharmacyFaxNumber = vwRefillFaxReportLog.PharmacyFaxNumber;

                    return Json(new { message = refillViewModel, success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetRefill() HomeController. " + exception.Message, exception);
            }
            return Json(new { message = "Exception thrown in Method GetRefill() HomeController. ", success = false });
        }

        public ActionResult GetRefills([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IQueryable<RefillViewModel> RefillFaxReportLogList = unitOfWork.vwRefillFaxReportLogsRepository
                    .GetAsQuerable(t => t.IsSent != true)
                    .Select(t => new RefillViewModel
                    {
                        PatientName = t.PatientName,
                        FullName = t.FullName,
                        NationalProviderIdentifier = t.NationalProviderIdentifier,
                        RXNum = t.RXNum,
                        Drug = t.Drug,
                        RXNumCount = t.RXNumCount,
                        HTMLText = t.HTMLText,
                        Id = t.id,
                        PatientId = t.PatientId,
                        RefillDocumentPath = t.RefillDocumentPath,
                        NotesCount = t.NotesCount
                    });

                if (UserManager.IsInRole(CurrentUser.Id, "Admin"))
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId)).Select(s => s.NationalProviderIdentifier);
                    RefillFaxReportLogList = RefillFaxReportLogList.Where(t => physicianNPIs.Contains(t.NationalProviderIdentifier));
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId)).Select(s => s.NationalProviderIdentifier);
                    RefillFaxReportLogList = RefillFaxReportLogList.Where(t => physicianNPIs.Contains(t.NationalProviderIdentifier));
                }

                return Json(RefillFaxReportLogList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method GetRefills() HomeController. " + e.Message, e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetRefillsDocuments([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var documentsList = unitOfWork.vwRefillFaxDocumentRepository.GetAsQuerable().Select(x => new RefillDocumentModel
                {
                    SentOn = x.SentOn,
                    //DocumentDescription = x.DocumentDescription,
                    DocumentId = x.id,
                    DocumentName = x.RefillDocumentName,
                    NationalProviderIdentifier = x.NPI,
                    PatientFullName = x.PatientName,
                    PhysicianFullName = x.PhysicianFullName,
                    PhysicianId = x.PhysicianId
                    //SubmittedDate = x.,
                    //PhysicianPortalPatientId = x.PhysicianPortalPatientId.Value
                });

                var filteredDocuments = documentsList.ToDataSourceResult(request);

                foreach (RefillDocumentModel document in filteredDocuments.Data)
                {
                    document.DocumentIdEncrypted = SecurityHelper.Encrypt(document.DocumentId.ToString());
                    document.DocumentId = -1;
                    document.PhysicianIdEncrypted = SecurityHelper.Encrypt(document.PhysicianId.ToString());
                    document.PhysicianId = -1;
                }
                return Json(filteredDocuments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method GetRefillsDocuments() HomeController. " + e.Message, e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetSentRefills([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IQueryable<RefillViewModel> RefillFaxReportLogList = unitOfWork.vwRefillFaxReportLogsRepository
                    .GetAsQuerable(t => t.IsSent == true)
                    .Select(t => new RefillViewModel
                    {
                        PatientName = t.PatientName,
                        FullName = t.FullName,
                        NationalProviderIdentifier = t.NationalProviderIdentifier,
                        RXNum = t.RXNum,
                        Drug = t.Drug,
                        RXNumCount = t.RXNumCount,
                        HTMLText = t.HTMLText,
                        Id = t.id,
                        RefillStatus = t.RefillStatus,
                        SentBy = t.SentBy,
                        SentOn = t.SentOn,
                        PatientId = t.PatientId,
                        NotesCount = t.NotesCount
                    });

                if (UserManager.IsInRole(CurrentUser.Id, "Admin"))
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId)).Select(s => s.NationalProviderIdentifier);
                    RefillFaxReportLogList = RefillFaxReportLogList.Where(t => physicianNPIs.Contains(t.NationalProviderIdentifier));
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId)).Select(s => s.NationalProviderIdentifier);
                    RefillFaxReportLogList = RefillFaxReportLogList.Where(t => physicianNPIs.Contains(t.NationalProviderIdentifier));
                }
                return Json(RefillFaxReportLogList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.InsertLog("Exception thrown in Method GetSentRefills() HomeController. " + e.Message, e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetHoldRefills([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IQueryable<RefillViewModel> RefillFaxReportLogList = unitOfWork.vwRefillFaxReportLogsRepository
                    .GetAsQuerable(t => t.IsSent == true && t.RefillStatus.Equals("Hold"))
                    .Select(t => new RefillViewModel
                    {
                        PatientName = t.PatientName,
                        FullName = t.FullName,
                        NationalProviderIdentifier = t.NationalProviderIdentifier,
                        RXNum = t.RXNum,
                        Drug = t.Drug,
                        RXNumCount = t.RXNumCount,
                        HTMLText = t.HTMLText,
                        Id = t.id,
                        RefillStatus = t.RefillStatus,
                        SentBy = t.SentBy,
                        SentOn = t.SentOn,
                        PatientId = t.PatientId,
                        NotesCount = t.NotesCount
                    });

                if (UserManager.IsInRole(CurrentUser.Id, "Admin"))
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId);
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId)).Select(s => s.NationalProviderIdentifier);
                    RefillFaxReportLogList = RefillFaxReportLogList.Where(t => physicianNPIs.Contains(t.NationalProviderIdentifier));
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId)).Select(s => s.NationalProviderIdentifier);
                    RefillFaxReportLogList = RefillFaxReportLogList.Where(t => physicianNPIs.Contains(t.NationalProviderIdentifier));
                }
                return Json(RefillFaxReportLogList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method GetHoldRefills() HomeController. " + e.Message, e);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RefillDetailsView(RefillViewModel model)
        {
            try
            {
                return PartialView("RefillDetailsView", model);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method RefillDetailsView() HomeController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "HomeController", "RefillDetailsView", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        public ActionResult NewMessage(PatientId_PhysicianIdViewModel patientId_PhysicianIdViewModel = null)
        {
            try
            {
                var currentUser = CurrentUser;
                //var currentPhysician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == currentUser.PhysicianId);
                var newMessage = new AddMessageViewModel();

                if (UserManager.IsInRole(currentUser.Id, "Physician"))
                {
                    int physicianId = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(x => x.UserId == currentUser.UserId).PhysicianId;
                    newMessage.PhysicianId = physicianId;
                    newMessage.PhysicianName = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == physicianId).FullName;
                }
                if (patientId_PhysicianIdViewModel != null)
                {
                    newMessage.PatientIdEncrypted = patientId_PhysicianIdViewModel.PatientIdEncrypted;
                    //newMessage.PhysicianId = patientId_PhysicianIdViewModel.PhysicianId;
                    //What can we do with PhysicianId? Where we get PhysicianId from login user or from vwPhysician?
                }

                return View(newMessage);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method NewMessage() HomeController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "HomeController", "NewMessage", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult NewMessage(AddMessageViewModel model)
        {
            try
            {
                if (model.PhysicianIdEncrypted == null || model.PatientIdEncrypted == null)
                {
                    return Json(new { success = false, message = "Failed to add message" });
                }

                Core.Data.Message newMessage = new Core.Data.Message()
                {
                    PatientId = int.Parse(SecurityHelper.Decrypt(model.PatientIdEncrypted)),
                    PhysicianId = int.Parse(SecurityHelper.Decrypt(model.PhysicianIdEncrypted)),
                    MessageBody = model.MessageBody,
                    SentBy = CurrentUser.RoleId,
                    CreatedBy = CurrentUser.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                //If Physician or Technician creates new message, set 'IsRead' to true
                if (UserManager.IsInRole(CurrentUser.Id, "Physician") || UserManager.IsInRole(CurrentUser.Id, "Technician"))
                {
                    newMessage.IsRead = true;
                }
                //else set 'IsRead' to true
                else
                {
                    newMessage.IsRead = false;
                }

                unitOfWork.MessagesRepository.Insert(newMessage);
                unitOfWork.Save();


                //Notify all clients of new message using signalR
                var context = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                context.Clients.All.BroadcastMessage(model.PhysicianId);

                AuditLogger.AuditTracker("New Message Created.", "A new message has been created. ", (int)AuditOperationType.Addition);

                return Json(new { success = true, message = "Message sent" });
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method NewMessage(AddMessageViewModel model) HomeController. " + ex.Message, ex);
                return Json(new { success = false, message = "Failed to send message, please try again" });
            }
        }

        public JsonResult ReplyToMessage(int threadID, string replyBody)
        {
            Message parentMessage = unitOfWork.MessagesRepository.GetSingle(x => x.MessageId == threadID);
            try
            {
                var creationTime = DateTime.UtcNow;
                Message newMessage = new Message()
                {
                    PatientId = parentMessage.PatientId,
                    PhysicianId = parentMessage.PhysicianId,
                    MessageBody = replyBody,
                    SentBy = CurrentUser.RoleId,
                    CreatedBy = CurrentUser.UserId,
                    CreatedOn = creationTime,
                    PreMessageId = threadID,
                    IsRead = false
                };
                //Setting the IsRead of the parent message to false so that the thread becomes 'Unread' after a reply has been submitted
                //Only set the 'IsRead' to false if the reply was submitted by an Admin or a Pharmacist
                if (UserManager.IsInRole(CurrentUser.Id, "SuperAdmin") || UserManager.IsInRole(CurrentUser.Id, "Admin") || UserManager.IsInRole(CurrentUser.Id, "Pharmacist"))
                {
                    parentMessage.IsRead = false;
                    unitOfWork.MessagesRepository.Update(parentMessage);
                }
                unitOfWork.MessagesRepository.Insert(newMessage);
                unitOfWork.Save();

                //Notify all clients of new reply using signalR
                var context = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                string senderName = CurrentUser.FirstName;
                senderName += CurrentUser.LastName != null ? " " : "";
                senderName += CurrentUser.LastName != null ? CurrentUser.LastName : "";
                context.Clients.All.SendReplyNotification(CurrentUser.Id, parentMessage.PhysicianId, replyBody, creationTime.ToLocalTime().ToString("M/d/yyyy, hh:mm:ss tt"), senderName);

                AuditLogger.AuditTracker("New Reply To Message Created.", "A new message has been created. PreMessageId: " + threadID, (int)AuditOperationType.Addition);

                return Json(new { success = true, message = "Reply sent", creationTime = creationTime.ToLocalTime().ToString("M/d/yyyy, hh:mm:ss tt") });
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method ReplyToMessage HomeController. " + ex.Message, ex);
                return Json(new { success = false, message = "Failed to send reply, please try again " });
            }
        }

        public ActionResult GetPhysicians([DataSourceRequest] DataSourceRequest request, string selectedPatientId, string searchText)
        {
            try
            {
                List<vwPhysician> physiciansList = null;

                if (UserManager.IsInRole(CurrentUser.Id, "Technician"))
                {
                    //Technician can only select physicians that are registered against him in the UserPhysicianRelationship table

                    List<int> physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId).ToList();
                    physiciansList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId) && t.FullName.Contains(searchText)).Take(AutoCompleteListSize).ToList();
                }
                else
                {
                    //Show all Physicians for Admin and SuperAdmin and Pharmacist
                    physiciansList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => t.FullName.Contains(searchText)).Take(AutoCompleteListSize).ToList();
                }

                foreach (var physicain in physiciansList)
                {
                    physicain.PhysicianIdEncrypted = SecurityHelper.Encrypt(physicain.PhysicianId.ToString());
                }

                return Json(physiciansList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPhysicians HomeController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPhysicianWithId(string physicianId)
        {
            try
            {
                int PhysicianId = 0;
                if (physicianId != null)
                {
                    PhysicianId = int.Parse(SecurityHelper.Decrypt(physicianId));
                }
                vwPhysician selectedPhysician = unitOfWork.PhysiciansRepository.GetSingle(t => t.PhysicianId == PhysicianId);
                selectedPhysician.PhysicianIdEncrypted = SecurityHelper.Encrypt(selectedPhysician.PhysicianId.ToString());
                return Json(selectedPhysician, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPhysicianWithId HomeController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPatients([DataSourceRequest] DataSourceRequest request, string selectedPhysicianId, string searchText)
        {
            var currentUser = CurrentUser;
            try
            {
                List<vwPatient> patientsList = null;
                if (UserManager.IsInRole(currentUser.Id, "SuperAdmin") || UserManager.IsInRole(currentUser.Id, "Pharmacist"))
                {
                    if (selectedPhysicianId != null)
                    {
                        int PhysicianId = int.Parse(SecurityHelper.Decrypt(selectedPhysicianId));
                        if (PhysicianId > 0)
                            patientsList = unitOfWork.PatientsRepository.GetAsQuerable(t => t.PhysicianId == PhysicianId && t.FullName.Contains(searchText)).Take(AutoCompleteListSize).ToList();
                    }
                    else
                    {
                        patientsList = unitOfWork.PatientsRepository.GetAsQuerable(t => t.FullName.Contains(searchText)).ToList();
                    }
                }
                else if (UserManager.IsInRole(currentUser.Id, "Admin"))
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList().ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    patientsList = unitOfWork.PatientsRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId) && x.FullName.Contains(searchText)).Take(AutoCompleteListSize).ToList();
                }
                else //if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == currentUser.UserId).Select(y => y.PhysicianId);
                    patientsList = unitOfWork.PatientsRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId) && x.FullName.Contains(searchText)).Take(AutoCompleteListSize).ToList();
                }

                foreach (vwPatient patient in patientsList)
                {
                    patient.PatientIdEncrypted = SecurityHelper.Encrypt(patient.PatientId.ToString());
                    patient.PhysicianIdEncrypted = SecurityHelper.Encrypt(patient.PhysicianId.ToString());
                }

                return Json(patientsList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPatients HomeController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RefillFax(RefillFaxModel model, NewRefillViewModel[] newRefillViewModelList)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { message = "Unexpected input parameters", success = false });
                }

                string fileNameDescription = GetDocumentSaveFormat(model.PatientMRN, 6) + ".pdf"; 
                string filePath = Path.Combine(GetLocalDocumentPath(), fileNameDescription); 
                
                var getPatientFullName = "";
                {
                    RefillFaxReportLog refillFaxReportLog = unitOfWork.RefillFaxReportLogRepository.GetSingle(t => t.RefillFaxReportLogId == model.Id);
                    if (refillFaxReportLog != null)
                    {
                        getPatientFullName = refillFaxReportLog.PatientName;
                        RefillStatu refillStatus = unitOfWork.RefillStatusRepository.GetSingle(t => t.Name.Contains("Approve"));
                        refillFaxReportLog.RefillStatusId = refillStatus?.RefillStatusId;
                        refillFaxReportLog.IsSent = true;
                        refillFaxReportLog.SentOn = DateTime.UtcNow;
                        refillFaxReportLog.SentBy = CurrentUser.UserId;
                        refillFaxReportLog.RefillDocumentPath = filePath;
                        refillFaxReportLog.RefillDocumentName = fileNameDescription;

                        unitOfWork.RefillFaxReportLogRepository.Update(refillFaxReportLog);
                        unitOfWork.Save();
                    }

                }

                //Save Refill information in ph.NewRefill table
                var NewRefillList = unitOfWork.NewRefillRepository.GetAsQuerable(t => t.RefillFaxReportLogId == model.Id).ToList();

                if (NewRefillList != null && NewRefillList.Count() > 0)
                {
                    for (int i = 0; i < NewRefillList.Count(); i++)
                    {
                        NewRefillList[i].Qty = newRefillViewModelList[i].Qty;
                        NewRefillList[i].AdditionalRefills = newRefillViewModelList[i].AdditionalRefills;
                        NewRefillList[i].DoNotSubstitute = newRefillViewModelList[i].DoNotSubstitute;
                        NewRefillList[i].ModifiedOn = DateTime.UtcNow;
                        NewRefillList[i].ModifiedBy = CurrentUser.UserId;

                        unitOfWork.NewRefillRepository.Update(NewRefillList[i]);
                    }
                    unitOfWork.Save();
                }
                else
                {
                    // Insert New
                    foreach (var newRefillViewModel in newRefillViewModelList)
                    {
                        NewRefill newRefill = new NewRefill();

                        newRefill.Qty = newRefillViewModel.Qty;
                        newRefill.AdditionalRefills = newRefillViewModel.AdditionalRefills;
                        newRefill.DoNotSubstitute = newRefillViewModel.DoNotSubstitute;
                        newRefill.ModifiedOn = DateTime.UtcNow;
                        newRefill.ModifiedBy = CurrentUser.UserId;
                        newRefill.CreatedOn = DateTime.UtcNow;
                        newRefill.CreatedBy = CurrentUser.UserId;
                        newRefill.RefillFaxReportLogId = model.Id;
                        unitOfWork.NewRefillRepository.Insert(newRefill);
                    }
                    unitOfWork.Save();
                }

                model.HTMLText += "<style type=\"text/css\"> table.maintable{ width: 100% !important; } </style>";

                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.PdfPageSize = PdfPageSize.Letter;
                converter.Options.MarginBottom = 10;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginTop = 10;
                converter.Options.MarginRight = 10;
                converter.Options.JavaScriptEnabled = true;

                // create a new pdf document converting an url
                PdfDocument doc = converter.ConvertHtmlString(model.HTMLText);

                // save pdf document
                doc.Save(filePath);

                string filePathDestination = (SettingManager.GetSettingByName("SubmitChecklistDocumentPath")?.Value ?? "") + fileNameDescription;

                if (System.IO.File.Exists(filePath))
                    if (!System.IO.File.Exists(filePathDestination))
                        System.IO.File.Copy(filePath, filePathDestination);

                doc.Close();

                AuditLogger.AuditTracker("New Refill Send", "A new refill has been uploaded into fax queue folder for patient " + getPatientFullName, (int)AuditOperationType.Addition);
                return Json(new { message = "Add to Fax Queue Successfully.", success = true });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method RefillFax HomeController. " + exception.Message, exception);
                return Json(new { message = "Failed To Add in Fax Queue", success = false });
            }
        }

        public JsonResult GetSignature(string PhysicianNPI, int? PhysicianId = null)
        {
            try
            {
                string message = string.Empty;
                PhysicianSignature physicianSignature = null;

                if (PhysicianId != null)
                {
                    physicianSignature = unitOfWork.PhysicianSignatureRepository.GetSingle(t => t.PhysicianMasterId == PhysicianId);
                }
                else if (!string.IsNullOrWhiteSpace(PhysicianNPI))
                {
                    var user = unitOfWork.UserRepository.GetSingle(t => t.NPI.Equals(PhysicianNPI));
                    if (user != null)
                    {
                        UserPhysicianRelationship userPhysicianRelationship = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(t => t.UserId == user.UserId);
                        physicianSignature = userPhysicianRelationship != null ?
                            unitOfWork.PhysicianSignatureRepository.GetSingle(t => t.PhysicianMasterId == userPhysicianRelationship.PhysicianId) : null;
                    }
                }
                //else
                //{
                //    physicianSignature = new PhysicianSignature();
                //}

                if (physicianSignature?.Signature != null)
                {
                    message = Convert.ToBase64String(physicianSignature.Signature);
                    return Json(new { message = message, success = true });
                }
                else
                {
                    message = "Signature Not Found";
                    return Json(new { message = message, success = false });
                }
                //return Json(new { message = message, success = true });
                //return Json(new { message = Convert.ToBase64String(CurrentUser.Signature), success = true });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetSignature HomeController. " + exception.Message, exception);
                return Json(new { message = "Not Found", success = false });
            }
        }

        //public JsonResult DenyRefill(int RefillFaxReportLogId)
        //{
        //    try
        //    {
        //        RefillFaxReportLog refillFaxReportLog = unitOfWork.RefillFaxReportLogRepository.GetSingle(t => t.RefillFaxReportLogId == RefillFaxReportLogId);
        //        if (refillFaxReportLog != null)
        //        {
        //            RefillStatu refillStatus = unitOfWork.RefillStatusRepository.GetSingle(t => t.Name.Contains("Deny"));
        //            refillFaxReportLog.RefillStatusId = refillStatus?.RefillStatusId;
        //            refillFaxReportLog.IsSent = true;
        //            refillFaxReportLog.SentOn = DateTime.Now;
        //            refillFaxReportLog.SentBy = CurrentUser.UserId;
        //            unitOfWork.RefillFaxReportLogRepository.Update(refillFaxReportLog);
        //            unitOfWork.Save();
        //        }
        //        return Json(new { message = "Successfully Denied Refill", success = true });
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.InsertLog("Exception thrown in Method DenyRefill HomeController. " + exception.Message, exception);
        //        return Json(new { message = "Failed to denied Refill", success = false });
        //    }
        //}

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult HIPAA()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult TermsAndConditions()
        {
            return View();
        }

        //For display total count in Badges
        public JsonResult GetMyPatientsCount()
        {
            try
            {
                int patientsCount = 0;
                float patientsAvg = 0.0f;
                var baselineDate = DateTime.Now.AddDays(-30);

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Pharmacist").Result)
                {
                    patientsCount = unitOfWork.PatientsRepository.GetAsQuerable().Count();
                    if (patientsCount != 0)
                    {
                        patientsAvg = (float)unitOfWork.PatientsRepository.GetAsQuerable(t => t.NeedMoreInfo == true).Count() / (float)patientsCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();

                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    patientsCount = unitOfWork.PatientsRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId)).Count();

                    if (patientsCount != 0)
                    {
                        patientsAvg = (float)unitOfWork.PatientsRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId) && t.NeedMoreInfo == true).Count() / (float)patientsCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    patientsCount = unitOfWork.PatientsRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId)).Count();

                    if (patientsCount != 0)
                    {
                        patientsAvg = (float)unitOfWork.PatientsRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId) && t.NeedMoreInfo == true).Count() / (float)patientsCount * 100;
                    }
                }
                return Json(new { message = patientsCount.ToString(), patientsAvg = patientsAvg.ToString("F1"), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetMyPatientsCount() HomeController. " + exception.Message, exception);
                return Json(new { message = exception.ToString(), success = false });
            }
        }

        public JsonResult GetPatientsCount()
        {
            try
            {
                int physicianPortalPatientsCount = 0;
                float physicianPortalPatientsAvg = 0.0f;
                var baselineDate = DateTime.Now.AddDays(-30);

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Pharmacist").Result)
                {
                    physicianPortalPatientsCount = unitOfWork.PhysicianPortalPatientRepository.GetAsQuerable(t => !t.IsDeleted).Count();
                    if (physicianPortalPatientsCount != 0)
                    {
                        physicianPortalPatientsAvg = (float)unitOfWork.PhysicianPortalPatientRepository.GetAsQuerable(t => t.CreatedOn > baselineDate && !t.IsDeleted).Count() / (float)physicianPortalPatientsCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    physicianPortalPatientsCount = unitOfWork.PhysicianPortalPatientRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0) && !t.IsDeleted).Count();

                    if (physicianPortalPatientsCount != 0)
                    {
                        physicianPortalPatientsAvg = (float)unitOfWork.PhysicianPortalPatientRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0) && !t.IsDeleted && t.CreatedOn > baselineDate).Count() / (float)physicianPortalPatientsCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    physicianPortalPatientsCount = unitOfWork.PhysicianPortalPatientRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0) && !t.IsDeleted).Count();

                    if (physicianPortalPatientsCount != 0)
                    {
                        physicianPortalPatientsAvg = (float)unitOfWork.PhysicianPortalPatientRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0) && !t.IsDeleted && t.CreatedOn > baselineDate).Count() / (float)physicianPortalPatientsCount * 100;
                    }
                }
                return Json(new { message = physicianPortalPatientsCount.ToString(), physicianPortalPatientsAvg = physicianPortalPatientsAvg.ToString("F1"), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPatientsCount() HomeController. " + exception.Message, exception);
                return Json(new { message = exception.ToString(), success = false });
            }
        }

        public JsonResult GetCommunicationsCount()
        {
            try
            {
                int messagesCount = 0;
                float messagesAvg = 0.0f;
                var baselineDate = DateTime.Now.AddDays(-30);

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Pharmacist").Result)
                {
                    //If SuperAdmin or Pharmacist logged in, get all the messages
                    messagesCount = unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(x => x.IsCompleted == false || x.IsCompleted == null).Count();
                    if (messagesCount != 0)
                    {
                        messagesAvg = (float)unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(t => t.CreatedOn > baselineDate && (t.IsCompleted == false || t.IsCompleted == null)).Count() / (float)messagesCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    //If office admin is logged in then get all the message of his office
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    messagesCount = unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId ?? 0) && (x.IsCompleted == false || x.IsCompleted == false)).Count();
                    if (messagesCount != 0)
                    {
                        messagesAvg = (float)unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0) && t.CreatedOn > baselineDate && (t.IsCompleted == false || t.IsCompleted == null)).Count() / (float)messagesCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    //If physician or technician is logged in, only get his messages
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    messagesCount = unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(x => physicianIds.Contains(x.PhysicianId ?? 0) && (x.IsCompleted == false || x.IsCompleted == null)).Count();
                    if (messagesCount != 0)
                    {
                        messagesAvg = (float)unitOfWork.vwLastThreadMessageRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0) && t.CreatedOn > baselineDate && (t.IsCompleted == false || t.IsCompleted == null)).Count() / (float)messagesCount * 100;
                    }
                }
                return Json(new { message = messagesCount.ToString(), messagesAvg = messagesAvg.ToString("F1"), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetCommunicationsCount() HomeController. " + exception.Message, exception);
                return Json(new { message = exception.ToString(), success = false });
            }
        }

        public JsonResult GetRefillsCount()
        {
            try
            {
                int RefillFaxReportLogCount = 0;
                float refillFaxReportLogAvg = 0.0f;
                var baselineDate = DateTime.Now.AddDays(-30);

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Pharmacist").Result)
                {
                    RefillFaxReportLogCount = unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent != true).Count();
                    if (RefillFaxReportLogCount != 0)
                    {
                        refillFaxReportLogAvg = (float)unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent != true && t.SentOn > baselineDate).Count() / (float)RefillFaxReportLogCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId)).Select(y => y.NationalProviderIdentifier);
                    RefillFaxReportLogCount = unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent != true && physicianNPIs.Contains(t.NationalProviderIdentifier)).Count();

                    if (RefillFaxReportLogCount != 0)
                    {
                        refillFaxReportLogAvg = (float)unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent != true && physicianNPIs.Contains(t.NationalProviderIdentifier) && t.SentOn > baselineDate).Count() / (float)RefillFaxReportLogCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId)).Select(y => y.NationalProviderIdentifier);
                    RefillFaxReportLogCount = unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent != true && physicianNPIs.Contains(t.NationalProviderIdentifier)).Count();
                    if (RefillFaxReportLogCount != 0)
                    {
                        refillFaxReportLogAvg = (float)unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent != true && physicianNPIs.Contains(t.NationalProviderIdentifier) && t.SentOn > baselineDate).Count() / (float)RefillFaxReportLogCount * 100;
                    }
                }

                return Json(new { message = RefillFaxReportLogCount.ToString(), refillFaxReportLogAvg = refillFaxReportLogAvg.ToString("F1"), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetRefillsCount() HomeController. " + exception.Message, exception);
                return Json(new { message = exception.ToString(), success = false });
            }
        }

        public JsonResult GetHoldRefillsCount()
        {
            try
            {
                int RefillFaxReportLogCount = 0;
                float refillFaxReportLogAvg = 0.0f;
                var baselineDate = DateTime.Now.AddDays(-30);

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Pharmacist").Result)
                {
                    RefillFaxReportLogCount = unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent == true && t.RefillStatus.Equals("Hold")).Count();
                    if (RefillFaxReportLogCount != 0)
                    {
                        refillFaxReportLogAvg = (float)unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent == true && t.RefillStatus.Equals("Hold") && t.SentOn > baselineDate).Count() / (float)RefillFaxReportLogCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId)).Select(y => y.NationalProviderIdentifier);
                    RefillFaxReportLogCount = unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent == true && t.RefillStatus.Equals("Hold") && physicianNPIs.Contains(t.NationalProviderIdentifier)).Count();

                    if (RefillFaxReportLogCount != 0)
                    {
                        refillFaxReportLogAvg = (float)unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent == true && t.RefillStatus.Equals("Hold") && physicianNPIs.Contains(t.NationalProviderIdentifier) && t.SentOn > baselineDate).Count() / (float)RefillFaxReportLogCount * 100;
                    }
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    var physicianNPIs = unitOfWork.PhysiciansRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId)).Select(y => y.NationalProviderIdentifier);
                    RefillFaxReportLogCount = unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent == true && t.RefillStatus.Equals("Hold") && physicianNPIs.Contains(t.NationalProviderIdentifier)).Count();
                    if (RefillFaxReportLogCount != 0)
                    {
                        refillFaxReportLogAvg = (float)unitOfWork.vwRefillFaxReportLogsRepository.GetAsQuerable(t => t.IsSent == true && t.RefillStatus.Equals("Hold") && physicianNPIs.Contains(t.NationalProviderIdentifier) && t.SentOn > baselineDate).Count() / (float)RefillFaxReportLogCount * 100;
                    }
                }

                return Json(new { message = RefillFaxReportLogCount.ToString(), refillFaxReportLogAvg = refillFaxReportLogAvg.ToString("F1"), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetHoldRefillsCount() HomeController. " + exception.Message, exception);
                return Json(new { message = exception.ToString(), success = false });
            }
        }

        public JsonResult GetNotificationAlertsCount()
        {
            try
            {
                int MissingDocumentsCount = 0, NewMessagesCount = 0, NeedMoreInfoCount = 0;
                IQueryable<NotificationAlertCountViewModel> vwNotificationAlertCount = Enumerable.Empty<NotificationAlertCountViewModel>().AsQueryable();

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Pharmacist").Result)
                {
                    //When SuperAdmin or Admin or Pharmacist is logged in.
                    vwNotificationAlertCount = unitOfWork.vwNotificationAlertRepository.GetAsQuerable()
                        .GroupBy(t => t.ItemId).Select(t => new NotificationAlertCountViewModel { ItemId = (int)t.Key, Count = t.Count() });
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    //When office admin log in.
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();

                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    vwNotificationAlertCount = unitOfWork.vwNotificationAlertRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0))
                        .GroupBy(t => t.ItemId).Select(t => new NotificationAlertCountViewModel { ItemId = (int)t.Key, Count = t.Count() });
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "Physician").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Technician").Result)
                {
                    //When physician or technician is logged in.
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    vwNotificationAlertCount = unitOfWork.vwNotificationAlertRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0))
                        .GroupBy(t => t.ItemId).Select(t => new NotificationAlertCountViewModel { ItemId = (int)t.Key, Count = t.Count() });
                }
                NewMessagesCount = vwNotificationAlertCount.FirstOrDefault(t => t.ItemId == 1)?.Count ?? 0;
                MissingDocumentsCount = vwNotificationAlertCount.FirstOrDefault(t => t.ItemId == 2)?.Count ?? 0;
                NeedMoreInfoCount = vwNotificationAlertCount.FirstOrDefault(t => t.ItemId == 3)?.Count ?? 0;

                return Json(new { NewMessagesCount = NewMessagesCount.ToString(), MissingDocumentsCount = MissingDocumentsCount.ToString(), NeedMoreInfoCount = NeedMoreInfoCount.ToString(), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetNotificationAlertsCount() HomeController. " + exception.Message, exception);
                return Json(new { message = exception.ToString(), success = false });
            }
        }

        public JsonResult SetMessageCompleted(int MessageId)
        {
            try
            {
                Message message = unitOfWork.MessagesRepository.GetSingle(t => t.MessageId == MessageId);
                if (message != null)
                {
                    message.IsCompleted = true;
                    unitOfWork.MessagesRepository.Update(message);
                    unitOfWork.Save();
                    return Json(new { message = "This message thread is completed. This thread is close now.", success = true });
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method SetMessageCompleted() HomeController. " + exception.Message, exception);
            }
            return Json(new { message = "Sorry, we encountered an error", success = false });
        }

        public JsonResult ContinueMessageThread(int MessageId)
        {
            try
            {
                Message message = unitOfWork.MessagesRepository.GetSingle(t => t.MessageId == MessageId);
                if (message != null)
                {
                    message.IsCompleted = false;
                    unitOfWork.MessagesRepository.Update(message);
                    unitOfWork.Save();
                    return Json(new { message = "This message thread has resumed.", success = true });
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ContinueMessageThread() HomeController. " + exception.Message, exception);
            }
            return Json(new { message = "Sorry, we encountered an error", success = false });
        }

        public JsonResult GetNotifiactions(int ItemId)
        {
            try
            {
                IQueryable<vwNotificationAlert> notificationAlerts = Enumerable.Empty<vwNotificationAlert>().AsQueryable();
                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result || UserManager.IsInRoleAsync(CurrentUser.Id, "Pharmacist").Result)
                {
                    //Show all Notifications for SuperAdmin and Pharmacist
                    notificationAlerts = unitOfWork.vwNotificationAlertRepository.GetAsQuerable(t => t.ItemId == ItemId);
                }
                else if (UserManager.IsInRole(CurrentUser.Id, "Admin"))
                {
                    //When office admin log in.
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    notificationAlerts = unitOfWork.vwNotificationAlertRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0) && t.ItemId == ItemId);
                }
                else if (UserManager.IsInRole(CurrentUser.Id, "Physician") || UserManager.IsInRole(CurrentUser.Id, "Technician"))
                {
                    //Physician can only see their own Notifications
                    //Technician can only select physicians Notifications that are registered against him in the UserPhysicianRelationship table
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId);
                    notificationAlerts = unitOfWork.vwNotificationAlertRepository.GetAsQuerable(t => physicianIds.Contains(t.PhysicianId ?? 0) && t.ItemId == ItemId);
                }


                //The following is to encrypt the parameter id values, this is not very efficient, need to use regex expressions here

                var notificationAlertsList = notificationAlerts.ToList();

                if (ItemId == 2)
                {
                    foreach (var notification in notificationAlertsList)
                    {
                        var components = notification.Url.Split('?');

                        if (components.Length > 1)
                        {
                            var parameters = components[1].Split('&');

                            string baseUrl = components[0] + "?";

                            int count = 0;
                            foreach (var parameter in parameters)
                            {
                                var pieces = parameter.Split('=');

                                if (count > 0)
                                {
                                    baseUrl += "&";
                                }
                                count++;
                                try
                                {
                                    if (pieces[0].ToLower() == "physicianportalpatientid" || pieces[0].ToLower() == "phyid" || pieces[0].ToLower() == "patid")
                                    {
                                        var value = long.Parse(pieces[1]);
                                        var encryptedValue = SecurityHelper.Encrypt(value.ToString());

                                        baseUrl += pieces[0] + "=" + Uri.EscapeUriString(encryptedValue);
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }
                                }
                                catch (Exception e)
                                {
                                    //The parameter was not a number, no need to encrypt
                                    baseUrl += pieces[0] + "=" + pieces[1];
                                    continue;
                                }
                            }
                            notification.Url = baseUrl;
                        }
                    }
                }

                return Json(new { message = notificationAlertsList, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetNotifiactions() HomeController. " + exception.Message, exception);
            }
            return Json(new { message = "Sorry, we encountered an error", success = false });
        }
    }

    public class SeriesData
    {
        public string name { get; set; }
        public List<int> data { get; set; }
    }

    public class listItem
    {
        public string Date { get; set; }
        public Nullable<int> PatientCount { get; set; }
    }
}