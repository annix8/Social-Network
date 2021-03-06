﻿namespace SocialNetwork.Services
{
    using DataModel;
    using DataModel.Enums;
    using DataModel.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
                .Include(u => u.ProfilePicture)
                .Include(u => u.FriendRequestsAccepted)
                .Include(u => u.FriendRequestsMade)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> DeleteFriendshipAsync(string issuerUsername, string userToCancel)
        {
            var issuer = await _db.Users.FirstOrDefaultAsync(u => u.UserName == issuerUsername);
            var friend = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userToCancel);

            if (friend == null || issuer == null)
            {
                return false;
            }

            var friendship = await _db.Friendships
                .FirstOrDefaultAsync(f => (f.User == issuer && f.Friend == friend) ||
                (f.Friend == issuer && f.User == friend));

            if (friendship == null)
            {
                return false;
            }

            _db.Friendships.Remove(friendship);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<(FriendshipStatus, string)> CheckFriendshipStatusAsync(string firstUserId, string secondUserId)
        {
            var friendship = await _db.Friendships
                .FirstOrDefaultAsync(fr => (fr.UserId == firstUserId && fr.FriendId == secondUserId) ||
                (fr.FriendId == firstUserId && fr.UserId == secondUserId));

            if (friendship == null)
            {
                return (FriendshipStatus.NotFriend, "");
            }

            var issuer = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == friendship.FriendshipIssuerId);

            return (friendship.FriendshipStatus, issuer.UserName);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Users.CountAsync();
        }

        public async Task<bool> MakeFriendRequestAsync(string issuerUsername, string userToBefriend)
        {
            var issuer = await _db.Users.FirstOrDefaultAsync(u => u.UserName == issuerUsername);
            var friend = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userToBefriend);

            if (issuer == null || friend == null)
            {
                return false;
            }

            var friendship = new Friendship
            {
                User = issuer,
                Friend = friend,
                FriendshipIssuerId = issuer.Id,
                FriendshipStatus = FriendshipStatus.Pending
            };

            await _db.Friendships.AddAsync(friendship);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<User>> PendingFriendsAsync(string userId)
        {
            return await _db.Friendships
                 .Include(fr => fr.User)
                 .Where(fr => fr.FriendId == userId && fr.FriendshipStatus == FriendshipStatus.Pending)
                 .Select(fr => fr.User)
                 .ToListAsync();
        }

        public async Task<bool> AcceptFriendshipAsync(string firstUsername, string secondUsername)
        {
            var firstUser = await _db.Users.FirstOrDefaultAsync(u => u.UserName == firstUsername);
            var secondUser = await _db.Users.FirstOrDefaultAsync(u => u.UserName == secondUsername);

            if (firstUser == null || secondUser == null)
            {
                return false;
            }

            var friendship = await _db.Friendships
                .FirstOrDefaultAsync(fr => (fr.User == firstUser && fr.Friend == secondUser) ||
                (fr.Friend == firstUser && fr.User == secondUser));

            if (friendship == null)
            {
                return false;
            }

            friendship.FriendshipStatus = FriendshipStatus.Accepted;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<int> ByContainingUsernameCountAsync(string username)
        {
            return await _db.Users
                 .Where(u => u.UserName.ToLower().Contains(username.ToLower()))
                 .CountAsync();
        }

        public async Task<IEnumerable<User>> ByContainingUsernamePaginationAsync(string username, int page = 1, int pageSize = 10)
        {
            var searchValue = username ?? "";

            return await _db.Users
                .Where(u => u.UserName.ToLower().Contains(searchValue.ToLower()))
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> FriendsPaginationAsync(string userId, int page = 1, int pageSize = 10)
        {
            var friends = await _db.Friendships
                .Include(fr => fr.User)
                .Where(fr => fr.UserId == userId && fr.FriendshipStatus == FriendshipStatus.Accepted)
                .Select(x => x.Friend)
                .ToListAsync();

            var friendsUsers = await _db.Friendships
                .Include(fr => fr.User)
                .Where(fr => fr.FriendId == userId && fr.FriendshipStatus == FriendshipStatus.Accepted)
                .Select(x => x.User)
                .ToListAsync();

            friends.AddRange(friendsUsers);

            return friends
                .OrderBy(fr => fr.FirstName).ThenBy(f => f.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<int> FriendsCountAsync(string userId)
        {
            var friends1 = await _db.Friendships
                .Where(fr => fr.UserId == userId && fr.FriendshipStatus == FriendshipStatus.Accepted)
                .CountAsync();

            var friends2 = await _db.Friendships
                .Where(fr => fr.FriendId == userId && fr.FriendshipStatus == FriendshipStatus.Accepted)
                .CountAsync();
            return friends1 + friends2;
        }

        public async Task<string> NamesByIdAsync(string userId)
        {
            var user = await _db.Users.FindAsync(userId);
            return $"{user.FirstName} {user.LastName}";
        }

        public async Task<bool> ValidateFriendshipAcceptance(string friendshipAccepterId, string userToAcceptId)
        {
            if (string.IsNullOrEmpty(friendshipAccepterId) || string.IsNullOrEmpty(userToAcceptId))
            {
                return false;
            }

            var friendship = await _db.Friendships
                .FirstOrDefaultAsync(fr => (fr.UserId == friendshipAccepterId && fr.FriendId == userToAcceptId) ||
                (fr.FriendId == friendshipAccepterId && fr.UserId == userToAcceptId));


            if (friendship.FriendshipIssuerId == friendshipAccepterId)
            {
                return false;
            }

            return true;
        }

        public async Task<string> IdByUsernameAsync(string username)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            return user != null ? user.Id : null;
        }

        public async Task<bool> DeleteAccountAsync(string usernameToDelete)
        {
            var userToDelete = await _db.Users
                .Include(pp => pp.ProfilePicture)
                .Include(f => f.FriendRequestsAccepted)
                .Include(f => f.FriendRequestsMade)
                .Include(m => m.MessagesReceived)
                .Include(m => m.MessagesSent)
                .Include(p => p.Posts).ThenInclude(c => c.Comments)
                .Include(a => a.Albums).ThenInclude(a => a.Pictures)
                .FirstOrDefaultAsync(u => u.UserName == usernameToDelete);

            var commentsFromUser = _db.Comments.Where(c => c.User == userToDelete);

            if (userToDelete == null)
            {
                return false;
            }

            _db.Friendships.RemoveRange(userToDelete.FriendRequestsAccepted);
            _db.Friendships.RemoveRange(userToDelete.FriendRequestsMade);
            _db.Messages.RemoveRange(userToDelete.MessagesReceived);
            _db.Messages.RemoveRange(userToDelete.MessagesSent);

            userToDelete.Posts
                .ToList()
                .ForEach(p =>
                {
                    _db.Comments.RemoveRange(p.Comments);
                });
            _db.Comments.RemoveRange(commentsFromUser);
            _db.Posts.RemoveRange(userToDelete.Posts);

            userToDelete.Albums
                .ToList()
                .ForEach(a =>
                {
                    _db.Pictures.RemoveRange(a.Pictures);
                });

            if (userToDelete.ProfilePicture != null)
                _db.Pictures.Remove(userToDelete.ProfilePicture);

            _db.Albums.RemoveRange(userToDelete.Albums);

            _db.Users.Remove(userToDelete);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
