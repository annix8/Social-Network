namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Models.Messages;
    using System.Threading.Tasks;

    public class MessagesController : UserAreaController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public IActionResult Send(string receiverUsername)
        {
            var viewModel = new MessageModel
            {
                ReceiverUsername = receiverUsername,
                SenderUsername = User.Identity.Name
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

            var result = await _messageService.CreateMessageAsync(messageModel.SenderUsername, messageModel.ReceiverUsername, messageModel.Content);

            if (!result)
            {
                return this.BadRequest();
            }

            return Ok();
        }
    }
}