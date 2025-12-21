using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;
using System.Collections.Generic;

namespace TerraJourn.Areas.Member.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly Context _context = new Context();

        [HttpGet]
        public ActionResult Create()
        {
            PopulateCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog, int[] selectedCategories)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)
            {
                if (selectedCategories != null)
                {
                    blog.BlogCategories = new List<BlogCategory>();
                    foreach (var categoryId in selectedCategories)
                    {
                        var category = _context.BlogCategories.Find(categoryId);
                        if (category != null)
                        {
                            blog.BlogCategories.Add(category);
                        }
                    }
                }

                blog.UserId = user.UserId;
                blog.CreatedDate = DateTime.Now;
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateCategories(selectedCategories);
            return View(blog);
        }

        public ActionResult Index()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            var userBlogs = _context.Blogs
                .Include(b => b.BlogCategories)
                .Where(b => b.UserId == user.UserId)
                .OrderByDescending(b => b.CreatedDate)
                .ToList();

            return View(userBlogs);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var blogToEdit = _context.Blogs
                                     .Include(b => b.BlogCategories)
                                     .SingleOrDefault(b => b.BlogId == id);

            if (blogToEdit == null) return HttpNotFound();

            var currentUser = GetCurrentUser();
            if (currentUser == null || blogToEdit.UserId != currentUser.UserId)
            {
                return new HttpUnauthorizedResult();
            }

            var selectedCategoryIds = blogToEdit.BlogCategories.Select(bc => bc.BlogCategoryId);
            PopulateCategories(selectedCategoryIds);

            return View(blogToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Blog updatedBlog, int[] selectedCategories)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null) return new HttpUnauthorizedResult();

            var blogInDb = _context.Blogs
                                 .Include(b => b.BlogCategories)
                                 .SingleOrDefault(b => b.BlogId == id);

            if (blogInDb == null || blogInDb.UserId != currentUser.UserId)
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)
            {
                blogInDb.Title = updatedBlog.Title;
                blogInDb.Content = updatedBlog.Content;
                blogInDb.ImageUrl = updatedBlog.ImageUrl;

                UpdateBlogCategories(selectedCategories, blogInDb);

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateCategories(selectedCategories);
            return View(updatedBlog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var blog = _context.Blogs.Find(id);
            var currentUser = GetCurrentUser();

            if (currentUser == null || blog == null || blog.UserId != currentUser.UserId)
            {
                return new HttpUnauthorizedResult();
            }

            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // --- PRIVATE HELPER METHODS ---
        private void PopulateCategories(IEnumerable<int> selectedCategoryIds = null)
        {
            var allCategories = _context.BlogCategories.ToList();
            ViewBag.BlogCategories = new SelectList(allCategories, "BlogCategoryId", "Name", selectedCategoryIds);
        }

        private void UpdateBlogCategories(int[] selectedCategories, Blog blogToUpdate)
        {
            blogToUpdate.BlogCategories.Clear();

            if (selectedCategories != null)
            {
                foreach (var categoryId in selectedCategories)
                {
                    var category = _context.BlogCategories.Find(categoryId);
                    if (category != null)
                    {
                        blogToUpdate.BlogCategories.Add(category);
                    }
                }
            }
        }

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