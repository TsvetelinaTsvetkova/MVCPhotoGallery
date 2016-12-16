using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCPhotoGallery.Models
{
    public class Category
    {
        public ICollection<Photo> photos;

        public Category()
        {
            this.photos = new HashSet<Photo>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

    }
}