using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCPhotoGallery.Models.DbModels
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public DateTime DateAdded { get; set; }

        public Comment(string authorId)
        {
            this.DateAdded = DateTime.Now;
            this.AuthorId = authorId;
        }


        [ForeignKey("Photo")]
        public int PhotoId { get; set; }

        public virtual Photo Photo { get; set; }

        public Comment()
        {

        }
        public Comment(string authorId, string content, int photoId)
        {
            this.AuthorId = authorId;
            this.Content = content;
            this.PhotoId = photoId;
        }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }

    }
}