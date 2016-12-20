using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCPhotoGallery.Models
{
    public class PhotoViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string Path { get; set; }

        public HttpPostedFileBase ImageUpload { get; set; }

        public int AlbumId { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public List<Album> Albums { get; internal set; }
    }
}