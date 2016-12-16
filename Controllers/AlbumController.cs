using MVCPhotoGallery.Models;
using MVCPhotoGallery.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCPhotoGallery.Models.DbModels;

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
            using (var database = new PhotoGalleryDbContext())
            {
                AlbumViewModel albumViewModel = new AlbumViewModel(null, database.Photos.ToList());

                return View(albumViewModel);
            }
        }

        [HttpPost]
        public ActionResult Create(string name, List<int> AllPhotos)
        {
            if (ModelState.IsValid)
            {
                using (var database = new PhotoGalleryDbContext())
                {
                    var album = new Album();
                    album.Name = name;
                    database.Albums.Add(album);
                    database.SaveChanges();

                    if (AllPhotos != null)
                    {
                        foreach (var photoId in AllPhotos)
                        {
                            database.PhotoAlbums.Add(new PhotoAlbums()
                            {
                                Photo_id = photoId,
                                Album_id = album.Id
                            });
                        }
                        database.SaveChanges();
                    }
                 
                    return RedirectToAction("List");
                }
            }

            return RedirectToAction("Create");
        }
    }
}