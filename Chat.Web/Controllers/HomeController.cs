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

        public HomeController(ILogger<HomeController> logger,
            IDateTimeService dateTimeService)
        {
            Logger = logger;
            DateTimeService = dateTimeService;

        }

        public IActionResult Index()
        {
            Logger.LogInformation("Hello, this is the index!");
            Logger.LogInformation($"Now: {DateTimeService.UtcNow:dd.MM.yyyy HH:mm}");

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


    }
}
