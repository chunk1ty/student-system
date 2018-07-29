using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Contracts
{
    public interface IStudentSystemDbContext
    {
        IDbSet<Course> Courses { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}