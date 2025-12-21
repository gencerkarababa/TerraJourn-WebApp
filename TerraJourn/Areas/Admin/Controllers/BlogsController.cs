using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TerraJourn.Models.ViewModels; 
using TerraJourn.Data;
using TerraJourn.Models.Entities;

namespace TerraJourn.Areas.Admin.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly Context _context = new Context();

        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new BlogFormViewModel
            {
                Blog = new Blog(),
                Categories = _context.BlogCategories.Select(c => new AssignedCategoryData
                {
                    CategoryID = c.BlogCategoryId,
                    Name = c.Name,
                    Assigned = false
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogFormViewModel viewModel, string[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                var newBlog = viewModel.Blog;
                newBlog.CreatedDate = System.DateTime.Now;
                newBlog.BlogCategories = new List<BlogCategory>();

                if (selectedCategories != null)
                {
                    foreach (var categoryId in selectedCategories)
                    {
                        var categoryToAdd = _context.BlogCategories.Find(int.Parse(categoryId));
                        newBlog.BlogCategories.Add(categoryToAdd);
                    }
                }

                _context.Blogs.Add(newBlog);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Categories = _context.BlogCategories.ToList().Select(c => new AssignedCategoryData
            {
                CategoryID = c.BlogCategoryId,
                Name = c.Name,
                Assigned = false
            }).ToList();
            return View(viewModel);
        }
        public ActionResult Index()
        {
            var blogs = _context.Blogs.Include(b => b.BlogCategories).ToList();
            return View(blogs);

        }
        public ActionResult Details(int id)
        {
            var blog = _context.Blogs.Include(b => b.User).Include(b => b.BlogCategories).FirstOrDefault(x => x.BlogId == id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var blog = _context.Blogs.Include(b => b.BlogCategories).FirstOrDefault(b => b.BlogId == id);
            if (blog == null) return HttpNotFound();

            var allCategories = _context.BlogCategories.ToList();
            var blogCategoryIds = new HashSet<int>(blog.BlogCategories.Select(c => c.BlogCategoryId));

            var viewModel = new BlogFormViewModel
            {
                Blog = blog,
                Categories = allCategories.Select(c => new AssignedCategoryData
                {
                    CategoryID = c.BlogCategoryId,
                    Name = c.Name,
                    Assigned = blogCategoryIds.Contains(c.BlogCategoryId)
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogFormViewModel viewModel, string[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                var blogToUpdate = _context.Blogs.Include(b => b.BlogCategories)
                    .FirstOrDefault(b => b.BlogId == viewModel.Blog.BlogId);

                if (blogToUpdate == null) return HttpNotFound();

                blogToUpdate.Title = viewModel.Blog.Title;
                blogToUpdate.Content = viewModel.Blog.Content;
                blogToUpdate.ImageUrl = viewModel.Blog.ImageUrl;

                UpdateBlogCategories(selectedCategories, blogToUpdate);

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Blog başarıyla silindi.";
            }
            return RedirectToAction("Index");
        }

        // HELPER METHOD
        private void UpdateBlogCategories(string[] selectedCategories, Blog blogToUpdate)
        {
            if (selectedCategories == null)
            {
                blogToUpdate.BlogCategories.Clear();
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var blogCategories = new HashSet<int>(blogToUpdate.BlogCategories.Select(c => c.BlogCategoryId));

            foreach (var category in _context.BlogCategories)
            {
                if (selectedCategoriesHS.Contains(category.BlogCategoryId.ToString()))
                {
                    if (!blogCategories.Contains(category.BlogCategoryId))
                    {
                        blogToUpdate.BlogCategories.Add(category);
                    }
                }
                else
                {
                    if (blogCategories.Contains(category.BlogCategoryId))
                    {
                        var categoryToRemove = blogToUpdate.BlogCategories.Single(c => c.BlogCategoryId == category.BlogCategoryId);
                        blogToUpdate.BlogCategories.Remove(categoryToRemove);
                    }
                }
            }
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