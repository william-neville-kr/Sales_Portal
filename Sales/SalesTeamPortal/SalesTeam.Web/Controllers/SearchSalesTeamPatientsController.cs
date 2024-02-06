using System.Linq;
using System.Web.Mvc;

namespace SalesTeam.Web.Controllers
{
    [Authorize]
    public class SearchSalesTeamPatientsController : BaseController
    {
        protected readonly Core.Repository.UnitOfWork _unitOfWork = new Core.Repository.UnitOfWork();
        // GET: SearchSalesTeamPatients
        public ActionResult Index()
        {
            return View();
        }
    }
}