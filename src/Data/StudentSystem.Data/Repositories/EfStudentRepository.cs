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

        public async Task<Student> GetStudentWithCoursesByEmailAsync(string email)
        {
            return await _studentSystemDbContext.Set<Student>()
                                                .Include(x => x.Courses)
                                                .SingleOrDefaultAsync(x => x.Email.Equals(email));
        }

        public void Add(Student entity)
        {
            var entry = _studentSystemDbContext.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                _studentSystemDbContext.Students.Add(entity);
            }
        }
    }
}
