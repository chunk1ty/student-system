using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

using StudentSystem.Persistence.Contracts;
using StudentSystem.Domain;
using StudentSystem.Infrastructure.Mapping;
using StudentSystem.Persistence.Entities;

namespace StudentSystem.Persistence.Repositories
{
    public class EfCourseRepository : ICourseRepository
    {
        private readonly StudentSystemDbContext _studentSystemDbContext;
        private readonly IMappingService _mapping;

        public EfCourseRepository(StudentSystemDbContext studentSystemDbContext, IMappingService mapping)
        {
            _studentSystemDbContext = studentSystemDbContext;
            _mapping = mapping;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            var courses =  await _studentSystemDbContext.Set<CourseEntity>()
                                                .AsNoTracking()
                                                .ToListAsync();

            return _mapping.Map<IEnumerable<Course>>(courses);
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            var course =  await _studentSystemDbContext.Set<CourseEntity>()
                                                .AsNoTracking()
                                                .SingleOrDefaultAsync(x => x.Id.Equals(id));

            return _mapping.Map<Course>(course);
        }

        public void Add(Course course)
        {
            var entity = _mapping.Map<CourseEntity>(course);
           
            _studentSystemDbContext.Courses.Add(entity);
        }

        public void Update(Course course)
        {
            var entity = _mapping.Map<CourseEntity>(course);
            _studentSystemDbContext.Courses.Attach(entity);

            var entry = _studentSystemDbContext.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = new CourseEntity {Id = id};
          
            _studentSystemDbContext.Courses.Attach(entity);
            _studentSystemDbContext.Courses.Remove(entity);
        }
    }
}
