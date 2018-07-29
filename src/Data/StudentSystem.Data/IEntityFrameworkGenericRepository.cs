using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSystem.Data
{
    public interface IEntityFrameworkGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(object id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}