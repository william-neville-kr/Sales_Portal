using SalesTeam.Core.Data;
using SalesTeam.Core.Helpers;
using SalesTeam.Core.Repository;
using System.Linq;
using System.Web.Mvc;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class SalesTeamDetailsController : BaseController
    {
        protected readonly UnitOfWork _unitOfWork = new UnitOfWork();
        // GET: SalesTeamDetails
        //public ActionResult Index(int? id)
        public ActionResult Index(string id)
        {
            int SalesTeamId = id.Decrypt().ToInt0();
            ViewBag.id = SalesTeamId;
            vwSalesTeam salesTeam = _unitOfWork.vwSalesTeamsRepository.GetAsQuerable(t => t.SalesTeamId == SalesTeamId).FirstOrDefault();
            if (salesTeam == null)
            {
                salesTeam = new vwSalesTeam();
            }
            return View(salesTeam);
        }
    }
}
