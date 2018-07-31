using System.Threading.Tasks;

using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Contracts
{
    public interface IStudentRepository
    {
        Task<Student> GetStudentWithCoursesByEmailAsync(string email);

        void Add(Student entity);
    }
}