using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SalesTeam.Core.Repository;
using SalesTeam.Core.Data;
using SalesTeam.Core.Helpers;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class ViewPhysicianDetailsController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();
        // GET: ViewPhysicianDetails
        public ActionResult Index(string id)
        {
            string[] ids = id.Decrypt().Split('&');

            int sTeamId = ids[0].ToInt0();
            int pId = ids[1].ToInt0();

            ViewBag.SalesTeamId = sTeamId;
            ViewBag.PhysicianId = pId;
            ViewBag.GetReferralFormCategories = GetReferralFormCategories();
            ViewBag.GetPhysicianAddressList = GetPhysicianAddressList(pId);

            var _vwPhysician =
                _unitOfWork.vwPhysiciansRepository.GetAsQuerable(t => t.PhysicianId == pId
                //&& t.SalesTeamId == sTeamId
                ).FirstOrDefault();

            ViewBag.SalesRepresentativeFullName = "";
            AssociatesSalesTeam associatesSalesTeam = _unitOfWork.AssociatesSalesTeamRepository.GetSingle(t => t.PhysicianMasterId == pId && t.SalesTeamId == sTeamId);
            if (associatesSalesTeam != null)
            {
                ViewBag.SalesRepresentativeFullName = associatesSalesTeam.SalesRepresentativeFullName;
            }
            //var associatesSalesTeamList = _unitOfWork.AssociatesSalesTeamRepository.GetAsQuerable(t => t.PhysicianMasterId == pId && t.PatientId == sTeamId).ToList();
            //if (associatesSalesTeamList != null)
            //{
            //    foreach (var associatesSalesTeam in associatesSalesTeamList)
            //    {
            //        if (ViewBag.SalesRepresentativeFullName != "")
            //        {
            //            ViewBag.SalesRepresentativeFullName += ", ";
            //        }
            //        ViewBag.SalesRepresentativeFullName += associatesSalesTeam.SalesRepresentativeFullName;
            //    }
            //}
            return View(_vwPhysician);
        }

        public ActionResult getPhysicianAddressRecords([DataSourceRequest] DataSourceRequest request,
            vwPhysician _vwPhysician = null)
        {
            //CheckMasterSalesTeamId();
            var physicianID = _vwPhysician.PhysicianId;
            var vwPhysicianAddressList = _unitOfWork.vwPhysicianAddressRepository.GetAsQuerable(t => t.PhysicianId == physicianID);
            return Json(vwPhysicianAddressList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IQueryable GetReferralFormCategories()
        {
            return _unitOfWork.ReferralFormCategoryRepository.GetAsQuerable().Select(t => new SelectListItem { Text = t.CategoryName, Value = t.ReferralFormCategoryId.ToString() });
        }
        private IQueryable GetPhysicianAddressList(int physicianId)
        {
            return _unitOfWork.vwPhysicianAddressRepository.GetAsQuerable(t => t.PhysicianId == physicianId);
        }
    }
}