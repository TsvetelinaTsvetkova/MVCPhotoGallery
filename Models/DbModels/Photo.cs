using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCPhotoGallery.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public DateTime DateAdded { get; set; }

        public string Path { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        private ICollection<Album> albums;

        public Photo()
        {
            this.DateAdded = DateTime.Now;
            this.albums = new HashSet<Album>();  
        }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }

        public virtual ICollection<Album> Albums
        {
            get
            {
                return this.albums;
            }

            set
            {
                this.albums = value;
            }
        }

    }
}