using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCPhotoGallery.Models.ViewModels
{
    public class AlbumViewModel
    {
        public Album Album { get; private set; }

        public List<Photo> AllPhotos { get; private set; }

        public AlbumViewModel(Album album, List<Photo> allPhotos)
        {
            this.Album = album;
            this.AllPhotos = allPhotos;
        }

        public AlbumViewModel()
        {

        }
    }
}