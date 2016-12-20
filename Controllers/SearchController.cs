using MVCPhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPhotoGallery.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(string search)
        {
            ViewBag.SearchPattern = search;

            using(var database = new PhotoGalleryDbContext())
            {
                var albums = database.Albums.Where(x => x.Name.Contains(search))
                    .Include(a=>a.Author).ToList();
                return View(albums);
            }       
        }
    }
}