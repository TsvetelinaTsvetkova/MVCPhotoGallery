using MVCPhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCPhotoGallery.Controllers
{
    public class AlbumController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var database = new PhotoGalleryDbContext())
            {
                var albums = database.Albums
                    .ToList();

                return View(albums);
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

                    return RedirectToAction("Index");
                }
            }

            return View(album);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new PhotoGalleryDbContext())
            {
                var album = database.Albums
                    .FirstOrDefault(c => c.Id == id);

                if (album == null)
                {
                    return HttpNotFound();
                }

                return View(album);
            }
        }

        [HttpPost]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                using (var database = new PhotoGalleryDbContext())
                {
                    database.Entry(album).State = System.Data.Entity.EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(album);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new PhotoGalleryDbContext())
            {
                var album = database.Albums
                    .FirstOrDefault(c => c.Id == id);

                if (album == null)
                {
                    return HttpNotFound();
                }

                return View(album);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var database = new PhotoGalleryDbContext())
            {
                var album = database.Albums
                    .FirstOrDefault(c => c.Id == id);

                var albumPhotos = album.Photos
                    .ToList();

                foreach (var photo in albumPhotos)
                {
                    database.Photos.Remove(photo);
                }

                database.Albums.Remove(album);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}