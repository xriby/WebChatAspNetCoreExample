using Chat.Common;
using Chat.Data;
using Chat.Data.Common;
using Chat.Data.Identity;
using Chat.Data.Models;
using Chat.Data.ModelsDto;
using Chat.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Services
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> Logger;
        private readonly ChatDbContext Db;
        private bool disposed = false;

        public MessageService(ILogger<MessageService> logger,
            ChatDbContext db)
        {
            Logger = logger;
            Db = db;
        }

        public async Task<MessageDto> SendPrivateMessageAsync(MessageDto messageDto)
        {
            Logger.LogInformation("Start SendPrivateMessage.");
            return null;
        }

        public async Task<AddMessageResult> AddMessageAsync(MessageDto messageDto, string userName)
        {
            var result = new AddMessageResult { Status = EDbQueryStatus.Success };
            if (string.IsNullOrEmpty(messageDto.Text))
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = "Ошибка. Введите сообщение.";
                return result;
            }
            if (string.IsNullOrEmpty(userName))
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = "Ошибка. Не задан пользователь.";
                return result;
            }
            ApplicationUser user = await Db.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = "Ошибка. Пользователь не найден.";
                return result;
            }

            Message message = (Message)messageDto;
            message.CreateDate = DateTime.UtcNow;
            message.User = user;
            try
            {
                await Db.Messages.AddAsync(message);
                await Db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string errorMessage = "Произошла ошибка при добавлении сообщения.";
                Logger.LogError(ex, $"{errorMessage} {ex.Message}");
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = errorMessage;
                return result;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<GetMessageResult> GetPublicMessagesAsync()
        {
            var result = new GetMessageResult { Status = EDbQueryStatus.Success };
            try
            {
                // Возьмем последние 1000 сообщений, поскольку в примере не реализован постраничный вывод.
                List<MessageDto> messages = await Db.Messages
                    .Include(x => x.User)
                    .Where(x => x.MessageType == EMessageType.Public)
                    .OrderByDescending(x => x.CreateDate)
                    .Take(1000)
                    .Select(x => (MessageDto)x)
                    .ToListAsync();
                result.Messages = messages;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ошибка при получении публичных сообщений: {ex.Message}";
                result.ErrorMessage = errorMessage;
                result.Status = EDbQueryStatus.Failure;
                Logger.LogError(ex, errorMessage);
            }
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Db.Dispose();

                }
                disposed = true;
            }
        }
    }
}
