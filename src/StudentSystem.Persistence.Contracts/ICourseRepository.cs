using System.Collections.Generic;
using System.Threading.Tasks;

using StudentSystem.Domain;

namespace StudentSystem.Persistence.Contracts
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();

        Task<Course> GetByIdAsync(int id);

        void Add(Course entity);

        void Update(Course entity);

        void Delete(Course entity);
    }
}