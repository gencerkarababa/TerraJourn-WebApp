using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;

namespace TerraJourn.Areas.Admin.Controllers
{
    [Authorize]
    public class TouristAttractionsController : Controller
    {
        private readonly Context _context = new Context();

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PlaceCategories = _context.PlaceCategories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TouristAttraction attraction, int[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                if (selectedCategories != null)
                {
                    attraction.PlaceCategories = new System.Collections.Generic.List<PlaceCategory>();
                    foreach (var categoryId in selectedCategories)
                    {
                        var category = _context.PlaceCategories.Find(categoryId);
                        if (category != null)
                        {
                            attraction.PlaceCategories.Add(category);
                        }
                    }
                }
                _context.TouristAttractions.Add(attraction);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlaceCategories = _context.PlaceCategories.ToList();
            return View(attraction);
        }
        public ActionResult Index()
        {
            var attractions = _context.TouristAttractions.Include(t => t.PlaceCategories).ToList();
            return View(attractions);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var attraction = _context.TouristAttractions
                                     .Include(t => t.PlaceCategories)
                                     .SingleOrDefault(t => t.TouristAttractionId == id);

            if (attraction == null)
            {
                return HttpNotFound();
            }

            return View(attraction);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var attraction = _context.TouristAttractions.Include(t => t.PlaceCategories).SingleOrDefault(t => t.TouristAttractionId == id);
            if (attraction == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaceCategories = _context.PlaceCategories.ToList();
            return View(attraction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TouristAttraction attraction, int[] selectedCategories)
        {
            var attractionToUpdate = _context.TouristAttractions.Include(t => t.PlaceCategories).SingleOrDefault(t => t.TouristAttractionId == id);

            if (attractionToUpdate == null)
            {
                return HttpNotFound();
            }

            if (TryUpdateModel(attractionToUpdate, "", new[] { "Name", "ImageUrl", "Description", "Address", "Country", "City" }))
            {
                UpdateTouristAttractionCategories(selectedCategories, attractionToUpdate);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaceCategories = _context.PlaceCategories.ToList();
            return View(attraction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var attraction = _context.TouristAttractions.Find(id);
            if (attraction != null)
            {
                _context.TouristAttractions.Remove(attraction);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // HELPER METHOD
        private void UpdateTouristAttractionCategories(int[] selectedCategories, TouristAttraction attractionToUpdate)
        {
            attractionToUpdate.PlaceCategories.ToList().ForEach(c => attractionToUpdate.PlaceCategories.Remove(c));

            if (selectedCategories != null)
            {
                foreach (var categoryId in selectedCategories)
                {
                    var category = _context.PlaceCategories.Find(categoryId);
                    if (category != null)
                    {
                        attractionToUpdate.PlaceCategories.Add(category);
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