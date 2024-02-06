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
//using System.Collections;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.Entity.Validation;
//using System.Diagnostics;
//using System.IO;
//using System.Net;
//using System.Net.Mail;
//using System.Reflection;
//using System.Runtime.Serialization;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using System.Xml;
//using System.Xml.Serialization;
//using Hcc.Core.Data;
//using Microsoft.VisualBasic.FileIO;
//using System.Data.SqlClient;
//using SalesTeam.Core.Data;

namespace SalesTeam.Core.Helpers
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

        #region SEND EMAILS
        public static void SendEmail(string subject, string message, string toAddress, string filePath, byte[] fileData = null, string fileType = null)
        {
            try
            {
                string adminEmail = SettingManager.GetSettingValue("MailFrom");
                string adminPassword = SettingManager.GetSettingValue("SMTPPassword");
                string SMPTServer = SettingManager.GetSettingValue("SMTP");
                int port = SettingManager.GetSettingValueInteger("SMTPPort");
                bool SMTPEnableSsl = SettingManager.GetSettingValueBoolean("SMTPUseSSL");
                string mailcc = SettingManager.GetSettingValue("Mailcc");

                if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword) && !string.IsNullOrEmpty(SMPTServer))
                {
                    var message1 = new MailMessage { From = new MailAddress(adminEmail) };

                    message1.To.Add(new MailAddress(toAddress));
                    message1.CC.Add(new MailAddress(mailcc));
                    message1.Subject = subject;
                    message1.Body = message;
                    message1.IsBodyHtml = true;
                    message1.ReplyToList.Add(adminEmail);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var attach = new Attachment(filePath);
                        message1.Attachments.Add(attach);
                    }
                    else if (fileData != null)
                    {
                        if (fileType == "pdf")
                        {
                            Attachment att = new Attachment(new MemoryStream(fileData), "Attachment", System.Net.Mime.MediaTypeNames.Application.Pdf);
                            message1.Attachments.Add(att);
                        }
                        else if (fileType == "tif")
                        {
                            Attachment att = new Attachment(new MemoryStream(fileData), "Attachment", System.Net.Mime.MediaTypeNames.Image.Tiff);
                            message1.Attachments.Add(att);
                        }

                        
                    }
                    SmtpClient smtpClient = new SmtpClient { Host = SMPTServer, Port = port, EnableSsl = SMTPEnableSsl, Credentials = new NetworkCredential(adminEmail, adminPassword), Timeout = 10000000 };
                    smtpClient.Send(message1);
                }
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                //LogManager.InsertLog("Exception thrown in Method SendEmail: " + exception.Message, exception);
            }
        }
        public static void SendFax(string faxNumber, string documentPath)
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
                SendEmail("SALES TEAM Refaxed Document", "", faxNumber, documentPath);
            }
            catch (Exception exception)
            {
                LogInfo(exception.Message, exception);
                //LogManager.InsertLog("Exception thrown in Method SendFax: " + exception.Message, exception);
            }
        }

        

        public static void LogInfo(string message, Exception data)
        {
            try
            {
                //string json = "";
                //if (data != null)
                //	json = new JavaScriptSerializer().Serialize(data);

                Trace.WriteLine(DateTime.Now.ToString("H:mm:ss.fff") + "\tMessage: " + message + "\t Data:" + data.ToString());
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

        #endregion


        //public static string RenderUserControl(string path, List<KeyValuePair<string, object>> data)
        //{
        //    Page pageHolder = new Page();
        //    HtmlForm frm = new HtmlForm();

        //    UserControl viewControl = (UserControl)pageHolder.LoadControl(path);

        //    if (data.Count > 0)
        //    {
        //        foreach (KeyValuePair<string, object> kp in data)
        //        {
        //            string propertyName = kp.Key;
        //            object propertyValue = kp.Value;

        //            if (propertyValue != null)
        //            {
        //                Type viewControlType = viewControl.GetType();
        //                PropertyInfo property = viewControlType.GetProperty(propertyName);
        //                if (property != null)
        //                {
        //                    property.SetValue(viewControl, propertyValue, null);
        //                }
        //            }
        //        }
        //    }

        //    frm.Controls.Add(viewControl);
        //    pageHolder.Controls.Add(frm);
        //    StringWriter output = new StringWriter();
        //    HttpContext.Current.Server.Execute(pageHolder, output, false);
        //    return output.ToString();
        //}

        //public static void GridViewAltRowStyling(GridViewRowEventArgs e, GridView gridView1)
        //{
        //    if (gridView1.SortExpression.Length > 0)
        //    {
        //        int cellIndex = -1;
        //        foreach (DataControlField field in gridView1.Columns.Cast<DataControlField>().Where(field => field.SortExpression == gridView1.SortExpression))
        //        {
        //            cellIndex = gridView1.Columns.IndexOf(field);
        //            break;
        //        }

        //        if (cellIndex > -1)
        //        {
        //            if (e.Row.RowType == DataControlRowType.Header)
        //            {
        //                //  this is a header row,
        //                //  set the sort style
        //                e.Row.Cells[cellIndex].CssClass +=
        //                    (gridView1.SortDirection == SortDirection.Ascending
        //                        ? " sortascheader"
        //                        : " sortdescheader");
        //            }
        //            else if (e.Row.RowType == DataControlRowType.DataRow)
        //            {
        //                //  this is an alternating row
        //                e.Row.Cells[cellIndex].CssClass +=
        //                    (e.Row.RowIndex % 2 == 0
        //                        ? " sortaltrow"
        //                        : "sortrow");
        //            }
        //        }
        //    }
        //}

        //public static string ConvertToString(this IEnumerable items)
        //{
        //    StringBuilder output = new StringBuilder();
        //    foreach (var item in items)
        //    {
        //        output.Append(item.ToString() + "<BR/>");
        //    }
        //    return output.ToString();
        //}

        //#region CSV HELPERS

        ///// <summary>
        /////     Serialize objects to Comma Separated Value (CSV) format [1].
        /////     Rather than try to serialize arbitrarily complex types with this
        /////     function, it is better, given type A, to specify a new type, A'.
        /////     Have the constructor of A' accept an object of type A, then assign
        /////     the relevant values to appropriately named fields or properties on
        /////     the A' object.
        ///// </summary>
        //public static string ToCSV<T>(this IEnumerable<T> objects, string CsvSeparator = ",")
        //{
        //    StringBuilder output = new StringBuilder();
        //    IEnumerable<MemberInfo> fields =
        //        from mi in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
        //        where new[] { MemberTypes.Field, MemberTypes.Property }.Contains(mi.MemberType)
        //        let orderAttr = (ColumnOrderAttribute)Attribute.GetCustomAttribute(mi, typeof(ColumnOrderAttribute))
        //        orderby orderAttr == null ? int.MaxValue : orderAttr.Order, mi.Name
        //        select mi;

        //    output.AppendLine(QuoteRecord(fields.Select(f => f.Name), CsvSeparator));

        //    foreach (T record in objects)
        //    {
        //        output.AppendLine(QuoteRecord(FormatObject(fields, record), CsvSeparator));
        //    }
        //    return output.ToString();
        //}

        //private static IEnumerable<string> FormatObject<T>(IEnumerable<MemberInfo> fields, T record)
        //{
        //    foreach (MemberInfo field in fields)
        //    {
        //        if (field is FieldInfo)
        //        {
        //            FieldInfo fi = (FieldInfo)field;
        //            yield return Convert.ToString(fi.GetValue(record));
        //        }
        //        else if (field is PropertyInfo)
        //        {
        //            PropertyInfo pi = (PropertyInfo)field;
        //            yield return Convert.ToString(pi.GetValue(record, null));
        //        }
        //        else
        //        {
        //            throw new Exception("Unhandled case.");
        //        }
        //    }
        //}

        ////const string CsvSeparator = ",";

        //private static string QuoteRecord(IEnumerable<string> record, string CsvSeparator)
        //{
        //    return String.Join(CsvSeparator, record.Select(field => QuoteField(field, CsvSeparator)).ToArray());
        //}

        //private static string QuoteField(string field, string CsvSeparator)
        //{
        //    if (String.IsNullOrEmpty(field))
        //    {
        //        return "\"\"";
        //    }
        //    else if (field.Contains(CsvSeparator) || field.Contains("\"") || field.Contains("\r") || field.Contains("\n"))
        //    {
        //        return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
        //    }
        //    else
        //    {
        //        return field;
        //    }
        //}

        //[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        //public class ColumnOrderAttribute : Attribute
        //{
        //    public ColumnOrderAttribute(int order)
        //    {
        //        Order = order;
        //    }

        //    public int Order { get; private set; }
        //}

        //#endregion

        //#region XML Utils

        ///// <summary>
        /////     Convert the Object from the XML Representaion of it
        ///// </summary>
        ///// <typeparam name="T"> </typeparam>
        ///// <param name="xml"> </param>
        ///// <returns> </returns>
        //public static T FromXml<T>(this string xml) where T : class
        //{
        //    if (String.IsNullOrEmpty(xml))
        //        return null;

        //    MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xml));

        //    using (
        //        XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.Unicode, XmlDictionaryReaderQuotas.Max, null))
        //    {
        //        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));

        //        return dataContractSerializer.ReadObject(reader) as T;
        //    }
        //}
        //public static object XmlDeserialize(Type objType, string xmlDoc)
        //{
        //    if (xmlDoc != null && objType != null)
        //    {
        //        var doc = new XmlDocument();
        //        doc.LoadXml(xmlDoc);
        //        //Assuming doc is an XML document containing a serialized object and objType is a System.Type set to the type of the object.
        //        var reader = new XmlNodeReader(doc.DocumentElement);
        //        var ser = new XmlSerializer(objType);
        //        return ser.Deserialize(reader);
        //    }
        //    return null;
        //}


        //public static string ToXml<T>(this T obj, bool removeNamespaces = false) where T : class
        //{
        //    if (obj == null)
        //        return "";

        //    DataContractSerializer dataContractSerializer = new DataContractSerializer(obj.GetType());

        //    String text;

        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        dataContractSerializer.WriteObject(memoryStream, obj);

        //        byte[] data = new byte[memoryStream.Length];
        //        Array.Copy(memoryStream.GetBuffer(), data, data.Length);
        //        text = Encoding.UTF8.GetString(data);
        //    }
        //    if (removeNamespaces)
        //    {
        //        text = text.Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
        //    }
        //    return text;
        //}


        //public static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        //{
        //    DataTable csvData = new DataTable();

        //    try
        //    {
        //        using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
        //        {
        //            csvReader.SetDelimiters(new string[] { "," });
        //            csvReader.HasFieldsEnclosedInQuotes = true;
        //            string[] colFields = csvReader.ReadFields();
        //            if (colFields != null)
        //                foreach (string column in colFields)
        //                {
        //                    DataColumn datecolumn = new DataColumn(column);
        //                    datecolumn.AllowDBNull = true;
        //                    csvData.Columns.Add(datecolumn);
        //                }

        //            while (!csvReader.EndOfData)
        //            {
        //                string[] fieldData = csvReader.ReadFields();
        //                //Making empty value as null
        //                for (int i = 0; i < fieldData.Length; i++)
        //                {
        //                    if (fieldData[i] == "")
        //                    {
        //                        fieldData[i] = null;
        //                    }
        //                }
        //                csvData.Rows.Add(fieldData);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.InsertLog(ex.Message, ex);
        //    }
        //    return csvData;
        //}

        //#endregion


        //#region DB Validation Error Logging



        ///// <summary>
        /////     A DbEntityValidationException extension method that formates validation errors to string.
        ///// </summary>
        //public static string DbEntityValidationExceptionToString(this DbEntityValidationException e)
        //{
        //    var validationErrors = e.DbEntityValidationResultToString();
        //    var exceptionMessage = string.Format("{0}{1}Validation errors:{1}{2}", e, Environment.NewLine, validationErrors);
        //    return exceptionMessage;
        //}

        ///// <summary>
        /////     A DbEntityValidationException extension method that aggregate database entity validation results to string.
        ///// </summary>
        //public static string DbEntityValidationResultToString(this DbEntityValidationException e)
        //{
        //    return e.EntityValidationErrors
        //            .Select(dbEntityValidationResult => dbEntityValidationResult.DbValidationErrorsToString(dbEntityValidationResult.ValidationErrors))
        //            .Aggregate(string.Empty, (current, next) => string.Format("{0}{1}{2}", current, Environment.NewLine, next));
        //}

        ///// <summary>
        /////     A DbEntityValidationResult extension method that to strings database validation errors.
        ///// </summary>
        //public static string DbValidationErrorsToString(this DbEntityValidationResult dbEntityValidationResult, IEnumerable<DbValidationError> dbValidationErrors)
        //{
        //    var entityName = string.Format("[{0}]", dbEntityValidationResult.Entry.Entity.GetType().Name);
        //    const string indentation = "\t - ";
        //    var aggregatedValidationErrorMessages = dbValidationErrors.Select(error => string.Format("[{0} - {1}]", error.PropertyName, error.ErrorMessage))
        //                                           .Aggregate(string.Empty, (current, validationErrorMessage) => current + (Environment.NewLine + indentation + validationErrorMessage));
        //    return string.Format("{0}{1}", entityName, aggregatedValidationErrorMessages);
        //}

        //#endregion


        //public static Control FindControlRecursive(this Control Root, string Id)
        //{
        //    if (Root.ID == Id)
        //        return Root;

        //    foreach (Control ctl in Root.Controls)
        //    {
        //        Control foundCtl = FindControlRecursive(ctl, Id);
        //        if (foundCtl != null)
        //            return foundCtl;
        //    }
        //    return null;
        //}

        //public static string getVisitTypeByPatientVisitID(long PID)
        //{
        //    var db = new HCCData();
        //    var res = db.PatientVisits.Where(s => s.PatientVisitID == PID);
        //    int VisitID = 0;

        //    foreach (var getVisitID in res)
        //    {
        //        VisitID = Convert.ToInt16(getVisitID.VisitTypeID);
        //    }


        //    string assmentName = "";
        //    var getAName = db.VisitTypes.Where(s => s.VisitTypeID == VisitID).ToList();
        //    foreach (var AssmenstName in getAName)
        //    {
        //        assmentName = AssmenstName.Name;
        //    }

        //    int id = 0;
        //    string tabsname = "";
        //    bool result = false;
        //    string li = "";
        //    var assesmentID = db.Assessments.Where(s => s.AssessmentName == assmentName).ToList();
        //    foreach (var getid in assesmentID)
        //        id = getid.AssessmentID;

        //    var assesmentType = db.AssessmentTypes.Where(s => s.AssessmentID == id).OrderBy(x => x.AssessmentTypeOrder).ToList();
        //    foreach (var getTabsName in assesmentType)
        //    {
        //        tabsname += getTabsName.AssessmentTypeName + "/";
        //    }
        //    return tabsname;
        //}


        //#region Encrypt/Decrypt

        ///// <summary>
        /////    Encrypt/Decrypt any field you want 
        /////    added by jawad ahmed on 20-10-2015 
        ///// </summary>

        //public static string Encrypt(string clearText)
        //{
        //    string EncryptionKey = SettingManager.GetSettingValue("Encryption_Key");
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }

        //    //clearText = clearText + ".>.";
        //    return clearText;
        //}

        //public static string Decrypt(string cipherText)
        //{
        //    string EncryptionKey = SettingManager.GetSettingValue("Encryption_Key");



        //    //string[] splitedText =  Regex.Split(cipherText, ".>.");  


        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}

        //#endregion


        //public static string GetEncryptedValue(string key, string value)
        //{
        //    string returnval = SettingManager.GetSettingValue(key).ToString();
        //    if (returnval.Equals("true"))
        //    {
        //        value = Encrypt(value);
        //    }
        //    return value;
        //}


        //public static string GetDecryptValue(string key, string value)
        //{
        //    string returnval = SettingManager.GetSettingValue(key).ToString();
        //    if (returnval.Equals("true"))
        //    {
        //        value = Decrypt(value);
        //    }
        //    return value;
        //}


        //public static string GetVisitNameByID(int vid)
        //{

        //    var db = new HCCData();
        //    Core.Data.VisitType item = db.VisitTypes.FirstOrDefault(x => x.VisitTypeID == vid);

        //    if (item != null)
        //        return item.Name;
        //    else
        //        return string.Empty;
        //}



        //#region GetUserSignatureFileName

        ///// <summary>
        /////    GetUserSignatureFileName
        /////    added by jawad ahmed on 11-2-2015
        ///// </summary>
        ///// 

        //public static string GetUserSignatureFileName()
        //{
        //    int LoginUserID = Convert.ToInt16(HttpContext.Current.Session["User_UserID"].ToString());
        //    var db = new HCCData();
        //    Core.Data.User item = db.Users.FirstOrDefault(x => x.UserID == LoginUserID);

        //    if (item != null)
        //        return item.UserID + "," + item.SignaturePath;
        //    else
        //        return string.Empty;
        //}
        //#endregion


        ///// <summary>
        /////  SQL ENCRYTION DECRYTION ADDED BY JAWAD AHMED
        ///// </summary>
        ///// 

        //public static string SQLEncrypt(string value)
        //{
        //    string temp = "0x";
        //    string constring = "Data Source=USMAN-LAPTOP;Initial Catalog=HCC_DEMO;Persist Security Info=True;User ID=sa;Password=sa;";
        //    using (SqlConnection con = new SqlConnection(constring))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("callingFunction", con))
        //        {
        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@key", "Jawad");
        //                cmd.Parameters.AddWithValue("@value", value);
        //                DataTable dt = new DataTable();
        //                sda.Fill(dt);
        //                temp = temp + dt.Rows[0][0].ToString();
        //            }
        //        }
        //    }
        //    return temp;

        //}

        //public static string SQLDecrypt(string value)
        //{
        //    string temp = "";
        //    string constring = "Data Source=USMAN-LAPTOP;Initial Catalog=HCC_DEMO;Persist Security Info=True;User ID=sa;Password=sa;";
        //    using (SqlConnection con = new SqlConnection(constring))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("callingDecryptFunction", con))
        //        {
        //            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@key", "Jawad");
        //                cmd.Parameters.AddWithValue("@value", value);
        //                DataTable dt = new DataTable();
        //                sda.Fill(dt);
        //                temp = temp + dt.Rows[0][0].ToString();
        //            }
        //        }
        //    }
        //    return temp;

        //}

    }
}