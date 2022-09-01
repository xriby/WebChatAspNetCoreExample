using System;
using System.Threading;
using System.Threading.Tasks;
using Chat.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Hosted
{
    public class MigrateService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<MigrateService> _logger;

        public MigrateService(ILogger<MigrateService> logger,
            IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = _services.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;
            try
            {
                _logger.LogInformation("Поиск, выполнение миграций.");

                ChatDbContext dbContext = serviceProvider.GetRequiredService<ChatDbContext>();
                await dbContext.Database.MigrateAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Произошла ошибка при выполнении миграций. {exception.Message}");
            }
        }
    }
}
