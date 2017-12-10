using System.Threading.Tasks;

namespace SocialNetwork.Services.Contracts
{
    public interface IMessageService
    {
        Task<bool> CreateMessageAsync(string senderUsername, string receiverUsername, string content);

        Task<bool> DeleteMessageAsync(int messageId);
    }
}
