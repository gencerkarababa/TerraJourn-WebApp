using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;

namespace TerraJourn.Controllers
{
    public class GuideController : Controller
    {
        private readonly Context _context = new Context();

        public ActionResult Index()
        {
            var categories = _context.PlaceCategories.ToList();
            return View(categories);
        }

        public ActionResult ListPlacesByCategory(int id)
        {
            var category = _context.PlaceCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var places = _context.TouristAttractions
                .Where(ta => ta.PlaceCategories.Any(pc => pc.PlaceCategoryId == id))
                .ToList();

            ViewBag.CategoryName = category.Name;
            return View(places);
        }

        public ActionResult PlaceDetail(int id)
        {
            var place = _context.TouristAttractions
                .Include(t => t.PlaceCategories)
                .FirstOrDefault(t => t.TouristAttractionId == id);

            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        [HttpGet]
        public ActionResult Search(string query)
        {
            var filteredPlaces = _context.TouristAttractions
                .Include(t => t.PlaceCategories)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                filteredPlaces = filteredPlaces.Where(t => t.Name.ToLower().Contains(query.ToLower())
                                                            || t.Description.ToLower().Contains(query.ToLower()));
            }

            var results = filteredPlaces.ToList();
            ViewBag.SearchQuery = query;

            if (!results.Any())
            {
                ViewBag.Message = "Aradığınız kelimeyi içeren bir mekan bulunamadı.";
            }

            return View("ListPlacesByCategory", results);
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