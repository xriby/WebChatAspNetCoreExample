using Chat.Data.Common;
using Chat.Data.ModelsDto;
using System;
using System.Threading.Tasks;

namespace Chat.Services.Interfaces
{
    public interface IMessageService : IDisposable
    {
        Task<MessageDto> SendPublicMessageAsync(MessageDto messageDto);
        Task<MessageDto> SendPrivateMessageAsync(MessageDto messageDto);
        Task<GetMessagesResult> GetPublicMessagesAsync();
    }
}
