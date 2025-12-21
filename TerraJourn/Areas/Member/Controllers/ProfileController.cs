using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;

namespace TerraJourn.Areas.Member.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly Context _context = new Context();
        public ActionResult Index()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // --- PRIVATE HELPER METHOD ---
        private User GetCurrentUser()
        {
            var userName = Session["UserName"] as string;
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}