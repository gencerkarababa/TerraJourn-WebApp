using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;

namespace TerraJourn.Areas.Member.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly Context _context = new Context();
        public ActionResult Index()
        {
            var user = GetCurrentUser();
            if (user == null) return RedirectToAction("Login", "Auth", new { area = "" });

            var userComments = _context.Comments.Include(c => c.Blog)
                                           .Where(c => c.UserId == user.UserId)
                                           .OrderByDescending(c => c.CreatedDate)
                                           .ToList();
            return View(userComments);
        }
        public ActionResult Edit(int id)
        {
            var commentToEdit = _context.Comments.Find(id);
            if (commentToEdit == null) return HttpNotFound();

            var currentUser = GetCurrentUser();
            if (currentUser == null || commentToEdit.UserId != currentUser.UserId)
            {
                return new HttpUnauthorizedResult();
            }

            return View(commentToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment updatedComment)
        {
            if (!ModelState.IsValid) return View(updatedComment);

            var currentUser = GetCurrentUser();
            var commentInDb = _context.Comments.Find(updatedComment.CommentId);

            if (currentUser == null || commentInDb == null || commentInDb.UserId != currentUser.UserId)
            {
                return new HttpUnauthorizedResult();
            }

            commentInDb.Text = updatedComment.Text;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var comment = _context.Comments.Find(id);
            var currentUser = GetCurrentUser();

            if (currentUser == null || comment == null || comment.UserId != currentUser.UserId)
            {
                return new HttpUnauthorizedResult();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // --- PRIVATE HELPER METHOD ---
        private User GetCurrentUser()
        {
            var userName = Session["UserName"] as string;
            if (string.IsNullOrEmpty(userName)) return null;
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