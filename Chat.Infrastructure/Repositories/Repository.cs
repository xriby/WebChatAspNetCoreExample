using Chat.Application.Interfaces.Repositories;
using Chat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ChatDbContext DbContext;

        public Repository(ChatDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async ValueTask<TEntity> GetByIdAsync(int id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAll(CancellationToken cancellationToken)
        {
            return await DbContext.Set<TEntity>().ToListAsync(cancellationToken);
        }
        
        public IQueryable<TEntity> GetAllQueryable(CancellationToken cancellationToken)
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var entityEntry = await DbContext.AddAsync(entity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return entityEntry.Entity;
        }

        public async Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            DbContext.Set<TEntity>().Update(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(TEntity entity, CancellationToken cancellationToken)
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}