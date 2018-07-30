using System.Collections.Generic;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Services.Contracts
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllAsync();

        Task<IEnumerable<Course>> GetAllByStudentEmailAsync(string studentid);

        Task<Course> GetByIdAsync(int id);

        OperationStatus<Course> Add(Course course);

        Task<OperationStatus<int>> DeleteAsync(int id);

        OperationStatus<Course> Update(Course course);
    }
}
