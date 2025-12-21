using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;

namespace TerraJourn.Areas.Admin.Controllers
{
    [Authorize]
    public class PlaceCategoriesController : Controller
    {
        private readonly Context _context = new Context();

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlaceCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.PlaceCategories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public ActionResult Index()
        {
            var categories = _context.PlaceCategories.ToList();
            return View(categories);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = _context.PlaceCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlaceCategory category)
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
            var category = _context.PlaceCategories.Find(id);
            if (category != null)
            {
                _context.PlaceCategories.Remove(category);
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