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
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }


    }
}