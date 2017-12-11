namespace SocialNetwork.Web.Areas.User.Models.Photos
{
    using SocialNetwork.DataModel.Models;
    using System.Collections.Generic;

    public class AlbumsModel
    {
        public IEnumerable<Album> Albums { get; set; }
        public string AlbumsOwnerNames { get; set; }
        public bool MyAlbums { get; set; }
    }
}
