using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TerraJourn.Models.Entities;
using PagedList;
using System.Data.Entity;
using TerraJourn.Data;
using TerraJourn.Models.ViewModels;

namespace TerraJourn.Controllers
{
    public class BlogController : Controller
    {
        private readonly Context _context = new Context();

        public ActionResult Index(string categoryName, int page = 1)
        {
            var viewModel = new BlogIndexViewModel();
            viewModel.Categories = _context.BlogCategories.ToList();
            IQueryable<Blog> blogsQuery = _context.Blogs.Include(b => b.BlogCategories);

            if (!string.IsNullOrEmpty(categoryName))
            {
                string actualCategoryName = categoryName.Replace("-", " ");
                blogsQuery = blogsQuery.Where(b => b.BlogCategories.Any(c => c.Name == actualCategoryName));
            }

            viewModel.PagedBlogs = blogsQuery.OrderByDescending(b => b.CreatedDate).ToPagedList(page, 5);
            ViewBag.CurrentCategory = categoryName;

            return View(viewModel);
        }

        public ActionResult BlogDetails(int id)
        {
            var blog = _context.Blogs
                               .Include(b => b.User)
                               .Include(b => b.BlogCategories)
                               .FirstOrDefault(b => b.BlogId == id);

            if (blog == null)
            {
                return HttpNotFound();
            }

            var viewModel = new BlogDetailsViewModel
            {
                Blog = blog,
                Comments = _context.Comments.Include(c => c.User).Where(c => c.BlogId == id).ToList(),
                Categories = _context.BlogCategories.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Search(string query, int page = 1)
        {
            var viewModel = new BlogIndexViewModel();
            viewModel.Categories = _context.BlogCategories.ToList();

            var results = _context.Blogs.Include(b => b.BlogCategories)
                                 .Where(b => b.Title.ToLower().Contains(query.ToLower())
                                             || b.Content.ToLower().Contains(query.ToLower()))
                                 .OrderByDescending(b => b.CreatedDate);

            viewModel.PagedBlogs = results.ToPagedList(page, 5);
            ViewBag.SearchQuery = query;

            if (!results.Any())
            {
                ViewBag.Message = "Aradığınız kelimeyi içeren bir blog bulunamadı.";
            }

            return View("Index", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitComment(Comment comment)
        {
            var userName = Session["UserName"] as string;
            if (string.IsNullOrEmpty(userName))
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı. Lütfen giriş yapın." });
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return Json(new { success = false, message = "Geçersiz kullanıcı." });
            }

            comment.UserId = user.UserId;
            comment.CreatedDate = DateTime.Now;

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Json(new { success = true, message = "Yorum başarıyla gönderildi." });
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