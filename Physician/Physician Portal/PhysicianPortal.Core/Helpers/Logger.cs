using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data.Entity.Validation;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Repository;
using Microsoft.AspNet.Identity;

namespace PhysicianPortal.Core.Helpers
{
    public class Logger
    {
        private static readonly UnitOfWork unitOfWork = new UnitOfWork();

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static List<ErrorLog> GetAllLogs()
        {
            return (from x in unitOfWork.ErrorLogRepository.GetAsQuerable() select x).ToList();
        }

        /// <summary>
        ///     Gets a log item
        /// </summary>
        /// <param name="LogID"> Log item identifier </param>
        /// <returns> Log item </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ErrorLog GetLogByID(int LogID)
        {
            return unitOfWork.ErrorLogRepository.GetAsQuerable(t => t.Id == LogID).FirstOrDefault();
        }

        /// <summary>
        ///     Gets a log item
        /// </summary>
        /// <param name="LogID"> Log item identifier </param>
        /// <returns> Log item </returns>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteLog(int LogID)
        {
            ErrorLog errorLog = unitOfWork.ErrorLogRepository.GetAsQuerable(t => t.Id == LogID).FirstOrDefault();
            if (errorLog != null)
            {
                unitOfWork.ErrorLogRepository.Delete(errorLog);
                unitOfWork.Save();
            }
        }

        public static void FormatException(StringBuilder sb, Exception ex)
        {
            sb.AppendLine("<b>Message</b>: <b>" + ex.Message + "</b><br />");
            sb.AppendLine("<b>Exception</b>: " + ex.GetType() + "<br />");

            if (ex.TargetSite != null)
                sb.AppendLine("<b>Targetsite</b>: " + ex.TargetSite + "<br />");

            sb.AppendLine("<b>Source</b>: " + ex.Source + "<br />");
            if (!string.IsNullOrEmpty(ex.StackTrace))
                sb.AppendLine("<b>StackTrace</b>: " + ex.StackTrace.Replace(Environment.NewLine, "<br />") + "<br />");

            sb.AppendLine("<b>Data count</b>: " + ex.Data.Count + "<br />");

            if (ex.Data.Count > 0)
            {
                var tbl = new HtmlTable { Border = 1 };

                var htr = new HtmlTableRow();
                var htc1 = new HtmlTableCell();
                var htc2 = new HtmlTableCell();
                var htc3 = new HtmlTableCell();
                var htc4 = new HtmlTableCell();

                htc1.InnerHtml = "<b>Key</b>";
                htc2.InnerHtml = "<b>Value</b>";
                htc3.InnerHtml = "Key Type";
                htc4.InnerHtml = "Value Type";

                htr.Cells.Add(htc1);
                htr.Cells.Add(htc2);
                htr.Cells.Add(htc3);
                htr.Cells.Add(htc4);

                tbl.Rows.Add(htr);

                foreach (DictionaryEntry de in ex.Data)
                {
                    var tr = new HtmlTableRow();

                    var tc1 = new HtmlTableCell();
                    var tc2 = new HtmlTableCell();
                    var tc3 = new HtmlTableCell();
                    var tc4 = new HtmlTableCell();

                    tc1.InnerHtml = "<b>" + de.Key + "</b>";
                    tc2.InnerHtml = "<b>" + de.Value + "</b>";

                    tc3.InnerHtml = de.Key.GetType().Name;
                    tc4.InnerHtml = de.Value.GetType().Name;

                    tc3.Align = "center";
                    tc4.Align = "center";

                    tr.Cells.Add(tc1);
                    tr.Cells.Add(tc2);
                    tr.Cells.Add(tc3);
                    tr.Cells.Add(tc4);

                    tbl.Rows.Add(tr);
                }
                var tblSb = new StringBuilder();
                var sw = new StringWriter(tblSb);
                var htw = new HtmlTextWriter(sw);

                tbl.RenderControl(htw);

                sb.AppendLine(tblSb.ToString());
            }

            sb.AppendLine("<br />");
            sb.AppendLine("<b>Exception</b>: " + ex.ToString().Replace(Environment.NewLine, "<br />") + "<br />");

            if (ex.InnerException != null)
            {
                sb.Append("<br/><br/><b>Inner Excception:</b><br/><br/>");
                FormatException(sb, ex.InnerException);
            }

            if (ex is DbEntityValidationException)
            {
                string message = ((DbEntityValidationException)ex).EntityValidationErrors.Aggregate("", (current1, validationErrors) => validationErrors.ValidationErrors.Aggregate(current1, (current, validationError) => current + string.Format(" Property: {0} Error: {1} ", validationError.PropertyName, validationError.ErrorMessage)));
                sb.Append("<br/><br/><b>EntityValidationErrors:</b><br/><br/>");
                sb.Append(message);
            }
        }

        public static ErrorLog InsertLog(string Message, string Exception)
        {
            return InsertLog(0, Message, Exception);
        }

        public static ErrorLog InsertLog(int Severity, string Message, string exception)
        {
            string ipAddress = string.Empty;

            if (HttpContext.Current != null)
                ipAddress = HttpContext.Current.Request.UserHostAddress;

            var userId = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId());
            var errorLog = new ErrorLog
            {
                ShortMessage = Message,
                FullMessage = exception,
                IpAddress = ipAddress,
                PageUrl = HttpContext.Current.Request.Url.AbsoluteUri,
                CreatedOn = DateTime.UtcNow,
                UserId = userId,
                CreatedByUserId = userId
            };
            unitOfWork.ErrorLogRepository.Insert(errorLog);
            unitOfWork.Save();

            return errorLog;
        }

        public static ErrorLog InsertLog(int Severity, string Message, Exception exception)
        {
            if (exception == null)
                return null;

            //don't log thread abort exception
            if ((exception is ThreadAbortException))
                return null;

            var sb = new StringBuilder();
            FormatException(sb, exception);

            return InsertLog(Severity, Message, sb.ToString());
        }

        public static ErrorLog InsertLog(string Message, Exception Exception)
        {
            Trace.WriteLine(Message + Exception);
            return InsertLog(0, Message, Exception);
        }

        public static void Error(Exception exception)
        {
            InsertLog(exception.Message, exception);
        }

        public static void LogException(Exception exception)
        {
            InsertLog(exception.Message, exception);
        }
    }
}