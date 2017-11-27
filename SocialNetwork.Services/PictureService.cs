using SocialNetwork.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.DataModel.Models;
using System.Threading.Tasks;
using SocialNetwork.DataModel;

namespace SocialNetwork.Services
{
    public class PictureService : IPictureService
    {
        private readonly SocialNetworkDbContext _db;

        public PictureService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<Picture> ById(int id)
        {
            return await _db.Pictures.FindAsync(id);
        }
    }
}
