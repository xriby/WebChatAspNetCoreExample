using Chat.Data.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Services.Interfaces
{
    public interface IMessageService : IDisposable
    {
        Task<MessageDto> SendPublicMessage(MessageDto messageDto);
        Task<MessageDto> SendPrivateMessage(MessageDto messageDto);
    }
}
