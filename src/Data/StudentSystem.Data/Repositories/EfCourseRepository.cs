using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using StudentSystem.Data.Contracts;
using StudentSystem.Data.Entities;

namespace StudentSystem.Data.Repositories
{
    public class EfCourseRepository : ICourseRepository
    {
        private readonly StudentSystemDbContext _studentSystemDbContext;

        public EfCourseRepository(StudentSystemDbContext studentSystemDbContext)
        {
            _studentSystemDbContext = studentSystemDbContext;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _studentSystemDbContext.Set<Course>()
                                                .AsNoTracking()
                                                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetAllByStudentIdAsync(string studentId)
        {
            return await _studentSystemDbContext.Set<Course>()
                                                .AsNoTracking()
                                                .Where(x => x.Students.Any(y => y.Email.Equals(studentId)))
                                                .ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _studentSystemDbContext.Set<Course>()
                                                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Add(Course entity)
        {
            var entry = _studentSystemDbContext.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                _studentSystemDbContext.Courses.Add(entity);
            }
        }

        public void Update(Course entity)
        {
            var entry = _studentSystemDbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                _studentSystemDbContext.Courses.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Delete(Course entity)
        {
            var entry = _studentSystemDbContext.Entry(entity);

            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                _studentSystemDbContext.Courses.Attach(entity);
                _studentSystemDbContext.Courses.Remove(entity);
            }
        }
    }
}
