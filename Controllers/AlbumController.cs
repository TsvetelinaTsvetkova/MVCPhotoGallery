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
        // GET: Category
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

        //[HttpPost]
        //public ActionResult Edit(AlbumModel category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (var database = new PhotoGalleryDbContext())
        //        {
        //            database.Entry(category).State = System.Data.Entity.EntityState.Modified;
        //            database.SaveChanges();

        //            return RedirectToAction("Index");
        //        }
        //    }

        //    return View(category);
        //}

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    using (var database = new PhotoGalleryDbContext())
        //    {
        //        var category = database.Categories
        //            .FirstOrDefault(c => c.Id == id);

        //        if (category == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        return View(category);
        //    }
        //}

        //[HttpPost]
        //[ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int? id)
        //{
        //    using (var database = new PhotoGalleryDbContext())
        //    {
        //        var category = database.Categories
        //            .FirstOrDefault(c => c.Id == id);

        //        var categoryPhotos = category.Photos
        //            .ToList();

        //        foreach (var photo in categoryPhotos)
        //        {
        //            database.Photos.Remove(photo);
        //        }

        //        database.Categories.Remove(category);
        //        database.SaveChanges();

        //        return RedirectToAction("Index");
        //    }
        //}
    }
}