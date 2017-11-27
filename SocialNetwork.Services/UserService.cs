using SocialNetwork.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.DataModel.Models;
using System.Threading.Tasks;
using SocialNetwork.DataModel;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Services
{
    public class UserService : IUserService
    {
        private readonly SocialNetworkDbContext _db;

        public UserService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<User> ByUsername(string username)
        {
            return await _db.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
