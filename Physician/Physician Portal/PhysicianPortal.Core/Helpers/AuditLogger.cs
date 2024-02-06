using System;
using System.Web;
using System.IO;
using PhysicianPortal.Core.Data;
using Microsoft.AspNet.Identity;
using PhysicianPortal.Core.Repository;

namespace PhysicianPortal.Core.Helpers
{
    public class AuditLogger
    {
        private static readonly UnitOfWork unitOfWork = new UnitOfWork();
        public static void AuditTracker(string LogMessage, string LogData,
            int AuditOperationType, string RecordType = "Audit", string file = "")
        {
            try
            {
                var Request = HttpContext.Current.Request;
                var browserCapabilities = Request.Browser;
                var browserName = browserCapabilities.Browser;
                var browserVersion = browserCapabilities.Version;
                var ipAddress = CommonHelper.GetUserIP(Request);
                var directoryName = string.IsNullOrEmpty(file) ? Path.GetDirectoryName(Request.AppRelativeCurrentExecutionFilePath) ?? "" : "";
                var fileName = string.IsNullOrEmpty(file) ? Path.GetFileNameWithoutExtension(Request.PhysicalPath) ?? "" : file;
                var url = Path.Combine(directoryName, fileName).Replace("~", "").Replace("\\", "/");
                var userId = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId());

                

                AuditTrail auditTrail = new AuditTrail
                {
                    LogMessage = LogMessage,
                    AuditStamp = DateTime.UtcNow,
                    Data = LogData,
                    UserId = userId,
                    AuditOperationTypeId = AuditOperationType,
                    Url = url,
                    IPAddress = ipAddress,
                    Device = browserName + " v" + browserVersion,
                    RecordType = RecordType
                };
                unitOfWork.AuditTrailRepository.Insert(auditTrail);
                unitOfWork.Save();
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
    }
}
