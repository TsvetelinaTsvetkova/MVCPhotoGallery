using MVCPhotoGallery.Migrations;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;
using MVCPhotoGallery.Models;

[assembly: OwinStartupAttribute(typeof(MVCPhotoGallery.Startup))]
namespace MVCPhotoGallery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<PhotoGalleryDbContext, Configuration>());
            ConfigureAuth(app);
        }
    }
}
