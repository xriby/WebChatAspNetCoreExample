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
        private readonly ChatDbContext Db;

        public ChatController(ILogger<ChatController> logger,
            IDateTimeService dateTimeService,
            IMessageService messageService,
            ChatDbContext db)
        {
            Logger = logger;
            DateTimeService = dateTimeService;
            MessageService = messageService;
            Db = db;
        }

        /// <summary>
        /// Имя пользователя текущего HTTP запроса
        /// </summary>
        public string UserName => User.Identity.Name;

        public async Task<ActionResult> Index()
        {
            GetMessageResult result = await MessageService.GetPublicMessagesAsync();
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


        // POST: ChatController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChatController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChatController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
                MessageService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
