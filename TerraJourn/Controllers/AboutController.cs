using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TerraJourn.Models.Entities;
using TerraJourn.Data;

namespace TerraJourn.Controllers
{
    public class AboutController : Controller
    {
        private readonly Context _context = new Context();

        public ActionResult Index()
        {
            var aboutUsSections = _context.AboutUsSections.ToList();
            return View(aboutUsSections);
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

