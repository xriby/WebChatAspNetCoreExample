using Chat.Data.Common;
using Chat.Data.ModelsDto;
using System;
using System.Threading.Tasks;

namespace Chat.Services.Interfaces
{
    public interface IMessageService : IDisposable
    {
        Task<AddMessageResult> AddMessageAsync(MessageDto messageDto, string userName);
        Task<MessageDto> SendPrivateMessageAsync(MessageDto messageDto);
        Task<GetMessageResult> GetPublicMessagesAsync();
    }
}
