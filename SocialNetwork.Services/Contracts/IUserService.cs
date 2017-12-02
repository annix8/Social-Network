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

        Task<FriendshipStatus> CheckFriendshipStatus(string firstUserId, string secondUserId);
    }
}
