using SocialNetwork.DataModel.Enums;
using SocialNetwork.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Contracts
{
    public interface IUserService
    {
        Task<User> ByUsernameAsync(string username);

        Task<int> CountAsync();

        Task<(FriendshipStatus, string)> CheckFriendshipStatusAsync(string firstUserId, string secondUserId);

        Task<bool> MakeFriendRequestAsync(string issuerUsername, string userToBefriend);

        Task<bool> DeleteFriendshipAsync(string issuerUsername, string userToCancel);

        Task<bool> AcceptFriendshipAsync(string firstUsername, string secondUsername);

        Task<IEnumerable<User>> PendingFriendsAsync(string userId);

        Task<int> ByContainingUsernameCountAsync(string username);

        Task<IEnumerable<User>> ByContainingUsernamePaginationAsync(string username, int page = 1, int pageSize = 10);
    }
}
