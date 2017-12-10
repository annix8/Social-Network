namespace SocialNetwork.Services
{
    using System.Threading.Tasks;
    using Services.Contracts;
    using SocialNetwork.DataModel.Models;
    using SocialNetwork.DataModel;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MessageService : IMessageService
    {
        private readonly SocialNetworkDbContext _db;

        public MessageService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Message>> ByReceiverUsernameAsync(string receiverUsername, int page = 1, int pageSize = 10)
        {
            var receiver = await _db.Users
                .Include(u => u.MessagesReceived)
                .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(u => u.UserName == receiverUsername);

            if (receiver == null)
            {
                return null;
            }

            return receiver.MessagesReceived
                .OrderByDescending(m => m.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<int> ByReceiverUsernameTotalAsync(string receiverUsername)
        {
            var receiver = await _db.Users
                .Include(u => u.MessagesReceived)
                .FirstOrDefaultAsync(u => u.UserName == receiverUsername);

            return receiver.MessagesReceived.Count();
        }

        public async Task<IEnumerable<Message>> BySenderUsernameAsync(string senderUsername, int page = 1, int pageSize = 10)
        {
            var receiver = await _db.Users
                .Include(u => u.MessagesSent)
                .ThenInclude(m => m.Receiver)
                .FirstOrDefaultAsync(u => u.UserName == senderUsername);

            if (receiver == null)
            {
                return null;
            }

            return receiver.MessagesSent
                .OrderByDescending(m => m.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public async Task<int> BySenderUsernameTotalAsync(string senderUsername)
        {
            var receiver = await _db.Users
                .Include(u => u.MessagesSent)
                .FirstOrDefaultAsync(u => u.UserName == senderUsername);

            return receiver.MessagesSent.Count();
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
                Content = content,
                Date = DateTime.UtcNow
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
