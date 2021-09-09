using Chat.Data;
using Chat.Data.ModelsDto;
using Chat.Services.Interfaces;
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

        public async Task<MessageDto> SendPrivateMessage(MessageDto messageDto)
        {
            Logger.LogInformation("Start SendPrivateMessage.");
            return null;
        }

        public Task<MessageDto> SendPublicMessage(MessageDto messageDto)
        {
            throw new NotImplementedException();
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
