using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ValueTask<TEntity> GetByIdAsync(int id);
        Task<IReadOnlyCollection<TEntity>> GetAll(CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetAllQueryable(CancellationToken cancellationToken = default);
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task Update(TEntity entity, CancellationToken cancellationToken = default);
        Task Delete(TEntity entity, CancellationToken cancellationToken = default);
    }
}