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
                    .Include(a=>a.Author)
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
                    var authorId = database.Users
                       .Where(u => u.UserName == this.User.Identity.Name)
                       .First()
                       .Id;

                    album.AuthorId = authorId;

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
                var album = database.Albums.Where(c => c.Id == id)
                                    .Include(c => c.Author)
                    .First();


                if (!IsUserAuthorizedToEdit(album))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

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
                    .Where(c => c.Id == id)
                                    .Include(c => c.Author)
                    .First();

                if (!IsUserAuthorizedToEdit(album))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

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
        private bool IsUserAuthorizedToEdit(Album album)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = album.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }       
    }
}