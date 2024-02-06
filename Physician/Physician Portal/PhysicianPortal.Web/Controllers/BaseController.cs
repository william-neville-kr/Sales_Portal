using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Repository;
using PhysicianPortal.Core.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using PhysicianPortal.Core.Helper;
using System.Security.Claims;
using PhysicianPortal.Web.Attributes;

namespace PhysicianPortal.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        private ApplicationUserManager userManager;
        private ApplicationSignInManager signInManager;
        private User currentUser;



        public enum AuditOperationType
        {
            Addition = 1,
            Change = 2,
            Deletion = 3,
            Query = 4,
            Login = 5,
            Logout = 6,
            Visit = 7,
            AuditLogEnabled = 8,
            AuditLogDisabled = 9
            //SetScheduleTimeEnabled = 10,
            //SetScheduleTimeDisabled = 11,
        }
        public User UserInfo
        {
            get
            {
                User user = new User();
                try
                {
                    int userId = Convert.ToInt32(User.Identity.GetUserId());
                    user = unitOfWork.UserRepository.GetAsQuerable(t => t.UserId == userId).FirstOrDefault();
                }
                catch (Exception exception)
                {
                    Logger.InsertLog("Exception thrown in Accessors UserInfo BaseController. " + exception.Message, exception);
                }
                return user;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                signInManager = value;
            }
        }
        public User CurrentUser
        {
            get
            {
                return currentUser ?? (User)UserManager.FindById(User.Identity.GetUserId());
            }
            private set
            {
                currentUser = value;
            }
        }

        public BaseController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public BaseController()
        {
        }
        //protected override void Dispose(bool disposing)
        //{
        //    try
        //    {
        //        if (disposing)
        //        {
        //            if (userManager != null)
        //            {
        //                userManager.Dispose();
        //                userManager = null;
        //            }

        //            if (signInManager != null)
        //            {
        //                signInManager.Dispose();
        //                signInManager = null;
        //            }
        //        }

        //        base.Dispose(disposing);
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.InsertLog("Exception thrown in Method Dispose AccountController. " + exception.Message, exception);
        //    }
        //}

        public int AutoCompleteListSize
        {
            get
            {
                try
                {
                    ApplicationSetting settingManager = SettingManager.GetSettingByName("AutoCompleteListSize");

                    if (settingManager != null)
                    {
                        return settingManager.Value.ToInt0() != 0 ? settingManager.Value.ToInt0() : 20;
                    }
                }
                catch (Exception exception)
                {
                    Logger.InsertLog("Exception thrown in Accessors UserInfo BaseController. " + exception.Message, exception);
                }
                return 20;    // AutoComplete List Size 20
            }
        }

        public string ErrorMessage
        {
            get
            {
                try
                {
                    ApplicationSetting settingManager = SettingManager.GetSettingByName("ErrorMessage");

                    if (settingManager != null)
                    {
                        return settingManager.Value;
                    }
                    else
                    {
                        return "Could not find the resource you were looking for, please try again";
                    }
                }
                catch (Exception exception)
                {
                    Logger.InsertLog("Exception thrown in Accessors ErrorMessage BaseController. " + exception.Message, exception);
                }
                return "Could not find the resource you were looking for, please try again";    // AutoComplete List Size 20
            }
        }

        public string ErrorTitle
        {
            get
            {
                try
                {
                    ApplicationSetting settingManager = SettingManager.GetSettingByName("ErrorTitle");

                    if (settingManager != null)
                    {
                        return settingManager.Value;
                    }
                    else
                    {
                        return "Sorry, we encountered an error";
                    }
                }
                catch (Exception exception)
                {
                    Logger.InsertLog("Exception thrown in Accessors ErrorMessage BaseController. " + exception.Message, exception);
                }
                return "Sorry, we encountered an error";    // AutoComplete List Size 20
            }
        }
        public string ClientName3
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ClientName3"].ToString();
            }
        }

        //This attribute will delete the Authentication Cookie from the response
        //So that a call to this function will not renew the Session Expiration Time
        [CookieStripperAttribute]
        public JsonResult CheckForTimeout()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                var claimType = "CookieExpirationTime";
                if (identity != null && identity.HasClaim(c => c.Type == claimType))
                {
                    var expireOn = identity.FindFirstValue(claimType);

                    DateTimeOffset currentUtc = DateTimeOffset.UtcNow;
                    //DateTimeOffset? expireUtc = new DateTimeOffset(long.Parse(expireOn), TimeSpan.Zero);
                    DateTimeOffset expireUtc = new DateTimeOffset(long.Parse(expireOn), TimeSpan.Zero);

                    var remaining = (expireUtc - currentUtc).TotalSeconds;

                    if (remaining > 0)
                    {
                        return Json(new { success = true, message = remaining });
                    }
                    else
                    {
                        return Json(new { success = true, message = 0 });
                    }
                }
                return Json(new { success = true, message = 0 });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method CheckForTimeout BaseController. " + exception.Message, exception);
                return Json(new { success = false, message = "" + exception.Message });
            }
        }

        public JsonResult RenewSession()
        {
            //Empty method because to renew the session the client just needs to send a call to the server, the session is renewed automatically
            try
            {
                return Json(new { success = true, message = "" });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method RenewSession BaseController. " + exception.Message, exception);
                return Json(new { success = false, message = "" });
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        private string GetDocumentPathWithOfficeId(int OfficeId)
        {
            string submitChecklistDocumentPath = SettingManager.GetSettingByName("SubmitChecklistDocumentPath")?.Value ?? "";
            try
            {
                submitChecklistDocumentPath = unitOfWork.OfficeRepository.GetSingle(t => t.OfficeId == OfficeId)?.SubmitChecklistDocumentPath ?? "";
                if (string.IsNullOrWhiteSpace(submitChecklistDocumentPath))
                {
                    submitChecklistDocumentPath = SettingManager.GetSettingByName("SubmitChecklistDocumentPath")?.Value ?? "";
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetDocumentPathWithOfficeId BaseController. " + exception.Message, exception);
            }
            return submitChecklistDocumentPath;
        }
        public string GetDocumentPathWithPhysicianId(int PhysicianId)
        {
            string submitChecklistDocumentPath = SettingManager.GetSettingByName("SubmitChecklistDocumentPath")?.Value ?? "";
            try
            {
                var userPhysicianRela = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(t => t.PhysicianId == PhysicianId);
                if (userPhysicianRela != null)
                {
                    var user_Office_Rela = unitOfWork.User_Office_RelationshipRepository.GetSingle(t => t.UserId == userPhysicianRela.UserId);
                    if (user_Office_Rela != null)
                    {
                        //submitChecklistDocumentPath = unitOfWork.OfficeRepository.GetSingle(t => t.OfficeId == user_Office_Rela.OfficeId)?.SubmitChecklistDocumentPath ?? "";
                        submitChecklistDocumentPath = GetDocumentPathWithOfficeId(user_Office_Rela.OfficeId);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetDocumentPathWithPhysicianId BaseController. " + exception.Message, exception);
            }
            return submitChecklistDocumentPath;
        }

        public string GetLocalDocumentPath()
        {
            string submitLocalDocumentPath = SettingManager.GetSettingByName("ChecklistDocumentPath")?.Value ?? "";
            //try
            //{
            //    //Under the current implementation this will always throw an exception because we are curretnly providing an absolute path on the server
            //    submitLocalDocumentPath = Server.MapPath(submitLocalDocumentPath);
            //}
            //catch (Exception exception)
            //{
            //    Logger.InsertLog("Exception thrown in Method GetLocalDocumentPath BaseController. " + exception.Message, exception);
            //}
            return submitLocalDocumentPath;
        }





        public string GetDocumentPathWithPhysicianNPI(string NPI)
        {
            string submitChecklistDocumentPath = SettingManager.GetSettingByName("SubmitChecklistDocumentPath")?.Value ?? "";
            try
            {
                var physician = unitOfWork.PhysiciansRepository.GetSingle(t => t.NationalProviderIdentifier.Equals(NPI));
                if (physician != null)
                {
                    var userPhysicianRela = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(t => t.PhysicianId == physician.PhysicianId);
                    if (userPhysicianRela != null)
                    {
                        var user_Office_Rela = unitOfWork.User_Office_RelationshipRepository.GetSingle(t => t.UserId == userPhysicianRela.UserId);
                        if (user_Office_Rela != null)
                        {
                            //submitChecklistDocumentPath = unitOfWork.OfficeRepository.GetSingle(t => t.OfficeId == user_Office_Rela.OfficeId)?.SubmitChecklistDocumentPath ?? "";
                            submitChecklistDocumentPath = GetDocumentPathWithOfficeId(user_Office_Rela.OfficeId);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetDocumentPathWithPhysicianNPI BaseController. " + exception.Message, exception);
            }
            return submitChecklistDocumentPath;
        }
        public string GetDocumentPathWithPhysicianPortalPatientId(int PhysicianPortalPatientId)
        {
            string submitChecklistDocumentPath = SettingManager.GetSettingByName("SubmitChecklistDocumentPath")?.Value ?? "";
            try
            {
                var physicianPortalPatient = unitOfWork.PhysicianPortalPatientRepository.GetSingle(t => t.PhysicianPortalPatientId == PhysicianPortalPatientId);
                if (physicianPortalPatient != null)
                {
                    var userPhysicianRela = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(t => t.PhysicianId == physicianPortalPatient.PhysicianId);
                    if (userPhysicianRela != null)
                    {
                        var user_Office_Rela = unitOfWork.User_Office_RelationshipRepository.GetSingle(t => t.UserId == userPhysicianRela.UserId);
                        if (user_Office_Rela != null)
                        {
                            //submitChecklistDocumentPath = unitOfWork.OfficeRepository.GetSingle(t => t.OfficeId == user_Office_Rela.OfficeId)?.SubmitChecklistDocumentPath ?? "";
                            submitChecklistDocumentPath = GetDocumentPathWithOfficeId(user_Office_Rela.OfficeId);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetDocumentPathWithPhysicianPortalPatientId BaseController. " + exception.Message, exception);
            }
            return submitChecklistDocumentPath;
        }
        public enum DocumentAddedType
        {
            AddedFromCPR = 1,
            LocalPatient = 2,
            ReferralFormNewPatient = 3,
            ReferralFormNewDrug = 4,
            SubmitReferralForm = 5,
            RefillFaxSubmitted = 6
            //SetScheduleTimeEnabled = 10,
            //SetScheduleTimeDisabled = 11,
        }

        public string GetDocumentSaveFormat(string mrnNo, int addedType)
        {
            string UploadDocumentNameSetting = SettingManager.GetSettingByName("UploadDocumentNameSetting")?.Value ?? "";
            try
            {
                if (UploadDocumentNameSetting != "")
                {
                    // addedfromCPR
                    if (addedType == 1)
                    {
                        if (mrnNo.Equals("No_MRN"))
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_new";
                        else
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_mrn" + mrnNo;
                    }
                    else if (addedType == 3)
                    {
                        if (mrnNo.Equals("No_MRN"))
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_new";
                        else
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_mrn" + mrnNo;
                    }
                    else if (addedType == 4)
                    {
                        if (mrnNo.Equals(""))
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_new_Ref";
                        else
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_mrn" + mrnNo + "_Ref";
                    }
                    else if (addedType == 5)
                    {
                        if (mrnNo.Equals("_new"))
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_new_Ref";
                        else
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_mrn" + mrnNo + "_Ref";

                    }
                    else if (addedType == 6)
                    {
                        if (mrnNo.Equals(""))
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_new_Rel";
                        else
                            UploadDocumentNameSetting = UploadDocumentNameSetting + DateTime.Now.ToString("yyyyMMddHHmmss") + "_mrn" + mrnNo + "_Rel";
                    }
                    //"PhPt_NewPat_" + fName + "_" + DateTime.Now.ToString("yyMMdd_HHmmss") + fileExtension;
                }
                else
                    return UploadDocumentNameSetting;
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetDocumentPathWithPhysicianPortalPatientId BaseController. " + exception.Message, exception);
            }
            return UploadDocumentNameSetting;
        }

    }
}