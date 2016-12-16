using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCPhotoGallery.Models.DbModels
{
    public class PhotoAlbums
    {
        [Key]
        public int Id { get; set; }

        public int Photo_id { get; set; }

        public int Album_id { get; set; }
    }
}