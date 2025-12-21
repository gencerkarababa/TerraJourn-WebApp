using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;

namespace TerraJourn.Areas.Admin.Controllers
{
    [Authorize]
    public class BlogCategoriesController : Controller
    {
        private readonly Context _context = new Context();

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.BlogCategories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Index()
        {
            var categories = _context.BlogCategories.ToList();
            return View(categories);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = _context.BlogCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var category = _context.BlogCategories.Find(id);
            if (category != null)
            {
                _context.BlogCategories.Remove(category);
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