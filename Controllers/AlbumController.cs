using MVCPhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPhotoGallery.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Album
        public ActionResult List()
        {
            using (var database = new PhotoGalleryDbContext())
            {
                return View(database.Albums.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                using (var database = new PhotoGalleryDbContext())
                {
                    database.Albums.Add(album);
                    database.SaveChanges();

                    return RedirectToAction("List");
                }
            }

            return View(album);
        }
    }
}