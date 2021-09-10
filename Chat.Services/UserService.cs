using Chat.Common;
using Chat.Data;
using Chat.Data.Common;
using Chat.Data.Identity;
using Chat.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Services
{
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

        public async Task<GetUsersResult> GetUsersAsync()
        {
            var result = new GetUsersResult { Status = EDbQueryStatus.Success };
            List<ApplicationUser> users = await Db.Users.OrderBy(x => x.UserName).ToListAsync();

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
