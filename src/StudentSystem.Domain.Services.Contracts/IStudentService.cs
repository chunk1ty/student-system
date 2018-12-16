using System.Threading.Tasks;

using StudentSystem.Common;
using StudentSystem.Domain.Services.Contracts.Models;

namespace StudentSystem.Domain.Services.Contracts
{
    public interface IStudentService
    {
        Task<OperationStatus<string>> EnrollStudentInCourseAsync(string studentId, int courseId);

        Task<OperationStatus<StudentCourses>> GetStudentCourses(string email);
    }
}
