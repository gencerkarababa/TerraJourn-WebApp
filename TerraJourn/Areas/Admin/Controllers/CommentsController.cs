using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;

namespace TerraJourn.Areas.Admin.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly Context _context = new Context();

        public ActionResult Index()
        {
            var comments = _context.Comments.ToList();
            return View(comments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var commentToDelete = _context.Comments.Find(id);
            if (commentToDelete != null)
            {
                _context.Comments.Remove(commentToDelete);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
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