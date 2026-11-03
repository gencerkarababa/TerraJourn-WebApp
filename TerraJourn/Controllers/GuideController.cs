using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;

namespace TerraJourn.Controllers
{
    public class GuideController : Controller
    {
        // GET: Guide
        public ActionResult Index()
        {
            return View();
        }
    }
}