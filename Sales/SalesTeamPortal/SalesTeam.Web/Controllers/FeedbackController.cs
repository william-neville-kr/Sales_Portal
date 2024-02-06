using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SalesTeam.Core.Data;
using SalesTeam.Core.Repository;
using SalesTeam.Web.Models;

namespace SalesTeam.Web.Controllers
{
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
                IQueryable<vwFeedback> FeedbackList = unitOfWork.vwFeedbackRepository.GetAsQuerable()
                    .OrderByDescending(t => t.ModifiedOn);

                //MasterSalesTeamId 219 is for siraj.rizvi
                if (MasterSalesTeamId != 219)
                {
                    FeedbackList = FeedbackList.Where(t => t.CreatedBy == MasterSalesTeamId);
                }
                return Json(FeedbackList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, ex);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEdit(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    Feedback feedback = null;
                    if (id != null && id != 0)
                    {
                        feedback = unitOfWork.FeedbacksRepository.GetSingle(t => t.FeedbackId == id);
                    }

                    if (feedback == null)
                    {
                        feedback = new Feedback();
                    }

                    return View(feedback);
                }
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
            }
            return View(new Feedback());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEdit(Feedback model)
        {
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    Feedback feedback = unitOfWork.FeedbacksRepository.GetSingle(t => t.FeedbackId == model.FeedbackId);
                    bool IsEdit = true;

                    if (feedback == null)
                    {
                        feedback = new Feedback();
                        feedback.CreatedOn = DateTime.Now;
                        feedback.CreatedBy = MasterSalesTeamId;
                        IsEdit = false;
                    }

                    feedback.Subject = model.Subject;
                    feedback.Description = model.Description;
                    feedback.FeedbackStatusId = model.FeedbackStatusId;
                    feedback.FeedbackRelatedId = model.FeedbackRelatedId;

                    feedback.ModifiedOn = DateTime.Now;
                    feedback.ModifiedBy = MasterSalesTeamId;

                    if (IsEdit)
                    {
                        unitOfWork.FeedbacksRepository.Update(feedback);
                    }
                    else
                    {
                        unitOfWork.FeedbacksRepository.Insert(feedback);
                    }
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
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
                LogInfo(exception.Message, exception);
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
                LogInfo(exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
