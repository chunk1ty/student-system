using System.Threading.Tasks;

using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Contracts
{
    public interface IStudentRepository
    {
        Task<Student> GetStudentByEmailAsync(string email);

        void Add(Student entity);
    }
}