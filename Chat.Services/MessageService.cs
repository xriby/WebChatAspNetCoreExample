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
    /// <summary>
    /// Сервис сообщений.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> Logger;
        private readonly IUserService UserService;
        private readonly ChatDbContext Db;
        private bool disposed = false;

        public MessageService(ILogger<MessageService> logger,
            IUserService userService,
            ChatDbContext db)
        {
            Logger = logger;
            UserService = userService;
            Db = db;
        }

        /// <inheritdoc />
        public async Task<AddMessageResult> AddMessageAsync(MessageDto messageDto, string fromUser)
        {
            int maxTextLength = ChatConfiguration.MaxTextLength;
            var result = new AddMessageResult { Status = EDbQueryStatus.Success };
            if (string.IsNullOrEmpty(messageDto.Text))
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = "Ошибка. Введите сообщение.";
                return result;
            }
            if (messageDto.Text.Length > maxTextLength)
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = $"Ошибка. Максимальная длина сообщения: {maxTextLength} символов.";
                return result;
            }
            if (string.IsNullOrEmpty(fromUser))
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = "Ошибка. Не задан пользователь.";
                return result;
            }
            ApplicationUser user = await Db.Users.FirstOrDefaultAsync(x => x.UserName == fromUser);
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
        public async Task<MessageInfoResult> GetMessageInfoAsync(string userName)
        {
            var result = new MessageInfoResult { Status = EDbQueryStatus.Success };
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

                // Получим всех пользователей, кроме инициатора.
                GetUsersResult userResult = await UserService.GetUsersAsync(userName);
                if (userResult.Status == EDbQueryStatus.Failure)
                {
                    result.Status = EDbQueryStatus.Failure;
                    result.ErrorMessage = userResult.ErrorMessage;
                    return result;
                }
                result.Users = userResult.Data;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Произошла ошибка при получении информации о сообщениях.";
                result.ErrorMessage = errorMessage;
                result.Status = EDbQueryStatus.Failure;
                Logger.LogError(ex, $"{errorMessage} {ex.Message}");
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<PrivateMessageInfoResult> GetPrivateMessageInfoAsync(string fromUser, string toUser)
        {
            var result = new PrivateMessageInfoResult { Status = EDbQueryStatus.Success };
            ApplicationUser userSender = await Db.Users.FirstOrDefaultAsync(x => x.UserName == fromUser);
            if (userSender == null)
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = $"Ошибка. Пользователь {fromUser} не найден.";
                return result;
            }
            result.FromUser = userSender;
            ApplicationUser userRecipient = await Db.Users.FirstOrDefaultAsync(x => x.UserName == toUser);
            if (userRecipient == null)
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = $"Ошибка. Пользователь {toUser} не найден.";
                return result;
            }
            var privateUserIds = new string[] { userSender.Id, userRecipient.Id };
            result.ToUser = userRecipient;
            try
            {
                // Возьмем последние 1000 приватных сообщений м/у пользователями.
                List<MessageDto> messages = await Db.Messages
                    .Include(x => x.User)
                    .Where(x => x.MessageType == EMessageType.Private)
                    .Where(x => privateUserIds.Contains(x.User.Id))
                    .Where(x => privateUserIds.Contains(x.RecipientId))
                    .OrderByDescending(x => x.CreateDate)
                    .Take(1000)
                    .Select(x => (MessageDto)x)
                    .ToListAsync();
                result.Messages = messages;

                // Получим всех пользователей, кроме инициатора.
                GetUsersResult userResult = await UserService.GetUsersAsync(fromUser);
                if (userResult.Status == EDbQueryStatus.Failure)
                {
                    result.Status = EDbQueryStatus.Failure;
                    result.ErrorMessage = userResult.ErrorMessage;
                    return result;
                }
                result.Users = userResult.Data;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Произошла ошибка при получении информации о приватных сообщениях.";
                result.ErrorMessage = errorMessage;
                result.Status = EDbQueryStatus.Failure;
                Logger.LogError(ex, $"{errorMessage} {ex.Message}");
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
                    UserService.Dispose();
                }
                disposed = true;
            }
        }

        
    }
}
