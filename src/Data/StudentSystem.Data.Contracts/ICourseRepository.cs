using System.Collections.Generic;
using System.Threading.Tasks;

using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Contracts
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();

        Task<IEnumerable<Course>> GetAllByStudentEmailAsync(string studentId);

        Task<Course> GetByIdAsync(int id);

        void Add(Course entity);

        void Update(Course entity);

        void Delete(Course entity);
    }
}