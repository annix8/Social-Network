using SocialNetwork.DataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Contracts
{
    public interface IMessageService
    {
        Task<bool> CreateMessageAsync(string senderUsername, string receiverUsername, string content);

        Task<bool> DeleteMessageAsync(int messageId);

        Task<IEnumerable<Message>> ByReceiverUsernameAsync(string receiverUsername, int page = 1, int pageSize = 10);

        Task<int> ByReceiverUsernameTotalAsync(string receiverUsername);

        Task<IEnumerable<Message>> BySenderUsernameAsync(string senderUsername, int page = 1, int pageSize = 10);

        Task<int> BySenderUsernameTotalAsync(string senderUsername);
    }
}
