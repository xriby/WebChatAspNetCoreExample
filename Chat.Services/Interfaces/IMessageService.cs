using System;
using System.Threading.Tasks;
using Chat.Infrastructure.Common;
using Chat.Infrastructure.ModelsDto;

namespace Chat.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса сообщений.
    /// </summary>
    public interface IMessageService : IDisposable
    {
        /// <summary>
        /// Добавить сообщение.
        /// </summary>
        Task<AddMessageResult> AddMessageAsync(MessageDto messageDto, string fromName);

        /// <summary>
        /// Получить информацию о сообщениях.
        /// </summary>
        Task<MessageInfoResult> GetMessageInfoAsync(string userName);
        
        /// <summary>
        /// Получить информацию о приватных сообщениях.
        /// </summary>
        Task<PrivateMessageInfoResult> GetPrivateMessageInfoAsync(string fromUser, string toUser);
    }
}
