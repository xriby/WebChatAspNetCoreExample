﻿using Chat.Application.Identity;
using Chat.Application.Interfaces.Repositories;
using Chat.Infrastructure.Data;

namespace Chat.Infrastructure.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
