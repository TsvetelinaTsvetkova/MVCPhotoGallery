using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPhotoGallery.Models
{
    public class Album
    {
        private ICollection<Photo> photos;

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Photo> Photos
        {
            get
            {
                return this.photos;
            }
            set
            {
                this.photos = value;
            }

        }

        public Album()
        {
            this.photos = new HashSet<Photo>();
        }
    }
}