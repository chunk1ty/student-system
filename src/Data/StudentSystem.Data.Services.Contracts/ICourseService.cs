using System.Collections.Generic;
using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Services.Contracts
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllAsync();

        Task<Course> GetByIdAsync(int id);

        OperationStatus Add(Course course);

        Task<OperationStatus> DeleteAsync(int id);

        OperationStatus Update(Course course);
    }
}
