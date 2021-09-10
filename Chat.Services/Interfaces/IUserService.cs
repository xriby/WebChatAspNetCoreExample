using Chat.Data.Common;
using System;
using System.Threading.Tasks;

namespace Chat.Services.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<GetUsersResult> GetUsersAsync();
    }
}
