using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TerraJourn.Models.Entities;
using TerraJourn.Data;

namespace TerraJourn.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context = new Context();

        public ActionResult Index()
        {
            var latestTouristAttractions = _context.TouristAttractions.OrderByDescending(b => b.TouristAttractionId).Take(4).ToList();
            return View(latestTouristAttractions);
        }

        public PartialViewResult RecentBlogsPartial()
        {
            var recentBlogs = _context.Blogs.OrderByDescending(b => b.BlogId).Take(10).ToList();
            return PartialView(recentBlogs);
        }

        public PartialViewResult AllBlogsPartial()
        {
            var allBlogs = _context.Blogs.ToList();
            return PartialView(allBlogs);
        }

        public PartialViewResult FeaturedAttractionsPartial()
        {
            var randomAttractions = _context.TouristAttractions.OrderBy(x => Guid.NewGuid()).Take(6).ToList();
            return PartialView(randomAttractions);
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