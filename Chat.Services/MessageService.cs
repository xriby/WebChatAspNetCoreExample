using Chat.Common;
using Chat.Data;
using Chat.Data.Common;
using Chat.Data.Models;
using Chat.Data.ModelsDto;
using Chat.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task<MessageDto> SendPublicMessageAsync(MessageDto messageDto)
        {
            throw new NotImplementedException();
        }

        public async Task<GetMessagesResult> GetPublicMessagesAsync()
        {
            var result = new GetMessagesResult { Status = EDbQueryStatus.Success };
            try
            {
                // Возьмем последние 100 сообщений, поскольку в примере не реализован постраничный вывод.
                List<Message> messages = await Db.Messages
                    .Where(x => x.MessageType == EMessageType.Public)
                    .OrderByDescending(x => x.CreateDate)
                    .Take(100) 
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

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        /// <inheritdoc />
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
