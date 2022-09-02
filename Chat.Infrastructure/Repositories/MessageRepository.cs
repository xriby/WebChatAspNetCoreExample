using Chat.Application.Interfaces.Repositories;
using Chat.Application.Models;
using Chat.Infrastructure.Data;

namespace Chat.Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
