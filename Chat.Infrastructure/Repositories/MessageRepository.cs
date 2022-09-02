using Chat.Application.Interfaces.Repositories;
using Chat.Application.Models;

namespace Chat.Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
