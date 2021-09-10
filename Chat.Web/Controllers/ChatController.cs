using Chat.Data;
using Chat.Data.Common;
using Chat.Services.Interfaces;
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

        
        public async Task<ActionResult> Index()
        {
            await MessageService.SendPrivateMessageAsync(null);
            GetMessagesResult result = await MessageService.GetPublicMessagesAsync();
            return View("Index", result);
        }

        // GET: ChatController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ChatController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChatController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ChatController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
