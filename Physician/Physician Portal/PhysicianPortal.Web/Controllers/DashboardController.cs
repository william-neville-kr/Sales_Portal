using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AnalysisServices.AdomdClient;
using System.Text;
using System.Data;
using Microsoft.AspNet.Identity;
using PhysicianPortal.Core.Helpers;
using PhysicianPortal.Core.Repository;
using PhysicianPortal.Web.Models;
using System.Globalization;
using PhysicianPortal.Web.Attributes;

namespace PhysicianPortal.Web.Controllers
{
    [CustomAuthorize]
    public class DashboardController : BaseController
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Dashboard
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Index()
        {
            try
            {
                var model = new AddPatientViewModel();
                List<int> listOfUserPhysicians = unitOfWork.UserRepository.GetAsQuerable(x => x.RoleId == 2).Select(y => y.UserId).ToList();
                List<int> listOfPhysicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => listOfUserPhysicians.Contains(x.UserId)).Select(y => y.PhysicianId).ToList();

                if (CurrentUser.RoleId == 2)
                {
                    listOfPhysicianIds = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId).Select(y => y.PhysicianId).ToList();
                    model.PhysiciansList = unitOfWork.PhysiciansRepository.GetAsQuerable(x => listOfPhysicianIds.Contains(x.PhysicianId)).Select(t => new SelectListItem { Text = t.FullName, Value = t.PhysicianId.ToString() }).ToList();
                }
                else if (CurrentUser.RoleId == 3)
                {
                    List<int> listOfPhysiciansAgainstTechs = unitOfWork.UserPhysicianRelationshippRepository.GetAsQuerable(x => x.UserId == CurrentUser.UserId && listOfPhysicianIds.Contains(x.PhysicianId)).Select(y => y.PhysicianId).ToList();
                    model.PhysiciansList = unitOfWork.PhysiciansRepository.GetAsQuerable(x => listOfPhysiciansAgainstTechs.Contains(x.PhysicianId)).Select(t => new SelectListItem { Text = t.FullName, Value = t.PhysicianId.ToString() }).ToList();
                }
                else
                {
                    model.PhysiciansList = unitOfWork.PhysiciansRepository.GetAsQuerable(x => listOfPhysicianIds.Contains(x.PhysicianId)).Select(t => new SelectListItem { Text = t.FullName, Value = t.PhysicianId.ToString() }).ToList();
                }
                model.PhysiciansList.Insert(0, new SelectListItem { Text = "", Value = "" });
                ViewBag.RoleId = CurrentUser.RoleId;

                return View(model);
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method Index() DashboardController. " + e.Message, e);
                return View("Error", new ErrorModel(e, "DashboardController", "Index", ErrorTitle, ErrorMessage, null));
            }
        }

        // charts functions and class
        public JsonResult GetPhysicianChartData(string startDate, string endDate, string selectedPhysicianId)
        {
            try
            {
                //Adding validations to inputs

                string format = "yyyy-MM-dd";
                DateTime dateTime;

                if (DateTime.TryParseExact(startDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine(dateTime);
                }
                else
                {
                    return Json(new { message = "Something went wrong", success = false });
                }
                if (DateTime.TryParseExact(endDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine(dateTime);
                }
                else
                {
                    return Json(new { message = "Something went wrong", success = false });
                }

                try
                {
                    int temp = int.Parse(selectedPhysicianId);

                    if (temp < 0)
                    {
                        return Json(new { message = "Something went wrong", success = false });
                    }
                }
                catch (Exception e)
                {
                    return Json(new { message = "Something went wrong", success = false });
                }


                List<listItem> listItems = new List<listItem>();
                DataSet ds = new DataSet();

                using (AdomdConnection conn = new AdomdConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Charts_CS"].ToString()))
                {
                    string npi = null;

                    if (UserManager.IsInRole(CurrentUser.Id, "SuperAdmin"))
                    {
                        if (selectedPhysicianId != "" || selectedPhysicianId != null)
                        {
                            int PhysicianId = Convert.ToInt32(selectedPhysicianId.ToString());
                            var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == PhysicianId);
                            if (physician != null)
                            {
                                npi = "[" + physician.NationalProviderIdentifier + "]";
                            }
                        }
                    }
                    else if (UserManager.IsInRole(CurrentUser.Id, "Physician"))
                    {
                        npi = "[" + CurrentUser.NPI + "]";
                    }
                    else if (UserManager.IsInRole(CurrentUser.Id, "Technician"))
                    {
                        int physicianId = 0;
                        if (UserManager.IsInRole(CurrentUser.Id, "SuperAdmin"))
                        {
                            physicianId = unitOfWork.UserPhysicianRelationshippRepository.Get(x => x.UserId == CurrentUser.UserId).First().PhysicianId;
                            //var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == physicianId);
                            //if (physician != null)
                            //{
                            //    npi = "[" + physician.NationalProviderIdentifier + "]";
                            //}

                        }
                        else
                        {
                            physicianId = unitOfWork.UserPhysicianRelationshippRepository.Get(x => x.UserId == CurrentUser.UserId).First().PhysicianId;
                            var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == physicianId);
                            if (physician != null)
                            {
                                npi = "[" + physician.NationalProviderIdentifier + "]";
                            }
                        }



                    }

                    string date1 = "[" + startDate + "T00:00:00]";
                    string date2 = "[" + endDate + "T00:00:00]";
                    conn.Open();
                    var mdxQuery = new StringBuilder();

                    //mdxQuery.Append("SELECT NON EMPTY { [Measures].[Patient Count] } ON 0,  NON EMPTY { ([Primary Physician].[National Provider Identifier].[National Provider Identifier].ALLMEMBERS * [Bill Date].[Day Date].[Day Date].ALLMEMBERS ) } ON 1 FROM ( SELECT ( { STRTOSET('[Primary Physician].[National Provider Identifier].&[1003849050]') } ) ON COLUMNS FROM ( SELECT ( STRTOMEMBER('[Bill Date].[Calendar].[Date].&[2017-03-01T00:00:00]', CONSTRAINED) : STRTOMEMBER('[Bill Date].[Calendar].[Date].&[2017-03-31T00:00:00]', CONSTRAINED) ) ON COLUMNS FROM [MH]))");
                    mdxQuery.Append("SELECT NON EMPTY { [Measures].[Patient Count] } ON 0,  NON EMPTY { ([Primary Physician].[National Provider Identifier].[National Provider Identifier].ALLMEMBERS * [Bill Date].[Day Date].[Day Date].ALLMEMBERS ) } ON 1 FROM ( SELECT ( { STRTOSET('[Primary Physician].[National Provider Identifier].& " + npi + "') } ) ON COLUMNS FROM ( SELECT ( STRTOMEMBER('[Bill Date].[Calendar].[Date].&" + date1 + "', CONSTRAINED) : STRTOMEMBER('[Bill Date].[Calendar].[Date].&" + date2 + "', CONSTRAINED) ) ON COLUMNS FROM [MH]))");

                    using (AdomdCommand cmd = new AdomdCommand(mdxQuery.ToString(), conn))
                    {

                        ds.EnforceConstraints = false;
                        ds.Tables.Add();
                        DataTable dt = ds.Tables[0];
                        dt.Load(cmd.ExecuteReader());

                        foreach (DataRow row in dt.Rows)
                        {
                            listItems.Add(new listItem { Date = (string)row[1], PatientCount = (int?)row[2] });
                        }

                    }


                    conn.Close();
                }


                return Json(new { message = listItems, success = true });
            }
            catch (Exception exception)
            {
                //LogInfo(exception.Message, exception);
                Logger.InsertLog("Exception thrown in Method GetPhysicianChartData HomeController. " + exception.Message, exception);
                return Json(new { message = "Error in GetPhysicianChartData Function in DashBoard Controller", success = false });
            }
        }

        public JsonResult GetPhysicianChartData2(string startDate, string endDate, string selectedPhysicianId)
        {
            try
            {

                string format = "yyyy-MM-dd";
                DateTime dateTime;

                if (DateTime.TryParseExact(startDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine(dateTime);
                }
                else
                {
                    return Json(new { message = "Something went wrong", success = false });
                }
                if (DateTime.TryParseExact(endDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine(dateTime);
                }
                else
                {
                    return Json(new { message = "Something went wrong", success = false });
                }

                try
                {
                    int temp = int.Parse(selectedPhysicianId);

                    if (temp < 0)
                    {
                        return Json(new { message = "Something went wrong", success = false });
                    }
                }
                catch (Exception e)
                {
                    return Json(new { message = "Something went wrong", success = false });
                }


                List<listItem> listItems1 = new List<listItem>();

                DataSet ds = new DataSet();

                using (AdomdConnection conn = new AdomdConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Charts_CS"].ToString()))
                {

                    string npi = null;

                    if (UserManager.IsInRole(CurrentUser.Id, "SuperAdmin"))
                    {
                        if (selectedPhysicianId != "" || selectedPhysicianId != null)
                        {
                            int PhysicianId = Convert.ToInt32(selectedPhysicianId.ToString());
                            var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == PhysicianId);
                            if (physician != null)
                            {
                                npi = "[" + physician.NationalProviderIdentifier + "]";
                            }
                        }
                    }
                    else if (UserManager.IsInRole(CurrentUser.Id, "Physician"))
                    {
                        npi = "[" + CurrentUser.NPI + "]";
                    }
                    else if (UserManager.IsInRole(CurrentUser.Id, "Technician"))
                    {
                        int physicianId = unitOfWork.UserPhysicianRelationshippRepository.Get(x => x.UserId == CurrentUser.UserId).First().PhysicianId;
                        var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == physicianId);

                        if (physician != null)
                        {
                            npi = "[" + physician.NationalProviderIdentifier + "]";
                        }
                    }

                    string date1 = "[" + startDate + "T00:00:00]";
                    string date2 = "[" + endDate + "T00:00:00]";
                    conn.Open();
                    var mdxQuery = new StringBuilder();

                    //mdxQuery.Append("SELECT NON EMPTY { [Measures].[Prescription Count] } ON 0, NON EMPTY { ([Primary Physician].[National Provider Identifier].[National Provider Identifier].ALLMEMBERS * [Bill Date].[Day Date].[Day Date].ALLMEMBERS ) *[Drug].[Product Name].[Product Name].AllMembers} ON 1  FROM ( SELECT ( { STRTOSET('[Primary Physician].[National Provider Identifier].&[1003849050]') } ) ON COLUMNS FROM ( SELECT ( STRTOMEMBER('[Bill Date].[Calendar].[Date].&[2017-03-01T00:00:00]', CONSTRAINED) : STRTOMEMBER('[Bill Date].[Calendar].[Date].&[2017-03-31T00:00:00]', CONSTRAINED) ) ON COLUMNS FROM [MH]))");
                    mdxQuery.Append("SELECT NON EMPTY { [Measures].[Prescription Count] } ON 0, NON EMPTY { ([Primary Physician].[National Provider Identifier].[National Provider Identifier].ALLMEMBERS * [Bill Date].[Day Date].[Day Date].ALLMEMBERS ) *[Drug].[Product Name].[Product Name].AllMembers} ON 1  FROM ( SELECT ( { STRTOSET('[Primary Physician].[National Provider Identifier].&" + npi + "') } ) ON COLUMNS FROM ( SELECT ( STRTOMEMBER('[Bill Date].[Calendar].[Date].&" + date1 + "', CONSTRAINED) : STRTOMEMBER('[Bill Date].[Calendar].[Date].&" + date2 + "', CONSTRAINED) ) ON COLUMNS FROM [MH]))");


                    using (AdomdCommand cmd = new AdomdCommand(mdxQuery.ToString(), conn))
                    {

                        ds.EnforceConstraints = false;
                        ds.Tables.Add();
                        DataTable dt = ds.Tables[0];
                        dt.Load(cmd.ExecuteReader());

                        dt.Columns[0].ColumnName = "Col1";
                        dt.Columns[1].ColumnName = "Col2";
                        dt.Columns[2].ColumnName = "Col3";
                        dt.Columns[3].ColumnName = "Col4";


                        DataView view = new DataView(dt);

                        //DataTable ds1 = dt.DefaultView.ToTable(true, "Col3");



                        var seriesNames = dt.AsEnumerable()
                       .Select(row => new
                       {

                           productname = row.Field<string>("Col3")

                       })
                       .Distinct().ToList();

                        var datesList = dt.AsEnumerable()
                     .Select(row => new
                     {
                         productname = row.Field<string>("Col2")
                     })
                     .Distinct().ToList();

                        List<SeriesData> srData = new List<SeriesData>();

                        foreach (var item in seriesNames.Select(t => t.productname))
                        {
                            var series = new SeriesData();
                            series.name = item.ToString();
                            series.data = new List<int>();
                            foreach (var date in datesList.Select(t => t.productname))
                            {
                                var aa = dt.AsEnumerable().Where(t => t.Field<string>("Col3") == series.name && t.Field<string>("Col2") == date.ToString())
                                    .Select(t => t.Field<int>("Col4")).FirstOrDefault();
                                if (aa != 0)
                                {
                                    series.data.Add(aa);
                                }
                                else
                                    series.data.Add(0);
                            }
                            srData.Add(series);
                        }
                        conn.Close();


                        return Json(new { srData = srData, datesList = datesList.Select(t => t.productname), success = true });

                    }

                }


                return Json(new { message = listItems1, success = false });
            }
            catch (Exception exception)
            {
                //LogInfo(exception.Message, exception);
                Logger.InsertLog("Exception thrown in Method GetPhysicianChartData2 HomeController. " + exception.Message, exception);
                return Json(new { message = "Error in GetPhysicianChartData2 Function in DashBoard Controller", success = false });
            }
        }

        public JsonResult GetPhysicianChartData3(string startDate, string endDate, string selectedPhysicianId)
        {
            try
            {

                string format = "yyyy-MM-dd";
                DateTime dateTime;

                if (DateTime.TryParseExact(startDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine(dateTime);
                }
                else
                {
                    return Json(new { message = "Something went wrong", success = false });
                }
                if (DateTime.TryParseExact(endDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine(dateTime);
                }
                else
                {
                    return Json(new { message = "Something went wrong", success = false });
                }

                try
                {
                    int temp = int.Parse(selectedPhysicianId);

                    if (temp < 0)
                    {
                        return Json(new { message = "Something went wrong", success = false });
                    }
                }
                catch (Exception e)
                {
                    return Json(new { message = "Something went wrong", success = false });
                }


                List<listItem> listItems1 = new List<listItem>();

                DataSet ds = new DataSet();

                using (AdomdConnection conn = new AdomdConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Charts_CS"].ToString()))
                {

                    string npi = null;

                    if (UserManager.IsInRole(CurrentUser.Id, "SuperAdmin"))
                    {
                        if (selectedPhysicianId != "" || selectedPhysicianId != null)
                        {
                            int PhysicianId = Convert.ToInt32(selectedPhysicianId.ToString());
                            var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == PhysicianId);
                            if (physician != null)
                            {
                                npi = "[" + physician.NationalProviderIdentifier + "]";
                            }
                        }
                    }
                    else if (UserManager.IsInRole(CurrentUser.Id, "Physician"))
                    {
                        npi = "[" + CurrentUser.NPI + "]";
                    }
                    else if (UserManager.IsInRole(CurrentUser.Id, "Technician"))
                    {
                        int physicianId = unitOfWork.UserPhysicianRelationshippRepository.Get(x => x.UserId == CurrentUser.UserId).First().PhysicianId;
                        var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == physicianId);

                        if (physician != null)
                        {
                            npi = "[" + physician.NationalProviderIdentifier + "]";
                        }
                    }

                    string date1 = "[" + startDate + "T00:00:00]";
                    string date2 = "[" + endDate + "T00:00:00]";
                    conn.Open();
                    var mdxQuery = new StringBuilder();

                    //mdxQuery.Append("SELECT NON EMPTY { [Measures].[Prescription Count] } ON 0, NON EMPTY { ([Primary Physician].[National Provider Identifier].[National Provider Identifier].ALLMEMBERS * [Bill Date].[Day Date].[Day Date].ALLMEMBERS ) *[Drug].[Product Name].[Product Name].AllMembers} ON 1  FROM ( SELECT ( { STRTOSET('[Primary Physician].[National Provider Identifier].&[1003849050]') } ) ON COLUMNS FROM ( SELECT ( STRTOMEMBER('[Bill Date].[Calendar].[Date].&[2017-03-01T00:00:00]', CONSTRAINED) : STRTOMEMBER('[Bill Date].[Calendar].[Date].&[2017-03-31T00:00:00]', CONSTRAINED) ) ON COLUMNS FROM [MH]))");
                    mdxQuery.Append("SELECT NON EMPTY { [Measures].[Prescription Count] } ON 0,  NON EMPTY { ([Primary Physician].[National Provider Identifier].[National Provider Identifier].ALLMEMBERS * [Bill Date].[Day Date].[Day Date].ALLMEMBERS ) *[Prescription].[Order Status].[Order Status].AllMembers} ON 1  FROM ( SELECT ( { STRTOSET('[Primary Physician].[National Provider Identifier].&" + npi + "') } ) ON COLUMNS FROM ( SELECT ( STRTOMEMBER('[Bill Date].[Calendar].[Date].&" + date1 + "', CONSTRAINED) : STRTOMEMBER('[Bill Date].[Calendar].[Date].&" + date2 + "', CONSTRAINED) ) ON COLUMNS FROM [MH]))");


                    using (AdomdCommand cmd = new AdomdCommand(mdxQuery.ToString(), conn))
                    {

                        ds.EnforceConstraints = false;
                        ds.Tables.Add();
                        DataTable dt = ds.Tables[0];
                        dt.Load(cmd.ExecuteReader());

                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    listItems1.Add(new listItem { Date = (string)row[1], PatientCount = (int?)row[3] });
                        //}
                        //return dt;

                        dt.Columns[0].ColumnName = "Col1";
                        dt.Columns[1].ColumnName = "Col2";
                        dt.Columns[2].ColumnName = "Col3";
                        dt.Columns[3].ColumnName = "Col4";


                        DataView view = new DataView(dt);

                        //DataTable ds1 = dt.DefaultView.ToTable(true, "Col3");

                        var seriesNames = dt.AsEnumerable()
                       .Select(row => new
                       {

                           productname = row.Field<string>("Col3")

                       })
                       .Distinct().ToList();

                        var datesList = dt.AsEnumerable()
                     .Select(row => new
                     {
                         productname = row.Field<string>("Col2")
                     })
                     .Distinct().ToList();

                        List<SeriesData> srData = new List<SeriesData>();

                        foreach (var item in seriesNames.Select(t => t.productname))
                        {
                            var series = new SeriesData();
                            series.name = item.ToString();
                            series.data = new List<int>();
                            foreach (var date in datesList.Select(t => t.productname))
                            {
                                var aa = dt.AsEnumerable().Where(t => t.Field<string>("Col3") == series.name && t.Field<string>("Col2") == date.ToString())
                                    .Select(t => t.Field<int>("Col4")).FirstOrDefault();
                                if (aa != 0)
                                {
                                    series.data.Add(aa);
                                }
                                else
                                    series.data.Add(0);
                            }
                            srData.Add(series);
                        }
                        conn.Close();


                        return Json(new { srData = srData, datesList = datesList.Select(t => t.productname), success = true });
                    }

                }


            }
            catch (Exception exception)
            {
                //LogInfo(exception.Message, exception);
                Logger.InsertLog("Exception thrown in Method GetPhysicianChartData3 HomeController. " + exception.Message, exception);
                return Json(new { message = "Error in GetPhysicianChartData3 Function in DashBoard Controller", success = false });
            }
        }

        public JsonResult GetPhysicianChartData4(string startDate, string endDate, string selectedPhysicianId)
        {
            try
            {
                string format = "yyyy-MM-dd";
                DateTime dateTime;

                if (DateTime.TryParseExact(startDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine(dateTime);
                }
                else
                {
                    return Json(new { message = "Something went wrong", success = false });
                }
                if (DateTime.TryParseExact(endDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                {
                    Console.WriteLine(dateTime);
                }
                else
                {
                    return Json(new { message = "Something went wrong", success = false });
                }

                try
                {
                    int temp = int.Parse(selectedPhysicianId);

                    if (temp < 0)
                    {
                        return Json(new { message = "Something went wrong", success = false });
                    }
                }
                catch (Exception e)
                {
                    return Json(new { message = "Something went wrong", success = false });
                }

                List<listItem> listItems1 = new List<listItem>();

                DataSet ds = new DataSet();

                using (AdomdConnection conn = new AdomdConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Charts_CS"].ToString()))
                {
                    string npi = null;


                    if (UserManager.IsInRole(CurrentUser.Id, "SuperAdmin"))
                    {
                        if (selectedPhysicianId != "" || selectedPhysicianId != null)
                        {
                            int PhysicianId = Convert.ToInt32(selectedPhysicianId.ToString());
                            var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == PhysicianId);
                            if (physician != null)
                            {
                                npi = "[" + physician.NationalProviderIdentifier + "]";
                            }
                        }
                    }
                    else if (UserManager.IsInRole(CurrentUser.Id, "Physician"))
                    {
                        npi = "[" + CurrentUser.NPI + "]";
                    }
                    else if (UserManager.IsInRole(CurrentUser.Id, "Technician"))
                    {
                        int physicianId = unitOfWork.UserPhysicianRelationshippRepository.Get(x => x.UserId == CurrentUser.UserId).First().PhysicianId;
                        var physician = unitOfWork.PhysiciansRepository.GetSingle(x => x.PhysicianId == physicianId);

                        if (physician != null)
                        {
                            npi = "[" + physician.NationalProviderIdentifier + "]";
                        }
                    }


                    string date1 = "[" + startDate + "T00:00:00]";
                    string date2 = "[" + endDate + "T00:00:00]";
                    conn.Open();
                    var mdxQuery = new StringBuilder();

                    //mdxQuery.Append("SELECT NON EMPTY { [Measures].[Prescription Count] } ON 0, NON EMPTY { ([Primary Physician].[National Provider Identifier].[National Provider Identifier].ALLMEMBERS * [Bill Date].[Day Date].[Day Date].ALLMEMBERS ) *[Drug].[Product Name].[Product Name].AllMembers} ON 1  FROM ( SELECT ( { STRTOSET('[Primary Physician].[National Provider Identifier].&[1003849050]') } ) ON COLUMNS FROM ( SELECT ( STRTOMEMBER('[Bill Date].[Calendar].[Date].&[2017-03-01T00:00:00]', CONSTRAINED) : STRTOMEMBER('[Bill Date].[Calendar].[Date].&[2017-03-31T00:00:00]', CONSTRAINED) ) ON COLUMNS FROM [MH]))");
                    mdxQuery.Append(" SELECT NON EMPTY { [Measures].[Prescription Count] } ON 0, NON EMPTY { ([Primary Physician].[National Provider Identifier].[National Provider Identifier].ALLMEMBERS * [Bill Date].[Day Date].[Day Date].ALLMEMBERS ) *[Primary Insurance].[Insurance Name].[Insurance Name].ALLMEMBERS} ON 1 FROM ( SELECT ( { STRTOSET('[Primary Physician].[National Provider Identifier].&" + npi + "') } ) ON COLUMNS FROM ( SELECT ( STRTOMEMBER('[Bill Date].[Calendar].[Date].&" + date1 + "', CONSTRAINED) : STRTOMEMBER('[Bill Date].[Calendar].[Date].&" + date2 + "', CONSTRAINED) ) ON COLUMNS FROM [MH]))");


                    using (AdomdCommand cmd = new AdomdCommand(mdxQuery.ToString(), conn))
                    {

                        ds.EnforceConstraints = false;
                        ds.Tables.Add();
                        DataTable dt = ds.Tables[0];
                        dt.Load(cmd.ExecuteReader());

                        foreach (DataRow row in dt.Rows)
                        {
                            listItems1.Add(new listItem { Date = (string)row[1], PatientCount = (int?)row[3] });
                        }
                        //return dt;

                        dt.Columns[0].ColumnName = "Col1";
                        dt.Columns[1].ColumnName = "Col2";
                        dt.Columns[2].ColumnName = "Col3";
                        dt.Columns[3].ColumnName = "Col4";


                        DataView view = new DataView(dt);

                        //DataTable ds1 = dt.DefaultView.ToTable(true, "Col3");

                        var seriesNames = dt.AsEnumerable()
                       .Select(row => new
                       {

                           productname = row.Field<string>("Col3")

                       })
                       .Distinct().ToList();

                        var datesList = dt.AsEnumerable()
                     .Select(row => new
                     {
                         productname = row.Field<string>("Col2")
                     })
                     .Distinct().ToList();

                        List<SeriesData> srData = new List<SeriesData>();

                        foreach (var item in seriesNames.Select(t => t.productname))
                        {
                            var series = new SeriesData();
                            series.name = item.ToString();
                            series.data = new List<int>();
                            foreach (var date in datesList.Select(t => t.productname))
                            {
                                var aa = dt.AsEnumerable().Where(t => t.Field<string>("Col3") == series.name && t.Field<string>("Col2") == date.ToString())
                                    .Select(t => t.Field<int>("Col4")).FirstOrDefault();
                                if (aa != 0)
                                {
                                    series.data.Add(aa);
                                }
                                else
                                    series.data.Add(0);
                            }
                            srData.Add(series);
                        }
                        conn.Close();


                        return Json(new { srData = srData, datesList = datesList.Select(t => t.productname), success = true });
                    }

                }


            }
            catch (Exception exception)
            {
                //LogInfo(exception.Message, exception);
                Logger.InsertLog("Exception thrown in Method GetPhysicianChartData4 HomeController. " + exception.Message, exception);
                return Json(new { message = "Error in GetPhysicianChartData4 Function in DashBoard Controller", success = false });
            }
        }
    }
}