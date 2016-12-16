﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using System.Linq;

namespace MVCPhotoGallery.Models
{
    
    public class PhotoGalleryDbContext : IdentityDbContext<ApplicationUser>
    {
        public PhotoGalleryDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        return base.SaveChanges();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
        //        throw new DbEntityValidationException(errorMessages);
        //    }
        //}
        public virtual IDbSet<Photo> Photos { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public static PhotoGalleryDbContext Create()
        {
            return new PhotoGalleryDbContext();
        }
    }
}
