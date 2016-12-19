using MVCPhotoGallery.Models.DbModels;
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

        [ForeignKey("Album")]
        public int AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public DateTime DateAdded { get; set; }

        public string Path { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public Photo()
        {
            this.DateAdded = DateTime.Now;
        }

        public Photo(string authorId, string title, int albumId, string path)
        {
            this.AuthorId = authorId;
            this.Title = title;
            this.AlbumId = albumId;
            this.Path = path;
        }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }

    }
}