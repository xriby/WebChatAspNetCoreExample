using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Application.Common;
using Chat.Application.Identity;
using Chat.Application.Interfaces;
using Chat.Application.Interfaces.Repositories;
using Chat.Application.Models;
using Chat.Application.ModelsDto;
using Chat.Application.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Services
{
    /// <summary>
    /// Сервис сообщений.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IUserService _userService;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private bool disposed = false;

        public MessageService(ILogger<MessageService> logger,
            IUserService userService,
            IMessageRepository messageRepository,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userService = userService;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<AddMessageResult> AddMessageAsync(MessageDto messageDto, string fromUser)
        {
            int maxTextLength = ChatConfiguration.MaxTextLength;
            AddMessageResult result = new() { Status = EDbQueryStatus.Success };
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
            ApplicationUser user = await _userRepository.GetAllQueryable().FirstOrDefaultAsync(x => x.UserName == fromUser);
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
                await _messageRepository.InsertAsync(message);
                result.Data = (MessageDto)message;
            }
            catch (Exception ex)
            {
                string errorMessage = "Произошла ошибка при добавлении сообщения.";
                _logger.LogError(ex, $"{errorMessage} {ex.Message}");
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = errorMessage;
                return result;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<MessageInfoResult> GetMessageInfoAsync(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }
            MessageInfoResult result = new() { Status = EDbQueryStatus.Success };
            try
            {
                // Возьмем последние 1000 сообщений, поскольку в примере не реализован постраничный вывод.
                List<MessageDto> messages = await _messageRepository.GetAllQueryable()
                    .Include(x => x.User)
                    .Where(x => x.MessageType == EMessageType.Public)
                    .OrderByDescending(x => x.CreateDate)
                    .Take(1000)
                    .Select(x => (MessageDto)x)
                    .AsNoTracking()
                    .ToListAsync();
                result.Messages = messages;

                // Получим всех пользователей, кроме инициатора.
                GetUsersResult userResult = await _userService.GetUsersAsync(userName);
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
                _logger.LogError(ex, $"{errorMessage} {ex.Message}");
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<PrivateMessageInfoResult> GetPrivateMessageInfoAsync(string fromUser, string toUser)
        {
            if (fromUser == null)
            {
                throw new ArgumentNullException(nameof(fromUser));
            }
            if (toUser == null)
            {
                throw new ArgumentNullException(nameof(toUser));
            }
            PrivateMessageInfoResult result = new() { Status = EDbQueryStatus.Success };
            ApplicationUser userSender = await _userRepository.GetAllQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == fromUser);
            if (userSender == null)
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = $"Ошибка. Пользователь {fromUser} не найден.";
                return result;
            }
            result.FromUser = userSender;
            ApplicationUser userRecipient = await _userRepository.GetAllQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == toUser);
            if (userRecipient == null)
            {
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = $"Ошибка. Пользователь {toUser} не найден.";
                return result;
            }
            string[] privateUserIds = new string[] { userSender.Id, userRecipient.Id };
            result.ToUser = userRecipient;
            try
            {
                // Возьмем последние 1000 приватных сообщений м/у пользователями.
                List<MessageDto> messages = await _messageRepository.GetAllQueryable()
                    .Include(x => x.User)
                    .Where(x => x.MessageType == EMessageType.Private)
                    .Where(x => privateUserIds.Contains(x.User.Id))
                    .Where(x => privateUserIds.Contains(x.RecipientId))
                    .OrderByDescending(x => x.CreateDate)
                    .Take(1000)
                    .Select(x => (MessageDto)x)
                    .AsNoTracking()
                    .ToListAsync();
                result.Messages = messages;

                // Получим всех пользователей, кроме инициатора.
                GetUsersResult userResult = await _userService.GetUsersAsync(fromUser);
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
                _logger.LogError(ex, $"{errorMessage} {ex.Message}");
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
                    //_messageRepository.Dispose();
                    _userService.Dispose();
                }
                disposed = true;
            }
        }


    }
}
