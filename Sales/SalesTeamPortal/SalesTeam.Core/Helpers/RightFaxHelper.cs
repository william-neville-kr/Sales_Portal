using RestSharp;
using RestSharp.Authenticators;
using SalesTeam.Core.Data;
using SalesTeam.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SalesTeam.Core.Helpers
{
    public class RightFaxHelper
    {
        private static RightFaxHelper _rightFaxHelper;
        private readonly RightFaxSenderConfig Config = null;
        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        //private readonly FaxSenderQueue Fax;

        private RightFaxHelper()
        {
            var rightFaxUsername = System.Web.Configuration.WebConfigurationManager.AppSettings["RightFaxUsername"].ToString();
            var rightFaxPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["RightFaxPassword"].ToString();
            var rightFaxUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RightFaxUrl"].ToString();

            this.Config = new RightFaxSenderConfig()
            {
                ConfigID = 1,
                Password = rightFaxPassword,
                ServerURL = rightFaxUrl,
                UserID = rightFaxUsername
            };
        }

        public static RightFaxHelper SharedInstance
        {
            get
            {
                if (_rightFaxHelper == null)
                {
                    _rightFaxHelper = new RightFaxHelper();
                }
                return _rightFaxHelper;
            }
        }

        public string SendFax_RightFax(string faxNumber, byte[] fileData, string toName, string fileType, int? physicianId = null)
        {
            try
            {
                var client = GetAuthenticatedClient(Config);

                if (client == null)
                {
                    unitOfWork.SalesPortalFaxLogRepository.Insert(new SalesPortalFaxLog
                    {
                        ErrorMessage = "Failed to initialize RestClient",
                        SentOn = DateTime.UtcNow,
                        SentTo = toName,
                        SentToPhysicianId = physicianId,
                        Status = "Failed"
                    });
                    unitOfWork.Save();
                    return "Could not send fax";
                }

                var attachmentPath = UploadAttachments(client, fileData, fileType);
                if (attachmentPath.Contains("Error"))
                {
                    unitOfWork.SalesPortalFaxLogRepository.Insert(new SalesPortalFaxLog
                    {
                        ErrorMessage = "Failed to Upload Attachment: " + attachmentPath,
                        SentOn = DateTime.UtcNow,
                        SentTo = toName,
                        SentToPhysicianId = physicianId,
                        Status = "Failed"
                    });
                    unitOfWork.Save();
                    return attachmentPath;
                }

                var request = new RestRequest("SendJobs", Method.POST) { RequestFormat = DataFormat.Json };

                var strreq = @"{
                                'Recipients':[
                                    {
                                        'Name':'" + toName + @"',
                                        'Destination':'" + faxNumber + @"',
                                        'PhoneNumber':'" + faxNumber + @"',
                                    }
                                ],
                                    'AttachmentUrls': ['" + attachmentPath + @"'],
                                    
                           }";
                //'HoldForPreview': true,
                strreq = strreq.Replace("'", "\"");
                request.AddParameter("application/json", strreq, ParameterType.RequestBody);

                var response = client.Execute<SendJobResponse>(request);

                if (response.ErrorException != null)
                {
                    unitOfWork.SalesPortalFaxLogRepository.Insert(new SalesPortalFaxLog
                    {
                        ErrorMessage = response.ErrorMessage,
                        SentOn = DateTime.UtcNow,
                        SentTo = toName,
                        SentToPhysicianId = physicianId,
                        Status = "Failed"
                    });
                    unitOfWork.Save();

                    return response.ErrorMessage;
                }
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    unitOfWork.SalesPortalFaxLogRepository.Insert(new SalesPortalFaxLog
                    {
                        ErrorMessage = "",
                        SentOn = DateTime.UtcNow,
                        SentTo = toName,
                        SentToPhysicianId = physicianId,
                        Status = "Success"
                    });
                    unitOfWork.Save();
                    return "Success";
                }
                else
                {
                    unitOfWork.SalesPortalFaxLogRepository.Insert(new SalesPortalFaxLog
                    {
                        ErrorMessage = response.ErrorMessage,
                        SentOn = DateTime.UtcNow,
                        SentTo = toName,
                        SentToPhysicianId = physicianId,
                        Status = "Failed"
                    });
                    unitOfWork.Save();
                    return "Failed to send fax";
                }
            }
            catch (Exception exception)
            {
                unitOfWork.SalesPortalFaxLogRepository.Insert(new SalesPortalFaxLog
                {
                    ErrorMessage = exception.Message,
                    SentOn = DateTime.UtcNow,
                    SentTo = toName,
                    SentToPhysicianId = physicianId,
                    Status = "Failed"
                });
                unitOfWork.Save();
                return exception.Message;
            }
        }

        private RestClient GetAuthenticatedClient(RightFaxSenderConfig config)
        {
            try
            {
                var url = config.ServerURL; //"https://rightfax.ha.kroger.com/RightFax/API/";
                var username = config.UserID; //  "KSP_PORTAL_API";
                var password = config.Password; // "Kr0ger@3";

                var client = new RestClient(url) { Authenticator = new HttpBasicAuthenticator(username, password) };

                return client;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string UploadAttachments(RestClient client, byte[] fileData, string fileType)
        {
            try
            {
                var request = new RestRequest("Attachments", Method.POST);

                request.AddFile("Attachment", fileData, "Attachment", fileType);
                var response = client.Execute(request);

                if (response.ErrorException != null)
                {
                    return "Error: " + response.ErrorMessage;
                }

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var fileheader = response.Headers.FirstOrDefault(t => t.Name == "Location");
                    if (fileheader != null) return fileheader.Value.ToString();
                }
                else
                {
                    return "Error: " + response.ErrorMessage;
                }

                return "";
            }
            catch (Exception e)
            {
                return "Error in UploadAttachment: " + e.Message;
            }
        }

    }
    public class RightFaxSenderConfig
    {
        public int ConfigID { get; set; }
        public string ServerURL { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }

    public class FaxSenderQueue
    {
        public long FaxID { get; set; }
        public string ApplicationID { get; set; }
        public string FaxTo { get; set; }
        public string FaxToName { get; set; }
        public string HTMLBody { get; set; }
        public string AttachmentPath { get; set; }
        public string Details { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string SentStatus { get; set; }
        public Nullable<System.DateTime> SentOn { get; set; }
        public string ErrorMessage { get; set; }
        public Nullable<int> SiteID { get; set; }
        public Nullable<int> FaxRefID { get; set; }
        public string RFJobID { get; set; }
        public string RFUserID { get; set; }
        public Nullable<int> ConfigID { get; set; }
    }

    public class SendJobResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<Recipient> Recipients { get; set; }
        public List<string> IncludedDocumentIds { get; set; }
        public List<string> AttachmentUrls { get; set; }
        public List<string> LibraryDocumentIds { get; set; }
        public DateTime SendAfter { get; set; }
        public bool HoldForPreview { get; set; }
        public string Priority { get; set; }
        public string DiagnosticMode { get; set; }
        public string Resolution { get; set; }
        public string CoversheetTemplateId { get; set; }
        public List<Tag> Tags { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public int DocumentCount { get; set; }
        public DateTime CreateTime { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<Link> Links { get; set; }
    }

    public class Tag
    {
        public string Id { get; set; }
        public string RelatedType { get; set; }
        public string RelatedId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ETag { get; set; }
        public List<Link> Links { get; set; }
    }

    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }

    public class Notification
    {
        public Condition Condition { get; set; }
        public string NotificationType { get; set; }
        public string Destination { get; set; }
    }

    public class Condition
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Recipient
    {
        public string Name { get; set; }
        public string Destination { get; set; }
        public string Company { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactId { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
