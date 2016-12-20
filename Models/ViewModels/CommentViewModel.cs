using MVCPhotoGallery.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCPhotoGallery.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public int PhotoId { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public List<Photo> Photos { get; set;}
    }
}