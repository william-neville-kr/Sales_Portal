using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PhysicianPortal.Core.Helpers;
using PhysicianPortal.Core.Repository;
using PhysicianPortal.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Helper;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using Microsoft.Owin.Security;
using PhysicianPortal.Core.Models;
using TuesPechkin;
using System.Drawing.Printing;
using System.Text;
using HtmlAgilityPack;
using SelectPdf;
using System.IO;
using PhysicianPortal.Web.Attributes;

namespace PhysicianPortal.Web.Controllers
{
    [CustomAuthorize(Roles = "SuperAdmin,Admin,Physician")]
    public class AdminController : BaseController
    {
        //private static IConverter pdfConverter =
        //                                    new ThreadSafeConverter(
        //                                        new RemotingToolset<PdfToolset>(
        //                                            new WinAnyCPUEmbeddedDeployment(
        //                                                new TempFolderDeployment())));
        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Admin
        public ActionResult Index()
        {
            try
            {
                OfficeUsersViewModel model = new OfficeUsersViewModel { OfficeId = 0 };
                return View(model);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method Index AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "Index", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult GetUsersList([DataSourceRequest] DataSourceRequest request, int officeId = -1)
        {
            try
            {
                if (officeId < 0)
                {
                    ModelState.AddModelError("", "Invalid OfficeId Format");
                    return View();
                }
                var currentUser = CurrentUser;

                if (UserManager.IsInRoleAsync(currentUser.Id, "SuperAdmin").Result)
                {
                    //If the logged in user is a super admin, get all the users registered in the system
                    var usersList = unitOfWork.UserRepository
                        .GetAsQuerable(x => (officeId > 0 ? x.User_Office_Relationship.Select(y => y.OfficeId).Contains(officeId) : true)).ToList()
                        .Select(t => new UsersListModel
                        {
                            UserId = t.UserId,
                            Email = t.Email,
                            FullName = t.FirstName + " " + t.LastName,
                            ContactNumber = t.PhoneNumber,
                            NPI = t.NPI,
                            Role = t.Role.Value,
                            LastLogin = t.LastLogin != null ? t.LastLogin.Value.ToLocalTime().ToString("M/d/yyyy, hh:mm:ss tt") : "",
                            LastLoginIP = t.LastLoginIP,
                            IsActive = t.IsActive ?? false,
                            IsVerified = t.IsVerified ?? false
                        });
                    return Json(usersList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
                else if (UserManager.IsInRoleAsync(currentUser.Id, "Admin").Result)
                {
                    //If the logged in user is an admin, first get the office ID from user claims, then get the users belonging to that office only
                    IList<Claim> userClaims = UserManager.GetClaims(currentUser.Id);
                    string currentOffice = userClaims.FirstOrDefault(c => c.Type == "UserOffice").Value;
                    int currentOfficeId = 0;
                    if (currentOffice != null)
                    {
                        currentOfficeId = currentOffice.ToInt0();
                    }
                    var usersList = unitOfWork.UserRepository
                        .GetAsQuerable(x => x.User_Office_Relationship.Select(y => y.OfficeId).Contains(currentOfficeId))
                        .ToList()
                        .Select(t => new UsersListModel
                        {
                            UserId = t.UserId,
                            Email = t.Email,
                            FullName = t.FirstName + " " + t.LastName,
                            ContactNumber = t.PhoneNumber,
                            NPI = t.NPI,
                            Role = t.Role.Value,
                            LastLogin = t.LastLogin != null ? t.LastLogin.Value.ToLocalTime().ToString("M/d/yyyy, hh:mm:ss tt") : "",
                            LastLoginIP = t.LastLoginIP,
                            IsActive = t.IsActive ?? false,
                            IsVerified = t.IsVerified ?? false
                        });
                    return Json(usersList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
                else if (UserManager.IsInRoleAsync(currentUser.Id, "Physician").Result)
                {
                    //If the logged in user is a physician, only get the the records of those technicians who are registered against him and in the current office
                    IList<Claim> userClaims = UserManager.GetClaims(currentUser.Id);
                    string currentOffice = userClaims.FirstOrDefault(c => c.Type == "UserOffice").Value;
                    int currentOfficeId = 0;
                    if (currentOffice != null)
                    {
                        currentOfficeId = currentOffice.ToInt0();
                    }
                    int physicianIdOfCurrentPhysician = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(x => x.UserId == currentUser.UserId).PhysicianId;
                    List<int> technicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.PhysicianId == physicianIdOfCurrentPhysician).Select(x => x.UserId).ToList();

                    var usersList = unitOfWork.UserRepository
                        .GetAsQuerable(t => technicianIds.Contains(t.UserId) && t.User_Office_Relationship.Select(y => y.OfficeId).Contains(currentOfficeId)).ToList()
                        .Select(t => new UsersListModel
                        {
                            UserId = t.UserId,
                            Email = t.Email,
                            FullName = t.FirstName + " " + t.LastName,
                            ContactNumber = t.PhoneNumber,
                            NPI = t.NPI,
                            Role = t.Role.Value,
                            LastLogin = t.LastLogin != null ? t.LastLogin.Value.ToLocalTime().ToString("M/d/yyyy, hh:mm:ss tt") : "",
                            LastLoginIP = t.LastLoginIP,
                            IsActive = t.IsActive ?? false,
                            IsVerified = t.IsVerified ?? false
                        });
                    return Json(usersList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetUsersList([DataSourceRequest] DataSourceRequest request) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "GetUsersList", ErrorTitle, ErrorMessage, null));
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNewUsersList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var currentUser = CurrentUser;

                if (UserManager.IsInRoleAsync(currentUser.Id, "SuperAdmin").Result)
                {
                    //If the logged in user is a super admin, get all the users registered in the system
                    var usersList = unitOfWork.UserRepository
                        .GetAsQuerable(x => x.UserId != currentUser.UserId && x.IsVerified != true).ToList()
                        .Select(t => new UsersListModel
                        {
                            UserId = t.UserId,
                            Email = t.Email,
                            FullName = t.FirstName + " " + t.LastName,
                            ContactNumber = t.PhoneNumber,
                            Role = t.Role.Value,
                            LastLogin = t.LastLogin != null ? t.LastLogin.Value.ToLocalTime().ToString("M/d/yyyy, hh:mm:ss tt") : "",
                            LastLoginIP = t.LastLoginIP,
                            IsActive = t.IsActive ?? false,
                            IsVerified = t.IsVerified ?? false
                        });
                    return Json(usersList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetNewUsersList([DataSourceRequest] DataSourceRequest request) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "GetNewUsersList", ErrorTitle, ErrorMessage, null));
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewUsersCount()
        {
            try
            {
                var currentUser = CurrentUser;

                if (UserManager.IsInRoleAsync(currentUser.Id, "SuperAdmin").Result)
                {
                    //If the logged in user is a super admin, get all the users registered in the system
                    var usersCount = unitOfWork.UserRepository
                        .GetAsQuerable(x => x.UserId != currentUser.UserId && x.IsVerified != true).Count();
                    return Json(new { message = usersCount.ToString(), success = true });
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetNewUsersCount() AdminController. " + exception.Message, exception);
                return Json(new { message = exception.ToString(), success = false });
            }
            return Json(new { message = "", success = false });
        }

        public ActionResult GetOfficesList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var officesList = unitOfWork.OfficeRepository.GetAsQuerable().Select(t => new OfficesListModel
                {
                    OfficeId = t.OfficeId,
                    OfficeTitle = t.OfficeTitle,
                    Address = t.Address,
                    City = t.City,
                    State = t.StateProvince,
                    PostalCode = t.PostalCode,
                    PhoneNumber = t.PhoneNumber,
                    FaxNumber = t.FaxNumber
                });
                return Json(officesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetOfficesList([DataSourceRequest] DataSourceRequest request) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "GetOfficesList", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult AddNewUser(string returnUrl)
        {
            try
            {
                var user = new AddUserModel();

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    List<int> OfficeList = new List<int>();
                    OfficeList.Add(CurrentUser.User_Office_Relationship.FirstOrDefault().OfficeId);
                    user.OfficeList = OfficeList;
                }

                ViewBag.ReturnUrl = returnUrl;
                return View(user);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddNewUser(string returnUrl) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "AddNewUser", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        public ActionResult TermsAndCondition(string returnUrl)
        {
            try
            {

                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddNewUser(string returnUrl) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "AddNewUser", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddNewUser(AddUserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = 0, message = "One or more inputs were not of the correct format." });
                }

                if (!string.IsNullOrWhiteSpace(model.PhysicianListStr))
                {
                    //Validating physicianIds
                   
                    var splittedIds = model.PhysicianListStr.Split(',');
                    if (splittedIds.Length > 0)
                    {
                        foreach (var id in splittedIds)
                        {                            
                            try
                            {
                                int.Parse(id);
                            }
                            catch
                            {
                                return Json(new { success = 0, message = "One or more input values were not of the expected format" });
                            }
                        }
                    }

                    model.PhysicianList = model.PhysicianListStr.Split(',').Select(t => Convert.ToInt32(t)).ToList();
                }

                var currentUser = CurrentUser;

                if (currentUser.RoleId == 2)
                {
                    //If a physician created a new user, we need to set the physician of the new user to the logged in physician and the office of the new user to the office of the current logged in physician
                    int physicianId = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(x => x.UserId == currentUser.UserId).PhysicianId;

                    //Getting officeId from User claims
                    UserClaim currentClaim = unitOfWork.UserClaimRepository.GetSingle(x => x.ClaimName == "UserOffice" && x.UserId == currentUser.UserId);
                    int officeId = currentClaim.ClaimValue.ToInt0();
                    model.PhysicianList = new List<int>();
                    model.PhysicianList.Add(physicianId);
                    model.OfficeList = new List<int>();
                    model.OfficeList.Add(officeId);
                }
                if (model.RoleId == 1 || model.RoleId == 5)
                {
                    //New Admin or SuperAdmin was created
                }
                else if (model.RoleId == 2)
                {
                    //New Physician was created
                }
                else if (model.RoleId == 3)
                {
                    //New Technician was created
                }
                else if (model.RoleId == 4)
                {
                    //New Pharmacist was created
                }

                HomePage homePage = null;

                if (model.RoleId == 4)
                {
                    //Pharmacist can only see the HomePage designed for the Pharmacist
                    homePage = unitOfWork.HomePagesRepository.GetSingle(t => t.HomePageValue.Equals("HomePharmacist"));
                }
                else
                {
                    //For all the rest roles set the HomeDetail as the default Home Page
                    homePage = unitOfWork.HomePagesRepository.GetSingle(t => t.HomePageValue.Equals("HomeDetail"));
                }

                Core.Data.User newUser = new Core.Data.User()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    RoleId = model.RoleId,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true,
                    IsActive = model.IsActive,
                    IsVerified = false,
                    NPI = model.NPI,
                    HomePageId = homePage?.HomePageId ?? 1
                };

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    List<int> OfficeList = new List<int>();
                    OfficeList.Add(CurrentUser.User_Office_Relationship.FirstOrDefault().OfficeId);
                    model.OfficeList = OfficeList;
                }

                if (model.RoleId == 1 || model.RoleId == 2 || model.RoleId == 3)
                {
                    // Add selected offices for new user in UserOfficeRelationship Table
                    foreach (var officeId in model.OfficeList)
                    {
                        if (!newUser.User_Office_Relationship.Any(t => t.OfficeId == officeId))
                        {
                            newUser.User_Office_Relationship.Add(new User_Office_Relationship { OfficeId = officeId });
                        }
                    }
                }

                var result = UserManager.Create(newUser, model.Password);
                if (!result.Succeeded)
                {
                    return Json(new { success = 0, message = "Failed to add user" });
                }
                else
                {
                    if (model.RoleId == 2 || model.RoleId == 3)
                    {
                        // Add selected physicians for new user in UserPhysicianRelationship Table
                        foreach (var physicianId in model.PhysicianList)
                        {
                            unitOfWork.UserPhysicianRelationshippRepository.Insert(new UserPhysicianRelationship { UserId = newUser.UserId, PhysicianId = physicianId });
                        }

                        unitOfWork.Save();

                        string code = newUser.UserId.ToString().Encrypt();
                        string callbackUrl = Url.Action("ConfirmEmail", "Admin", new { userId = newUser.UserId, code }, Request.Url.Scheme);
                        string template = SettingManager.GetSettingValue("Email.VerifyTemplate");
                        if (string.IsNullOrEmpty(template))
                        {
                            template =
                                @"<div lang='x-wrapper' style='min-width: 320px;background-color: #f5f7fa;' class='wrapper'><div style='Margin: 0 auto;max-width: 560px;min-width: 280px; width: 280px;width: calc(28000% - 173040px);' class='preheader'><div style='border-collapse: collapse;display: table;width: 100%;'><!--[if (mso)|(IE)]><table align='center' class='preheader' cellpadding='0' cellspacing='0'><tr><td style='width: 280px' valign='top'><![endif]--><div style='Float: left;font-size: 12px;line-height: 19px;max-width: 280px;min-width: 140px; width: 140px;width: calc(14000% - 78120px);padding: 10px 0 5px 0;color: #b9b9b9;font-family: &quot;Open Sans&quot;,sans-serif;' class='snippet'></div><!--[if (mso)|(IE)]></td><td style='width: 280px' valign='top'><![endif]--><div style='Float: left;font-size: 12px;line-height: 19px;max-width: 280px;min-width: 139px; width: 139px;width: calc(14100% - 78680px);padding: 10px 0 5px 0;text-align: right;color: #b9b9b9;font-family: &quot;Open Sans&quot;,sans-serif;' class='webversion'></div><!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div><div id='emb-email-header-container' style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 173000px);' class='header'><!--[if (mso)|(IE)]><table align='center' class='header' cellpadding='0' cellspacing='0'><tr><td style='width: 600px'><![endif]--><div align='center' style='font-size: 26px;line-height: 32px;Margin-top: 6px;Margin-bottom: 20px;color: #c3ced9;font-family: Roboto,Tahoma,sans-serif;Margin-left: 20px;Margin-right: 20px;' class='logo emb-logo-margin-box'><div align='center' id='emb-email-header' style='font-size:0px !important;line-height:0 !important;' class='logo-center'><img width='100px' alt='' src='" + System.Web.Configuration.WebConfigurationManager.AppSettings["ClientNameWebLogo"].ToString() + "' style='height: auto;width: 100%;border: 0;max-width: 100px;'></div></div><!--[if (mso)|(IE)]></td></tr></table><![endif]--></div><div style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 173000px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;' class='layout one-col fixed-width'><div emb-background-style='' style='border-collapse: collapse;display: table;width: 100%;background-color: #ffffff;' class='layout__inner'><!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0'><tr class='layout-fixed-width' emb-background-style><td style='width: 600px' class='w560'><![endif]--><div style='text-align: left;color: #60666d;font-size: 14px;line-height: 21px;font-family: &quot;Open Sans&quot;,sans-serif;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);' class='column'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 24px;'><div style='line-height:10px;font-size:1px'>&nbsp;</div></div><div style='Margin-left: 20px;Margin-right: 20px;Margin-bottom: 24px;'><h2 lang='x-size-24' style='Margin-top: 0;Margin-bottom: 0;font-style: normal;font-weight: normal;color: #44a8c7;font-size: 20px;line-height: 28px;text-align: center;' class='size-24'>Welcome to " + ClientName3 + " Physician Portal</h2><p lang='x-size-16' style='Margin-top: 16px;Margin-bottom: 0;font-size: 16px;line-height: 24px;text-align: left;' class='size-16'>Hello @FirstName @LastName</p><p style='Margin-top: 20px;Margin-bottom: 0;text-align: left;'>Please confirm your account by clicking <b>@EmailVerify</b>.</p></div></div><!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div><div style='Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 173000px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;' class='layout email-footer'><div style='border-collapse: collapse;display: table;width: 100%;' class='layout__inner'><!--[if (mso)|(IE)]><table align='center' cellpadding='0' cellspacing='0'><tr class='layout-email-footer'><td style='width: 400px;' valign='top' class='w360'><![endif]--><div style='text-align: left;font-size: 12px;line-height: 19px;color: #b9b9b9;font-family: &quot;Open Sans&quot;,sans-serif;Float: left;max-width: 400px;min-width: 320px; width: 320px;width: calc(8000% - 47600px);' class='column wide'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 10px;Margin-bottom: 10px;'><div><div>" + ClientName3 + "</div></div><div style='Margin-top: 18px;'><div>You are receiving this email because you have registered on the " + ClientName3 + " Physician Portal</div></div></div></div><!--[if (mso)|(IE)]></td><td style='width: 200px;' valign='top' class='w160'><![endif]--><div style='text-align: left;font-size: 12px;line-height: 19px;color: #b9b9b9;font-family: &quot;Open Sans&quot;,sans-serif;Float: left;max-width: 320px;min-width: 200px; width: 320px;width: calc(72200px - 12000%);' class='column narrow'><div style='Margin-left: 20px;Margin-right: 20px;Margin-top: 10px;Margin-bottom: 10px;'></div></div><!--[if (mso)|(IE)]></td></tr></table><![endif]--></div></div><div style='line-height:40px;font-size:40px;'>&nbsp;</div></div>";
                            SettingManager.Add("Email.VerifyTemplate", template);
                        }

                        template = template.Replace("@FirstName", model.FirstName)
                            .Replace("@LastName", model.LastName)
                            .Replace("@EmailVerify", "<a href=" + callbackUrl + ">here</a>")
                            .Replace("@Email", model.Email);

                        CommonHelper.SendEmail(ClientName3 + " Physician Portal Confirm your Account", template, model.Email, "");
                    }

                    if (model.RoleId == 2)
                    {
                        //New physician added,  we need to add the join date and the signature in PhysicianSignature Table
                        PhysicianSignature physicianSignature = new Core.Data.PhysicianSignature()
                        {
                            PhysicianMasterId = model.PhysicianList.Count > 0 ? model.PhysicianList[0] : 0,
                            CreatedBy = CurrentUser.UserId,
                            CreatedOn = DateTime.UtcNow,
                            JoiningDate = model.JoinDate
                        };

                        if (model.Signature != null)
                        {
                            physicianSignature.Signature = model.Signature;
                        }

                        unitOfWork.PhysicianSignatureRepository.Insert(physicianSignature);
                        unitOfWork.Save();
                    }

                    AuditLogger.AuditTracker("New User Created.", "A new user has been created with name: " + newUser.FirstName + " " + newUser.LastName + " and Email: " + newUser.Email, (int)AuditOperationType.Addition);
                }

                return Json(new { success = 1, message = "User added successfully" });
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method AddNewUser(AddUserModel model) AdminController. " + ex.Message, ex);
                return Json(new { success = 0, message = "Failed to add user: " + ex.Message });
            }
        }

        public ActionResult AddNewOffice(string returnUrl)
        {
            try
            {
                var office = new AddOfficeModel();
                ViewBag.ReturnUrl = returnUrl;
                return View(office);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddNewOffice(string returnUrl) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "AddNewOffice", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddNewOffice(AddOfficeModel model)
        {
            try
            {
                Office office = new Office();

                office.OfficeTitle = model.OfficeTitle;
                office.Address = model.Address;
                office.City = model.City;
                office.StateProvince = model.State;
                office.PostalCode = model.Zip;
                office.FaxNumber = model.FaxNumber;
                office.PhoneNumber = model.PhoneNumber;
                office.SubmitChecklistDocumentPath = model.SubmitChecklistDocumentPath;
                office.CreatedBy = CurrentUser.UserId;
                office.CreatedOn = DateTime.UtcNow;
                office.ModifiedBy = CurrentUser.UserId;
                office.ModifiedOn = DateTime.UtcNow;

                unitOfWork.OfficeRepository.Insert(office);
                unitOfWork.Save();

                AuditLogger.AuditTracker("New Office Created.", "A new office has been created with name: " + office.OfficeTitle, (int)AuditOperationType.Addition);

                return Json(new { success = 1, message = "Office added successfully" });
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method AddNewOffice(AddUserModel model) AdminController. " + ex.Message, ex);
                return Json(new { success = 0, message = "Failed to add office: " + ex.Message });
            }
        }

        public ActionResult EditUser(int selectedUserId = -1)
        {
            try
            {

                if (selectedUserId < 0)
                {
                    ModelState.AddModelError("", "Invalid UserId Format");
                    return View();
                }

                var userToEdit = unitOfWork.UserRepository.GetSingle(x => x.UserId == selectedUserId);

                if (userToEdit == null)
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return View();
                }

                List<int> officeList = new List<int>();
                List<int> physicianList = new List<int>();
                if (userToEdit.User_Office_Relationship.Any())
                {
                    officeList = userToEdit.User_Office_Relationship.Select(t => t.OfficeId).ToList();
                }
                physicianList = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == selectedUserId).Select(y => y.PhysicianId).ToList();
                DateTime? joindate = null;
                if (userToEdit.RoleId == 2 && physicianList != null)
                {
                    if (physicianList.Count > 0)
                    {
                        var physicianId = physicianList[0];
                        PhysicianSignature signature = unitOfWork.PhysicianSignatureRepository.GetAsQuerable(x => x.PhysicianMasterId == physicianId).FirstOrDefault();

                        if (signature != null)
                        {
                            joindate = signature.JoiningDate;
                        }
                    }
                }

                AddUserModel userModel = new AddUserModel()
                {
                    UserId = userToEdit.UserId,
                    RoleId = userToEdit.RoleId,
                    Email = userToEdit.Email,
                    FirstName = userToEdit.FirstName,
                    LastName = userToEdit.LastName,
                    PhoneNumber = userToEdit.PhoneNumber,
                    NPI = userToEdit.NPI,
                    IsActive = userToEdit.IsActive ?? false,
                    OfficeList = officeList,
                    PhysicianList = physicianList,
                    JoinDate = joindate
                };

                /*HAVE TO IMPLEMENT USERPHYSICIANRELATIONSHIPS*/

                return View(userModel);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method EditUser(int selectedUserId) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "EditUser", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditUser(AddUserModel model)
        {
            try
            {
                var currentUser = CurrentUser;
                var userToEdit = unitOfWork.UserRepository.GetSingle(x => x.UserId == model.UserId);

                if (!string.IsNullOrWhiteSpace(model.PhysicianListStr))
                {
                    var splittedIds = model.PhysicianListStr.Split(',');
                    if (splittedIds.Length > 0)
                    {
                        foreach (var id in splittedIds)
                        {
                            try
                            {
                                int.Parse(id);
                            }
                            catch
                            {
                                return Json(new { success = 0, message = "One or more input values were not of the expected format" });
                            }
                        }
                    }

                    model.PhysicianList = model.PhysicianListStr.Split(',').Select(t => Convert.ToInt32(t)).ToList();
                }

                if (model.RoleId == 1 || model.RoleId == 5)
                {
                    //User was edited to be a admin or SuperAdmin
                    if (model.RoleId == 5)
                    {
                        //If a superadmin is created we need to remove the office registered against him from here
                    }
                    model.NPI = null;
                }
                else if (model.RoleId == 2)
                {
                    //User was edited to be a physician
                }
                else if (model.RoleId == 3)
                {
                    //User was edited to be a technician  
                    model.NPI = null;
                }
                else if (model.RoleId == 4)
                {
                    //User was edited to be a pharmacist
                    model.NPI = null;
                }

                if (String.Equals(userToEdit.Email, model.Email) == false)
                {
                    //If the email was changed, we have to forcefully logout the user who was edited
                    UserManager.UpdateSecurityStamp(userToEdit.Id);
                }

                if (userToEdit.RoleId == 4)
                {
                    //If the user was a pharmacist before editing, if role changed to someone else we need to change the default homepage
                    if (model.RoleId != 4)
                    {
                        userToEdit.HomePageId = 1;
                    }
                    else
                    {
                        userToEdit.HomePageId = 4;
                    }
                }
                if (model.RoleId == 4)
                {
                    //If the role was changed to pharmacist we need to changed the default homepage to the pharmacist homepage
                    userToEdit.HomePageId = 4;
                }

                userToEdit.RoleId = model.RoleId;
                userToEdit.Email = model.Email;
                userToEdit.FirstName = model.FirstName;
                userToEdit.LastName = model.LastName;
                userToEdit.PhoneNumber = model.PhoneNumber;
                //userToEdit.PhysicianId = physicianId;
                userToEdit.IsActive = model.IsActive;
                userToEdit.NPI = model.NPI;

                if (model.OfficeList != null)
                {
                    //When a physician edits a user, the officelist will be null, and we do not need to make any changes to user's offices
                    // Add Office
                    var userCurrentOfficeIdList = userToEdit.User_Office_Relationship.ToList();

                    foreach (var officeId in model.OfficeList)
                    {
                        for (int i = 0; i < userCurrentOfficeIdList.Count(); i++)
                        {
                            if (userCurrentOfficeIdList[i].OfficeId == officeId)
                            {
                                userCurrentOfficeIdList.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    // Delete User Office
                    foreach (var userOfficeRemove in userCurrentOfficeIdList)
                    {
                        unitOfWork.User_Office_RelationshipRepository.Delete(userOfficeRemove);
                    }

                    foreach (var officeId in model.OfficeList)
                    {
                        if (!userToEdit.User_Office_Relationship.Any(t => t.OfficeId == officeId))
                        {
                            userToEdit.User_Office_Relationship.Add(new User_Office_Relationship { OfficeId = officeId });
                        }
                    }
                }

                //Removing all existing UserPhysician Relationships
                var relationshipList = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == model.UserId);

                foreach (var physicianRelation in relationshipList)
                {
                    unitOfWork.UserPhysicianRelationshippRepository.Delete(physicianRelation);
                }
                if (model.PhysicianList != null)
                {
                    //When a physician edits a user, the physicianlist will be null, and we do not need to make any changes to user's offices
                    //Adding new UserPhysician Relationships

                    foreach (var physicianId in model.PhysicianList)
                    {
                        unitOfWork.UserPhysicianRelationshippRepository.Insert(new UserPhysicianRelationship { UserId = model.UserId, PhysicianId = physicianId });
                    }
                }

                unitOfWork.UserRepository.Update(userToEdit);
                unitOfWork.Save();

                if (model.RoleId == 2 && model.PhysicianList != null)
                {
                    if (model.PhysicianList.Count > 0)
                    {
                        var physicianid = model.PhysicianList[0];
                        PhysicianSignature physicianSignature = unitOfWork.PhysicianSignatureRepository.GetAsQuerable(x => x.PhysicianMasterId == physicianid).FirstOrDefault();

                        if (physicianSignature == null)
                        {
                            //New physician added,  we need to add the join date in PhysicianSignature Table
                            physicianSignature = new Core.Data.PhysicianSignature()
                            {
                                PhysicianMasterId = model.PhysicianList.Count > 0 ? model.PhysicianList[0] : 0,
                                CreatedBy = CurrentUser.UserId,
                                CreatedOn = DateTime.UtcNow,
                                JoiningDate = model.JoinDate
                            };
                            unitOfWork.PhysicianSignatureRepository.Insert(physicianSignature);
                        }
                        else
                        {
                            physicianSignature.JoiningDate = model.JoinDate;
                            physicianSignature.ModifiedBy = CurrentUser.UserId;
                            physicianSignature.ModifiedOn = DateTime.UtcNow;
                            unitOfWork.PhysicianSignatureRepository.Update(physicianSignature);
                        }
                        if (model.Signature != null)
                        {
                            physicianSignature.Signature = model.Signature;
                        }
                        unitOfWork.Save();
                    }
                }

                //var token = UserManager.GeneratePasswordResetToken(userToEdit.Id);

                AuditLogger.AuditTracker("Existing User with UserId: " + userToEdit.Id + "  was edited.", "UserId = " + userToEdit.Id, (int)AuditOperationType.Change);
                return Json(new { success = 1, message = "User edited successfully." });
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method EditUser(AddUserModel model) AdminController. " + ex.Message, ex);
                return Json(new { success = 0, message = "Failed to edit user: " + ex.Message });
            }
        }


        [HttpPost]
        public ActionResult EditPhysicianSignature(EditPhysicianSignature model)
        {
            try
            {
                PhysicianSignature physicianSignature = unitOfWork.PhysicianSignatureRepository.GetAsQuerable(x => x.PhysicianMasterId == model.PhysicianId).FirstOrDefault();

                if (physicianSignature == null)
                {
                    //New physician added,  we need to add the join date in PhysicianSignature Table
                    physicianSignature = new Core.Data.PhysicianSignature()
                    {
                        PhysicianMasterId = model.PhysicianId,
                        CreatedBy = CurrentUser.UserId,
                        CreatedOn = DateTime.UtcNow,
                        Signature = model.Signature
                        //JoiningDate = model.JoinDate
                    };
                    unitOfWork.PhysicianSignatureRepository.Insert(physicianSignature);
                }
                else
                {
                    //physicianSignature.JoiningDate = model.JoinDate;
                    physicianSignature.ModifiedBy = CurrentUser.UserId;
                    physicianSignature.ModifiedOn = DateTime.UtcNow;
                    physicianSignature.Signature = model.Signature;
                    unitOfWork.PhysicianSignatureRepository.Update(physicianSignature);
                }
                unitOfWork.Save();

                AuditLogger.AuditTracker("Signature for physician was added/edited", "PhysicianId = " + model.PhysicianId, (int)AuditOperationType.Change);
                return Json(new { message = "Signature added successfully", success = true });

            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method EditPhysicianSignature(EditPhysicianSignature model) AdminController. " + e.Message, e);
                return Json(new { success = 0, message = "Failed to edit user: " + e.Message });
            }
        }

        public ActionResult ResetUserPassword(int UserId)
        {
            try
            {
                return View(new ResetUserPasswordViewModel { UserId = UserId });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ResetUserPassword() AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "ResetUserPassword", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ResetUserPassword(ResetUserPasswordViewModel resetUserPasswordViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = unitOfWork.UserRepository.GetSingle(x => x.UserId == resetUserPasswordViewModel.UserId);
        //            if (user != null)
        //            {
        //                var token = UserManager.GeneratePasswordResetToken(user.Id);
        //                var result = UserManager.ResetPassword(user.Id, token, resetUserPasswordViewModel.NewPassword);
        //                if (result.Succeeded)
        //                {
        //                    AuditLogger.AuditTracker("Password reset of user name \'" + user.FullName + "\'.", "UserId = " + user.Id, (int)AuditOperationType.Change);
        //                    return Json(new { message = "User password reset successfully", success = true });
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return View(resetUserPasswordViewModel);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.InsertLog("Exception thrown in Method ResetUserPassword() AdminController. " + exception.Message, exception);
        //        return Json(new { message = "Failed to reset user password " + exception.Message, success = false });
        //    }
        //    return Json(new { message = "Failed to reset user password", success = false });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ResetUserPassword(ResetUserPasswordViewModel resetUserPasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = unitOfWork.UserRepository.GetSingle(x => x.UserId == resetUserPasswordViewModel.UserId);
                    if (user != null)
                    {
                        var token = UserManager.GeneratePasswordResetToken(user.Id);
                        var result = UserManager.ResetPassword(user.Id, token, resetUserPasswordViewModel.NewPassword);
                        if (result.Succeeded)
                        {
                            AuditLogger.AuditTracker("Password reset of user name \'" + user.FullName + "\'.", "UserId = " + user.Id, (int)AuditOperationType.Change);
                            return Json(new { message = "User password reset successfully", success = true });
                        }
                        else if (result.Errors.Count() > 0)
                        {
                            return Json(new { message = result.Errors.FirstOrDefault(), success = false });
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ResetUserPassword() AdminController. " + exception.Message, exception);
                return Json(new { message = "Failed to reset user password " + exception.Message, success = false });
            }
            return Json(new { message = "Failed to reset user password", success = false });
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult EditOffice(int selectedOfficeId)
        {
            try
            {
                var officeToEdit = unitOfWork.OfficeRepository.GetSingle(x => x.OfficeId == selectedOfficeId);

                AddOfficeModel officeModel = new AddOfficeModel
                {
                    OfficeId = officeToEdit.OfficeId,
                    OfficeTitle = officeToEdit.OfficeTitle,
                    Address = officeToEdit.Address,
                    PhoneNumber = officeToEdit.PhoneNumber,
                    FaxNumber = officeToEdit.FaxNumber,
                    City = officeToEdit.City,
                    State = officeToEdit.StateProvince,
                    Zip = officeToEdit.PostalCode,
                    SubmitChecklistDocumentPath = officeToEdit.SubmitChecklistDocumentPath
                };

                return View(officeModel);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method EditOffice(int selectedOfficeId) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "EditOffice", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOffice(AddOfficeModel model)
        {
            try
            {
                Office existingOffice = unitOfWork.OfficeRepository.GetSingle(x => x.OfficeId == model.OfficeId);

                existingOffice.OfficeTitle = model.OfficeTitle;
                existingOffice.Address = model.Address;
                existingOffice.City = model.City;
                existingOffice.StateProvince = model.State;
                existingOffice.PhoneNumber = model.PhoneNumber;
                existingOffice.FaxNumber = model.FaxNumber;
                existingOffice.PostalCode = model.Zip;
                existingOffice.SubmitChecklistDocumentPath = model.SubmitChecklistDocumentPath;
                existingOffice.ModifiedBy = CurrentUser.UserId;
                existingOffice.ModifiedOn = DateTime.UtcNow;

                unitOfWork.OfficeRepository.Update(existingOffice);
                unitOfWork.Save();

                AuditLogger.AuditTracker("Existing Office with OfficeId: " + existingOffice.OfficeId + "  was edited.", "OfficeId = " + existingOffice.OfficeId, (int)AuditOperationType.Change);

                //return Json(new { success = 1, message = "Office edited successfully" });
                return Redirect("ManageOffices");
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method EditOffice(AddOfficeModel model) AdminController. " + exception.Message, exception);
                //return Json(new { success = 0, message = "Failed to edit office: " + exception.Message });
                return View("Error", new ErrorModel(exception, "Admin", "EditOffice", ErrorTitle, ErrorMessage, null));
            }
        }

        public JsonResult DeleteUser(AddUserModel model)
        {
            try
            {
                var userToDelete = unitOfWork.UserRepository.GetSingle(x => x.UserId == model.UserId);

                if (userToDelete != null)
                {
                    //Remove office Relationships

                    var officeRelationships = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.UserId == model.UserId);

                    foreach (var relationship in officeRelationships)
                    {
                        unitOfWork.User_Office_RelationshipRepository.Delete(relationship);
                    }
                    unitOfWork.Save();

                    //Remove physician Relationships

                    var physicianRelationships = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == model.UserId);

                    foreach (var relationship in physicianRelationships)
                    {
                        unitOfWork.UserPhysicianRelationshippRepository.Delete(relationship);
                    }
                    unitOfWork.Save();


                    unitOfWork.UserRepository.Delete(userToDelete);
                    unitOfWork.Save();

                    AuditLogger.AuditTracker("Existing User was Deleted.", "UserId = " + userToDelete.Id, (int)AuditOperationType.Deletion);
                    return Json(new { message = "User Deleted Successfully.", success = true });
                }
                return Json(new { message = "User record not found.", success = false });
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method DeleteUser AdminController. " + e.Message, e);
                return Json(new { message = e.Message, success = false });
            }
        }

        public ActionResult GetPhysicians()
        {
            try
            {
                IQueryable physiciansList = unitOfWork.PhysiciansRepository.GetAsQuerable().Take(20);
                return Json(physiciansList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPhysicians AdminController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult ErrorLogs()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ErrorLogs AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "ErrorLogs", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult PatientDocuments()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method PatientDocuments AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "PatientDocuments", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult PatientDetailDocuments()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method PatientDetailDocuments AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "PatientDetailDocuments", ErrorTitle, ErrorMessage, null));
            }
        }




        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult ReferralDocuments()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ReferralDocuments AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "ReferralDocuments", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult RefillDocuments()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method RefillDocuments AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "RefillDocuments", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult GetErrorLogs([DataSourceRequest] DataSourceRequest request, string sreachString = null, ErrorLog _ErrorLog = null)
        {
            try
            {
                var currentUser = CurrentUser;
                IQueryable ErrorLogList;
                if (UserManager.IsInRoleAsync(currentUser.Id, "SuperAdmin").Result)
                {
                    ErrorLogList = unitOfWork.ErrorLogRepository.GetAsQuerable();
                    return Json(ErrorLogList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetErrorLogs AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "Index", ErrorTitle, ErrorMessage, null));
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GenerateDocumentsReport(ActivityLogReportViewModel activityLogReportViewModel)
        {
            try
            {
                IQueryable<vwAuditTrail> AuditTrailList = null;
                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result)
                {

                    DateTime startDate = activityLogReportViewModel.StartDate ?? DateTime.UtcNow;
                    string format = "yyyy-MM-dd";
                    string strDateTime = startDate.ToString(format);

                    DateTime endDate = activityLogReportViewModel.EndDate ?? DateTime.UtcNow;
                    string strEndDateTime = startDate.ToString(format);


                    activityLogReportViewModel.EndDate = activityLogReportViewModel.EndDate.Value.AddDays(1).AddSeconds(-1);

                    var ruleDate = strEndDateTime;
                    //AuditTrailList = unitOfWork.AuditTrailRepository.GetAsQuerable().Select(t => new { t.AuditStamp, t.LogMessage, t.Data, t.Url, t.Device });
                    AuditTrailList = unitOfWork.vwAuditTrailRepository.GetAsQuerable(t => t.AuditStamp >= activityLogReportViewModel.StartDate && t.AuditStamp <= activityLogReportViewModel.EndDate);



                    StringBuilder htmlString = new StringBuilder();

                    htmlString.Append("<html><head><meta charset=\"utf-8\" /><title></title>");

                    htmlString.Append("<link href=\"" + Server.MapPath("~/Content/bootstrap.css") + "\" rel=\"stylesheet\" media = \"all\" />");
                    htmlString.Append("<link href=\"" + Server.MapPath("~/Content/Site.css") + "\" rel=\"stylesheet\" media = \"all\" />");
                    //htmlString.Append("<link href=\"" + Server.MapPath("~/Content/pdfForms.css") + "\" rel=\"stylesheet\" media = \"all\" />");
                    //htmlString.Append("<link href=\"" + Server.MapPath("~/Content/PrintForm.css") + "\" rel=\"stylesheet\" media = \"all\" />");

                    htmlString.Append("</head><body>");
                    htmlString.Append("</body></html>");

                    var finalHTML = htmlString.ToString();

                    var doc1 = new HtmlDocument();
                    doc1.LoadHtml(finalHTML);


                    HtmlNode body = doc1.DocumentNode.SelectSingleNode("//body");

                    HtmlNode heading = doc1.CreateElement("h2");
                    heading.InnerHtml = "Activity Log";
                    body.AppendChild(heading);

                    body.SetAttributeValue("style", "text-align: center");

                    HtmlNode table = doc1.CreateElement("table");
                    table.AddClass("table table-hover table-bordered audit-log-pdf-table");
                    body.AppendChild(table);

                    HtmlNode thead = doc1.CreateElement("thead");
                    HtmlNode theadTr = doc1.CreateElement("tr");

                    HtmlNode th1 = doc1.CreateElement("th");
                    th1.InnerHtml = "Message";

                    HtmlNode th2 = doc1.CreateElement("th");
                    th2.InnerHtml = "Data";

                    HtmlNode th3 = doc1.CreateElement("th");
                    th3.InnerHtml = "User";

                    HtmlNode th4 = doc1.CreateElement("th");
                    th4.InnerHtml = "Date";

                    theadTr.AppendChild(th1);
                    theadTr.AppendChild(th2);
                    theadTr.AppendChild(th3);
                    theadTr.AppendChild(th4);

                    thead.AppendChild(theadTr);

                    table.AppendChild(thead);

                    HtmlNode tBody = doc1.CreateElement("tbody");

                    foreach (var item in AuditTrailList)
                    {
                        HtmlNode tr = doc1.CreateElement("tr");

                        HtmlNode td1 = doc1.CreateElement("td");
                        td1.InnerHtml = item.LogMessage;

                        HtmlNode td2 = doc1.CreateElement("td");
                        td2.InnerHtml = item.Data;

                        HtmlNode td3 = doc1.CreateElement("td");
                        td3.InnerHtml = unitOfWork.UserRepository.GetSingle(x => x.UserId == item.UserId).FullName;

                        HtmlNode td4 = doc1.CreateElement("td");
                        td4.InnerHtml = item.AuditStamp.Value.ToString("hh:mm:ss dd/MM/yyyy");

                        tr.AppendChild(td1);
                        tr.AppendChild(td2);
                        tr.AppendChild(td3);
                        tr.AppendChild(td4);

                        tBody.AppendChild(tr);
                    }
                    table.AppendChild(tBody);

                    finalHTML = doc1.DocumentNode.OuterHtml;

                    var document = new HtmlToPdfDocument
                    {
                        GlobalSettings =
                            {
                                ProduceOutline = true,
                                DocumentTitle = "Document Report",
                                PaperSize = PaperKind.Letter, // Implicit conversion to PechkinPaperSize
                                Margins =
                                {
                                    All = 10,
                                    Unit = Unit.Millimeters
                                }
                            },
                        Objects = {
                                new ObjectSettings { HtmlText = finalHTML },
                            }
                    };

                    //var buffer = PDFHelper.SharedInstance.Convert(document);

                    byte[] contents = PDFHelper.SharedInstance.Convert(document);
                    Response.AddHeader("Content-Disposition", "inline; filename=test.pdf");
                    return File(contents, "application/pdf");


                    //var filePath = Path.Combine(Server.MapPath("~/Document/FilledPDF"), "ActivityLog.pdf");

                    //System.IO.File.WriteAllBytes(filePath, buffer);

                    //Response.AddHeader("Content-Disposition", "inline; filename=auditlog.pdf");
                    //return File(buffer, "application/pdf");

                }
                return null;
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method LogoutOtherUser AdminController. " + exception.Message, exception);
                return null;
            }

        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult ActivityLogs()
        {
            try
            {
                ActivityLogReportViewModel activityLogReportViewModel = new ActivityLogReportViewModel();
                return View(activityLogReportViewModel);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ActivityLogs AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "ActivityLogs", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult GetAuditTrail([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IQueryable AuditTrailList;
                if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result)
                {
                    //AuditTrailList = unitOfWork.AuditTrailRepository.GetAsQuerable().Select(t => new { t.AuditStamp, t.LogMessage, t.Data, t.Url, t.Device });
                    AuditTrailList = unitOfWork.vwAuditTrailRepository.GetAsQuerable();//.Select(t => new { t.AuditStamp, t.LogMessage, t.Data, t.Url, t.Device });
                    return Json(AuditTrailList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetAuditTrail AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "GetAuditTrail", ErrorTitle, ErrorMessage, null));
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]

        //public FileResult ExportToPdf()
        //{
        //    try
        //    {
        //        List<vwAuditTrail> AuditTrailList = null;
        //        if (UserManager.IsInRoleAsync(CurrentUser.Id, "SuperAdmin").Result)
        //        {
        //            DateTime time = DateTime.Now;
        //            string format = "yyyy-MM-dd";
        //            string strDateTime = time.ToString(format);

        //            var ruleDate = DateTime.Now.Date;
        //            //AuditTrailList = unitOfWork.AuditTrailRepository.GetAsQuerable().Select(t => new { t.AuditStamp, t.LogMessage, t.Data, t.Url, t.Device });
        //            AuditTrailList = unitOfWork.vwAuditTrailRepository.GetAsQuerable(t=>t.AuditStamp.Value.Year == ruleDate.Year && t.AuditStamp.Value.Month == ruleDate.Month && t.AuditStamp.Value.Day == ruleDate.Day).ToList();//.Select(t => new { t.AuditStamp, t.LogMessage, t.Data, t.Url, t.Device });




        //            StringBuilder htmlString = new StringBuilder();

        //            htmlString.Append("<html><head><meta charset=\"utf-8\" /><title></title>");

        //            htmlString.Append("<link href=\"" + Server.MapPath("~/Content/bootstrap.css") + "\" rel=\"stylesheet\" media = \"all\" />");
        //            htmlString.Append("<link href=\"" + Server.MapPath("~/Content/Site.css") + "\" rel=\"stylesheet\" media = \"all\" />");
        //            //htmlString.Append("<link href=\"" + Server.MapPath("~/Content/pdfForms.css") + "\" rel=\"stylesheet\" media = \"all\" />");
        //            //htmlString.Append("<link href=\"" + Server.MapPath("~/Content/PrintForm.css") + "\" rel=\"stylesheet\" media = \"all\" />");

        //            htmlString.Append("</head><body>");
        //            htmlString.Append("</body></html>");

        //            var finalHTML = htmlString.ToString();

        //            var doc1 = new HtmlDocument();
        //            doc1.LoadHtml(finalHTML);


        //            HtmlNode body = doc1.DocumentNode.SelectSingleNode("//body");

        //            HtmlNode heading = doc1.CreateElement("h2");
        //            heading.InnerHtml = "Activity Log";
        //            body.AppendChild(heading);

        //            body.SetAttributeValue("style", "text-align: center");

        //            HtmlNode table = doc1.CreateElement("table");
        //            table.AddClass("table table-hover table-bordered audit-log-pdf-table");
        //            body.AppendChild(table);

        //            HtmlNode thead = doc1.CreateElement("thead");
        //            HtmlNode theadTr = doc1.CreateElement("tr");

        //            HtmlNode th1 = doc1.CreateElement("th");
        //            th1.InnerHtml = "Message";

        //            HtmlNode th2 = doc1.CreateElement("th");
        //            th2.InnerHtml = "Data";

        //            HtmlNode th3 = doc1.CreateElement("th");
        //            th3.InnerHtml = "User";

        //            HtmlNode th4 = doc1.CreateElement("th");
        //            th4.InnerHtml = "Date";

        //            theadTr.AppendChild(th1);
        //            theadTr.AppendChild(th2);
        //            theadTr.AppendChild(th3);
        //            theadTr.AppendChild(th4);

        //            thead.AppendChild(theadTr);

        //            table.AppendChild(thead);

        //            HtmlNode tBody = doc1.CreateElement("tbody");

        //            foreach (var item in AuditTrailList)
        //            {
        //                HtmlNode tr = doc1.CreateElement("tr");

        //                HtmlNode td1 = doc1.CreateElement("td");
        //                td1.InnerHtml = item.LogMessage;

        //                HtmlNode td2 = doc1.CreateElement("td");
        //                td2.InnerHtml = item.Data;

        //                HtmlNode td3 = doc1.CreateElement("td");
        //                td3.InnerHtml = unitOfWork.UserRepository.GetSingle(x => x.UserId == item.UserId).FullName;

        //                HtmlNode td4 = doc1.CreateElement("td");
        //                td4.InnerHtml = item.AuditStamp.Value.ToString("hh:mm:ss dd/MM/yyyy");

        //                tr.AppendChild(td1);
        //                tr.AppendChild(td2);
        //                tr.AppendChild(td3);
        //                tr.AppendChild(td4);

        //                tBody.AppendChild(tr);
        //            }
        //            table.AppendChild(tBody);

        //            finalHTML = doc1.DocumentNode.OuterHtml;





        //            var document = new HtmlToPdfDocument
        //            {
        //                GlobalSettings =
        //            {
        //                ProduceOutline = true,
        //                DocumentTitle = "Test Pdf",
        //                PaperSize = PaperKind.Letter, // Implicit conversion to PechkinPaperSize
        //                Margins =
        //                {
        //                    All = 10,
        //                    Unit = Unit.Millimeters
        //                }
        //            },
        //                Objects = {
        //                new ObjectSettings { HtmlText = finalHTML },
        //            }
        //            };



        //            var buffer = pdfConverter.Convert(document);



        //            //var filePath = Path.Combine(Server.MapPath("~/Document/FilledPDF"), "ActivityLog.pdf");

        //            //System.IO.File.WriteAllBytes(filePath, buffer);

        //            //Response.AddHeader("Content-Disposition", "inline; filename=auditlog.pdf");
        //            return  File(buffer, "application/pdf");

        //        }
        //        return null;
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.InsertLog("Exception thrown in Method LogoutOtherUser AdminController. " + exception.Message, exception);
        //        return null;
        //    }
        //}

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult ApplicationSettings()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ApplicationSettings AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "ApplicationSettings", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult ManageOffices()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ManageOffices AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "ManageOffices", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult ManageOfficeUsers(int officeId)
        {
            try
            {
                OfficeUsersViewModel model = new OfficeUsersViewModel { OfficeId = officeId };
                return View(model);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ManageOfficeUsers AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "ManageOfficeUsers", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult GetAppSettings([DataSourceRequest] DataSourceRequest request, string sreachString = null, ApplicationSetting _ApplicationSetting = null)
        {
            try
            {
                var currentUser = CurrentUser;
                IQueryable AppSettingsList;
                if (UserManager.IsInRoleAsync(currentUser.Id, "SuperAdmin").Result)
                {
                    AppSettingsList = unitOfWork.ApplicationSettingRepository.GetAsQuerable();
                    return Json(AppSettingsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetAppSetting AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "GetAppSettings", ErrorTitle, ErrorMessage, null));
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewSetting(string returnUrl)
        {
            try
            {
                var AppSetting = new ApplicationSetting();
                //ViewBag.ReturnUrl = returnUrl;
                return PartialView("AddNewSetting", AppSetting);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddNewSetting AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "AddNewSetting", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddNewSetting(ApplicationSetting model)
        {
            try
            {
                ApplicationSetting newSetting = new ApplicationSetting()
                {
                    Name = model.Name,
                    Value = model.Value
                };

                SettingManager.Add(newSetting.Name, newSetting.Value);
                //unitOfWork.ApplicationSettingRepository.Insert(model);
                //unitOfWork.Save();
                AuditLogger.AuditTracker("New Setting Created.", "A new setting has been created with name: " + model.Name, (int)AuditOperationType.Addition);
                return Json(new { success = 1, message = "Setting Added Successfully" });
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method AddNewSetting AdminController. " + ex.Message, ex);
                return Json(new { success = 0, message = "Failed to Add Setting: " + ex.Message });
            }
        }

        public ActionResult EditSetting(int ApplicationSettingId)
        {
            try
            {
                var settingToEdit = unitOfWork.ApplicationSettingRepository.GetSingle(x => x.ApplicationSettingId == ApplicationSettingId);

                ApplicationSetting applicationSetting = new ApplicationSetting()
                {
                    ApplicationSettingId = settingToEdit.ApplicationSettingId,
                    Name = settingToEdit.Name,
                    Value = settingToEdit.Value
                };
                return PartialView("EditSetting", applicationSetting);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method EditSetting(int ApplicationSettingId) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "EditSetting", ErrorTitle, ErrorMessage, null));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditSetting(ApplicationSetting model)
        {
            try
            {
                var editSetting = unitOfWork.ApplicationSettingRepository.GetSingle(x => x.ApplicationSettingId == model.ApplicationSettingId);
                editSetting.Name = model.Name;
                editSetting.Value = model.Value;


                unitOfWork.ApplicationSettingRepository.Update(editSetting);
                unitOfWork.Save();
                AuditLogger.AuditTracker("Existing Setting was Edited.", "SettingId = " + editSetting.ApplicationSettingId, (int)AuditOperationType.Change);
                return Json(new { success = 1, message = "Setting Edited Successfully" });
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method EditSetting(ApplicationSetting model) AdminController. " + ex.Message, ex);
                return Json(new { success = 0, message = "Failed to Edit Setting: " + ex.Message });
            }
        }

        public JsonResult DeleteSetting(ApplicationSetting model)
        {
            try
            {
                SettingManager.Delete(model.ApplicationSettingId);
                AuditLogger.AuditTracker("Existing Setting was Deleted.", "SettingId = " + model.ApplicationSettingId, (int)AuditOperationType.Deletion);
                return Json(new { message = "Setting Deleted Successfully.", success = true });
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method DeleteSetting AdminController. " + e.Message, e);
                return Json(new { message = e.Message, success = false });
            }
        }

        public JsonResult ValidateEmail(string email)
        {
            try
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    if (addr.Address != email)
                    {
                        return Json(new { success = false, message = "The email entered is not of correct format" });
                    }
                }
                catch
                {
                    return Json(new { success = false, message = "The email entered is not of correct format" });
                }

                var emailExists = unitOfWork.UserRepository.GetAsQuerable(x => x.Email == email);
                if (emailExists.Count() > 0)
                {
                    return Json(new { success = false, message = "A user already exists with this email" });
                }
                else
                {
                    return Json(new { success = true, message = "This email is valid" });
                }

            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ValidateEmail AdminController. " + exception.Message, exception);
                return Json(new { success = false, message = "An error occured" });
            }
        }

        public JsonResult VerifyUser(int userId)
        {
            try
            {
                var userToChnage = unitOfWork.UserRepository.GetSingle(x => x.UserId == userId);
                userToChnage.IsVerified = true;

                unitOfWork.UserRepository.Update(userToChnage);
                unitOfWork.Save();

                return Json(new { success = true, message = "User verified" });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method VerifyUser AdminController. " + exception.Message, exception);
                return Json(new { success = false, message = "An error occured" });
            }
        }

        public JsonResult UnverifiedUser(int userId)
        {
            try
            {
                var userToChnage = unitOfWork.UserRepository.GetSingle(x => x.UserId == userId);
                userToChnage.IsVerified = false;

                unitOfWork.UserRepository.Update(userToChnage);
                unitOfWork.Save();

                return Json(new { success = true, message = "User Unverified" });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method UnverifiedUser AdminController. " + exception.Message, exception);
                return Json(new { success = false, message = "An error occured" });
            }
        }

        public JsonResult ImpersonateUser(int userId)
        {
            try
            {
                var currentUserId = CurrentUser.UserId;
                var impersonatingUserId = userId;
                var impersonatedUser = UserManager.FindById(impersonatingUserId.ToString());

                //First we create a new identity of the user we want to imperosnate and add claims to that identity so that on SecurityStampValidation we can check if the current session 
                //is an impersonation session (Check Startup.auth)
                var impersonatedIdentity = UserManager.CreateIdentity(impersonatedUser, DefaultAuthenticationTypes.ApplicationCookie);
                impersonatedIdentity.AddClaim(new Claim("IsImpersonating", "true"));
                impersonatedIdentity.AddClaim(new Claim("OriginalIdOfImpersonatingUser", currentUserId.ToString()));


                //Then we signout the current user(Super Admin) and sign in using the newly created impersonated Identity
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, impersonatedIdentity);


                return Json(new { success = true, message = "Impersonation Successfull" });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ImpersonateUser AdminController. " + exception.Message, exception);
                return Json(new { success = false, message = "An error occured" });
            }
        }

        public JsonResult EndImpersonationSession()
        {
            try
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var identity = (ClaimsIdentity)User.Identity;

                if (!identity.HasClaim(x => x.Type == "OriginalIdOfImpersonatingUser"))
                {
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return Json(new { success = false, message = "ID for original user not found" });
                }

                string impersonatingUserId = identity.FindFirst("OriginalIdOfImpersonatingUser").Value;
                var adminUser = UserManager.FindById(impersonatingUserId);

                var originalIdentity = UserManager.CreateIdentity(adminUser, DefaultAuthenticationTypes.ApplicationCookie);

                authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, originalIdentity);

                return Json(new { success = true, message = "Impersonation Ended Successfull" });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method EndImpersonationSession AdminController. " + exception.Message, exception);
                return Json(new { success = false, message = "An error occured" });
            }
        }

        public JsonResult LogoutOtherUser(string userId)
        {
            try
            {
                UserManager.UpdateSecurityStamp(userId);
                return Json(new { success = true, message = "User has been logged out from all devices" });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method LogoutOtherUser AdminController. " + exception.Message, exception);
                return Json(new { success = false, message = "Could not logout user" });
            }
        }

        public JsonResult GetPhysicianDetails(string npiNo)
        {
            try
            {
                IQueryable PhysicianList;

                PhysicianList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => t.NationalProviderIdentifier == npiNo);

                //return Json(PhysicianList, JsonRequestBehavior.AllowGet);

                return Json(new { success = true, message = "NPI found", PhysicianList = PhysicianList });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPhysicianDetails AdminController. " + exception.Message, exception);
                return Json(new { success = false, message = "Unable to find Physician" });
            }
        }

        [AllowAnonymous]
        public ActionResult GetOfficeSelectListItems([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var OfficeList = unitOfWork.OfficeRepository.GetAsQuerable().Select(t => new { t.OfficeId, t.OfficeTitle });

                if (UserManager.IsInRoleAsync(CurrentUser.Id, "Admin").Result)
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();

                    OfficeList = unitOfWork.OfficeRepository.GetAsQuerable(t => t.OfficeId == currentOffice)
                        .Select(t => new { t.OfficeId, t.OfficeTitle });
                }
                return Json(OfficeList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetOfficeSelectListItems AdminController. " + exception.Message, exception);
                return Json(null);
            }
        }

        public ActionResult GetPhysicianSelectListItems(string text, string PhysicianIdListStr)
        {
            try
            {
                IQueryable<Physican_Id_NameViewModel> PhysicianList = Enumerable.Empty<Physican_Id_NameViewModel>().AsQueryable();

                if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(PhysicianIdListStr))
                {
                    var physicianIdList = PhysicianIdListStr.Split(',').Select(t => Convert.ToInt32(t)).ToList();
                    PhysicianList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => physicianIdList.Contains(t.PhysicianId))
                        .Select(t => new Physican_Id_NameViewModel { PhysicianId = t.PhysicianId, FullName = t.FullName, NPI = t.NationalProviderIdentifier });
                }
                else
                {
                    //text = string.IsNullOrWhiteSpace(text) ? "a" : text;
                    text = string.IsNullOrWhiteSpace(text) ? "" : text;

                    if (UserManager.IsInRole(CurrentUser.Id, "Admin"))
                    {
                        int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                        
                        var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                        var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                        PhysicianList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => t.FullName.Contains(text) && physicianIds.Contains(t.PhysicianId))
                            .Select(t => new Physican_Id_NameViewModel { PhysicianId = t.PhysicianId, FullName = t.FullName, NPI = t.NationalProviderIdentifier })
                            .Take(AutoCompleteListSize);
                    }
                    else
                    {
                        PhysicianList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => t.FullName.Contains(text))
                            .Select(t => new Physican_Id_NameViewModel { PhysicianId = t.PhysicianId, FullName = t.FullName, NPI = t.NationalProviderIdentifier })
                            .Take(AutoCompleteListSize);
                    }
                }
                return Json(PhysicianList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPhysicianSelectListItems AdminController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPhysicianByNPI(string text, string OfficeList, int RoleId)
        {
            try
            {
                IQueryable<vwPhysician> PhysicianList = Enumerable.Empty<vwPhysician>().AsQueryable();
                List<int> OfficeIds = null;

                if (!string.IsNullOrWhiteSpace(OfficeList))
                {
                    OfficeIds = OfficeList.Split(',').Select(t => Convert.ToInt32(t)).ToList();
                }

                text = string.IsNullOrWhiteSpace(text) ? "" : text;

                if (UserManager.IsInRole(CurrentUser.Id, "Admin"))
                {
                    int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(x => x.OfficeId == currentOffice).Select(x => x.UserId).ToList();
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();
                    PhysicianList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => t.NationalProviderIdentifier.Contains(text) && physicianIds.Contains(t.PhysicianId))
                        .Take(AutoCompleteListSize);
                }
                else if (OfficeIds != null && RoleId != 2)
                {
                    //int currentOffice = UserManager.GetClaims(CurrentUser.Id).FirstOrDefault(c => c.Type == "UserOffice").Value.ToInt0();
                    //var userIds = unitOfWork.UserRepository.GetAsQuerable(t => t.User_Office_Relationship.Select(s => s.OfficeId).Contains(currentOffice)).Select(s => s.UserId);
                    var userIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(t => OfficeIds.Contains(t.OfficeId)).Select(t => t.UserId);
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => userIds.Contains(x.UserId)).Select(y => y.PhysicianId);
                    PhysicianList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => t.NationalProviderIdentifier.Contains(text) && physicianIds.Contains(t.PhysicianId))
                        .Take(AutoCompleteListSize);
                }
                else
                {
                    PhysicianList = unitOfWork.PhysiciansRepository.GetAsQuerable(t => t.NationalProviderIdentifier.Contains(text))
                        .Take(AutoCompleteListSize);
                }
                return Json(PhysicianList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPhysicianByNPI AdminController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [OverrideAuthorization()]
        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                //return View("Error");
                return View("Error", new ErrorModel(new Exception(), "Admin", "ConfirmEmail", ErrorTitle, ErrorMessage, null));
            }
            if (code.Decrypt() == userId)
            {
                //// Login 
                //var currentUserId = userId;
                //var newUser = UserManager.FindById(currentUserId.ToString());
                //newUser.EmailVarified = true;
                //unitOfWork.Save();
                //var newUserIdentity = UserManager.CreateIdentity(newUser, DefaultAuthenticationTypes.ApplicationCookie);                
                //var authenticationManager = HttpContext.GetOwinContext().Authentication;
                //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, newUserIdentity);

                long uId = long.Parse(userId);
                User user = unitOfWork.UserRepository.GetSingle(t => t.UserId == uId);
                if (user != null)
                    user.EmailVarified = true;
                unitOfWork.Save();
                return View("ConfirmEmail");
            }
            //if (code.Decrypt() == userId)
            //{
            //    long uId = long.Parse(userId);
            //    User user = db.Users.SingleOrDefault(t => t.UserId == uId);
            //    if (user != null)
            //        user.EmailVarified = true;
            //    db.SaveChanges();
            //    return View("ConfirmEmail");
            //}
            //return View("Error");
            return View("ConfirmEmail");

            //return RedirectToAction("HomeDetail", "Home");
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult NewUsers()
        {
            try
            {
                OfficeUsersViewModel model = new OfficeUsersViewModel { OfficeId = 0 };
                return View(model);
                //return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method NewUsers AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "NewUsers", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult VerifyNewUser(int selectedUserId)
        {
            try
            {
                var userToEdit = unitOfWork.UserRepository.GetSingle(x => x.UserId == selectedUserId);

                List<int> officeList = new List<int>();
                List<int> physicianList = new List<int>();
                if (userToEdit.User_Office_Relationship.Any())
                {
                    officeList = userToEdit.User_Office_Relationship.Select(t => t.OfficeId).ToList();
                }
                physicianList = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == selectedUserId).Select(y => y.PhysicianId).ToList();

                AddUserModel userModel = new AddUserModel()
                {
                    UserId = userToEdit.UserId,
                    RoleId = userToEdit.RoleId,
                    Email = userToEdit.Email,
                    FirstName = userToEdit.FirstName,
                    LastName = userToEdit.LastName,
                    PhoneNumber = userToEdit.PhoneNumber,
                    NPI = userToEdit.NPI,
                    IsActive = userToEdit.IsActive ?? false,
                    OfficeList = officeList,
                    PhysicianList = physicianList
                };

                /*HAVE TO IMPLEMENT USERPHYSICIANRELATIONSHIPS*/

                return View(userModel);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method EditUser(int selectedUserId) AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "Admin", "EditUser", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult VerifyNewUser(AddUserModel model)
        {
            try
            {
                var currentUser = CurrentUser;
                var userToEdit = unitOfWork.UserRepository.GetSingle(x => x.UserId == model.UserId);

                if (model.RoleId == 1 || model.RoleId == 5)
                {
                    //User was edited to be a admin or SuperAdmin
                    if (model.RoleId == 5)
                    {
                        //If a superadmin is created we need to remove the office registered against him from here
                    }
                }
                else if (model.RoleId == 2)
                {
                    //User was edited to be a physician
                }
                else if (model.RoleId == 3)
                {
                    //User was edited to be a technician  
                }
                else if (model.RoleId == 4)
                {
                    //User was edited to be a pharmacist
                }

                if (String.Equals(userToEdit.Email, model.Email) == false)
                {
                    //If the email was changed, we have to forcefully logout the user who was edited
                    UserManager.UpdateSecurityStamp(userToEdit.Id);
                }

                userToEdit.RoleId = model.RoleId;
                userToEdit.Email = model.Email;
                userToEdit.FirstName = model.FirstName;
                userToEdit.LastName = model.LastName;
                userToEdit.PhoneNumber = model.PhoneNumber;
                //userToEdit.PhysicianId = physicianId;
                userToEdit.IsActive = model.IsActive;

                if (model.OfficeList != null)
                {
                    //When a physician edits a user, the officelist will be null, and we do not need to make any changes to user's offices
                    // Add Office
                    var userCurrentOfficeIdList = userToEdit.User_Office_Relationship.ToList();

                    foreach (var officeId in model.OfficeList)
                    {
                        for (int i = 0; i < userCurrentOfficeIdList.Count(); i++)
                        {
                            if (userCurrentOfficeIdList[i].OfficeId == officeId)
                            {
                                userCurrentOfficeIdList.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    // Delete User Office
                    foreach (var userOfficeRemove in userCurrentOfficeIdList)
                    {
                        unitOfWork.User_Office_RelationshipRepository.Delete(userOfficeRemove);
                    }

                    foreach (var officeId in model.OfficeList)
                    {
                        if (!userToEdit.User_Office_Relationship.Any(t => t.OfficeId == officeId))
                        {
                            userToEdit.User_Office_Relationship.Add(new User_Office_Relationship { OfficeId = officeId });
                        }
                    }
                }

                if (model.PhysicianList != null)
                {
                    //When a physician edits a user, the physicianlist will be null, and we do not need to make any changes to user's offices
                    //Removing all existing UserPhysician Relationships

                    var relationshipList = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == model.UserId);

                    foreach (var physicianRelation in relationshipList)
                    {
                        unitOfWork.UserPhysicianRelationshippRepository.Delete(physicianRelation);
                    }

                    //Adding new UserPhysician Relationships

                    foreach (var physicianId in model.PhysicianList)
                    {
                        unitOfWork.UserPhysicianRelationshippRepository.Insert(new UserPhysicianRelationship { UserId = model.UserId, PhysicianId = physicianId });
                    }
                }
                unitOfWork.UserRepository.Update(userToEdit);
                unitOfWork.Save();


                AuditLogger.AuditTracker("Existing User with UserId: " + userToEdit.Id + "  was edited.", "UserId = " + userToEdit.Id, (int)AuditOperationType.Change);
                return Json(new { success = 1, message = "User edited successfully." });
            }
            catch (Exception ex)
            {
                Logger.InsertLog("Exception thrown in Method EditUser(AddUserModel model) AdminController. " + ex.Message, ex);
                return Json(new { success = 0, message = "Failed to edit user: " + ex.Message });
            }
        }

        #region Pharmacy
        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult Pharmacies()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method Pharmacies AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "Pharmacies", ErrorTitle, ErrorMessage, null));
            }
        }

        public ActionResult GetPharmaciesList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var pharmacyList = unitOfWork.PharmacyRepository.GetAsQuerable();
                return Json(pharmacyList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetPharmaciesList AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "GetPharmaciesList", ErrorTitle, ErrorMessage, null));
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public ActionResult AddNewPharmacy(int? PharmacyId)
        {
            try
            {
                Pharmacy pharmacy = unitOfWork.PharmacyRepository.GetSingle(t => t.PharmacyId == PharmacyId);
                if (pharmacy == null)
                {
                    pharmacy = new Pharmacy();
                }

                PharmacyViewModel pharmacyViewModel = new PharmacyViewModel();

                pharmacyViewModel.PharmacyId = pharmacy.PharmacyId;
                pharmacyViewModel.ERPCode = pharmacy.ERPCode;
                pharmacyViewModel.PharmacyName = pharmacy.PharmacyName;
                pharmacyViewModel.RecordSourceId = pharmacy.RecordSourceId;
                pharmacyViewModel.CompanyCode = pharmacy.CompanyCode;
                pharmacyViewModel.NABP = pharmacy.NABP;
                pharmacyViewModel.DEANumber = pharmacy.DEANumber;
                pharmacyViewModel.NationalProviderIdentifier = pharmacy.NationalProviderIdentifier;
                pharmacyViewModel.AddressLine1 = pharmacy.AddressLine1;
                pharmacyViewModel.AddressLine2 = pharmacy.AddressLine2;
                pharmacyViewModel.City = pharmacy.City;
                pharmacyViewModel.County = pharmacy.County;
                pharmacyViewModel.StateProvince = pharmacy.StateProvince;
                pharmacyViewModel.Country = pharmacy.Country;
                pharmacyViewModel.PostalCode = pharmacy.PostalCode;
                pharmacyViewModel.PhoneNumber = pharmacy.PhoneNumber;
                pharmacyViewModel.FaxNumber = pharmacy.FaxNumber;

                return View(pharmacyViewModel);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddNewPharmacy AdminController. " + exception.Message, exception);
                return View("Error", new ErrorModel(exception, "AdminController", "AddNewPharmacy", ErrorTitle, ErrorMessage, "~/Views/Shared/_ErrorLayout.cshtml"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddNewPharmacy(PharmacyViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string message = "";
                    bool IsEdit = false;
                    Pharmacy pharmacy = unitOfWork.PharmacyRepository.GetSingle(t => t.PharmacyId == model.PharmacyId);
                    if (pharmacy == null)
                    {
                        pharmacy = new Pharmacy();
                        IsEdit = false;
                    }
                    else
                    {
                        IsEdit = true;
                    }

                    //Pharmacy pharmacy = new Pharmacy();

                    //pharmacy.PharmacyId = model.PharmacyId;
                    pharmacy.ERPCode = model.ERPCode;
                    pharmacy.PharmacyName = model.PharmacyName;
                    pharmacy.RecordSourceId = model.RecordSourceId;
                    pharmacy.CompanyCode = model.CompanyCode;
                    pharmacy.NABP = model.NABP;
                    pharmacy.DEANumber = model.DEANumber;
                    pharmacy.NationalProviderIdentifier = model.NationalProviderIdentifier;
                    pharmacy.AddressLine1 = model.AddressLine1;
                    pharmacy.AddressLine2 = model.AddressLine2;
                    pharmacy.City = model.City;
                    pharmacy.County = model.County;
                    pharmacy.StateProvince = model.StateProvince;
                    pharmacy.Country = model.Country;
                    pharmacy.PostalCode = model.PostalCode;
                    pharmacy.PhoneNumber = model.PhoneNumber;
                    pharmacy.FaxNumber = model.FaxNumber;

                    //pharmacy.ModifiedBy = CurrentUser.UserId;
                    //pharmacy.ModifiedOn = DateTime.Now;

                    if (IsEdit)
                    {
                        unitOfWork.PharmacyRepository.Update(pharmacy);
                        AuditLogger.AuditTracker("Pharmacy Edited.", "A Pharmacy has been Edited with name: " + model.PharmacyName, (int)AuditOperationType.Change);
                        message = "Pharmacy Edited Successfully";
                    }
                    else
                    {
                        //pharmacy.CreatedBy = CurrentUser.UserId;
                        //pharmacy.CreatedOn = DateTime.Now;
                        unitOfWork.PharmacyRepository.Insert(pharmacy);
                        AuditLogger.AuditTracker("New Pharmacy Added.", "A New Pharmacy has been Added with name: " + model.PharmacyName, (int)AuditOperationType.Addition);
                        message = "Pharmacy Added Successfully";
                    }
                    unitOfWork.Save();

                    return Json(new { message = message, success = true });
                }
                else
                {
                    return Json(new { message = "Failed to Add Pharmacy", success = false });
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AddNewPharmacy AdminController. " + exception.Message, exception);
                return Json(new { message = "Failed to Add Pharmacy: " + exception.Message, success = false });
            }
        }

        [CustomAuthorize(Roles = "SuperAdmin")]
        public JsonResult DeletePharmacy(PharmacyViewModel model)
        {
            try
            {
                Pharmacy pharmacy = unitOfWork.PharmacyRepository.GetSingle(t => t.PharmacyId == model.PharmacyId);

                if (pharmacy != null)
                {
                    //pharmacy.IsDeleted = true;
                    //pharmacy.ModifiedBy = CurrentUser.UserId;
                    //pharmacy.ModifiedOn = DateTime.Now;
                    //unitOfWork.PharmacyRepository.Update(pharmacy);

                    unitOfWork.PharmacyRepository.Delete(pharmacy);
                    unitOfWork.Save();

                    AuditLogger.AuditTracker("Pharmacy Deleted.", "A Pharmacy has been deleted with name: " + model.PharmacyName, (int)AuditOperationType.Deletion);
                    return Json(new { message = "Pharmacy Deleted Successfully", success = true });
                }
                else
                {
                    return Json(new { message = "Failed to Delete Pharmacy", success = false });
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method DeletePharmacy AdminController. " + exception.Message, exception);
                return Json(new { message = "Failed to Delete Pharmacy: " + exception.Message, success = false });
            }
        }

        #endregion

        public JsonResult GetStates([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var statesList = unitOfWork.StateRepository.GetAsQuerable();
                return Json(statesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method GetStates AdminController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //Physician grid in add/edit user
        public ActionResult PhysicianGrid_Read([DataSourceRequest] DataSourceRequest request, string PhysicianIdListStr, string OfficeIdListStr, int RoleId = -1)
        {
            try
            {
                if (RoleId < 0)
                {
                    return Json(new { success = 0, message = "One or more input values were not of the expected format" });
                }

                List<int> physicianIdList = new List<int>();
                IQueryable<PhysicianViewModel> PhysicianList = unitOfWork.PhysiciansRepository.GetAsQuerable()
                                                                .Select(t => new PhysicianViewModel
                                                                {
                                                                    PhysicianId = t.PhysicianId,
                                                                    FullName = t.FullName,
                                                                    PhoneNumber = t.PhoneNumber,
                                                                    NPI = t.NationalProviderIdentifier,
                                                                    Address = t.AddressLine1 + " " + t.City + ", " + t.StateProvince + " " + t.PostalCode
                                                                });
                if (!string.IsNullOrWhiteSpace(PhysicianIdListStr))
                {
                    var splittedIds = PhysicianIdListStr.Split(',');
                    if (splittedIds.Length > 0)
                    {
                        foreach (var id in splittedIds)
                        {
                            try
                            {
                                int.Parse(id);
                            }
                            catch
                            {
                                return Json(new { success = 0, message = "One or more input values were not of the expected format" });
                            }
                        }
                    }
                    physicianIdList = PhysicianIdListStr.Split(',').Select(t => Convert.ToInt32(t)).ToList();
                }

                if (CurrentUser.RoleId == 1)
                {
                    IList<Claim> userClaims = UserManager.GetClaims(CurrentUser.Id);
                    string currentOffice = userClaims.FirstOrDefault(c => c.Type == "UserOffice").Value;
                    int currentOfficeId = 0;
                    if (currentOffice != null)
                    {
                        currentOfficeId = currentOffice.ToInt0();

                        var user_Office_RelationshipIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(t => currentOfficeId == t.OfficeId).Select(t => t.UserId);
                        var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => user_Office_RelationshipIds.Contains(x.UserId)).Select(y => y.PhysicianId);

                        var physicianIdViewModel = physicianIdList.Select(t => new PhysicianIdViewModel { PhysicianId = t }).ToList();

                        var PhysicianList1 = PhysicianList.Where(t => physicianIds.Contains(t.PhysicianId))
                                            .ToList()
                                            .GroupJoin(physicianIdViewModel, phy => phy.PhysicianId, phyId => phyId.PhysicianId, (phy, phyId) => new { phy = phy, phyId = phyId })
                                            .SelectMany(
                                                temp => temp.phyId.DefaultIfEmpty(),
                                                  (temp, p) =>
                                                    new PhysicianViewModel
                                                    {
                                                        PhysicianId = temp.phy.PhysicianId,
                                                        FullName = temp.phy.FullName,
                                                        PhoneNumber = temp.phy.PhoneNumber,
                                                        NPI = temp.phy.NPI,
                                                        Address = temp.phy.Address,
                                                        IsExist = p?.PhysicianId == temp.phy.PhysicianId
                                                    }
                                            );

                        return Json(PhysicianList1.ToDataSourceResult(request));
                    }
                }

                if (RoleId == 2) //RoleId 2 is Physician
                {
                    PhysicianList = PhysicianList.Where(t => physicianIdList.Contains(t.PhysicianId));
                }
                else if (!string.IsNullOrWhiteSpace(OfficeIdListStr) && RoleId == 3) //RoleId 3 is Technician
                {
                    var splittedIds = OfficeIdListStr.Split(',');
                    if (splittedIds.Length > 0)
                    {
                        foreach (var id in splittedIds)
                        {
                            try
                            {
                                int.Parse(id);
                            }
                            catch
                            {
                                return Json(new { success = 0, message = "One or more input values were not of the expected format" });
                            }
                        }
                    }

                    var officeIdList = OfficeIdListStr.Split(',').Select(t => Convert.ToInt32(t)).ToList();
                    var user_Office_RelationshipIds = unitOfWork.User_Office_RelationshipRepository.GetAsQuerable(t => officeIdList.Contains(t.OfficeId)).Select(t => t.UserId);
                    var physicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => user_Office_RelationshipIds.Contains(x.UserId)).Select(y => y.PhysicianId);

                    var physicianIdViewModel = physicianIdList.Select(t => new PhysicianIdViewModel { PhysicianId = t }).ToList();

                    var PhysicianList1 = PhysicianList.Where(t => physicianIds.Contains(t.PhysicianId))
                                        .ToList()
                                        .GroupJoin(physicianIdViewModel, phy => phy.PhysicianId, phyId => phyId.PhysicianId, (phy, phyId) => new { phy = phy, phyId = phyId })
                                        .SelectMany(
                                            temp => temp.phyId.DefaultIfEmpty(),
                                              (temp, p) =>
                                                new PhysicianViewModel
                                                {
                                                    PhysicianId = temp.phy.PhysicianId,
                                                    FullName = temp.phy.FullName,
                                                    PhoneNumber = temp.phy.PhoneNumber,
                                                    NPI = temp.phy.NPI,
                                                    Address = temp.phy.Address,
                                                    IsExist = p?.PhysicianId == temp.phy.PhysicianId
                                                }
                                        );

                    return Json(PhysicianList1.ToDataSourceResult(request));
                }

                return Json(PhysicianList.ToDataSourceResult(request));
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method PhysicianGrid_Read AdminController. " + exception.Message, exception);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AssignPhysicianToTechnician(int UserId, int PhysicianId)
        {
            try
            {
                string message = "";
                var user = unitOfWork.UserRepository.GetSingle(t => t.UserId == UserId);
                if (user != null)
                {
                    //var abc = user.UserPhysicianRelationship.Where(t => t.PhysicianId == PhysicianId);
                    var userPhysicianRelationship = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(t => t.UserId == UserId && t.PhysicianId == PhysicianId);
                    if (userPhysicianRelationship != null)
                    {
                        //TODO Soft Delete
                        unitOfWork.UserPhysicianRelationshippRepository.Delete(userPhysicianRelationship);
                        message = "Physician Unassigned to Technician Successfully";
                    }
                    else
                    {
                        userPhysicianRelationship = new UserPhysicianRelationship();
                        userPhysicianRelationship.UserId = UserId;
                        userPhysicianRelationship.PhysicianId = PhysicianId;
                        unitOfWork.UserPhysicianRelationshippRepository.Insert(userPhysicianRelationship);
                        message = "Physician Assigned to Technician Successfully";

                        //AuditLogger.AuditTracker("Assign physician to technician", "UserId = " + UserId, (int)AuditOperationType.Change);
                    }
                    unitOfWork.Save();
                }
                return Json(new { message = message, success = true });
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method AssignPhysicianToTechnician(int UserId,int PhysicianId) AdminController. " + exception.Message, exception);
                return Json(new { message = "Failed to Assign physician to technician. " + exception.Message, success = false });
            }
        }

        [HttpPost]
        public JsonResult ReSubmitRefillFax(string documentIdEncrypted, string physicianIdEncrypted)
        {
            try
            {
                int RefillFaxReportLogId = int.Parse(SecurityHelper.Decrypt(documentIdEncrypted.ToString()));
                int physicianId = int.Parse(SecurityHelper.Decrypt(physicianIdEncrypted.ToString()));

                RefillFaxReportLog refillFaxReportLog = unitOfWork.RefillFaxReportLogRepository.GetSingle(t => t.RefillFaxReportLogId == RefillFaxReportLogId);

                if (refillFaxReportLog == null)
                {
                    refillFaxReportLog = new RefillFaxReportLog();
                    return Json(new { message = "Failed to Assign get refillFaxReportLog", success = false });
                }
                else
                {
                    string localPath = refillFaxReportLog.RefillDocumentPath;
                    string filePathDestination = (SettingManager.GetSettingByName("SubmitChecklistDocumentPath")?.Value ?? "") + refillFaxReportLog.RefillDocumentName;
                    if (System.IO.File.Exists(localPath))
                        if (!System.IO.File.Exists(filePathDestination))
                            System.IO.File.Copy(localPath, filePathDestination);

                    AuditLogger.AuditTracker("Refill fax ReSubmitted", "A new refill has been resubmitted for patient " + refillFaxReportLog.PatientName, (int)AuditOperationType.Addition);
                    return Json(new { message = "Refill fax Re-Submitted Successfully.", success = true });

                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ReSubmitRefillFax AdminController. " + exception.Message, exception);
                return Json(new { message = "Failed To Re-Submit Refill Fax", success = false });
            }
        }


        public ActionResult PhysicianSignature(int? userId = null)
        {
            try
            {
                if (userId == null)
                {
                    return View();
                }
                else
                {
                    UserPhysicianRelationship relationship = unitOfWork.UserPhysicianRelationshippRepository.GetSingle(x => x.UserId == userId.Value);
                    PhysicianSignature signature = unitOfWork.PhysicianSignatureRepository.GetSingle(x => x.PhysicianMasterId == relationship.PhysicianId);
                    if (relationship != null)
                    {
                        return View("PhysicianSignatureEdit", new EditPhysicianSignature { PhysicianId = relationship.PhysicianId, SignatureBase64String = signature != null ? signature.Signature != null ? "data:image/png;base64," + Convert.ToBase64String(signature.Signature) : null : null });
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method PhysicianSignature AdminController. " + e.Message, e);
                return null;
            }
        }
    }

    public class PhysicianViewModel
    {
        public int PhysicianId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string NPI { get; set; }
        public string Address { get; set; }
        public bool IsExist { get; set; }
    }

    public class PhysicianIdViewModel
    {
        public int PhysicianId { get; set; }
    }
}