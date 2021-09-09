using Chat.Data;
using Chat.Data.Models;
using Chat.Data.ModelsDto;
using Chat.Services;
using Chat.Services.Interfaces;
using Chat.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> Logger;
        private readonly IDateTimeService DateTimeService;
        private readonly IMessageService MessageService;
        private readonly ChatDbContext Db;

        public HomeController(ILogger<HomeController> logger,
            IDateTimeService dateTimeService,
            IMessageService messageService,
            ChatDbContext db)
        {
            Logger = logger;
            DateTimeService = dateTimeService;
            MessageService = messageService;
            Db = db;
        }

        public async Task<IActionResult> IndexAsync()
        {
            Logger.LogInformation("Hello, this is the index!");
            Logger.LogInformation($"Now: {DateTimeService.UtcNow:dd.MM.yyyy HH:mm}");
            Logger.LogInformation($"Firsrt user: {Db.Users.FirstOrDefault()?.UserName}");

            await MessageService.SendPrivateMessage(null);


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
