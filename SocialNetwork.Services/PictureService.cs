using SocialNetwork.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.DataModel.Models;
using System.Threading.Tasks;
using SocialNetwork.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace SocialNetwork.Services
{
    public class PictureService : IPictureService
    {
        private readonly SocialNetworkDbContext _db;

        public PictureService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<Picture> ByIdAsync(int id)
        {
            return await _db.Pictures.FindAsync(id);
        }

        public async Task<bool> UploadProfilePictureAsync(string username, IFormFile picture)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if(user == null)
            {
                return false;
            }

            using (var stream = new MemoryStream())
            {
                await picture.CopyToAsync(stream);
                var imageData = stream.ToArray();

                var pic = new Picture { ImageData = imageData };

                user.ProfilePicture = pic;

                await _db.SaveChangesAsync();
            }

            return true;
        }
    }
}
