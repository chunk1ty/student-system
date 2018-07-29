using System.Data.Entity;
using System.Threading.Tasks;

using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Repositories
{
    public class EfStudentRepository : IStudentRepository
    {
        private readonly StudentSystemDbContext _studentSystemDbContext;

        public EfStudentRepository(StudentSystemDbContext studentSystemDbContext)
        {
            _studentSystemDbContext = studentSystemDbContext;
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            return await _studentSystemDbContext.Set<Student>().Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
