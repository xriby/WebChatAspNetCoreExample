using Chat.Common;
using Chat.Data;
using Chat.Data.Common;
using Chat.Data.ModelsDto;
using Chat.Services.Interfaces;
using Chat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> Logger;
        private readonly IDateTimeService DateTimeService;
        private readonly IMessageService MessageService;
        private readonly IUserService UserService;
        
        public ChatController(ILogger<ChatController> logger,
            IDateTimeService dateTimeService,
            IMessageService messageService,
            IUserService userService)
        {
            Logger = logger;
            DateTimeService = dateTimeService;
            MessageService = messageService;
            UserService = userService;
        }

        /// <summary>
        /// Имя пользователя текущего HTTP запроса
        /// </summary>
        public string UserName => User.Identity.Name;

        public async Task<ActionResult> Index()
        {
            MessageInfoResult result = await MessageService.GetMessageInfoAsync(UserName);
            return View("Index", result);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(MessageDto message)
        {
            AddMessageResult result = await MessageService.AddMessageAsync(message, UserName);
            if (result.Status == EDbQueryStatus.Failure)
            {
                var error = new ErrorViewModel { ErrorMessage = result.ErrorMessage };
                return View("Error", error);
            }
            return RedirectToAction(nameof(Index));
        }
        
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
