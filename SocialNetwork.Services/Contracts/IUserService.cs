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

        Task<FriendshipStatus> CheckFriendshipStatusAsync(string firstUserId, string secondUserId);

        Task<bool> MakeFriendRequestAsync(string issuerUsername, string userToBefriend);
    }
}
