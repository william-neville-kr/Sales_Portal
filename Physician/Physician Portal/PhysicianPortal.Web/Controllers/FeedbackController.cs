using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Helpers;
using PhysicianPortal.Core.Repository;
using PhysicianPortal.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicianPortal.Web.Controllers
{
    [CustomAuthorize]
    public class FeedbackController : BaseController
    {
        protected readonly UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Feedbacks_Read([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IQueryable<vwPhysicianPortalFeedback> FeedbackList = null;

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    //When the use is an admin, get all feedback for submitted by this office

                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();

                    FeedbackList = unitOfWork.vwPhysicianPortalFeedback.GetAsQuerable(x => userIds.Contains(x.CreatedBy.Value)).OrderByDescending(t => t.ModifiedOn);
                }
                else if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result)
                {
                    //Get all feedback for superadmin
                    FeedbackList = unitOfWork.vwPhysicianPortalFeedback.GetAsQuerable().OrderByDescending(t => t.ModifiedOn);
                }
                else
                {
                    //Get only the user's feedback for all other roles
                    FeedbackList = unitOfWork.vwPhysicianPortalFeedback.GetAsQuerable(x => x.CreatedBy == CurrentUser.UserId).OrderByDescending(t => t.ModifiedOn);
                }
                                               
                return Json(FeedbackList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method Feedbacks_Read FeedbackController. " + ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEdit(int? id)
        {
            try
            {
                PhysicianPortalFeedback feedback = null;

                if (id != null && id != 0)
                {
                    feedback = unitOfWork.PhysicianPortalFeedbackRepository.GetSingle(t => t.FeedbackId == id);
                }

                if (feedback == null)
                {
                    feedback = new PhysicianPortalFeedback();
                }

                return View(feedback);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddEdit FeedbackController. " + exception.Message, exception);
            }
            return View(new PhysicianPortalFeedback());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEdit(PhysicianPortalFeedback model)
        {
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    PhysicianPortalFeedback feedback = unitOfWork.PhysicianPortalFeedbackRepository.GetSingle(t => t.FeedbackId == model.FeedbackId);
                    bool IsEdit = true;

                    if (feedback == null)
                    {
                        feedback = new PhysicianPortalFeedback();
                        feedback.CreatedOn = DateTime.Now;
                        feedback.CreatedBy = CurrentUser.UserId;
                        IsEdit = false;
                    }

                    feedback.Subject = model.Subject;
                    feedback.Description = model.Description;
                    feedback.FeedbackStatusId = model.FeedbackStatusId;
                    feedback.FeedbackRelatedId = model.FeedbackRelatedId;

                    feedback.ModifiedOn = DateTime.Now;
                    feedback.ModifiedBy = CurrentUser.UserId;

                    if (IsEdit)
                    {
                        unitOfWork.PhysicianPortalFeedbackRepository.Update(feedback);
                    }
                    else
                    {
                        unitOfWork.PhysicianPortalFeedbackRepository.Insert(feedback);
                    }
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddEdit POST FeedbackController. " + exception.Message, exception);
            }
            //return View(new Feedback());
            ModelState.AddModelError(string.Empty, "Please enter valid data in the fields.");
            return View(model);
        }

        public JsonResult GetFeedbackStatusList()
        {
            try
            {
                var feedbackStatusList = unitOfWork.FeedbackStatusRepository.GetAsQuerable().Select(t => new { Name = t.Name, FeedbackStatusId = t.FeedbackStatusId });
                return Json(feedbackStatusList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetFeedbackStatusList FeedbackController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFeedbackRelatedList()
        {
            try
            {
                var feedbackRelatedList = unitOfWork.FeedbackRelatedRepository.GetAsQuerable().Select(t => new { Name = t.Name, FeedbackRelatedId = t.FeedbackRelatedId });
                return Json(feedbackRelatedList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetFeedbackRelatedList FeedbackController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
