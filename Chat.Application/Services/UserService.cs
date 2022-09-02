using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Application.Identity;
using Chat.Application.Interfaces;
using Chat.Application.Interfaces.Repositories;
using Chat.Application.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Services
{
    /// <summary>
    /// Сервис работы с пользователями.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private bool disposed = false;

        public UserService(ILogger<UserService> logger,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<GetUsersResult> GetUsersAsync(string excludeUserName = "")
        {
            var result = new GetUsersResult { Status = EDbQueryStatus.Success };
            try
            {
                List<ApplicationUser> users = await _userRepository.GetAllQueryable()
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
                _logger.LogError(ex, $"{errorMessage} {ex.Message}");
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
                    //Db.Dispose();
                }
                disposed = true;
            }
        }
    }
}
