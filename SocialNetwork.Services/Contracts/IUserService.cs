namespace SocialNetwork.Services.Contracts
{
    using DataModel.Enums;
    using DataModel.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<User> ByUsernameAsync(string username);

        Task<string> IdByUsernameAsync(string username);

        Task<string> NamesByIdAsync(string userId);

        Task<int> CountAsync();

        Task<(FriendshipStatus, string)> CheckFriendshipStatusAsync(string firstUserId, string secondUserId);

        Task<bool> MakeFriendRequestAsync(string issuerUsername, string userToBefriend);

        Task<bool> DeleteFriendshipAsync(string issuerUsername, string userToCancel);

        Task<bool> AcceptFriendshipAsync(string firstUsername, string secondUsername);

        Task<IEnumerable<User>> PendingFriendsAsync(string userId);

        Task<int> ByContainingUsernameCountAsync(string username);

        Task<IEnumerable<User>> ByContainingUsernamePaginationAsync(string username, int page = 1, int pageSize = 10);

        Task<IEnumerable<User>> FriendsPaginationAsync(string userId, int page = 1, int pageSize = 10);

        Task<int> FriendsCountAsync(string userId);

        Task<bool> ValidateFriendshipAcceptance(string friendshipAccepterId, string userToAcceptId);
    }
}
