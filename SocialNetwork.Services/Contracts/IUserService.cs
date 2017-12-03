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
    }
}
