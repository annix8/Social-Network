namespace SocialNetwork.Services
{
    using DataModel;
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Services.Contracts;
    using System.IO;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    public class PictureService : IPictureService
    {
        private readonly SocialNetworkDbContext _db;

        public PictureService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<Album> AlbumByIdAsync(int albumId)
        {
            return await _db.Albums
                .Include(a => a.Pictures)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == albumId);
        }

        public async Task<string> AlbumOwnerId(int albumId)
        {
            var album = await _db.Albums
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
            {
                return null;
            }

            return album.User.Id;
        }

        public async Task<Picture> ByIdAsync(int id)
        {
            return await _db.Pictures.FindAsync(id);
        }

        public async Task<bool> CreateAlbumAsync(string albumName, string description, string userId)
        {
            var user = await _db.Users
                .Include(u => u.Albums)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var album = new Album
            {
                Name = albumName,
                Description = description,
                User = user
            };

            await _db.Albums.AddAsync(album);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAlbumByIdAsync(int albumId)
        {
            var album = await _db.Albums
                .Include(a => a.Pictures)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if(album == null)
            {
                return false;
            }

            foreach (var pic in album.Pictures)
            {
                var picture = await _db.Pictures.FindAsync(pic.Id);

                _db.Remove(picture);
            }

            _db.Albums.Remove(album);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAlbumPictureAsync(int pictureId)
        {
            var picture = await _db.Pictures.FindAsync(pictureId);

            if(picture == null)
            {
                return false;
            }

            _db.Pictures.Remove(picture);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UploadPictureToAlbumAsync(int albumId, string uploaderId, IFormFile picture)
        {
            var album = await _db.Albums
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
            {
                return false;
            }
            if (album.UserId != uploaderId)
            {
                return false;
            }

            using (var stream = new MemoryStream())
            {
                await picture.CopyToAsync(stream);
                var imageData = stream.ToArray();

                var pic = new Picture
                {
                    ImageData = imageData
                };

                 album.Pictures.Add(pic);

                await _db.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> UploadProfilePictureAsync(string username, IFormFile picture)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return false;
            }

            using (var stream = new MemoryStream())
            {
                await picture.CopyToAsync(stream);
                var imageData = stream.ToArray();

                Picture pic = null;

                if (user.ProfilePictureId != null)
                {
                    pic = _db.Pictures.Find(user.ProfilePictureId);
                    pic.ImageData = imageData;
                }
                else
                {
                    pic = new Picture { ImageData = imageData };
                }

                user.ProfilePicture = pic;

                await _db.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<Album>> UserAlbumsAsync(string userId)
        {
            var user = await _db.Users
                .Include(u => u.Albums)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            return user.Albums.ToList();
        }
    }
}
