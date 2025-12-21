using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TerraJourn.Data;
using TerraJourn.Helpers;
using TerraJourn.Models.Entities;
using TerraJourn.Models.ViewModels;

namespace TerraJourn.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context = new Context();

        // --- LOGIN ACTIONS ---
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Lütfen tüm alanları doldurun.";
                return View(model);
            }

            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == model.UserName);

            if (userInDb != null && PasswordHelper.VerifyPassword(model.Password, userInDb.PasswordHash))
            {
                FormsAuthentication.SetAuthCookie(userInDb.UserName, false);
                Session["UserName"] = userInDb.UserName;

                if (userInDb.Role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");
                else
                    return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre!";
            return View(model);
        }

        // --- LOGOUT ACTION ---
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        // --- REGISTER ACTIONS ---
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = _context.Users
                .FirstOrDefault(u => u.UserName == model.UserName || u.Email == model.Email);

            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Bu kullanıcı adı veya e-posta adresi zaten kullanılıyor.";
                return View(model);
            }

            try
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

                TempData["SuccessMessage"] = "Hesabınız başarıyla oluşturuldu. Şimdi giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => e.PropertyName + ": " + e.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                ViewBag.ErrorMessage = "Kayıt sırasında hata oluştu: " + fullErrorMessage;
                return View(model);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }
    }
}