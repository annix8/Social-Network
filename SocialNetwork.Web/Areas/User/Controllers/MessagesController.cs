namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Models.Messages;
    using SocialNetwork.Web.Infrastructure;
    using System;
    using System.Threading.Tasks;

    public class MessagesController : UserAreaController
    {
        private readonly IMessageService _messageService;
        private const int MessagePageSize = 10;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IActionResult Send(string receiverUsername)
        {
            var viewModel = new MessageModel
            {
                ReceiverUsername = receiverUsername
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Send(MessageModel messageModel)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var result = await _messageService.CreateMessageAsync(User.Identity.Name, messageModel.ReceiverUsername, messageModel.Content);

            if (!result)
            {
                return this.BadRequest();
            }

            TempData.Add(GlobalConstants.SuccessMessageKey, "Message sent!");
            return View(messageModel);
        }

        public async Task<IActionResult> MyReceived(int page = 1)
        {
            var messages = await _messageService.ByReceiverUsernameAsync(User.Identity.Name, page, MessagePageSize);

            if (messages == null)
            {
                return View(GlobalConstants.NotFoundView);
            }

            var viewModel = new MessagesPaginationModel
            {
                Messages = messages,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await _messageService.ByReceiverUsernameTotalAsync(User.Identity.Name) / (double)MessagePageSize),
                Sent = false
            };

            return View(viewModel);
        }

        public async Task<IActionResult> MySent(int page = 1)
        {
            var messages = await _messageService.BySenderUsernameAsync(User.Identity.Name, page, MessagePageSize);

            if (messages == null)
            {
                return View(GlobalConstants.NotFoundView);
            }

            var viewModel = new MessagesPaginationModel
            {
                Messages = messages,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await _messageService.BySenderUsernameTotalAsync(User.Identity.Name) / (double)MessagePageSize),
                Sent = true
            };

            return View(viewModel);
        }
    }
}