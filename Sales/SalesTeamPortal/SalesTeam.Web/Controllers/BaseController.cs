using System;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Mvc;
using SalesTeam.Core.Repository;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private int gMasterSalesTeamId;
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public int MasterSalesTeamId
        {
            get
            {
                try
                {
                    var userName = "";

                    if (gMasterSalesTeamId == 0)
                    {
                        try
                        {
                            if (User != null)
                            {
                                userName = User.Identity.Name;
                                var salesPerson = UnitOfWork.vwSalesTeamsRepository.GetAsQuerable(t => t.ADUserName.ToLower() == userName.ToLower()).FirstOrDefault();
                                if (salesPerson != null)
                                    gMasterSalesTeamId = salesPerson.SalesTeamId;
                                LogInfo("Current User1: " + userName + " MasterSalesTeamID:" + gMasterSalesTeamId, new Exception("Log MasterSalesTeamID"));
                            }
                        }
                        catch (Exception ex)
                        {
                            LogInfo(ex.Message, ex);
                        }
                    }

                    if (gMasterSalesTeamId == 0)
                    {
                        try
                        {
                            if (User != null)
                            {
                                using (var context = new PrincipalContext(ContextType.Domain))
                                {
                                    var principal = UserPrincipal.FindByIdentity(context, User.Identity.Name);
                                    if (principal != null)
                                    {
                                        userName = string.Format("{0} {1}", principal.GivenName, principal.Surname);
                                    }
                                }
                                var salesPerson =
                                    UnitOfWork.vwSalesTeamsRepository.GetAsQuerable(
                                        t => t.SalesRepresentativeFullName.ToLower() == userName.ToLower()).FirstOrDefault();
                                if (salesPerson != null)
                                    gMasterSalesTeamId = salesPerson.SalesTeamId;
                                LogInfo("Current User2: " + userName + " MasterSalesTeamID:" + gMasterSalesTeamId, new Exception("Log MasterSalesTeamID"));
                            }
                        }
                        catch (Exception ex)
                        {
                            LogInfo(ex.Message, ex);
                        }
                    }

                    if (gMasterSalesTeamId == 0)
                    {
                        try
                        {
                            if (UserPrincipal.Current != null)
                            {
                                userName = UserPrincipal.Current.DisplayName;
                                var salesPerson =
                                    UnitOfWork.vwSalesTeamsRepository.GetAsQuerable(
                                        t => t.SalesRepresentativeFullName.ToLower() == userName.ToLower()).FirstOrDefault();
                                if (salesPerson != null)
                                    gMasterSalesTeamId = salesPerson.SalesTeamId;
                                LogInfo("Current User3: " + userName + " MasterSalesTeamID:" + gMasterSalesTeamId, new Exception("Log MasterSalesTeamID"));
                            }
                        }
                        catch (Exception ex)
                        {
                            LogInfo(ex.Message, ex);
                        }
                    }

                    if (gMasterSalesTeamId == 0)
                    {
                        gMasterSalesTeamId = 1;
                        LogInfo("Current User4: Hard Code User ID" + userName + " MasterSalesTeamID:" + gMasterSalesTeamId, new Exception("Log MasterSalesTeamID"));
                    }

                    LogInfo("Current User: " + userName + " MasterSalesTeamID:" + gMasterSalesTeamId, new Exception("Log MasterSalesTeamID"));
                }
                catch (Exception ex)
                {
                    LogInfo(ex.Message, ex);
                }
                return gMasterSalesTeamId;
            }
        }

        public void LogInfo(string message, Exception data)
        {
            try
            {
                string traceString = "", lineString = "";
                for (int i = 0; i < message.Length; i++)
                {
                    lineString += "-";
                }
                traceString = lineString + "\r\n" + DateTime.Now.ToString("hh:mm:ss tt") + "\r\nMessage: " + message + "\r\nData: " + data.ToString() + "\r\n\r\n" + lineString + "\r\n\r\n";
                Trace.WriteLine(traceString);
            }
            catch (Exception exception)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(exception.ToString(), EventLogEntryType.Error, 101, 1);
                    eventLog.WriteEntry(message + " ::: " + data, EventLogEntryType.Error, 101, 1);
                }
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
    }
}