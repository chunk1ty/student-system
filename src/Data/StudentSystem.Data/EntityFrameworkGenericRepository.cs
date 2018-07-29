using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;
using StudentSystem.Data.Extensions;

namespace StudentSystem.Data
{
    public class EntityFrameworkGenericRepository<TEntity> : IEntityFrameworkGenericRepository<TEntity> where TEntity : class
    {
        private readonly IStudentSystemDbContext _studentSystemDbContext;

        public EntityFrameworkGenericRepository(IStudentSystemDbContext studentSystemDbContext)
        {
            _studentSystemDbContext = studentSystemDbContext;
            DbSet = _studentSystemDbContext.Set<TEntity>();
        }

        protected IDbSet<TEntity> DbSet { get; set; }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _studentSystemDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _studentSystemDbContext.Set<TEntity>().FindAsync(id);
        }

        public void Add(TEntity entity)
        {
            var entry = _studentSystemDbContext.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public void Update(TEntity entity)
        {
            var entry = _studentSystemDbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            var entry = _studentSystemDbContext.Entry(entity);

            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            return await _studentSystemDbContext.Set<Student>().Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<Course>> GetAllByStudentIdAsync(string studentId)
        {
            return await _studentSystemDbContext.Set<Course>()
                .Where(x => x.Students.Any(y => y.Id == studentId))
                .ToListAsync();
        }
    }
}
