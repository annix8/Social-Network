using SocialNetwork.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.DataModel.Models;
using System.Threading.Tasks;
using SocialNetwork.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SocialNetwork.DataModel.Enums;

namespace SocialNetwork.Services
{
    public class UserService : IUserService
    {
        private readonly SocialNetworkDbContext _db;

        public UserService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<User> ByUsernameAsync(string username)
        {
            return await _db.Users
                .Include(u => u.Posts)
                .ThenInclude(p => p.Picture)
                .Include(u => u.ProfilePicture)
                .Include(u => u.FriendRequestsAccepted)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<FriendshipStatus> CheckFriendshipStatus(string firstUserId, string secondUserId)
        {
            var friendship = await _db.Friendships
                .FirstOrDefaultAsync(fr => (fr.UserId == firstUserId && fr.FriendId == secondUserId) ||
                (fr.FriendId == firstUserId && fr.UserId == secondUserId));

            if(friendship == null)
            {
                return FriendshipStatus.NotFriend;
            }

            return friendship.FriendshipStatus;
        }

        public async Task<int> CountAsync()
        {
            return await _db.Users.CountAsync();
        }
    }
}
