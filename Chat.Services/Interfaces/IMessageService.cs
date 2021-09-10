using Chat.Data.Common;
using Chat.Data.ModelsDto;
using System;
using System.Threading.Tasks;

namespace Chat.Services.Interfaces
{
    public interface IMessageService : IDisposable
    {
        Task<AddMessageResult> AddMessageAsync(MessageDto messageDto, string fromName);

        Task<AddMessageResult> AddPrivateMessageAsync(MessageDto messageDto, string fromUser, string toUser);
        
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
