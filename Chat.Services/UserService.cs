using Chat.Common;
using Chat.Data;
using Chat.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Infrastructure;
using Chat.Infrastructure.Common;
using Chat.Infrastructure.Identity;

namespace Chat.Services
{
    /// <summary>
    /// Сервис работы с пользователями.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> Logger;
        private readonly ChatDbContext Db;
        private bool disposed = false;

        public UserService(ILogger<UserService> logger,
            ChatDbContext db)
        {
            Logger = logger;
            Db = db;
        }

        /// <inheritdoc />
        public async Task<GetUsersResult> GetUsersAsync(string excludeUserName = "")
        {
            var result = new GetUsersResult { Status = EDbQueryStatus.Success };
            try
            {
                List<ApplicationUser> users = await Db.Users
                    .OrderBy(x => x.UserName)
                    .AsNoTracking()
                    .ToListAsync();
                if (users?.Count > 0 && !string.IsNullOrEmpty(excludeUserName))
                {
                    ApplicationUser excludeUser = users.FirstOrDefault(x => x.UserName == excludeUserName);
                    if (excludeUser != null)
                    {
                        users.Remove(excludeUser);
                    }
                }
                result.Data = users;
            }
            catch (Exception ex)
            {
                string errorMessage = "Произошла ошибка при получении пользователей.";
                Logger.LogError(ex, $"{errorMessage} {ex.Message}");
                result.Status = EDbQueryStatus.Failure;
                result.ErrorMessage = errorMessage;
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
                }
                disposed = true;
            }
        }
    }
}
