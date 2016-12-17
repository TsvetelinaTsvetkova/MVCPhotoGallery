using MVCPhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCPhotoGallery.Controllers
{
    public class PhotoController : Controller
    {

        private string SavePostedFile(HttpPostedFileBase picture)
        {
            string pic = System.IO.Path.GetFileName(picture.FileName);
            string path = System.IO.Path.Combine(
                                   Server.MapPath("~/Content/Images"), pic);

            picture.SaveAs(path);

            using (MemoryStream ms = new MemoryStream())
            {
                picture.InputStream.CopyTo(ms);
                byte[] array = ms.GetBuffer();
            }

            return "/Content/Images/" + pic;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Upload()
        {
            using (var database = new PhotoGalleryDbContext())
            {
                var model = new PhotoViewModel();

                model.Albums = database.Albums
                                    .OrderBy(a => a.Name)
                                    .ToList();
                return View(model);
            }

        }



        public ActionResult Photos()
        {
            using (PhotoGalleryDbContext dbContext = new PhotoGalleryDbContext())
            {
                var photos = dbContext.Photos
                    .Include(p => p.Author)
                        .ToList();

                return View(photos);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Upload(HttpPostedFileBase picture, Photo photo, PhotoViewModel model)
        {
            if (picture != null)
            {
                photo.Path = this.SavePostedFile(picture);
           
                using (PhotoGalleryDbContext dbContext = new PhotoGalleryDbContext())
                {
                    var authorId = dbContext.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;


                    photo.AuthorId = authorId;

                    photo.AlbumId = model.AlbumId;

                    dbContext.Photos.Add(photo);
                    dbContext.SaveChanges();
                }

                return RedirectToAction("ListAlbums", "Home");
            }

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (PhotoGalleryDbContext dbContext = new PhotoGalleryDbContext())
            {
                var photo = dbContext.Photos
                    .Where(p => p.Id == id)
                    .Include(p => p.Author)
                    .First();

                if (photo == null)
                {
                    return HttpNotFound();
                }

                return View(photo);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new PhotoGalleryDbContext())
            {
                var photo = database.Photos
                    .Where(p => p.Id == id)
                    .Include(p => p.Author)
                    .First();


                if (!IsUserAuthorizedToEdit(photo))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (photo == null)
                {
                    return HttpNotFound();
                }

                return View(photo);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new PhotoGalleryDbContext())
            {
                var photo = database.Photos
                                    .Where(p => p.Id == id)
                                    .Include(p => p.Author)
                                    .First();

                if (photo == null)
                {
                    return HttpNotFound();
                }

                database.Photos.Remove(photo);
                database.SaveChanges();

                return RedirectToAction("Photos");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new PhotoGalleryDbContext())
            {
                var photo = database.Photos
                                    .Where(p => p.Id == id)
                                    .First();
                if (!IsUserAuthorizedToEdit(photo))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (photo == null)
                {
                    return HttpNotFound();
                }

                var model = new PhotoViewModel();
                model.Id = photo.Id;
                model.Title = photo.Title;
                model.Path = photo.Path;


                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(PhotoViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new PhotoGalleryDbContext())
                {
                    var photo = database.Photos
                        .FirstOrDefault(p => p.Id == model.Id);

                    photo.Title = model.Title;
                    photo.Path = this.SavePostedFile(model.ImageUpload);

                    database.Entry(photo).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Photos");
                }
            }
            return View(model);
        }

        private bool IsUserAuthorizedToEdit(Photo photo)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = photo.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}
