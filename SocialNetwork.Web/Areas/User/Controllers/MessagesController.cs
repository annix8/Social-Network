namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Services.Contracts;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Areas.User.Models.Messages;
    using Web.Infrastructure;
    using Web.Infrastructure.Extensions;

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
                var errors = ModelState.Values
                    .Where(v => v.ValidationState == ModelValidationState.Invalid)
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                TempData.AddErrorMessage(string.Join(", ", errors));
                return View(messageModel.ReceiverUsername);
            }

            var result = await _messageService.CreateMessageAsync(User.Identity.Name, messageModel.ReceiverUsername, messageModel.Content);

            if (!result)
            {
                TempData.AddErrorMessage("Something went wrong.");
                return View(messageModel.ReceiverUsername);
            }

            TempData.AddSuccessMessage("Message sent.");
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