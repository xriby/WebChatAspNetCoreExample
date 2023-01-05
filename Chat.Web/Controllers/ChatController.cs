using Chat.Application.Interfaces;
using Chat.Application.ModelsDto;
using Chat.Application.Results;
using Chat.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public ChatController(IMessageService messageService,
            IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        /// <summary>
        /// Имя пользователя текущего HTTP запроса
        /// </summary>
        public string UserName => User?.Identity?.Name;

        /// <summary>
        /// Общий чат.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Index()
        {
            MessageInfoResult result = await _messageService.GetMessageInfoAsync(UserName);

            return View("Index", (MessageInfoVm)result);
        }

        /// <summary>
        /// Добавить сообщение.
        /// </summary>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MessageVm message)
        {
            AddMessageResult result = await _messageService.AddMessageAsync((MessageDto)message, UserName);

            return Ok(result);
        }

        /// <summary>
        /// Приватный чат.
        /// </summary>
        /// <param name="with">Имя пользователя</param>
        [Authorize]
        public async Task<IActionResult> Private(string with)
        {
            PrivateMessageInfoResult result = await _messageService.GetPrivateMessageInfoAsync(UserName, with);
            if (result.Status == EDbQueryStatus.Failure)
            {
                ErrorViewModel error = new() { ErrorMessage = result.ErrorMessage };

                return View("Error", error);
            }

            return View("Private", (PrivateMessageInfoVm)result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _messageService.Dispose();
                _userService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
