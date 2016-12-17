using System.Data.Entity;
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

        public virtual IDbSet<Photo> Photos { get; set; }


        public virtual IDbSet<Album> Albums { get; set; }


        public static PhotoGalleryDbContext Create()
        {
            return new PhotoGalleryDbContext();
        }
    }
}
