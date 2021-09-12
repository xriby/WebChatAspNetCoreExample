using Chat.Common;
using Chat.Data.Common;
using Chat.Data.ModelsDto;
using Chat.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> Logger;
        private readonly IMessageService MessageService;
        private readonly IUserService UserService;

        public ChatController(ILogger<ChatController> logger,
            IMessageService messageService,
            IUserService userService)
        {
            Logger = logger;
            MessageService = messageService;
            UserService = userService;
        }

        /// <summary>
        /// Имя пользователя текущего HTTP запроса
        /// </summary>
        public string UserName => User.Identity.Name;

        /// <summary>
        /// Общий чат.
        /// </summary>
        public async Task<ActionResult> Index()
        {
            MessageInfoResult result = await MessageService.GetMessageInfoAsync(UserName);
            return View("Index", result);
        }

        /// <summary>
        /// Добавить сообщение.
        /// </summary>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(MessageDto message)
        {
            AddMessageResult result = await MessageService.AddMessageAsync(message, UserName);
            return Content(JsonSerializer.Serialize(result), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// Приватный чат.
        /// </summary>
        /// <param name="with">Имя пользователя</param>
        [Authorize]
        public async Task<ActionResult> Private(string with)
        {
            PrivateMessageInfoResult result = await MessageService.GetPrivateMessageInfoAsync(UserName, with);
            if (result.Status == EDbQueryStatus.Failure)
            {
                var error = new ErrorViewModel { ErrorMessage = result.ErrorMessage };
                return View("Error", error);
            }
            return View("Private", result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MessageService.Dispose();
                UserService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
