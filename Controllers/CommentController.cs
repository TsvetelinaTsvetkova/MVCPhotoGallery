using MVCPhotoGallery.Models;
using MVCPhotoGallery.Models.DbModels;
using MVCPhotoGallery.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCPhotoGallery.Controllers
{
    public class CommentController : Controller
    {

        // GET: Comment
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(int? id)
        {           
            return RedirectToAction("Details","Photo",new { id = id });   
        }

        [Authorize]
        public  ActionResult Create(int id)
        {
            ViewBag.PhotoId = id;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Comment comment, int? id)
        {
            if (ModelState.IsValid)
            {
                using (var database = new PhotoGalleryDbContext())
                {
                    var authorId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    comment.AuthorId = authorId;
                    comment.PhotoId = id.Value;
                    comment.DateAdded = DateTime.Now;
                    database.Comments.Add(comment);

                    database.SaveChanges();
                    ViewBag.Id = id;

                    return RedirectToAction("List", new { id = id });
                }
            }

            ViewBag.PhotoId = id.Value;
            return View(comment);
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
                var comment = database.Comments
                    .Where(c => c.Id == id)
                    .Include(c => c.Author)
                    .First();

                if (!IsUserAuthorizedToEdit(comment))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (comment == null)
                {
                    return HttpNotFound();
                }

                return View(comment);
            }
        }

        private bool IsUserAuthorizedToEdit(Comment comment)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = comment.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
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
                var comment = database.Comments
                                    .Where(c => c.Id == id)
                                    .Include(c => c.Author)
                                    .First();

                if (comment == null)
                {
                    return HttpNotFound();
                }

                database.Comments.Remove(comment);
                database.SaveChanges();

                return RedirectToAction("Details","Photo", new {id = comment.PhotoId });
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
                var comment = database.Comments
                                    .Where(a => a.Id == id)
                                    .First();

                if (!IsUserAuthorizedToEdit(comment))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (comment == null)
                {
                    return HttpNotFound();
                }

                var model = new CommentViewModel();

                model.Id = comment.Id;
                model.Content = comment.Content;

                ViewBag.PhotoId = comment.PhotoId;

                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new PhotoGalleryDbContext())
                {
                    var comment = database.Comments
                        .FirstOrDefault(c => c.Id == model.Id);

                    comment.Content = model.Content;
                    comment.DateAdded = DateTime.Now;

                    database.Entry(comment).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Details","Photo",new { id=comment.PhotoId});
                }
            }

            return View(model);
        }
    }
}