using PhysicianPortal.Core.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Twilio;


namespace PhysicianPortal.Core.Helpers
{
    /// <summary>
    ///     Represents a common helper
    /// </summary>
    public static class CommonHelper
    {
        

        public static int? ToInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            int result;
            if (Int32.TryParse(value, out result))
                return result;
            return null;
        }

        public static int ToInt0(this string value)
        {
            int result;
            if (Int32.TryParse(value, out result))
                return result;
            return 0;
        }

        public static DateTime ToDateTime(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? DateTime.Now : DateTime.ParseExact(value.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        public static decimal? ToDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            decimal result;
            if (decimal.TryParse(value, out result))
                return result;
            return null;
        }

        public static string ToString0(this decimal? value)
        {
            if (value != null)
            {
                var v = (int)value;
                return v.ToString();
            }
            return "";
        }

        public static string ToString0(this bool? value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            return "False";
        }

        public static bool? ToBool(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (value.ToLower() == "true")
                return true;
            if (value == "1")
                return true;
            if (value == "0")
                return false;
            bool result;
            if (bool.TryParse(value, out result))
                return result;
            return null;
        }

        public static DateTime? toDate(this string dateTimeStr, string dateFmt)
        {
            const DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces;
            DateTime dt;
            if (DateTime.TryParseExact(dateTimeStr, dateFmt, CultureInfo.InvariantCulture, style, out dt))
                return dt;
            return null;
        }

        public static DateTime? toDate(this string dateTimeStr)
        {
            DateTime dt;
            if (DateTime.TryParse(dateTimeStr, out dt))
                return dt;
            return null;
        }

        public static DateTime toDateNow(this string dateTimeStr)
        {
            DateTime dt;
            if (DateTime.TryParse(dateTimeStr, out dt))
                return dt;
            return DateTime.Now;
        }

        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;

            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public static string CamelCaseToWords(this string input)
        {
            return Regex.Replace(input.FirstCharToUpper(), "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }

        public static string GetUserIP(System.Web.HttpRequest Request)
        {
            var ipAddress = Request.ServerVariables["HTTP_CLIENT_IP"];

            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress == "localhost")
                ipAddress = "127.0.0.1";

            return ipAddress;
        }

        public static string ToProperCaseString(this string str, CultureInfo cultureInfo = null)
        {
            try
            {
                if (!String.IsNullOrEmpty(str))
                {
                    string title = str.ToLower();
                    TextInfo textInfo = null;
                    if (cultureInfo == null)
                        textInfo = new CultureInfo("en-US", false).TextInfo;
                    else
                        textInfo = new CultureInfo(cultureInfo.Name, false).TextInfo;
                    title = textInfo.ToTitleCase(title);
                    return title;
                }
                return "";
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method ToProperCaseString. " + exception.Message, exception);
                return str;
            }
        }

        #region XML Utils

        /// <summary>
        ///     Convert the Object from the XML Representaion of it
        /// </summary>
        public static T FromXml<T>(this string xml) where T : class
        {
            if (String.IsNullOrEmpty(xml))
                return null;

            var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xml));

            using (
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.Unicode,
                    XmlDictionaryReaderQuotas.Max, null))
            {
                var dataContractSerializer = new DataContractSerializer(typeof(T));

                return dataContractSerializer.ReadObject(reader) as T;
            }
        }

        public static object XmlDeserialize(Type objType, string xmlDoc)
        {
            if (xmlDoc != null && objType != null)
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlDoc);
                //Assuming doc is an XML document containing a serialized object and objType is a System.Type set to the type of the object.
                var reader = new XmlNodeReader(doc.DocumentElement);
                var ser = new XmlSerializer(objType);
                return ser.Deserialize(reader);
            }
            return null;
        }


        public static string ToXml<T>(this T obj, bool removeNamespaces = false) where T : class
        {
            if (obj == null)
                return "";

            var dataContractSerializer = new DataContractSerializer(obj.GetType());

            String text;

            using (var memoryStream = new MemoryStream())
            {
                dataContractSerializer.WriteObject(memoryStream, obj);

                var data = new byte[memoryStream.Length];
                Array.Copy(memoryStream.GetBuffer(), data, data.Length);
                text = Encoding.UTF8.GetString(data);
            }
            if (removeNamespaces)
            {
                text = text.Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            }
            return text;
        }

        #endregion

        #region SEND EMAILS
        public static void SendEmail(string subject, string message, string toAddress, string filePath, AlternateView alternateView = null)
        {
            try
            {
                //For Local Host
                //string adminEmail = SettingManager.GetSettingValue("Email.FromAddress");
                //string adminPassword = SettingManager.GetSettingValue("Email.Password");
                //string SMPTServer = SettingManager.GetSettingValue("Email.SMTPHost");
                //int port = SettingManager.GetSettingValueInteger("Email.SMTPPort");
                //bool SMTPEnableSsl = SettingManager.GetSettingValueBoolean("Email.SMTPEnableSsl");

                //For Online Web App
                string adminEmail = SettingManager.GetSettingValue("MailFrom");
                string adminPassword = SettingManager.GetSettingValue("SMTPPassword");
                string SMPTServer = SettingManager.GetSettingValue("SMTP");
                int port = SettingManager.GetSettingValueInteger("SMTPPort");
                bool SMTPEnableSsl = SettingManager.GetSettingValueBoolean("SMTPUseSSL");
                

                if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword) && !string.IsNullOrEmpty(SMPTServer))
                {
                    var message1 = new MailMessage { From = new MailAddress(adminEmail) };

                    message1.To.Add(new MailAddress(toAddress));
                    message1.Subject = subject;
                    message1.Body = message;
                    message1.IsBodyHtml = true;
                    message1.ReplyToList.Add(adminEmail);

                    if (alternateView != null)
                    {
                        //Add view to the Email Message
                        message1.AlternateViews.Add(alternateView);
                    }

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var attach = new Attachment(filePath);
                        message1.Attachments.Add(attach);
                    }
                    SmtpClient smtpClient = new SmtpClient { Host = SMPTServer, Port = port, EnableSsl = SMTPEnableSsl, Credentials = new NetworkCredential(adminEmail, adminPassword), Timeout = 10000000 };
                    smtpClient.Send(message1);
                }
            }
            catch (Exception exception)
            {
                //Logger.LogException(exception);
                Logger.InsertLog("Exception thrown in Method SendEmail. " + exception.Message, exception);
            }
        }

        public static void SendSms(string messageDestination, string messageBody)
        {
            try
            {
                //For Local Host
                //Getting Twilio Settings form the AppSettings
                string SMSAccountIdentification = SettingManager.GetSettingValue("SMSAccountIdentification");
                string SMSAccountPassword = SettingManager.GetSettingValue("SMSAccountPassword");
                string SMSAccountFrom = SettingManager.GetSettingValue("SMSAccountFrom");

                // Twilio Begin
                var Twilio = new TwilioRestClient(
                    SMSAccountIdentification,
                    SMSAccountPassword);
                var result = Twilio.SendMessage(
                    SMSAccountFrom,
                    messageDestination, messageBody
                );
                Trace.TraceInformation(result.Status);
                // Twilio End
            }
            catch (Exception exception)
            {
                //Logger.LogException(exception);
                Logger.InsertLog("Exception thrown in Method SendSms. " + exception.Message, exception);
            }
        }

        public static void SendFax(string faxNumber, string documentPath, AlternateView alternateView = null)
        {
            try
            {
                string faxNoSuffix = SettingManager.GetSettingValue("FaxNoSuffix");
                bool testMode = SettingManager.GetSettingValueBoolean("TestMode");
                string testPhysicianFaxNo = SettingManager.GetSettingValue("TestPhysicianFaxNo");

                if (testMode)
                {
                    faxNumber = testPhysicianFaxNo + faxNoSuffix;
                }
                else
                {
                    if (!string.IsNullOrEmpty(faxNumber))
                    {
                        faxNumber = faxNumber.Replace("-", "") + faxNoSuffix;
                    }
                }
                if (alternateView != null)
                {
                    SendEmail("Physician Portal Refaxed Document", "", faxNumber, documentPath, alternateView);
                }
                else
                {
                    SendEmail("Physician Portal Refaxed Document", "", faxNumber, documentPath);
                }
            }
            catch (Exception exception)
            {
                //Logger.LogException(exception);
                Logger.InsertLog("Exception thrown in Method SendFax. " + exception.Message, exception);
            }
        }

        #endregion

        

        public static IEnumerable<TResult> FullOuterJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
                    where TInner : class
                    where TOuter : class
        {
            var innerLookup = inner.ToLookup(innerKeySelector);
            var outerLookup = outer.ToLookup(outerKeySelector);

            var innerJoinItems = inner
                .Where(innerItem => !outerLookup.Contains(innerKeySelector(innerItem)))
                .Select(innerItem => resultSelector(null, innerItem));

            return outer
                .SelectMany(outerItem =>
                {
                    var innerItems = innerLookup[outerKeySelector(outerItem)];

                    return innerItems.Any() ? innerItems : new TInner[] { null };
                }, resultSelector)
                .Concat(innerJoinItems);
        }

        public static IEnumerable<TResult> LeftOuterJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source, IEnumerable<TInner> other, Func<TSource, TKey> func, Func<TInner, TKey> innerkey, Func<TSource, TInner, TResult> res)
        {
            return from f in source
                   join b in other on func.Invoke(f) equals innerkey.Invoke(b) into g
                   from result in g.DefaultIfEmpty()
                   select res.Invoke(f, result);
        }
    }
}
