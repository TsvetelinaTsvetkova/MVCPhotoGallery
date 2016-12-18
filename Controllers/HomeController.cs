﻿using MVCPhotoGallery.Models;
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
            using (var database = new PhotoGalleryDbContext())
            {
                var albums = database.Albums
                    .Include(a => a.Photos)
                    .OrderBy(a => a.Name)
                    .ToList();

                return View(albums);
            }
        }

        public ActionResult ListPhotos(int? albumId, int page = 1)
        {
            int pageSize = 6;

            if (albumId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new PhotoGalleryDbContext())
            {
                var allPhotos = database.Photos
                     .Where(a => a.AlbumId == albumId).Include(a => a.Author).ToList();

                var photos = allPhotos.Skip((page - 1) * pageSize)
                     .Take(pageSize).ToList();

                var count = database.Photos
                     .Where(a => a.AlbumId == albumId).ToList().Count();
                //this.ViewBag.MaxPage = (count / pageSize) -
                //    (count % pageSize == 0 ? 1 : 0);

                this.ViewBag.Page = page;
                this.ViewBag.AlbumId = albumId.ToString();

                var maxPage = Math.Ceiling(count / (double)pageSize);
                this.ViewBag.MaxPage = maxPage;

                return View(photos);
            }
        }
    }
}