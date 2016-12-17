using MVCPhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCPhotoGallery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("ListAlbums");
        }

        public ActionResult ListAlbums()
        {
            using(var database = new PhotoGalleryDbContext())
            {
                var albums = database.Albums
                    .Include(a => a.Photos)
                    .OrderBy(a => a.Name)
                    .ToList();

                return View(albums);
            }
        }

        public ActionResult ListPhotos(int? albumId)
        {
            if (albumId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new PhotoGalleryDbContext())
            {
                var photos = database.Photos
                    .Where(a => a.AlbumId == albumId)
                    .Include(a => a.Author)
                    .ToList();

                return View(photos);
            }
        }

    
}
}