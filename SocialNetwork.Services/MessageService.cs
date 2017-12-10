namespace SocialNetwork.Services
{
    using System.Threading.Tasks;
    using Services.Contracts;
    using SocialNetwork.DataModel.Models;
    using SocialNetwork.DataModel;
    using Microsoft.EntityFrameworkCore;

    public class MessageService : IMessageService
    {
        private readonly SocialNetworkDbContext _db;

        public MessageService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateMessageAsync(string senderUsername, string receiverUsername, string content)
        {
            var sender = await _db.Users.FirstOrDefaultAsync(u => u.UserName == senderUsername);
            var receiver = await _db.Users.FirstOrDefaultAsync(u => u.UserName == receiverUsername);

            if (sender == null || receiver == null)
            {
                return false;
            }

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = content
            };

            await _db.Messages.AddAsync(message);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            var message = await _db.Messages.FindAsync(messageId);

            if (message == null)
            {
                return false;
            }

            _db.Messages.Remove(message);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
