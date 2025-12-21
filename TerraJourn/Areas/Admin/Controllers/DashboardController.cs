using System.Web.Mvc;
using TerraJourn.Data;

namespace TerraJourn.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}