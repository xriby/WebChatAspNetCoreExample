using System;
using System.Threading.Tasks;
using Chat.Infrastructure.Common;

namespace Chat.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса работы с пользователями.
    /// </summary>
    public interface IUserService : IDisposable
    {
        /// <summary>
        /// Получить всех пользователей.
        /// </summary>
        Task<GetUsersResult> GetUsersAsync(string excludeUserName = "");
    }
}
