using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;
using TerraJourn.Helpers;
using TerraJourn.Models.ViewModels;

namespace TerraJourn.Areas.Admin.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly Context _context = new Context();

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model)
        {
            if (_context.Users.Any(u => u.UserName == model.UserName || u.Email == model.Email))
            {
                ModelState.AddModelError("", "Bu kullanıcı adı veya e-posta adresi zaten kullanılıyor.");
            }

            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = PasswordHelper.HashPassword(model.Password),
                    Role = "User"
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        public ActionResult Details(int id)
        {
            var user = _context.Users
                             .Include(u => u.Blogs)
                             .Include(u => u.Comments)
                             .SingleOrDefault(u => u.UserId == id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userInDb = _context.Users.Find(id);
            if (userInDb == null) return HttpNotFound();

            var viewModel = new UserViewModel
            {
                UserId = userInDb.UserId,
                UserName = userInDb.UserName,
                Email = userInDb.Email,
                Role = userInDb.Role
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel model)
        {

            if (ModelState.IsValid)
            {
                var userInDb = _context.Users.Find(model.UserId);
                if (userInDb == null)
                {
                    return HttpNotFound();
                }

                userInDb.UserName = model.UserName;
                userInDb.Email = model.Email;
                userInDb.Role = model.Role;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    userInDb.PasswordHash = PasswordHelper.HashPassword(model.Password);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var userToDelete = _context.Users.Find(id);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
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