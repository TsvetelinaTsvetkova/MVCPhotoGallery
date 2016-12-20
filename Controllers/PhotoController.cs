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
            var  FileName = Guid.NewGuid().ToString() + "_" +
             Path.GetFileName(picture.FileName);

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

                    if (string.IsNullOrEmpty(photo.Title))
                    {
                        photo.Title = "No name";
                    }

                    dbContext.Photos.Add(photo);
                    dbContext.SaveChanges();
                }

                return RedirectToAction("ListPhotos", "Home",new {@albumId = photo.AlbumId });
            }

            return RedirectToAction("Upload");
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

                ViewBag.Comments = dbContext.Comments.Where(c => c.PhotoId == id).Include(c=>c.Author).ToList();

                return View(photo);
            }
        }

        [Authorize]
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
                    .Include(p=>p.Album)
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

        [Authorize]
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

                string fullPath = AppDomain.CurrentDomain.BaseDirectory + photo.Path;

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                return RedirectToAction("ListPhotos", "Home", new { albumId = photo.AlbumId });
            }
        }

        [Authorize]
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
                model.AlbumId = photo.AlbumId;
                model.Albums = database.Albums
                    .OrderBy(a => a.Name)
                    .ToList();

                return View(model);
            }
        }

        [Authorize]
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
                    photo.AlbumId = model.AlbumId;

                    if (model.ImageUpload != null)
                    {
                        photo.Path = this.SavePostedFile(model.ImageUpload);
                    }
                    
                    database.Entry(photo).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Details","Photo", new { id = model.Id });
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
