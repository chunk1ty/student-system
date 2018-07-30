using System.Threading.Tasks;

using StudentSystem.Common;

namespace StudentSystem.Data.Services.Contracts
{
    public interface IStudentService
    {
        Task<OperationStatus<string>> EnrollStudentInCourseAsync(string studentId, int courseId);
    }
}
